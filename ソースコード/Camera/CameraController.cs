using UnityEngine;


/// <summary>
/// �J��������p�X�N���v�g
/// </summary>
public class CameraController : MonoBehaviour
{

    #region �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField, Tooltip("�Q�[���̃C�x���g��Time.deltaTime���Ǘ�")]
    GameSystem _gameSystem;

    [SerializeField, Tooltip("�v���C���[�̃R���g���[���[")]
    PlayerController _playerController;
    #endregion


    #region �Ǐ]�ړ�
    [Header("�ړ�")]
    [SerializeField, Tooltip("�Ǐ]�Ώ�")]
    private Transform _targetPos;

    [SerializeField, Tooltip("�^�[�Q�b�g�ƃJ�����̗�������")]
    private float _toTargetDistance;

    //�^�[�Q�b�g�ƃJ�����̌��݂̋���
    private Vector3 _toTarget;

    [SerializeField, Tooltip("�J�����̒Ǐ]���x")]
    private float _followingSpeed;

    //�J�����̏����ʒu
    private Vector3 _defaultPos;
    #endregion


    #region ���͈ړ�
    [SerializeField, Tooltip("�ړ���̈ʒu")]
    private Vector2 _moveClamp;

    //�J�����̈ړ��ʒu
    private Vector3 _movePos;
    #endregion


    #region �p�x
    [Header("�p�x")]

    [SerializeField, Tooltip("�p�x�̈ړ���̍ő�l")]
    private Vector3 _rotateClamp;

    [SerializeField, Tooltip("�Ǐ]���̃J�����̉�]���x")]
    private float _followingRotateSpeed;

    //�Ǐ]���̃J�����̊p�x�̂݌v�Z
    private Quaternion _defaultRoatete;

    //�J�����p�x�̑��
    private Vector3 _cameraRotate;

    //Angle��Rotate�ɕϊ�
    private const int _angleToRotate = 180;
    #endregion


    #region ���
    [Header("���")]
    [SerializeField, Tooltip("������̃J�����ړ�")]
    private float _doageMovePos;

    //�����̈ʒu�v�Z
    private float _doagePos;
    #endregion



    //����-------------------------------------------------------------------------------------------------

    private void Start()
    {

        //�����l�̐ݒ�
        DefaultSetting();

    }


    private void FixedUpdate()
    {

        //�J�����p�x�̌v�Z
        CameraRotate();

        //�J�����̍��E�ړ��̌v�Z
        CameraMove();

        //�J�����̒Ǐ]�ړ��̌v�Z
        CameraFollowing();

    }


    void LateUpdate()
    {

        //�J�����̊p�x�ƈʒu���X�V
        UpdatePosRotate();

    }



    //private�̃��\�b�h�Q------------------------------------------------------------------------------------------------

    /// <summary>
    /// �����l�̐ݒ�
    /// </summary>
    private void DefaultSetting()
    {

        //�J�����̏����ʒu����ʒu�ɐݒ�
        this.gameObject.transform.position = _targetPos.TransformPoint(0, 0, _toTargetDistance);

        //�v���C���[�𒼎�
        this.gameObject.transform.LookAt(_targetPos, Vector3.up);

        //�J�����̊�p�x��ۊ�
        _defaultRoatete = this.gameObject.transform.rotation;

        //�J�����̊�ʒu��ۊ�
        _defaultPos = _targetPos.localPosition;

        //�J�����̈ʒu����ʒu�ɂ���
        _movePos = new Vector3(0, 0, _defaultPos.z);

    }


    /// <summary>
    /// �J�����p�x�̌v�Z
    /// </summary>
    private void CameraRotate()
    {

        //�Ǐ]�̃J�����p�x�v�Z-------------------------------------------------------------------
        //�Ǐ]���̃J�����̊p�x�̂݌v�Z
        _defaultRoatete = Quaternion.Slerp(_defaultRoatete, Quaternion.LookRotation(_targetPos.transform.position - transform.position).normalized, Time.deltaTime * _followingRotateSpeed);

        //Quaternion(4�����p)��Euler�p(3�����p)�ɕϊ�
        Vector3 defaultEuler = _defaultRoatete.eulerAngles;


        //���͂̃J�����p�x�v�Z-------------------------------------------------------------------        �@
        //�J�����̍ő�p�x�ɏ�L�̊�������Z
        Vector2 moveRotate = new Vector2(_rotateClamp.x * PlayerMovePercentage().x, _rotateClamp.y * PlayerMovePercentage().y);

        //�p�x��360�x���]���Ȃ��悤�ɂ���
        Vector2 rotateAngle = new Vector3(moveRotate.x / _angleToRotate, moveRotate.y / _angleToRotate);


        //�J�����̊p�x�v�Z------------------------------------------------------------------------
        //�Ǐ]�Ɠ��͂̃J�����p�x����A����p�x���v�Z
        _cameraRotate = new Vector3(defaultEuler.x - moveRotate.y, defaultEuler.y + moveRotate.x, defaultEuler.z);

    }


    /// <summary>
    /// �J�����̍��E�ړ��̌v�Z
    /// </summary>
    private void CameraMove()
    {

        //����0��������,�C�x���g���Ȃ牽�����Ȃ�
        if (Time.timeScale == 0 || _gameSystem._isEvent)
        {
            return;
        }

        //�J�����̍ő�ړ��ʂɏ�L�̊�������Z
        _movePos = new Vector2(_moveClamp.x * PlayerMovePercentage().x, _moveClamp.y * PlayerMovePercentage().y);

    }


    /// <summary>
    /// �v���C���[�̎��ۂ̈ړ��ʂƍő�̈ړ��ʂ���A�ǂꂾ���̊����ړ����������v�Z
    /// </summary>
    private Vector2 PlayerMovePercentage()
    {

        //�v���C���[�̎��ۂ̈ړ��ʂƍő�̈ړ��ʂ���A�ǂꂾ���̊����ړ����������v�Z
        Vector2 playerMovePercentage = new Vector2(_playerController.InputMove().x / _playerController.MoveClamp().x,
                                                   _playerController.InputMove().y / _playerController.MoveClamp().y);

        return playerMovePercentage;

    }


    /// <summary>
    /// �J�����̒Ǐ]�ړ��̌v�Z
    /// </summary>
    private void CameraFollowing()
    {

        //�^�[�Q�b�g�ʒu�Ɖ�](0.1�Âv��)���������ʒu���v�Z
        _toTarget = _targetPos.position - Vector3.Lerp(transform.localPosition, _targetPos.TransformPoint(0, 0, _toTargetDistance), 0.1f);

        //���K�����Ċ��炩��
        _toTarget.Normalize();

    }


    /// <summary>
    /// ������̈ʒu�v�Z
    /// </summary>
    private void DoageCalculate()
    {

        //������̈ʒu�v�Z
        _doagePos = _movePos.x + _doageMovePos * _playerController.DoageDirection();

    }


    /// <summary>
    /// �J�����̊p�x�ƈʒu���X�V
    /// </summary>
    private void UpdatePosRotate()
    {

        //�J�����̊p�x�ύX
        this.gameObject.transform.eulerAngles = _cameraRotate;

        //�J�����ړ���
        _targetPos.localPosition = new Vector3(_movePos.x, _movePos.y, _defaultPos.z);

        //�J�����̒Ǐ]�ړ��̌v�Z
        this.gameObject.transform.localPosition = _targetPos.position - _toTarget * _followingSpeed;

    }

}
