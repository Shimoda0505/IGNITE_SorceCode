using UnityEngine;
using UnityEngine.Splines;/*【SplineのEditor拡張が必要です】*/


/// <summary>
/// プレイヤーのSpline上移動管理
/// </summary>
public class PlayerMoveSpline : MonoBehaviour
{

    #region スプラインのデータ
    [Header("スプライン")]
    [SerializeField, Tooltip("ステージのスプライン")]
    private SplineContainer _stageSpline;/*【SplineのEditor拡張が必要です】*/

    [SerializeField, Tooltip("ループのスプライン")]
    private SplineContainer _loopSpline;/*【SplineのEditor拡張が必要です】*/

    //設定されたスプライン
    private SplineContainer _settingSpline;/*【SplineのEditor拡張が必要です】*/


    //移動中のスプライン状態
    private RootSpline _rootSpline = RootSpline.STAGE;
    enum RootSpline
    {

        STAGE,//ステージ
        LOOP,//ループ
    }
    #endregion


    #region 移動関連
    [Header("移動関連")]
    [SerializeField, Tooltip("ルート速度")]
    private float _rootSpeed;

    [SerializeField, Tooltip("速度の変更速度")]
    private float _changeingSpeed;

    //移動速度
    private float _moveSpeed;

    //変更後の速度
    private float _changeSpeed;

    //基準速度
    private float _defaultSpeed;

    //スプラインに沿って移動させる対象
    private Transform _moveTarget;

    //補間の割合(0~1の間を始点^終点で移動)
    private float _percentage;

    //前フレームのワールド位置
    private Vector3 _prevPos;

    //スプラインの終点
    private const int _endSpline = 1;

    //スプラインの開始
    private const int _startSpline = 0;

    //停止中
    private bool _isStop = false;

    #endregion



    //処理部-----------------------------------------------------------------------------------------------------------------------

    private void Start()
    {

        //ステージのスプラインを設定
        _settingSpline = _stageSpline;

        //スプラインに沿って移動させる対象
        _moveTarget = this.gameObject.transform;

        //移動速度をルート速度に設定
        _moveSpeed = _rootSpeed;
        _changeSpeed = _moveSpeed;

    }


    private void FixedUpdate()
    {

        //割合を時間で加算
        _percentage += Time.deltaTime * _moveSpeed;

        //移動中のスプライン状態
        switch (_rootSpline)
        {

            //ステージ
            case RootSpline.STAGE:

                //スプラインのデータ更新
                UpdateSpline();

                break;


            //ループ
            case RootSpline.LOOP:

                //スプラインをループ
                LoopSpline();

                break;

        }

        //速度の変更
        ChangeSpeed();

        //ベジェ曲線での移動と進行方向に角度を変更
        PlayerMovePosRotate();

    }



    //処理部-----------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 速度の変更
    /// </summary>
    private void ChangeSpeed()
    {

        if (_moveSpeed != _changeSpeed && !_isStop)
        {
            _moveSpeed = Mathf.MoveTowards(_moveSpeed, _changeSpeed, Time.deltaTime * _changeingSpeed);
        }

    }

    /// <summary>
    /// スプラインのデータ更新
    /// </summary>
    private void UpdateSpline()
    {

        //終点についたらループのスプラインに変更
        if (_percentage >= _endSpline)
        {

            //ループのスプラインを設定
            _settingSpline = _loopSpline;

            //補間の割合を初期化
            _percentage = _startSpline;

            //ループに遷移
            _rootSpline = RootSpline.LOOP;
        }
    }


    /// <summary>
    /// スプラインをループ
    /// </summary>
    private void LoopSpline()
    {

        //終点についたら始点に変更
        if (_percentage >= _endSpline)
        {

            //補間の割合を初期化
            _percentage = _startSpline;

        }
    }


    /// <summary>
    /// ベジェ曲線での移動と進行方向に角度を変更
    /// </summary>
    private void PlayerMovePosRotate()
    {

        // 計算した位置（ワールド座標）をターゲットに代入
        _moveTarget.position = _settingSpline.EvaluatePosition(_percentage);

        // 現在フレームのフレーム位置
        Vector3 position = _moveTarget.position;

        // 移動量を計算
        Vector3 moveVolume = position - _prevPos;

        // 次のUpdateで使うための前フレーム位置補完
        _prevPos = position;

        // 静止している状態だと、進行方向を特定できないため回転しない
        if (moveVolume == Vector3.zero) { return; }

        // 進行方向に角度を変更
        _moveTarget.rotation = Quaternion.LookRotation(moveVolume, Vector3.up);

    }

    /// <summary>
    /// 移動速度の変更(停止/移動)
    /// </summary>
    public void ChangeMoveSpeed(string name)
    {

        if (name == "移動")
        {

            //基準速度に変更
            _moveSpeed = _defaultSpeed;

            _isStop = false;
        }
        else if (name == "停止")
        {

            //速度の保管
            _defaultSpeed = _moveSpeed;

            //停止に変更
            _moveSpeed = 0;

            _isStop = true;
        }
    }

    /// <summary>
    /// 速度の変更
    /// </summary>
    /// <param name="speed"></param>
    public void ChangeSplineSpeed(float speed)
    {
        _changeSpeed = speed;
    }

}
