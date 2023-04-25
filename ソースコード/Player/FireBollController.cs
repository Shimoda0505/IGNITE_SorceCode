using UnityEngine;



/// <summary>
/// プレイヤーの弾挙動管理
/// </summary>
public class FireBollController : MonoBehaviour
{

    #region 取得関連
    //追従する対象
    private GameObject _targetObj;

    //ロックオンのスクリプト
    PointerLockOn _pointerLockOn;

    //敵格納の配列
    PlayerLinkEnemy _playerLinkEnemy;

    //プレイヤーのステータス
    PlayerStatus _playerStatus;

    ScoreManager _scoreManager;
    #endregion


    #region 移動関連
    [SerializeField, Tooltip("弾速")]
    private float _moveSpeed;

    [SerializeField, Tooltip("発射時の旋回速度")]
    private float rotateSpeedStart;

    [SerializeField, Tooltip("旋回速度")]
    private float rotateSpeed;

    private float rotateSpeedIn;
    #endregion


    #region 時間関連
    [SerializeField, Tooltip("ロックオン対象がいない時の消えるまでの時間")]
    private float _disappearTime;

    //ロックオン対象がいない時の消えるまでの時間を計測
    private float _disappearCount;

    [SerializeField, Tooltip("旋回速度切り替わり時間")]
    private float _changeTime;

    //旋回速度切り替わり時間を計測
    private float _changeCount = 0;
    #endregion

    //火球の火力
    private const int ATTACK_DAMAGE = 100;
    //ロックオン数
    private int _lockOnCount;

    [SerializeField, Tooltip("弾の当たり判定範囲")]
    private float _hitClamp;

    //追従対象がいるかどうか
    public Motion _motion = Motion.HOMING;
    public enum Motion
    {

        HOMING,//追従移動
        LINE//直線移動

    }



    //処理-------------------------------------------------------------------------------------------------

    void Start()
    {
        //プレイヤーの駅管理配列を取得
        _playerLinkEnemy = GameObject.FindGameObjectWithTag("PlayerArray").GetComponent<PlayerLinkEnemy>();

        //プレイヤーのステータスを取得
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        _scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();

    }

    private void FixedUpdate()
    {

        switch (_motion)
        {

            case Motion.HOMING:

                //弾の移動(追従)
                HomingMoveObj();

                break;


            case Motion.LINE:

                //弾の移動(直線)
                LineMoveobj();

                break;

        }

        //ターゲットとの距離を計測
        TargetDistance();

    }



    //publicのメソッド群-------------------------------------------------------------------------------------------------

    /// <summary>
    /// 弾の初期設定
    /// </summary>
    public void TargetStore(GameObject target, int count, PointerLockOn pointer)
    {

        //弾のターゲットを設定
        _targetObj = target;

        //ターゲット数を格納
        _lockOnCount = count;

        _pointerLockOn = pointer;
    }

    /// <summary>
    /// 弾の挙動を変更
    /// </summary>
    public void ChangeEnum() { _motion = Motion.LINE; }

    /// <summary>
    /// ターゲットの初期化
    /// </summary>
    public void TargetNull() { _targetObj = null; }



    //privateのメソッド群---------------------------------------------------------------------------------------------------

    //ホーミング
    /// <summary>
    /// 弾の移動(追従)
    /// </summary>
    private void HomingMoveObj()
    {

        //ターゲットがNullになったら
        if (_targetObj == null) { _motion = Motion.LINE; return; }

        //ターゲットと弾の距離
        Vector3 targetDirection = _targetObj.transform.position - this.gameObject.transform.position;

        //始点,終点,速度 * 時間,振幅
        Vector3 newDirection = Vector3.RotateTowards(this.gameObject.transform.forward, targetDirection, RotateSpeed() * Time.deltaTime, 0f);

        //前方方向に直進
        this.gameObject.transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);

