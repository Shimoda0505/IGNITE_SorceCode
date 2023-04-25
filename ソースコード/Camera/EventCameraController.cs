using UnityEngine;



/// <summary>
/// イベント時のカメラ制御
/// </summary>
public class EventCameraController : MonoBehaviour
{
    [Header("スクリプト")]
    [SerializeField]
    GameStart _gameStart;

    [SerializeField]
    ScoreManager _scoreManager;

    [SerializeField]
    PlayerBgm _playerBgm;

    [SerializeField]
    PlayerMoveSpline _moveSpline;

    AnyUseMethod _anyUseMethod;

    Motion _motion;
    enum Motion
    {
        START,
        END
    }



    [Header("開始")]
    [SerializeField, Tooltip("開始時の直視位置")]
    private Transform _lookStartPos;

    [SerializeField, Tooltip("スタートカメラの始点")]
    private Transform _gameStartStart;

    [SerializeField, Tooltip("スタートカメラの終点")]
    private Transform _gameStartEnd;

    [SerializeField, Tooltip("カメラの追従速度")]
    private float _moveSpeed;

    [SerializeField, Tooltip("開始までの時間")]
    private float _moveStartTime;

    [SerializeField, Tooltip("終了までの時間")]
    private float _moveEndTime;

    private float _moveCount;

    private Vector3 _cameraMovePos;


    [Header("終了")]
    [SerializeField, Tooltip("終了時の直視位置")]
    private Transform _lookEndPos;

    [SerializeField]
    private Transform _moveEndPos;

    private bool _isEndOne = false;

    //処理部------------------------------------------------------------------------------------------------------------------------

    private void Start()
    {

        //初期位置の設定
        _cameraMovePos = _gameStartStart.localPosition;

    }

    private void LateUpdate()
    {

        switch (_motion)
        {

            case Motion.START:

                //時間の計測
                _moveCount += Time.deltaTime;

                //時間計測後
                if (_moveCount >= _moveStartTime)
                {

                    //２点間の計算
                    Vector3 differencePos = _anyUseMethod.MoveToWardsPercentage(_cameraMovePos, _gameStartEnd.localPosition);

                    //２点間の移動
                    _cameraMovePos = _anyUseMethod.MoveToWardsVector3(_cameraMovePos, _gameStartEnd.localPosition, Time.deltaTime * _moveSpeed, differencePos);

                    //時間計測後
                    if(_cameraMovePos == _gameStartEnd.localPosition)
                    {

                        //終了Enumに遷移
                        _motion = Motion.END;

                        //ゲーム開始の終了
                        _gameStart.StartCamEnd();

                        //Active終了
                        this.gameObject.SetActive(false);

                    }

                }

                //移動処理
                MoveProcess(_cameraMovePos, _lookStartPos);

                break;


            case Motion.END:

                //終了イベント
                GameEnd();

                //移動処理
                MoveProcess(_moveEndPos.localPosition, _lookEndPos);

                break;        
        
        }

    }



    //メソッド部------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 移動処理
    /// </summary>
    private void MoveProcess(Vector3 move,Transform look)
    {

        //カメラ距離を計算して旋回速度を掛け代入
        this.gameObject.transform.localPosition = move;

        //プレイヤーを直視
        this.gameObject.transform.LookAt(look, Vector3.up);

    }


    /// <summary>
    /// ゲーム終了
    /// </summary>
    public void GameEnd()
    {
        if(!_isEndOne)
        {

            //移動
            _moveSpline.ChangeMoveSpeed("移動");

            //終了UI
            _scoreManager.ScoreShowing();

            //ゲームクリアBGM
            _playerBgm.ClearBgm();

            _isEndOne = true;

        }

    }

}
