using UnityEngine;



/// <summary>
/// �C�x���g���̃J��������
/// </summary>
public class EventCameraController : MonoBehaviour
{
    [Header("�X�N���v�g")]
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



    [Header("�J�n")]
    [SerializeField, Tooltip("�J�n���̒����ʒu")]
    private Transform _lookStartPos;

    [SerializeField, Tooltip("�X�^�[�g�J�����̎n�_")]
    private Transform _gameStartStart;

    [SerializeField, Tooltip("�X�^�[�g�J�����̏I�_")]
    private Transform _gameStartEnd;

    [SerializeField, Tooltip("�J�����̒Ǐ]���x")]
    private float _moveSpeed;

    [SerializeField, Tooltip("�J�n�܂ł̎���")]
    private float _moveStartTime;

    [SerializeField, Tooltip("�I���܂ł̎���")]
    private float _moveEndTime;

    private float _moveCount;

    private Vector3 _cameraMovePos;


    [Header("�I��")]
    [SerializeField, Tooltip("�I�����̒����ʒu")]
    private Transform _lookEndPos;

    [SerializeField]
    private Transform _moveEndPos;

    private bool _isEndOne = false;

    //������------------------------------------------------------------------------------------------------------------------------

    private void Start()
    {

        //�����ʒu�̐ݒ�
        _cameraMovePos = _gameStartStart.localPosition;

    }

    private void LateUpdate()
    {

        switch (_motion)
        {

            case Motion.START:

                //���Ԃ̌v��
                _moveCount += Time.deltaTime;

                //���Ԍv����
                if (_moveCount >= _moveStartTime)
                {

                    //�Q�_�Ԃ̌v�Z
                    Vector3 differencePos = _anyUseMethod.MoveToWardsPercentage(_cameraMovePos, _gameStartEnd.localPosition);

                    //�Q�_�Ԃ̈ړ�
                    _cameraMovePos = _anyUseMethod.MoveToWardsVector3(_cameraMovePos, _gameStartEnd.localPosition, Time.deltaTime * _moveSpeed, differencePos);

                    //���Ԍv����
                    if(_cameraMovePos == _gameStartEnd.localPosition)
                    {

                        //�I��Enum�ɑJ��
                        _motion = Motion.END;

                        //�Q�[���J�n�̏I��
                        _gameStart.StartCamEnd();

                        //Active�I��
                        this.gameObject.SetActive(false);

                    }

                }

                //�ړ�����
                MoveProcess(_cameraMovePos, _lookStartPos);

                break;


            case Motion.END:

                //�I���C�x���g
                GameEnd();

                //�ړ�����
                MoveProcess(_moveEndPos.localPosition, _lookEndPos);

                break;        
        
        }

    }



    //���\�b�h��------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// �ړ�����
    /// </summary>
    private void MoveProcess(Vector3 move,Transform look)
    {

        //�J�����������v�Z���Đ��񑬓x���|�����
        this.gameObject.transform.localPosition = move;

        //�v���C���[�𒼎�
        this.gameObject.transform.LookAt(look, Vector3.up);

    }


    /// <summary>
    /// �Q�[���I��
    /// </summary>
    public void GameEnd()
    {
        if(!_isEndOne)
        {

            //�ړ�
            _moveSpline.ChangeMoveSpeed("�ړ�");

            //�I��UI
            _scoreManager.ScoreShowing();

            //�Q�[���N���ABGM
            _playerBgm.ClearBgm();

            _isEndOne = true;

        }

    }

}
