using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// カーソルに重なっている敵をロックオン
/// </summary>
public class PointerLockOn : MonoBehaviour
{

    #region スクリプト
    [Header("スクリプト")]
    [SerializeField, Tooltip("ゲームのイベントやTime.deltaTimeを管理")]
    GameSystem _gameSystem;

    [SerializeField, Tooltip("敵とプレイヤーの繋ぎを管理")]
    PlayerLinkEnemy _playerLinkEnemy;

    [SerializeField, Tooltip("プレイヤーのプール管理")]
    PoolManager _poolManager;

    [SerializeField]
    PointerChange _pointerChange;

    //プレイヤーの音関連
    PlayerSE _playerSe;

    //プレイヤーのステータス
    PlayerStatus _playerStatus;

    //プレイヤーコントローラー
    PlayerController _playerController;

    //火球プール
    PoolController _fireBollPool;
    private string _fireBoll = "火球";

    //火球(ウルト)プール
    PoolController _fireBollUltPool;
    private string _fireBollUlt = "火球(ウルト)";

    //爆発プール
    PoolController _explosionPool;
    private string _explosion = "爆発";

    //爆発(特別)プール
    PoolController _explosionUltPool;
    private string _explosionUlt = "爆発(ウルト)";

    //入力周り
    _InputSystemController _inputController;
    #endregion


    #region ポインター関連
    [Header("ポインター関連")]
    [SerializeField, Tooltip("ポインターのロックオン範囲")]
    private Vector3 _pointerClamp;

    [SerializeField, Tooltip("ポインターのロックオン範囲(ウルト)")]
    private float _pointerClampUlt;

    //ポインターのロックオン範囲の代入
    private float _pointerClampAssignment;

    [SerializeField, Tooltip("カーソルの親オブジェクト")]
    private GameObject _cursorObj;

    private List<GameObject> _cursorObjs = new List<GameObject>();

    //Ui座標
    private RectTransform _rectTr;

    //Ui座標の最大値
    private Vector2 _maxRectTr = new Vector2(800, 450);

    //カメラ座標の値を(-0.5~0.5)から(0~1)に変換
    private const float _valueChange = 0.5f;
    #endregion


    #region 距離関連
    [Header("距離")]
    [SerializeField, Tooltip("プレイヤー")]
    private GameObject _playerObj;
    #endregion


    #region 弾関連
    [Header("弾関連")]
    [SerializeField, Tooltip("弾の発射位置")]
    private Transform _shotPos;

    [SerializeField, Tooltip("発射位置を変更する時の範囲")]
    private Vector2[] _shotRotates;

    //弾の発射角度の番号
    private int _shotRotateNumber = 0;
    #endregion


    [SerializeField, Tooltip("連射のインターバル")]
    private float _shotIntervalTime;
    private float _shotIntervalCount = 0;

    //ロックオンの状態
    private LockOnMotion _lockOnMotion = LockOnMotion.LOCK_ON;
    private enum LockOnMotion
    {
        WAIT,
        LOCK_ON,//ロックオン
        BOLL_SHOT//火球の発射
    }



    //処理-------------------------------------------------------------------------------------------------

    private void Start()
    {

        //初期設定
        Setting();
    }


    private void Update()
    {

        //時間0もしくは,イベント中なら何もしない
        if (Time.timeScale == 0 || _gameSystem._isEvent || _playerController._playerMotion == PlayerController.PlayerMotion.Death) { return; }


        switch (_lockOnMotion)
        {

            //ロックオン
            case LockOnMotion.LOCK_ON:

                if (_playerController._playerMotion == PlayerController.PlayerMotion.Fly)
                {

                    //右トリガー or　左トリガーを押した時/火球の発射
                    if (_inputController.LeftTriggerDown() || _inputController.RightTriggerDown()) { _lockOnMotion = LockOnMotion.BOLL_SHOT; }

                }

                break;


            case LockOnMotion.BOLL_SHOT:

                break;


            case LockOnMotion.WAIT:

                //時間計測
                _shotIntervalCount += Time.deltaTime;
                if (_shotIntervalCount >= _shotIntervalTime)
                {

                    //時間の初期化
                    _shotIntervalCount = 0;

                    _lockOnMotion = LockOnMotion.LOCK_ON;

                }

                break;

        }

    }


