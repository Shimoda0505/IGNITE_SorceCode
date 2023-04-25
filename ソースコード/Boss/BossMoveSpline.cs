using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;/*【SplineのEditor拡張が必要です】*/
using System;



/// <summary>
/// ボスのSpline上の移動制御
/// </summary>
public class BossMoveSpline : MonoBehaviour
{

    #region スクリプト
    [Header("スクリプト")]
    [SerializeField]
    RootNav _rootNav;

    [SerializeField]
    RizouSystem _rizouSystem;/*【他メンバーが制作したため添付してません】*/

    [SerializeField]
    GameSystem _gameSystem;

    [SerializeField]
    BossStatus _bossStatus;

    [SerializeField]
    RaizoAnimationController _raizoAnim;

    [SerializeField]
    PlayerController _playerController;

    [SerializeField]
    PlayerStatus _playerStatus;

    [SerializeField]
    CameraShake _cameraShake;

    [SerializeField]
    RaizoSe _raizoSe;

    #endregion

    #region プレイヤー関連
    [Header("プレイヤー関連")]
    [SerializeField, Tooltip("プレイヤー")]
    private GameObject _playerObj;
    #endregion

    #region 攻撃関連
    [Header("攻撃関連")]
    [SerializeField, Tooltip("突進の判定")]
    private float _tackleDis;

    //突進中かどうか
    private bool _istackle;

    [SerializeField, Tooltip("タックルエフェクト")]
    private GameObject _tackleEff;

    [SerializeField, Tooltip("カメラの振動時間/振動幅")]
    private Vector2 _shakeTimeMag;

    //タックルの攻撃力
    private int _tackleAttack = 250;
    #endregion

    #region スプライン関連
    //ルート速度
    private float _rootSpeed;

    //補間の割合(0~1の間を始点^終点で移動)
    private float _percentage;

    //前フレームのワールド位置
    private Vector3 _prevPos;

    //前回のNowRootを補完
    private GameObject _lastPoint = null;

    //設定されたスプライン
    private SplineContainer _settingSpline;/*【SplineのEditor拡張が必要です】*/
    #endregion

    #region 行動情報
    [Header("行動情報")]
    [SerializeField, Tooltip("変更ポイントとの距離")]
    private float _pointDistance;

    //現在のモーションデータの番号
    private int _motionNumber = 0;

    //挙動変更番号
    private int _changeNumber = 0;

    [Serializable, Tooltip("ターゲット関連の格納要素")]
    public class MotionDatas
    {

        [SerializeField, Tooltip("プレイヤーの移動情報")]
        public GameObject _playerRoot;

        [SerializeField, Tooltip("雷蔵のスプライン情報")]
        public SplineContainer _moveSpline;/*【SplineのEditor拡張が必要です】*/

        [SerializeField, Tooltip("雷蔵の移動速度")]
        public float[] _moveSpeed;

        [SerializeField, Tooltip("雷蔵のモーション変化ポイント")]
        public GameObject[] _motionChange;

        [SerializeField, Tooltip("各ポイントのウルトモーション")]
        public ActiveMotion[] _activeMotion;
        public enum ActiveMotion
        {
            SHOT,
            HOMING,
            BIG,
            TACKLE,
            Roar,
            CHANGE_LOOK,
            CAMERA_SHAKE
        }

    }

    //ターゲットのList
    public List<MotionDatas> _motionDatas = new List<MotionDatas>();
    #endregion


    /// <summary>
    /// 行動中かどうか
    /// </summary>
    Motion _motion = Motion.WAIT;
    enum Motion
    {
        WAIT,//待機
        MOVE,//移動
        Death
    }

    /// <summary>
    /// 視点の選択
    /// </summary>
    LookMotion _lookMotion = LookMotion.LOOK_FOWARD;
    enum LookMotion
    {

        LOOK_FOWARD,
        LOOK_PLAYER
    }




