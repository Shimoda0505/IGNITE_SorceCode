using UnityEngine;


/// <summary>
/// カメラ内に写ってるかの判別しplayerLinkEnemyスクリプトの配列に格納
/// </summary>
public class EnemyCameraView : MonoBehaviour
{

    PlayerLinkEnemy _playerLinkEnemy;

    //プレイヤーのステータス
    PlayerStatus _playerStatus;

    GameSystem _gameSystem;

    //多重ロックオン避け
    private bool _isLock = false;

    //スクリーン座標
    Rect rect = new Rect(0, 0, 1, 1);

    private const float _disMaxPos = 500;

    //処理-------------------------------------------------------------------------------------------------

    private void Start()
    {
        //プレイヤーのタグからPlayerStatusスクリプトを取得
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        //プレイヤー配列タグからPlayerLinkEnemyスクリプトを取得
        _playerLinkEnemy = GameObject.FindGameObjectWithTag("PlayerArray").GetComponent<PlayerLinkEnemy>();

        _gameSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameSystem>();
    }

    private void FixedUpdate()
    {

        if (_gameSystem._isEvent) { return; }

        //カメラの画面内にオブジェクトがあるかどうか
        CamView();

    }

    //メソッド群--------------------------------------------------------------------------------------------


    /// <summary>
    /// カメラの画面内にオブジェクトがあるかどうか
    /// </summary>
    private void CamView()
    {

        //オブジェクト座標の取得
        Transform targetPos = this.gameObject.transform;

        //前方方向にオブジェクトがあるかどうか
        Vector3 upperForward = Camera.main.WorldToScreenPoint(targetPos.position);

        //スクリーン座標を、カメラView座標(0~1,0~1)に変換
        Vector2 upperCam = Camera.main.WorldToViewportPoint(targetPos.position);

        //カメラの画角内なら && 前方方向にオブジェクトがあるか
        if (rect.Contains(upperCam) && _disMaxPos >= upperForward.z && upperForward.z >= 0)
        {

            //プレイヤーの敵を格納したListに、このオブジェクトを格納
            InPlayerArray();

        }

        //カメラの画角外なら
        else
        {

            //自信を探索して配列要素を削除,弾のターゲットをNull,カーソルを削除
            OutPlayerArray();

        }

    }

    /// <summary>
    /// プレイヤーの敵を格納したListに、このオブジェクトを格納
    /// </summary>
    public void InPlayerArray()
    {

        if (!_isLock)
        {

            //List内にオブジェクトとフラグを格納
            _playerLinkEnemy._targetList.Add(new PlayerLinkEnemy.Targets { _targetObj = this.gameObject, _isLock = false });

            _isLock = true;

        }

    }

    /// <summary>
    /// 自信を探索して配列要素を削除,弾のターゲットをNull,カーソルを削除
    /// </summary>
    public void OutPlayerArray()
    {

        for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
        {

            //i番のListを設定
            PlayerLinkEnemy.Targets target = _playerLinkEnemy._targetList[i];

            //ターゲットなら
            if (target._targetObj == this.gameObject)
            {

                if (target._lockOnCursor != null)
                {

                    //カーソルを消す
                    target._lockOnCursor.GetComponent<CursorController>()._target = null;

                    //ロックオン数を減算
                    _playerStatus.LockPrice("減算");

                }

                if(target._fireBoll != null)
                {

                    //火球のターゲットをnull
                    target._fireBoll.GetComponent<FireBollController>().ChangeEnum();

                }

                //i番の配列要素を削除
                _playerLinkEnemy._targetList.RemoveAt(i);

                break;
            }

        }

        _isLock = false;


    }

    /// <summary>
    /// ロックオン可能な状態に戻す
    /// </summary>
    public void IsLockFalse()
    {

        _isLock = false;
    }

}
