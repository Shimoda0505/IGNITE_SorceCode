using UnityEngine;



/// <summary>
/// �Q�[���I�����̃J��������
/// </summary>
public class GameEndCam : MonoBehaviour
{

    [SerializeField]
    private GameObject eventCamera;

    [SerializeField, Tooltip("�v���C���[")]
    private GameObject _playerObj;

    [SerializeField]
    private GameObject _targetObj;

    private bool _isResult = false;

    private void LateUpdate()
    {

        if(_isResult)
        {
            //�J�����������v�Z���Đ��񑬓x���|�����
            eventCamera.gameObject.transform.localPosition = _targetObj.transform.localPosition;

            //�v���C���[�𒼎�
            eventCamera.gameObject.transform.LookAt(_playerObj.transform, Vector3.up);


        }


    }

    /// <summary>
    /// Result�J�����ɕύX
    /// </summary>
    public void ResultCam()
    {


        eventCamera.SetActive(true);

        _isResult = true;

    }

}
