using UnityEngine;



/// <summary>
/// �v���C���[�̒e�����Ǘ�
/// </summary>
public class FireBollController : MonoBehaviour
{

    #region �擾�֘A
    //�Ǐ]����Ώ�
    private GameObject _targetObj;

    //���b�N�I���̃X�N���v�g
    PointerLockOn _pointerLockOn;

    //�G�i�[�̔z��
    PlayerLinkEnemy _playerLinkEnemy;

    //�v���C���[�̃X�e�[�^�X
    PlayerStatus _playerStatus;

    ScoreManager _scoreManager;
    #endregion


    #region �ړ��֘A
    [SerializeField, Tooltip("�e��")]
    private float _moveSpeed;

    [SerializeField, Tooltip("���ˎ��̐��񑬓x")]
    private float rotateSpeedStart;

    [SerializeField, Tooltip("���񑬓x")]
    private float rotateSpeed;

    private float rotateSpeedIn;
    #endregion


    #region ���Ԋ֘A
    [SerializeField, Tooltip("���b�N�I���Ώۂ����Ȃ����̏�����܂ł̎���")]
    private float _disappearTime;

    //���b�N�I���Ώۂ����Ȃ����̏�����܂ł̎��Ԃ��v��
    private float _disappearCount;

    [SerializeField, Tooltip("���񑬓x�؂�ւ�莞��")]
    private float _changeTime;

    //���񑬓x�؂�ւ�莞�Ԃ��v��
    private float _changeCount = 0;
    #endregion

    //�΋��̉Η�
    private const int ATTACK_DAMAGE = 100;
    //���b�N�I����
    private int _lockOnCount;

    [SerializeField, Tooltip("�e�̓����蔻��͈�")]
    private float _hitClamp;

    //�Ǐ]�Ώۂ����邩�ǂ���
    public Motion _motion = Motion.HOMING;
    public enum Motion
    {

        HOMING,//�Ǐ]�ړ�
        LINE//�����ړ�

    }



    //����-------------------------------------------------------------------------------------------------

    void Start()
    {
        //�v���C���[�̉w�Ǘ��z����擾
        _playerLinkEnemy = GameObject.FindGameObjectWithTag("PlayerArray").GetComponent<PlayerLinkEnemy>();

        //�v���C���[�̃X�e�[�^�X���擾
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        _scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();

    }

    private void FixedUpdate()
    {

        switch (_motion)
        {

            case Motion.HOMING:

                //�e�̈ړ�(�Ǐ])
                HomingMoveObj();

                break;


            case Motion.LINE:

                //�e�̈ړ�(����)
                LineMoveobj();

                break;

        }

        //�^�[�Q�b�g�Ƃ̋������v��
        TargetDistance();

    }



    //public�̃��\�b�h�Q-------------------------------------------------------------------------------------------------

    /// <summary>
    /// �e�̏����ݒ�
    /// </summary>
    public void TargetStore(GameObject target, int count, PointerLockOn pointer)
    {

        //�e�̃^�[�Q�b�g��ݒ�
        _targetObj = target;

        //�^�[�Q�b�g�����i�[
        _lockOnCount = count;

        _pointerLockOn = pointer;
    }

    /// <summary>
    /// �e�̋�����ύX
    /// </summary>
    public void ChangeEnum() { _motion = Motion.LINE; }

    /// <summary>
    /// �^�[�Q�b�g�̏�����
    /// </summary>
    public void TargetNull() { _targetObj = null; }



    //private�̃��\�b�h�Q---------------------------------------------------------------------------------------------------

    //�z�[�~���O
    /// <summary>
    /// �e�̈ړ�(�Ǐ])
    /// </summary>
    private void HomingMoveObj()
    {

        //�^�[�Q�b�g��Null�ɂȂ�����
        if (_targetObj == null) { _motion = Motion.LINE; return; }

        //�^�[�Q�b�g�ƒe�̋���
        Vector3 targetDirection = _targetObj.transform.position - this.gameObject.transform.position;

        //�n�_,�I�_,���x * ����,�U��
        Vector3 newDirection = Vector3.RotateTowards(this.gameObject.transform.forward, targetDirection, RotateSpeed() * Time.deltaTime, 0f);

        //�O�������ɒ��i
        this.gameObject.transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);