    //処理群--------------------------------------------------------------------------------------------------------------------

    private void FixedUpdate()
    {

        //死亡したら
        if (_bossStatus.IsDeath()) { _raizoAnim.DamageAnim(); this.gameObject.SetActive(false); }

        //ウルトをくらったら
        if (_bossStatus.IsUlt()) { _raizoAnim.DamageAnim(); _bossStatus.IsUltFalse(); }

        //イベント中 || プレイヤー死亡時は停止
        if (_gameSystem._isEvent || _playerController._playerMotion == PlayerController.PlayerMotion.Death) { return; }

        //Hpが半分になったら
        if (_bossStatus.IsHpHalf()) { _tackleAttack = 500; }

        //攻撃モーション
        switch (_motion)
        {

            //待機
            case Motion.WAIT:

                //モーションデータの探索
                MotionArray();

                break;


            //移動
            case Motion.MOVE:

                //Spline上を移動
                MoveSpline();

                //視点
                switch (_lookMotion)
                {

                    case LookMotion.LOOK_FOWARD:

                        //進行方向を直視
                        LookFoward();

                        break;

                    case LookMotion.LOOK_PLAYER:

                        //プレイヤーを直視
                        LockPlayer();

                        break;

                }

                //攻撃用のモーションの変更
                ActivePoint();

                //突進時の処理
                TackleProcess();

                break;

        }

    }



    //メソッド群--------------------------------------------------------------------------------------------------------------------

    //探索
    /// <summary>
    /// モーションデータの探索
    /// </summary>
    private void MotionArray()
    {

        //前回からプレイヤーの位置情報が更新されているなら
        if (_lastPoint != _rootNav.NowPoint())
        {

            //モーションデータを探索
            for (int i = 0; i <= _motionDatas.Count - 1; i++)
            {

                //モーションデータの補完
                MotionDatas data = _motionDatas[i];

                //プレイヤーの位置情報を補完
                GameObject _motionPos = data._playerRoot;

                //プレイヤーの現在位置情報と一致したなら
                if (_motionPos == _rootNav.NowPoint())
                {

                    //アニメーションの初期化
                    _raizoAnim.LookForwardAnim();

                    //視点の初期化
                    _lookMotion = LookMotion.LOOK_FOWARD;

                    //突進状態の初期化
                    _istackle = false;
                    _tackleEff.SetActive(false);

                    //モーションデータの番号を補完
                    _motionNumber = i;

                    //位置情報を更新
                    _lastPoint = _motionPos;

                    //スプライン情報の更新
                    _settingSpline = data._moveSpline;

                    //移動速度の更新
                    _rootSpeed = data._moveSpeed[0];

                    //移動開始
                    _motion = Motion.MOVE;

                }

            }

        }

    }


    //移動
    /// <summary>
    /// Spline上を移動
    /// </summary>
    private void MoveSpline()
    {

        //割合を時間で加算
        _percentage += Time.deltaTime * _rootSpeed;

        // 計算した位置（ワールド座標）をターゲットに代入
        this.gameObject.transform.position = _settingSpline.EvaluatePosition(_percentage);

        //設定されているSplineの終点
        if (_percentage >= 1)
        {

            //割合の初期化
            _percentage = 0;

            //移動中のモーション変更暗号を初期化
            _changeNumber = 0;

            //待機Enum
            _motion = Motion.WAIT;

        }

    }


    //視点
    /// <summary>
    /// 進行方向を直視
    /// </summary>
    private void LookFoward()
    {

        //視点の向き------------------------------------------------------------------------
        // 現在フレームのフレーム位置
        Vector3 position = this.gameObject.transform.position;

        // 移動量を計算
        Vector3 moveVolume = position - _prevPos;

        // 次のUpdateで使うための前フレーム位置補完
        _prevPos = position;

        // 進行方向に角度を変更
        this.gameObject.transform.rotation = Quaternion.LookRotation(moveVolume, Vector3.up);

    }

