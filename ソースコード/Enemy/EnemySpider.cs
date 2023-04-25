using UnityEngine;



/// <summary>
/// 敵(蜘蛛)の挙動
/// </summary>
public class EnemySpider : MonoBehaviour
{

    #region スクリプト
    [Header("スクリプト")]
    [SerializeField, Tooltip("プレイヤーのルートナビ")]
    RootNav _rootNav;

    [SerializeField, Tooltip("プール管理")]
    PoolManager _poolManager;

    //蜘蛛の糸
    PoolController _poolController;

    //カメラ描画スクリプト
    EnemyCameraView _enemyCameraView;
    #endregion

    [SerializeField, Tooltip("アクティブポイント")]
    private GameObject _movePos;

    private string _spiderBoll = "蜘蛛の糸";

    #region 発射
    [Header("発射")]
    [SerializeField, Tooltip("プレイヤー")]
    private GameObject _player;

    [SerializeField, Tooltip("弾発射位置")]
    private GameObject _shotPos;

    [SerializeField, Tooltip("プレイヤーとの距離")]
    private float _activeDistance;

    private GameObject _bulletObj;

    #endregion

    //アニメーター
    private Animation _anim;

    #region 時間
    [Header("時間")]
    [SerializeField, Tooltip("移動からの消えるまでの時間")]
    private float _activeFalseTime;

    //時間計測
    private float _count = 0;
    #endregion

    Motion _motion = Motion.WAIT;
    enum Motion
    {

        WAIT,
        MOVE,
        ATTACK,
        DEATH

    }


    //処理部-------------------------------------------------------------------------------------------------

    private void Start()
    {

        //初期設定
        Setting();

    }

    private void FixedUpdate()
    {

        //時間計測後アクティブfalse
        TimeCount();

        switch (_motion)
        {

            case Motion.WAIT:

                //移動開始
                MoveStart();

                break;

            case Motion.MOVE:

                //攻撃の開始
                AttackStart();

                break;


            case Motion.ATTACK:

                break;


            case Motion.DEATH:

                Death();

                break;

        }

    }


    //メソッド部--------------------------------------------------------------------------------------------

    //Start
    /// <summary>
    /// 初期設定
    /// </summary>
    private void Setting()
    {

        //アニメーター取得
        _anim = this.gameObject.GetComponent<Animation>();

        //アニメーターfalse
        _anim.enabled = false;

        //スクリプト取得
        _enemyCameraView = this.gameObject.GetComponent<EnemyCameraView>();

        //スクリプトfalse
        _enemyCameraView.enabled = false;

        //プレイヤーのプール管理からプール設定
        for (int i = 0; i <= _poolManager._poolArrays.Length - 1; i++)
        {

            //プール名を取得
            string poolName = _poolManager._poolArrays[i]._poolName;

            //スクリプトを取得
            PoolController poolScript = _poolManager._poolArrays[i]._poolControllers;

            //名前一致のプールを探索かける
            if (poolName == _spiderBoll)
            {

                _poolController = poolScript;
            }



        }

    }


    //その他
    /// <summary>
    /// 時間計測後アクティブfalse
    /// </summary>
    private void TimeCount()
    {

        if (_motion != Motion.WAIT && _motion != Motion.MOVE)
        {

            //時間計測
            _count += Time.deltaTime;

            //計測後
            if (_count >= _activeFalseTime)
            {

                _enemyCameraView.OutPlayerArray();

                //アクティブfalse
                this.gameObject.SetActive(false);

            }

        }

    }


    //移動
    /// <summary>
    /// 移動開始
    /// </summary>
    private void MoveStart()
    {

        //移動ポイントに達したら
        if (_movePos == _rootNav.NowPoint())
        {

            //アニメーターtrue
            _anim.enabled = true;

            //スクリプトtrue
            _enemyCameraView.enabled = true;

            //移動Enum
            _motion = Motion.MOVE;

        }

    }


    //攻撃
    /// <summary>
    /// 攻撃開始
    /// </summary>
    private void AttackStart()
    {

        //弾の位置
        Vector3 thisPos = this.gameObject.transform.position;

        //ターゲットの位置
        Vector3 playerPos = _player.transform.position;

        //次のポイントとの距離を計測
        float distans = Vector3.Distance(thisPos, playerPos);

        //距離が近づいたら
        if (distans <= _activeDistance)
        {

            //攻撃アニメーション
            _anim.CrossFade("attack");

            //弾をプールから取得
            GameObject bullet = _poolController.GetObj();

            _bulletObj = bullet;

            //スクリプトを取得
            BulletController bullCon = bullet.GetComponent<BulletController>();

            //弾をアクティブにする
            bullet.SetActive(true);

            //ターゲットを弾に設定
            bullCon._targetObj = _player;

            //発射Posの向きを変更
            _shotPos.transform.LookAt(_player.transform);

            //弾の発射位置をPlayerの銃口に設定
            bullet.transform.position = _shotPos.transform.position;

            //弾の発射位置をPlayerの銃口に設定
            bullet.transform.rotation = _shotPos.transform.rotation;

            //攻撃Enum
            _motion = Motion.ATTACK;

        }


    }


    //死亡
    /// <summary>
    /// 死亡
    /// </summary>
    private void Death()
    {
        transform.Translate(Vector3.down);
    }
    public void EnemyDeath()
    {

        //Active中の弾を削除
        if (_bulletObj != null) { _bulletObj.SetActive(false); }

        //死亡アニメーション
        _anim.CrossFade("death");

        //死亡Enum
        _motion = Motion.DEATH;

    }

}
