using UnityEngine;



/// <summary>
/// プレイヤーのウルト時の火球制御
/// </summary>
public class UltBoll : MonoBehaviour
{

    #region 挙動
    [Header("挙動")]
    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _moveTime;

    [SerializeField]
    private float _exTime;

    [SerializeField]
    private float _waitTime;

    private float _moveCount = 0;

    private GameObject _shutPos;

    //ボスに攻撃したかどうか
    private bool _isBoss = false;

    private GameObject _bossObj;

    private const int ATTACK_DAMAGE = 1000;
    #endregion

    #region スクリプト
    PlayerLinkEnemy _playerLinkEnemy;

    PlayerStatus _playerStatus;

    BossStatus _bossStatus;

    ScoreManager _scoreManager;

    PoolController _exPool;
    #endregion


    Motion _motion = Motion.WAIT;
    enum Motion
    {
        WAIT,
        MOVE,
        STOP
    }
    private void Start()
    {

        //プレイヤーの駅管理配列を取得
        _playerLinkEnemy = GameObject.FindGameObjectWithTag("PlayerArray").GetComponent<PlayerLinkEnemy>();

        //プレイヤーのステータスを取得
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        //ボスステータスを取得
        _bossStatus = GameObject.FindGameObjectWithTag("BossBody").GetComponent<BossStatus>();

        _scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();

        _exPool = GameObject.FindGameObjectWithTag("UltBollEx").GetComponent<PoolController>();

        _shutPos = GameObject.FindGameObjectWithTag("ShutArea").gameObject;

    }


    void FixedUpdate()
    {

        switch (_motion)
        {

            case Motion.WAIT:


                this.gameObject.transform.position = _shutPos.transform.position;

                _moveCount += Time.deltaTime;

                if(_moveCount >= _waitTime)
                {
                    _moveCount = 0;

                    _motion = Motion.MOVE;
                }

                break;


            case Motion.MOVE:

                //前方方向に直進
                this.gameObject.transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);

                _moveCount += Time.deltaTime;

                if (_moveCount >= _moveTime)
                {

                    GameObject obj = _exPool.GetObj();

                    obj.transform.position = this.gameObject.transform.position;

                    obj.SetActive(true);

                    //爆発音
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSE>().ExplosionBigSe();

                    _moveCount = 0;

                    DamageEnemy();

                    _motion = Motion.STOP;

                }

                break;

            case Motion.STOP:

                _moveCount += Time.deltaTime;

                if(_moveCount >= _exTime)
                {

                    _moveCount = 0;

                    //弾の位置を追従する対象をBurretPoolタグで指定
                    this.gameObject.transform.position = GameObject.FindGameObjectWithTag("PlayerArray").transform.position;

                    //アクティブ終了
                    this.gameObject.SetActive(false);

                    _motion = Motion.WAIT;

                }

                break;
        
        }

    }

    private void DamageEnemy()
    {

        //敵格納の配列を全探索
        for(int i = _playerLinkEnemy._targetList.Count - 1; i >= 0;i--)
        {

            PlayerLinkEnemy.Targets targets = _playerLinkEnemy._targetList[i];

            GameObject target = targets._targetObj;

            if(target.tag == "Enemy")
            {

                //敵のダメージメソッド
                if (target.GetComponent<EnemyDeath>()) { target.GetComponent<EnemyDeath>().EnemyDeathController(); }/*【他メンバーが制作したため添付してません】*/

                //撃破数の加算
                _scoreManager.SmashEnemyCount(1);

            }

            else if(target.tag == "Boss")
            {

                //雷蔵君のロックオン状態を解除
                if (targets._targetObj.GetComponent<EnemyCameraView>()) { targets._targetObj.GetComponent<EnemyCameraView>().IsLockFalse(); }

                //ボスオブジェクトを格納
                _bossObj = targets._targetObj;

                _isBoss = true;

            }


            if (targets._fireBoll != null)
            {

                targets._fireBoll.GetComponent<FireBollController>().TargetNull();
            }

            if (targets._lockOnCursor != null)
            {

                //カーソルを消す
                targets._lockOnCursor.GetComponent<CursorController>()._target = null;

            }

            //i番の配列要素を削除
            _playerLinkEnemy._targetList.RemoveAt(i);

            //チェイン数の加算
            _playerStatus.ChainAddition();


        }

        if (_isBoss)
        {

            BossStatus bossStatus = _bossObj.transform.root.gameObject.GetComponent<BossStatus>();

            bossStatus.BossDamage(ATTACK_DAMAGE);
            bossStatus.IsUltTrue();

            _isBoss = false;

        }

    }
}
