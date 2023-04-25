using UnityEngine;



/// <summary>
/// ゲーム終了時のカメラ制御
/// </summary>
public class GameEndCam : MonoBehaviour
{

    [SerializeField]
    private GameObject eventCamera;

    [SerializeField, Tooltip("プレイヤー")]
    private GameObject _playerObj;

    [SerializeField]
    private GameObject _targetObj;

    private bool _isResult = false;

    private void LateUpdate()
    {

        if(_isResult)
        {
            //カメラ距離を計算して旋回速度を掛け代入
            eventCamera.gameObject.transform.localPosition = _targetObj.transform.localPosition;

            //プレイヤーを直視
            eventCamera.gameObject.transform.LookAt(_playerObj.transform, Vector3.up);


        }


    }

    /// <summary>
    /// Resultカメラに変更
    /// </summary>
    public void ResultCam()
    {


        eventCamera.SetActive(true);

        _isResult = true;

    }

}