    private void FixedUpdate()
    {

        //時間0もしくは,イベント中なら何もしない
        if (Time.timeScale == 0 || _gameSystem._isEvent || _playerController._playerMotion == PlayerController.PlayerMotion.Death) { return; }


        switch (_lockOnMotion)
        {

            //ロックオン
            case LockOnMotion.LOCK_ON:

                //配列内からポインターと重なっているオブジェクトを探索
                if (_playerController._playerMotion == PlayerController.PlayerMotion.Fly) { SearchArray(); }

                break;


            //火球の発射
            case LockOnMotion.BOLL_SHOT:

                //弾の発射と初期設定
                ShotBoll();

                //ロックオンEnumに戻る
                _lockOnMotion = LockOnMotion.WAIT;

                break;

        }

    }



    //publicのメソッド群-----------------------------------------------------------------------

    /// <summary>
    /// 爆発プールから呼び出し
    /// </summary>
    public GameObject ExplosionPool()
    {
        if (_playerController.IsUlt()) { return _explosionUltPool.GetObj(); }//爆発
        else if (!_playerController.IsUlt()) { return _explosionPool.GetObj(); }//爆発(ウルト)

        return null;
    }

    /// <summary>
    /// ダメージを受けたときにロックオン配列を探索してロックオン解除
    /// </summary>
    public void DamageResetting()
    {

        //配列を全探索
        for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
        {

            //i番のListを設定
            PlayerLinkEnemy.Targets target = _playerLinkEnemy._targetList[i];

            //ターゲットがロックオン中  && 火球が設定されていない
            if (target._isLock && target._fireBoll == null)
            {

                //ロックオンの解除
                target._isLock = false;

                //ロックオンカーソルのターゲットをNull
                target._lockOnCursor.GetComponent<CursorController>()._target = null;

            }

        }

    }

    /// <summary>
    /// ロックオン範囲の変更
    /// </summary>
    public void ChangePointerClamp(string name)
    {

        if (name == "1")
        {
            _pointerClampAssignment = _pointerClamp.x;

            _pointerChange.Change1();
        }
        else if (name == "2")
        {
            _pointerClampAssignment = _pointerClamp.y;

            _pointerChange.Change2();

        }
        else if (name == "3")
        {
            _pointerClampAssignment = _pointerClamp.z;

            _pointerChange.Change3();

        }

        if (name == "ウルト")
        {
            _pointerClampAssignment = _pointerClampUlt;

            _pointerChange.Change1();

        }
    }



    //privateのメソッド群-----------------------------------------------------------------------

    /// <summary>
    /// 初期設定
    /// </summary>
    private void Setting()
    {

        //プレイヤーのステータス
        _playerStatus = _playerObj.GetComponent<PlayerStatus>();

        //プレイヤーコントローラー
        _playerController = _playerObj.GetComponent<PlayerController>();

        //プレイヤーSE
        _playerSe = _playerObj.GetComponent<PlayerSE>();


        //Ui座標
        _rectTr = this.gameObject.GetComponent<RectTransform>();

        //カーソルを全て配列に格納
        for (int i = 0; i <= _cursorObj.transform.childCount - 1; i++)
        {
            _cursorObjs.Add(_cursorObj.transform.GetChild(i).gameObject);

            _cursorObjs[i].SetActive(false);
        }


        //プレイヤーのプール管理からプール設定
        for (int i = 0; i <= _poolManager._poolArrays.Length - 1; i++)
        {

            //プール名を取得
            string poolName = _poolManager._poolArrays[i]._poolName;

            //スクリプトを取得
            PoolController poolScript = _poolManager._poolArrays[i]._poolControllers;

            //名前一致のプールを探索かける
            if (poolName == _fireBoll) { _fireBollPool = poolScript; }//火球
            else if (poolName == _fireBollUlt) { _fireBollUltPool = poolScript; }//火球(ウルト)
            else if (poolName == _explosion) { _explosionPool = poolScript; }//爆発
            else if (poolName == _explosionUlt) { _explosionUltPool = poolScript; }//爆発(ウルト)

        }

        //ロックオン範囲の初期設定
        _pointerClampAssignment = _pointerClamp.x;
    }