        //�^�[�Q�b�g�̕����Ɍ�����ς���
        this.gameObject.transform.rotation = Quaternion.LookRotation(newDirection);

    }

    /// <summary>
    /// �Ǐ]���x
    /// </summary>
    private float RotateSpeed()
    {

        if (_lockOnCount >= 2)
        {
            _changeCount += Time.deltaTime;

            if (_changeCount <= _changeTime) { return rotateSpeedStart; }
            else { return rotateSpeed; }

        }

        return rotateSpeed;

    }


    //�z�[�~���O�Ȃ�
    /// <summary>
    /// �e�̈ړ�(����)
    /// </summary>
    private void LineMoveobj()
    {
        //�O�������ɒ��i
        this.gameObject.transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);

        //_disappearTime�̎��Ԍv��
        _disappearCount += Time.deltaTime;

        //_disappearTime�̎��Ԃ��߂����Ƃ�
        if (_disappearCount >= _disappearTime)
        {

            //������
            Resetting();

            //_disappearTime�̎��Ԍv��(������)
            _disappearCount = 0;

            return;

        }

    }


    //�e�̏Փˎ�
    /// <summary>
    /// �^�[�Q�b�g�Ƃ̋������v���ƃq�b�g���o
    /// </summary>
    private void TargetDistance()
    {

        //�^�[�Q�b�g���Ȃ��Ȃ珈�����Ȃ�
        if (_targetObj == null) { return; }

        //�e�̈ʒu
        Vector3 thisPos = this.gameObject.transform.position;

        //�^�[�Q�b�g�̈ʒu
        Vector3 enemyPos = _targetObj.transform.position;

        //���̃|�C���g�Ƃ̋������v��
        float distans = Vector3.Distance(thisPos, enemyPos);

        //�e�͈̔͂��^�[�Q�b�g�͈͓̔��ɓ�������
        if (-_hitClamp <= distans && distans <= _hitClamp)
        {

            //BurretPool����e���Ăяo��
            GameObject explosionObj = _pointerLockOn.ExplosionPool();

            //�����G�t�F�N�g�̈ʒu
            Explosion(explosionObj);

            //�G�Ƀq�b�g�������G���̃��\�b�h�Ăяo��###############################################
            HitEnemy();

            //������
            Resetting();

        }

    }

    /// <summary>
    /// �����G�t�F�N�g�̌Ăяo��
    /// </summary>
    private void Explosion(GameObject explosionObj)
    {

        //�������A�N�e�B�u
        explosionObj.SetActive(true);

        //�e�̔��ˈʒu��Player�̏e���ɐݒ�
        explosionObj.transform.localPosition = this.gameObject.transform.position;

        //������
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSE>().ExplosionMiniSe();

    }

    /// <summary>
    /// ���M��T��
    /// </summary>
    public void HitEnemy()
    {

        //�^�O��Enemy�Ȃ�
        if (_targetObj.tag == "Enemy")
        {

            for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
            {

                //i�Ԃ�List��ݒ�
                PlayerLinkEnemy.Targets target = _playerLinkEnemy._targetList[i];

                //�^�[�Q�b�g�Ȃ�
                if (target._fireBoll == this.gameObject)
                {

                    //�J�[�\��������
                    target._lockOnCursor.GetComponent<CursorController>()._target = null;

                    //i�Ԃ̔z��v�f���폜
                    _playerLinkEnemy._targetList.RemoveAt(i);

                    break;
                }

            }

            //�`�F�C�����̉��Z
            _playerStatus.ChainAddition();

            //���j���̉��Z
            _scoreManager.SmashEnemyCount(_lockOnCount);

        }

        //�^�O��Boss�Ȃ�
        else if (_targetObj.tag == "Boss")
        {

            for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
            {

                //i�Ԃ�List��ݒ�
                PlayerLinkEnemy.Targets target = _playerLinkEnemy._targetList[i];

                //�^�[�Q�b�g�Ȃ�
                if (target._fireBoll == this.gameObject)
                {

                    //�{�X�ɉ΋����̃_���[�W
                    target._targetObj.transform.root.gameObject.GetComponent<BossStatus>().BossDamage(ATTACK_DAMAGE);

                    //�J�[�\��������
                    target._lockOnCursor.GetComponent<CursorController>()._target = null;

                    //���b�N��Ԃ̉���
                    target._isLock = false;

                    //�����N�̃��b�N�I����Ԃ�����
                    if (target._targetObj.GetComponent<EnemyCameraView>())
                    {
                        target._targetObj.GetComponent<EnemyCameraView>().IsLockFalse();

                        //i�Ԃ̔z��v�f���폜
                        _playerLinkEnemy._targetList.RemoveAt(i);

                    }

                    break;
                }

            }

            //�`�F�C�����̉��Z
            _playerStatus.ChainAddition();

        }

    }

    /// <summary>
    /// ������
    /// </summary>
    private void Resetting()
    {

        //�^�[�Q�[�b�g������
        _targetObj = null;

        //���Ԍv��������
        _changeCount = 0;

        //���b�N�I����������
        _lockOnCount = 0;

        //�z�[�~���OEnum
        _motion = Motion.HOMING;

        //�e�̈ʒu��Ǐ]����Ώۂ�BurretPool�^�O�Ŏw��
        this.gameObject.transform.position = GameObject.FindGameObjectWithTag("PlayerArray").transform.position;

        //�A�N�e�B�u�I��
        this.gameObject.SetActive(false);

    }


    /// <summary>
    /// �M�Y���\��
    /// </summary>
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(this.gameObject.transform.position, _hitClamp);
    }

}
