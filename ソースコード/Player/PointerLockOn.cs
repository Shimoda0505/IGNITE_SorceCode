using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �J�[�\���ɏd�Ȃ��Ă���G�����b�N�I��
/// </summary>
public class PointerLockOn : MonoBehaviour
{

    #region �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField, Tooltip("�Q�[���̃C�x���g��Time.deltaTime���Ǘ�")]
    GameSystem _gameSystem;

    [SerializeField, Tooltip("�G�ƃv���C���[�̌q�����Ǘ�")]
    PlayerLinkEnemy _playerLinkEnemy;

    [SerializeField, Tooltip("�v���C���[�̃v�[���Ǘ�")]
    PoolManager _poolManager;

    [SerializeField]
    PointerChange _pointerChange;

    //�v���C���[�̉��֘A
    PlayerSE _playerSe;

    //�v���C���[�̃X�e�[�^�X
    PlayerStatus _playerStatus;

    //�v���C���[�R���g���[���[
    PlayerController _playerController;

    //�΋��v�[��
    PoolController _fireBollPool;
    private string _fireBoll = "�΋�";

    //�΋�(�E���g)�v�[��
    PoolController _fireBollUltPool;
    private string _fireBollUlt = "�΋�(�E���g)";

    //�����v�[��
    PoolController _explosionPool;
    private string _explosion = "����";

    //����(����)�v�[��
    PoolController _explosionUltPool;
    private string _explosionUlt = "����(�E���g)";

    //���͎���
    _InputSystemController _inputController;
    #endregion


    #region �|�C���^�[�֘A
    [Header("�|�C���^�[�֘A")]
    [SerializeField, Tooltip("�|�C���^�[�̃��b�N�I���͈�")]
    private Vector3 _pointerClamp;

    [SerializeField, Tooltip("�|�C���^�[�̃��b�N�I���͈�(�E���g)")]
    private float _pointerClampUlt;

    //�|�C���^�[�̃��b�N�I���͈͂̑��
    private float _pointerClampAssignment;

    [SerializeField, Tooltip("�J�[�\���̐e�I�u�W�F�N�g")]
    private GameObject _cursorObj;

    private List<GameObject> _cursorObjs = new List<GameObject>();

    //Ui���W
    private RectTransform _rectTr;

    //Ui���W�̍ő�l
    private Vector2 _maxRectTr = new Vector2(800, 450);

    //�J�������W�̒l��(-0.5~0.5)����(0~1)�ɕϊ�
    private const float _valueChange = 0.5f;
    #endregion


    #region �����֘A
    [Header("����")]
    [SerializeField, Tooltip("�v���C���[")]
    private GameObject _playerObj;
    #endregion


    #region �e�֘A
    [Header("�e�֘A")]
    [SerializeField, Tooltip("�e�̔��ˈʒu")]
    private Transform _shotPos;

    [SerializeField, Tooltip("���ˈʒu��ύX���鎞�͈̔�")]
    private Vector2[] _shotRotates;

    //�e�̔��ˊp�x�̔ԍ�
    private int _shotRotateNumber = 0;
    #endregion


    [SerializeField, Tooltip("�A�˂̃C���^�[�o��")]
    private float _shotIntervalTime;
    private float _shotIntervalCount = 0;

    //���b�N�I���̏��
    private LockOnMotion _lockOnMotion = LockOnMotion.LOCK_ON;
    private enum LockOnMotion
    {
        WAIT,
        LOCK_ON,//���b�N�I��
        BOLL_SHOT//�΋��̔���
    }



    //����-------------------------------------------------------------------------------------------------

    private void Start()
    {

        //�����ݒ�
        Setting();
    }


    private void Update()
    {

        //����0��������,�C�x���g���Ȃ牽�����Ȃ�
        if (Time.timeScale == 0 || _gameSystem._isEvent || _playerController._playerMotion == PlayerController.PlayerMotion.Death) { return; }


        switch (_lockOnMotion)
        {

            //���b�N�I��
            case LockOnMotion.LOCK_ON:

                if (_playerController._playerMotion == PlayerController.PlayerMotion.Fly)
                {

                    //�E�g���K�[ or�@���g���K�[����������/�΋��̔���
                    if (_inputController.LeftTriggerDown() || _inputController.RightTriggerDown()) { _lockOnMotion = LockOnMotion.BOLL_SHOT; }

                }

                break;


            case LockOnMotion.BOLL_SHOT:

                break;


            case LockOnMotion.WAIT:

                //���Ԍv��
                _shotIntervalCount += Time.deltaTime;
                if (_shotIntervalCount >= _shotIntervalTime)
                {

                    //���Ԃ̏�����
                    _shotIntervalCount = 0;

                    _lockOnMotion = LockOnMotion.LOCK_ON;

                }

                break;

        }

    }


    private void FixedUpdate()
    {

        //����0��������,�C�x���g���Ȃ牽�����Ȃ�
        if (Time.timeScale == 0 || _gameSystem._isEvent || _playerController._playerMotion == PlayerController.PlayerMotion.Death) { return; }


        switch (_lockOnMotion)
        {

            //���b�N�I��
            case LockOnMotion.LOCK_ON:

                //�z�������|�C���^�[�Əd�Ȃ��Ă���I�u�W�F�N�g��T��
                if (_playerController._playerMotion == PlayerController.PlayerMotion.Fly) { SearchArray(); }

                break;


            //�΋��̔���
            case LockOnMotion.BOLL_SHOT:

                //�e�̔��˂Ə����ݒ�
                ShotBoll();

                //���b�N�I��Enum�ɖ߂�
                _lockOnMotion = LockOnMotion.WAIT;

                break;

        }

    }



    //public�̃��\�b�h�Q-----------------------------------------------------------------------

    /// <summary>
    /// �����v�[������Ăяo��
    /// </summary>
    public GameObject ExplosionPool()
    {
        if (_playerController.IsUlt()) { return _explosionUltPool.GetObj(); }//����
        else if (!_playerController.IsUlt()) { return _explosionPool.GetObj(); }//����(�E���g)

        return null;
    }

    /// <summary>
    /// �_���[�W���󂯂��Ƃ��Ƀ��b�N�I���z���T�����ă��b�N�I������
    /// </summary>
    public void DamageResetting()
    {

        //�z���S�T��
        for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
        {

            //i�Ԃ�List��ݒ�
            PlayerLinkEnemy.Targets target = _playerLinkEnemy._targetList[i];

            //�^�[�Q�b�g�����b�N�I����  && �΋����ݒ肳��Ă��Ȃ�
            if (target._isLock && target._fireBoll == null)
            {

                //���b�N�I���̉���
                target._isLock = false;

                //���b�N�I���J�[�\���̃^�[�Q�b�g��Null
                target._lockOnCursor.GetComponent<CursorController>()._target = null;

            }

        }

    }

    /// <summary>
    /// ���b�N�I���͈͂̕ύX
    /// </summary>
    public void ChangePointerClamp(string name)
    {

        if (name == "1")
        {
            _pointerClampAssignment = _pointerClamp.x;

            _pointerChange.Change1();
        }
        else if (name == "2")
        {
            _pointerClampAssignment = _pointerClamp.y;

            _pointerChange.Change2();

        }
        else if (name == "3")
        {
            _pointerClampAssignment = _pointerClamp.z;

            _pointerChange.Change3();

        }

        if (name == "�E���g")
        {
            _pointerClampAssignment = _pointerClampUlt;

            _pointerChange.Change1();

        }
    }



    //private�̃��\�b�h�Q-----------------------------------------------------------------------

    /// <summary>
    /// �����ݒ�
    /// </summary>
    private void Setting()
    {

        //�v���C���[�̃X�e�[�^�X
        _playerStatus = _playerObj.GetComponent<PlayerStatus>();

        //�v���C���[�R���g���[���[
        _playerController = _playerObj.GetComponent<PlayerController>();

        //�v���C���[SE
        _playerSe = _playerObj.GetComponent<PlayerSE>();


        //Ui���W
        _rectTr = this.gameObject.GetComponent<RectTransform>();

        //�J�[�\����S�Ĕz��Ɋi�[
        for (int i = 0; i <= _cursorObj.transform.childCount - 1; i++)
        {
            _cursorObjs.Add(_cursorObj.transform.GetChild(i).gameObject);

            _cursorObjs[i].SetActive(false);
        }


        //�v���C���[�̃v�[���Ǘ�����v�[���ݒ�
        for (int i = 0; i <= _poolManager._poolArrays.Length - 1; i++)
        {

            //�v�[�������擾
            string poolName = _poolManager._poolArrays[i]._poolName;

            //�X�N���v�g���擾
            PoolController poolScript = _poolManager._poolArrays[i]._poolControllers;

            //���O��v�̃v�[����T��������
            if (poolName == _fireBoll) { _fireBollPool = poolScript; }//�΋�
            else if (poolName == _fireBollUlt) { _fireBollUltPool = poolScript; }//�΋�(�E���g)
            else if (poolName == _explosion) { _explosionPool = poolScript; }//����
            else if (poolName == _explosionUlt) { _explosionUltPool = poolScript; }//����(�E���g)

        }

        //���b�N�I���͈͂̏����ݒ�
        _pointerClampAssignment = _pointerClamp.x;
    }

    /// <summary>
    /// �z�������|�C���^�[�Əd�Ȃ��Ă���I�u�W�F�N�g��T��
    /// </summary>
    private void SearchArray()
    {

        //���b�N�I�������ő�ɒB�����炱��ȏ㏈�����Ȃ�
        if (_playerStatus.MaxLockCount() || _playerController._playerMotion == PlayerController.PlayerMotion.Damage)
        {
            return;
        }

        //�G�i�[�̔z���T��
        for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
        {

            //��ʓ��z��̃^�[�Q�b�g���擾
            PlayerLinkEnemy.Targets targetObj = _playerLinkEnemy._targetList[i];

            //�^�[�Q�b�g�Ƃ̋���
            float dis = Vector3.Distance(_playerObj.transform.position, targetObj._targetObj.transform.position);

            //�G�l�~�[�����b�N�I�����ł͂Ȃ��Ȃ� $$ �����ȓ��Ȃ�
            if (!targetObj._isLock)
            {

                //�G�l�~�[�̍��W���A�J����View���W(0~1,0~1)�ɕϊ�
                Vector2 enemyPos = Camera.main.WorldToViewportPoint(targetObj._targetObj.transform.position);

                //�|�C���^�[�̃J����View���W(0~1,0~1)
                //Vector(0~800 , 0~460)��(0~1 , 0~1)�ɕϊ�
                Vector2 uiPos = new Vector2(_rectTr.localPosition.x / _maxRectTr.x + _valueChange,
                                            _rectTr.localPosition.y / _maxRectTr.y + _valueChange);

                //�|�C���^�[�ƃG�l�~�[�̋������v��
                float distans = Vector2.Distance(uiPos, enemyPos);


                //�͈͓��ɃG�l�~�[�����邩�ǂ���
                if (-_pointerClampAssignment <= distans && distans <= _pointerClampAssignment)
                {

                    //���b�N�I���������Z
                    _playerStatus.LockPrice("���Z");

                    //�^�[�Q�b�g�����b�N�I����Ԃɂ���
                    targetObj._isLock = true;

                    //���b�N�I������炷
                    _playerSe.RockOnSe();

                    for (int j = 0; j <= _cursorObjs.Count - 1; j++)
                    {

                        if (_cursorObjs[j].activeSelf == false)
                        {

                            //�J�[�\�����A�N�e�B�u�ɂ���
                            _cursorObjs[j].SetActive(true);

                            //�J�[�\�����^�[�Q�b�g��ݒ�
                            _cursorObjs[j].GetComponent<CursorController>()._target = targetObj._targetObj;

                            //�J�[�\����z��Ɋi�[
                            targetObj._lockOnCursor = _cursorObjs[j];

                            break;
                        }

                    }


                }
            }

        }

    }

    /// <summary>
    /// �e�̔��˂Ə����ݒ�
    /// </summary>
    private void ShotBoll()
    {

        //�z���S�T��
        for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
        {

            //�z��̃^�[�Q�b�g���擾
            PlayerLinkEnemy.Targets targetObj = _playerLinkEnemy._targetList[i];

            //���b�N�I�����̃^�[�Q�b�g��T��
            if (targetObj._isLock && targetObj._fireBoll == null)
            {

                //�J�[�\���̃J���[��e���ˎ��J���[�ɕύX
                targetObj._lockOnCursor.GetComponent<PointerImage>().ShotColorChange();

                //�΋��̏�����
                GameObject fireBoll = null;

                //�e���v�[������擾
                if (!_playerController.IsUlt()) { fireBoll = _fireBollPool.GetObj(); }//�΋�
                else if (_playerController.IsUlt()) { fireBoll = _fireBollUltPool.GetObj(); }//�΋�(�E���g)


                //�X�N���v�g���擾
                FireBollController fireBollController = fireBoll.GetComponent<FireBollController>();

                //�e��z��Ɋi�[
                targetObj._fireBoll = fireBoll;

                //�e���A�N�e�B�u�ɂ���
                fireBoll.SetActive(true);

                //�e�̏����ݒ�
                fireBollController.TargetStore(targetObj._targetObj, _playerStatus.LockCount(), this.gameObject.GetComponent<PointerLockOn>());

                //�e�̔��ˈʒu��Player�̏e���ɐݒ�
                fireBoll.transform.position = _shotPos.position;

                //�e�̔��ˈʒu��Player�̏e���ɐݒ�
                fireBoll.transform.rotation = _shotPos.rotation;


                //�e��
                _playerSe.FireBollSe();

                //���̔��ˊp�x
                _shotRotateNumber++;
            }
        }

        //���ˊp�x�̔ԍ���������
        _shotRotateNumber = 0;

        //�e�̔��ˊp�x��������
        _shotPos.rotation = new Quaternion(0, 0, 0, 0);

        //���b�N�I�����̏�����
        _playerStatus.LockPrice("������");

    }


}