    /// <summary>
    /// 配列内からポインターと重なっているオブジェクトを探索
    /// </summary>
    private void SearchArray()
    {

        //ロックオン数が最大に達したらこれ以上処理しない
        if (_playerStatus.MaxLockCount() || _playerController._playerMotion == PlayerController.PlayerMotion.Damage)
        {
            return;
        }

        //敵格納の配列を探索
        for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
        {

            //画面内配列のターゲットを取得
            PlayerLinkEnemy.Targets targetObj = _playerLinkEnemy._targetList[i];

            //ターゲットとの距離
            float dis = Vector3.Distance(_playerObj.transform.position, targetObj._targetObj.transform.position);

            //エネミーがロックオン中ではないなら $$ 距離以内なら
            if (!targetObj._isLock)
            {

                //エネミーの座標を、カメラView座標(0~1,0~1)に変換
                Vector2 enemyPos = Camera.main.WorldToViewportPoint(targetObj._targetObj.transform.position);

                //ポインターのカメラView座標(0~1,0~1)
                //Vector(0~800 , 0~460)を(0~1 , 0~1)に変換
                Vector2 uiPos = new Vector2(_rectTr.localPosition.x / _maxRectTr.x + _valueChange,
                                            _rectTr.localPosition.y / _maxRectTr.y + _valueChange);

                //ポインターとエネミーの距離を計測
                float distans = Vector2.Distance(uiPos, enemyPos);


                //範囲内にエネミーがあるかどうか
                if (-_pointerClampAssignment <= distans && distans <= _pointerClampAssignment)
                {

                    //ロックオン数を加算
                    _playerStatus.LockPrice("加算");

                    //ターゲットをロックオン状態にする
                    targetObj._isLock = true;

                    //ロックオン音を鳴らす
                    _playerSe.RockOnSe();

                    for (int j = 0; j <= _cursorObjs.Count - 1; j++)
                    {

                        if (_cursorObjs[j].activeSelf == false)
                        {

                            //カーソルをアクティブにする
                            _cursorObjs[j].SetActive(true);

                            //カーソルもターゲットを設定
                            _cursorObjs[j].GetComponent<CursorController>()._target = targetObj._targetObj;

                            //カーソルを配列に格納
                            targetObj._lockOnCursor = _cursorObjs[j];

                            break;
                        }

                    }


                }
            }

        }

    }

    /// <summary>
    /// 弾の発射と初期設定
    /// </summary>
    private void ShotBoll()
    {

        //配列を全探索
        for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
        {

            //配列のターゲットを取得
            PlayerLinkEnemy.Targets targetObj = _playerLinkEnemy._targetList[i];

            //ロックオン中のターゲットを探索
            if (targetObj._isLock && targetObj._fireBoll == null)
            {

                //カーソルのカラーを弾発射時カラーに変更
                targetObj._lockOnCursor.GetComponent<PointerImage>().ShotColorChange();

                //火球の初期化
                GameObject fireBoll = null;

                //弾をプールから取得
                if (!_playerController.IsUlt()) { fireBoll = _fireBollPool.GetObj(); }//火球
                else if (_playerController.IsUlt()) { fireBoll = _fireBollUltPool.GetObj(); }//火球(ウルト)


                //スクリプトを取得
                FireBollController fireBollController = fireBoll.GetComponent<FireBollController>();

                //弾を配列に格納
                targetObj._fireBoll = fireBoll;

                //弾をアクティブにする
                fireBoll.SetActive(true);

                //弾の初期設定
                fireBollController.TargetStore(targetObj._targetObj, _playerStatus.LockCount(), this.gameObject.GetComponent<PointerLockOn>());

                //弾の発射位置をPlayerの銃口に設定
                fireBoll.transform.position = _shotPos.position;

                //弾の発射位置をPlayerの銃口に設定
                fireBoll.transform.rotation = _shotPos.rotation;


                //弾音
                _playerSe.FireBollSe();

                //次の発射角度
                _shotRotateNumber++;
            }
        }

        //発射角度の番号を初期化
        _shotRotateNumber = 0;

        //弾の発射角度を初期化
        _shotPos.rotation = new Quaternion(0, 0, 0, 0);

        //ロックオン数の初期化
        _playerStatus.LockPrice("初期化");

    }


}