    /// <summary>
    /// プレイヤーを直視
    /// </summary>
    private void LockPlayer()
    {

        //プレイヤーを直視
        this.gameObject.transform.LookAt(_playerObj.transform);

    }


    //行動関連
    /// <summary>
    /// 挙動の変更
    /// </summary>
    private void ActivePoint()
    {

        //データがあるなら
        if (_changeNumber <= _motionDatas[_motionNumber]._motionChange.Length - 1)
        {

            //ボスの位置
            Vector3 thisPos = this.gameObject.transform.position;

            //変更ポイントの位置
            Vector3 changePos = _motionDatas[_motionNumber]._motionChange[_changeNumber].transform.position;

            //2点の距離
            float distance = Vector3.Distance(thisPos, changePos);

            //一定範囲内なら
            if (-_pointDistance <= distance && distance <= _pointDistance)
            {

                //移動速度の変更
                _rootSpeed = _motionDatas[_motionNumber]._moveSpeed[_changeNumber + 1];

                //モーションデータの格納
                MotionDatas.ActiveMotion activeMotion = _motionDatas[_motionNumber]._activeMotion[_changeNumber];

                //モーションの変更
                if (activeMotion == MotionDatas.ActiveMotion.SHOT) { _rizouSystem.Barrage(); _raizoAnim.AttackAnim(); _raizoSe.BulletSe(); }//通常弾
                else if (activeMotion == MotionDatas.ActiveMotion.HOMING) { _rizouSystem.Homing(); _raizoAnim.AttackAnim(); _raizoSe.BulletSe(); }//ホーミング弾
                else if (activeMotion == MotionDatas.ActiveMotion.BIG) { _rizouSystem.LargeFireBall(); _raizoAnim.AttackAnim(); _raizoSe.BigBulletSe(); }//大弾
                else if (activeMotion == MotionDatas.ActiveMotion.TACKLE) { _istackle = true; _raizoAnim.TackleAnimT(); _raizoSe.TackleSe(); _tackleEff.SetActive(true); }//突進
                else if (activeMotion == MotionDatas.ActiveMotion.Roar) { _raizoSe.RoarSe(); }//咆哮
                else if (activeMotion == MotionDatas.ActiveMotion.CHANGE_LOOK) //視点変更
                {
                    _raizoSe.RoarSe();
                    _raizoSe.WingSe();

                    if (_lookMotion == LookMotion.LOOK_FOWARD) { _lookMotion = LookMotion.LOOK_PLAYER; _raizoAnim.LookPlayerAnim(); }//プレイヤー直視に変更
                    else if (_lookMotion == LookMotion.LOOK_PLAYER) { _lookMotion = LookMotion.LOOK_FOWARD; _raizoAnim.LookForwardAnim(); }//前方直視に変更  

                }
                else if (activeMotion == MotionDatas.ActiveMotion.CAMERA_SHAKE) { _cameraShake.Shake(_shakeTimeMag.x, _shakeTimeMag.y); _raizoSe.WingSe(); _raizoSe.BigRoarSe(); }//カメラ揺れ

                //地点更新
                _changeNumber++;

            }

        }

    }

    /// <summary>
    /// 突進時の処理
    /// </summary>
    private void TackleProcess()
    {

        if (_istackle)
        {
            //プレイヤーとの距離
            float dis = Vector3.Distance(this.gameObject.transform.position, _playerObj.transform.position);

            //範囲内に入ったら
            if (-_tackleDis <= dis && dis <= _tackleDis)
            {

                //プレイヤーにダメージ
                _playerStatus.Hit(_tackleAttack);

                //カメラ揺れ
                _cameraShake.Shake(_shakeTimeMag.x, _shakeTimeMag.y);

            }

        }

    }




    /// <summary>
    /// ギズモ表示
    /// </summary>
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(this.gameObject.transform.position, _tackleDis);
    }

}