        //ターゲットの方向に向きを変える
        this.gameObject.transform.rotation = Quaternion.LookRotation(newDirection);

    }

    /// <summary>
    /// 追従速度
    /// </summary>
    private float RotateSpeed()
    {

        if (_lockOnCount >= 2)
        {
            _changeCount += Time.deltaTime;

            if (_changeCount <= _changeTime) { return rotateSpeedStart; }
            else { return rotateSpeed; }

        }

        return rotateSpeed;

    }


    //ホーミングなし
    /// <summary>
    /// 弾の移動(直線)
    /// </summary>
    private void LineMoveobj()
    {
        //前方方向に直進
        this.gameObject.transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);

        //_disappearTimeの時間計測
        _disappearCount += Time.deltaTime;

        //_disappearTimeの時間を過ぎたとき
        if (_disappearCount >= _disappearTime)
        {

            //初期化
            Resetting();

            //_disappearTimeの時間計測(初期化)
            _disappearCount = 0;

            return;

        }

    }


    //弾の衝突時
    /// <summary>
    /// ターゲットとの距離を計測とヒット検出
    /// </summary>
    private void TargetDistance()
    {

        //ターゲットがないなら処理しない
        if (_targetObj == null) { return; }

        //弾の位置
        Vector3 thisPos = this.gameObject.transform.position;

        //ターゲットの位置
        Vector3 enemyPos = _targetObj.transform.position;

        //次のポイントとの距離を計測
        float distans = Vector3.Distance(thisPos, enemyPos);

        //弾の範囲がターゲットの範囲内に入ったら
        if (-_hitClamp <= distans && distans <= _hitClamp)
        {

            //BurretPoolから弾を呼び出し
            GameObject explosionObj = _pointerLockOn.ExplosionPool();

            //爆発エフェクトの位置
            Explosion(explosionObj);

            //敵にヒットした時敵側のメソッド呼び出し###############################################
            HitEnemy();

            //初期化
            Resetting();

        }

    }

    /// <summary>
    /// 爆発エフェクトの呼び出し
    /// </summary>
    private void Explosion(GameObject explosionObj)
    {

        //爆発をアクティブ
        explosionObj.SetActive(true);

        //弾の発射位置をPlayerの銃口に設定
        explosionObj.transform.localPosition = this.gameObject.transform.position;

        //爆発音
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSE>().ExplosionMiniSe();

    }

    /// <summary>
    /// 自信を探索
    /// </summary>
    public void HitEnemy()
    {

        //タグがEnemyなら
        if (_targetObj.tag == "Enemy")
        {

            for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
            {

                //i番のListを設定
                PlayerLinkEnemy.Targets target = _playerLinkEnemy._targetList[i];

                //ターゲットなら
                if (target._fireBoll == this.gameObject)
                {

                    //カーソルを消す
                    target._lockOnCursor.GetComponent<CursorController>()._target = null;

                    //i番の配列要素を削除
                    _playerLinkEnemy._targetList.RemoveAt(i);

                    break;
                }

            }

            //チェイン数の加算
            _playerStatus.ChainAddition();

            //撃破数の加算
            _scoreManager.SmashEnemyCount(_lockOnCount);

        }

        //タグがBossなら
        else if (_targetObj.tag == "Boss")
        {

            for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
            {

                //i番のListを設定
                PlayerLinkEnemy.Targets target = _playerLinkEnemy._targetList[i];

                //ターゲットなら
                if (target._fireBoll == this.gameObject)
                {

                    //ボスに火球分のダメージ
                    target._targetObj.transform.root.gameObject.GetComponent<BossStatus>().BossDamage(ATTACK_DAMAGE);

                    //カーソルを消す
                    target._lockOnCursor.GetComponent<CursorController>()._target = null;

                    //ロック状態の解除
                    target._isLock = false;

                    //雷蔵君のロックオン状態を解除
                    if (target._targetObj.GetComponent<EnemyCameraView>())
                    {
                        target._targetObj.GetComponent<EnemyCameraView>().IsLockFalse();

                        //i番の配列要素を削除
                        _playerLinkEnemy._targetList.RemoveAt(i);

                    }

                    break;
                }

            }

            //チェイン数の加算
            _playerStatus.ChainAddition();

        }

    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Resetting()
    {

        //ターゲーット初期化
        _targetObj = null;

        //時間計測初期化
        _changeCount = 0;

        //ロックオン数初期化
        _lockOnCount = 0;

        //ホーミングEnum
        _motion = Motion.HOMING;

        //弾の位置を追従する対象をBurretPoolタグで指定
        this.gameObject.transform.position = GameObject.FindGameObjectWithTag("PlayerArray").transform.position;

        //アクティブ終了
        this.gameObject.SetActive(false);

    }


    /// <summary>
    /// ギズモ表示
    /// </summary>
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(this.gameObject.transform.position, _hitClamp);
    }

}
