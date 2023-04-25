using UnityEngine;



/// <summary>
/// �G(�w�)�̋���
/// </summary>
public class EnemySpider : MonoBehaviour
{

    #region �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField, Tooltip("�v���C���[�̃��[�g�i�r")]
    RootNav _rootNav;

    [SerializeField, Tooltip("�v�[���Ǘ�")]
    PoolManager _poolManager;

    //�w偂̎�
    PoolController _poolController;

    //�J�����`��X�N���v�g
    EnemyCameraView _enemyCameraView;
    #endregion

    [SerializeField, Tooltip("�A�N�e�B�u�|�C���g")]
    private GameObject _movePos;

    private string _spiderBoll = "�w偂̎�";

    #region ����
    [Header("����")]
    [SerializeField, Tooltip("�v���C���[")]
    private GameObject _player;

    [SerializeField, Tooltip("�e���ˈʒu")]
    private GameObject _shotPos;

    [SerializeField, Tooltip("�v���C���[�Ƃ̋���")]
    private float _activeDistance;

    private GameObject _bulletObj;

    #endregion

    //�A�j���[�^�[
    private Animation _anim;

    #region ����
    [Header("����")]
    [SerializeField, Tooltip("�ړ�����̏�����܂ł̎���")]
    private float _activeFalseTime;

    //���Ԍv��
    private float _count = 0;
    #endregion

    Motion _motion = Motion.WAIT;
    enum Motion
    {

        WAIT,
        MOVE,
        ATTACK,
        DEATH

    }


    //������-------------------------------------------------------------------------------------------------

    private void Start()
    {

        //�����ݒ�
        Setting();

    }

    private void FixedUpdate()
    {

        //���Ԍv����A�N�e�B�ufalse
        TimeCount();

        switch (_motion)
        {

            case Motion.WAIT:

                //�ړ��J�n
                MoveStart();

                break;

            case Motion.MOVE:

                //�U���̊J�n
                AttackStart();

                break;


            case Motion.ATTACK:

                break;


            case Motion.DEATH:

                Death();

                break;

        }

    }


    //���\�b�h��--------------------------------------------------------------------------------------------

    //Start
    /// <summary>
    /// �����ݒ�
    /// </summary>
    private void Setting()
    {

        //�A�j���[�^�[�擾
        _anim = this.gameObject.GetComponent<Animation>();

        //�A�j���[�^�[false
        _anim.enabled = false;

        //�X�N���v�g�擾
        _enemyCameraView = this.gameObject.GetComponent<EnemyCameraView>();

        //�X�N���v�gfalse
        _enemyCameraView.enabled = false;

        //�v���C���[�̃v�[���Ǘ�����v�[���ݒ�
        for (int i = 0; i <= _poolManager._poolArrays.Length - 1; i++)
        {

            //�v�[�������擾
            string poolName = _poolManager._poolArrays[i]._poolName;

            //�X�N���v�g���擾
            PoolController poolScript = _poolManager._poolArrays[i]._poolControllers;

            //���O��v�̃v�[����T��������
            if (poolName == _spiderBoll)
            {

                _poolController = poolScript;
            }



        }

    }


    //���̑�
    /// <summary>
    /// ���Ԍv����A�N�e�B�ufalse
    /// </summary>
    private void TimeCount()
    {

        if (_motion != Motion.WAIT && _motion != Motion.MOVE)
        {

            //���Ԍv��
            _count += Time.deltaTime;

            //�v����
            if (_count >= _activeFalseTime)
            {

                _enemyCameraView.OutPlayerArray();

                //�A�N�e�B�ufalse
                this.gameObject.SetActive(false);

            }

        }

    }


    //�ړ�
    /// <summary>
    /// �ړ��J�n
    /// </summary>
    private void MoveStart()
    {

        //�ړ��|�C���g�ɒB������
        if (_movePos == _rootNav.NowPoint())
        {

            //�A�j���[�^�[true
            _anim.enabled = true;

            //�X�N���v�gtrue
            _enemyCameraView.enabled = true;

            //�ړ�Enum
            _motion = Motion.MOVE;

        }

    }


    //�U��
    /// <summary>
    /// �U���J�n
    /// </summary>
    private void AttackStart()
    {

        //�e�̈ʒu
        Vector3 thisPos = this.gameObject.transform.position;

        //�^�[�Q�b�g�̈ʒu
        Vector3 playerPos = _player.transform.position;

        //���̃|�C���g�Ƃ̋������v��
        float distans = Vector3.Distance(thisPos, playerPos);

        //�������߂Â�����
        if (distans <= _activeDistance)
        {

            //�U���A�j���[�V����
            _anim.CrossFade("attack");

            //�e���v�[������擾
            GameObject bullet = _poolController.GetObj();

            _bulletObj = bullet;

            //�X�N���v�g���擾
            BulletController bullCon = bullet.GetComponent<BulletController>();

            //�e���A�N�e�B�u�ɂ���
            bullet.SetActive(true);

            //�^�[�Q�b�g��e�ɐݒ�
            bullCon._targetObj = _player;

            //����Pos�̌�����ύX
            _shotPos.transform.LookAt(_player.transform);

            //�e�̔��ˈʒu��Player�̏e���ɐݒ�
            bullet.transform.position = _shotPos.transform.position;

            //�e�̔��ˈʒu��Player�̏e���ɐݒ�
            bullet.transform.rotation = _shotPos.transform.rotation;

            //�U��Enum
            _motion = Motion.ATTACK;

        }


    }


    //���S
    /// <summary>
    /// ���S
    /// </summary>
    private void Death()
    {
        transform.Translate(Vector3.down);
    }
    public void EnemyDeath()
    {

        //Active���̒e���폜
        if (_bulletObj != null) { _bulletObj.SetActive(false); }

        //���S�A�j���[�V����
        _anim.CrossFade("death");

        //���SEnum
        _motion = Motion.DEATH;

    }

}
