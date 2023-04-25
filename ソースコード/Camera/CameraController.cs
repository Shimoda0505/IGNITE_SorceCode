using UnityEngine;


/// <summary>
/// カメラ制御用スクリプト
/// </summary>
public class CameraController : MonoBehaviour
{

    #region スクリプト
    [Header("スクリプト")]
    [SerializeField, Tooltip("ゲームのイベントやTime.deltaTimeを管理")]
    GameSystem _gameSystem;

    [SerializeField, Tooltip("プレイヤーのコントローラー")]
    PlayerController _playerController;
    #endregion


    #region 追従移動
    [Header("移動")]
    [SerializeField, Tooltip("追従対象")]
    private Transform _targetPos;

    [SerializeField, Tooltip("ターゲットとカメラの離す距離")]
    private float _toTargetDistance;

    //ターゲットとカメラの現在の距離
    private Vector3 _toTarget;

    [SerializeField, Tooltip("カメラの追従速度")]
    private float _followingSpeed;

    //カメラの初期位置
    private Vector3 _defaultPos;
    #endregion


    #region 入力移動
    [SerializeField, Tooltip("移動先の位置")]
    private Vector2 _moveClamp;

    //カメラの移動位置
    private Vector3 _movePos;
    #endregion


    #region 角度
    [Header("角度")]

    [SerializeField, Tooltip("角度の移動先の最大値")]
    private Vector3 _rotateClamp;

    [SerializeField, Tooltip("追従時のカメラの回転速度")]
    private float _followingRotateSpeed;

    //追従時のカメラの角度のみ計算
    private Quaternion _defaultRoatete;

    //カメラ角度の代入
    private Vector3 _cameraRotate;

    //AngleをRotateに変換
    private const int _angleToRotate = 180;
    #endregion


    #region 回避
    [Header("回避")]
    [SerializeField, Tooltip("回避時のカメラ移動")]
    private float _doageMovePos;

    //回避後の位置計算
    private float _doagePos;
    #endregion



    //処理-------------------------------------------------------------------------------------------------

    private void Start()
    {

        //初期値の設定
        DefaultSetting();

    }


    private void FixedUpdate()
    {

        //カメラ角度の計算
        CameraRotate();

        //カメラの左右移動の計算
        CameraMove();

        //カメラの追従移動の計算
        CameraFollowing();

    }


    void LateUpdate()
    {

        //カメラの角度と位置を更新
        UpdatePosRotate();

    }



    //privateのメソッド群------------------------------------------------------------------------------------------------

    /// <summary>
    /// 初期値の設定
    /// </summary>
    private void DefaultSetting()
    {

        //カメラの初期位置を基準位置に設定
        this.gameObject.transform.position = _targetPos.TransformPoint(0, 0, _toTargetDistance);

        //プレイヤーを直視
        this.gameObject.transform.LookAt(_targetPos, Vector3.up);

        //カメラの基準角度を保管
        _defaultRoatete = this.gameObject.transform.rotation;

        //カメラの基準位置を保管
        _defaultPos = _targetPos.localPosition;

        //カメラの位置を基準位置にする
        _movePos = new Vector3(0, 0, _defaultPos.z);

    }


    /// <summary>
    /// カメラ角度の計算
    /// </summary>
    private void CameraRotate()
    {

        //追従のカメラ角度計算-------------------------------------------------------------------
        //追従時のカメラの角度のみ計算
        _defaultRoatete = Quaternion.Slerp(_defaultRoatete, Quaternion.LookRotation(_targetPos.transform.position - transform.position).normalized, Time.deltaTime * _followingRotateSpeed);

        //Quaternion(4次元角)をEuler角(3次元角)に変換
        Vector3 defaultEuler = _defaultRoatete.eulerAngles;


        //入力のカメラ角度計算-------------------------------------------------------------------        　
        //カメラの最大角度に上記の割合を乗算
        Vector2 moveRotate = new Vector2(_rotateClamp.x * PlayerMovePercentage().x, _rotateClamp.y * PlayerMovePercentage().y);

        //角度を360度反転しないようにする
        Vector2 rotateAngle = new Vector3(moveRotate.x / _angleToRotate, moveRotate.y / _angleToRotate);


        //カメラの角度計算------------------------------------------------------------------------
        //追従と入力のカメラ角度から、代入角度を計算
        _cameraRotate = new Vector3(defaultEuler.x - moveRotate.y, defaultEuler.y + moveRotate.x, defaultEuler.z);

    }


    /// <summary>
    /// カメラの左右移動の計算
    /// </summary>
    private void CameraMove()
    {

        //時間0もしくは,イベント中なら何もしない
        if (Time.timeScale == 0 || _gameSystem._isEvent)
        {
            return;
        }

        //カメラの最大移動量に上記の割合を乗算
        _movePos = new Vector2(_moveClamp.x * PlayerMovePercentage().x, _moveClamp.y * PlayerMovePercentage().y);

    }


    /// <summary>
    /// プレイヤーの実際の移動量と最大の移動量から、どれだけの割合移動したかを計算
    /// </summary>
    private Vector2 PlayerMovePercentage()
    {

        //プレイヤーの実際の移動量と最大の移動量から、どれだけの割合移動したかを計算
        Vector2 playerMovePercentage = new Vector2(_playerController.InputMove().x / _playerController.MoveClamp().x,
                                                   _playerController.InputMove().y / _playerController.MoveClamp().y);

        return playerMovePercentage;

    }


    /// <summary>
    /// カメラの追従移動の計算
    /// </summary>
    private void CameraFollowing()
    {

        //ターゲット位置と回転(0.1づつ計測)を引いた位置を計算
        _toTarget = _targetPos.position - Vector3.Lerp(transform.localPosition, _targetPos.TransformPoint(0, 0, _toTargetDistance), 0.1f);

        //正規化して滑らかに
        _toTarget.Normalize();

    }


    /// <summary>
    /// 回避時の位置計算
    /// </summary>
    private void DoageCalculate()
    {

        //回避時の位置計算
        _doagePos = _movePos.x + _doageMovePos * _playerController.DoageDirection();

    }


    /// <summary>
    /// カメラの角度と位置を更新
    /// </summary>
    private void UpdatePosRotate()
    {

        //カメラの角度変更
        this.gameObject.transform.eulerAngles = _cameraRotate;

        //カメラ移動量
        _targetPos.localPosition = new Vector3(_movePos.x, _movePos.y, _defaultPos.z);

        //カメラの追従移動の計算
        this.gameObject.transform.localPosition = _targetPos.position - _toTarget * _followingSpeed;

    }

}
