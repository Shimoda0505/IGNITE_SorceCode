using UnityEngine;



/// <summary>
/// �v���C���[�̒e�����𐧌�
/// </summary>
public class BulletController : MonoBehaviour
{
    #region �擾�֘A
    //�Ǐ]����Ώ�
    public GameObject _targetObj;

    //�v���C���[�̃X�e�[�^�X
    PlayerStatus _playerStatus;

    [SerializeField, Tooltip("�����ʒu")]
    private GameObject _defPos;
    #endregion


    #region �ړ��֘A
    [SerializeField, Tooltip("�e��")]
    private float _moveSpeed;

    [SerializeField, Tooltip("���񑬓x")]
    private float _rotateSpeed;
    #endregion


    #region ���Ԋ֘A
    //������܂ł̎���
    [SerializeField] private float _time = default;

    //���Ԍv��
    private float _count = 0;
    #endregion

    //���̉Η�
    private const int ATTACK_DAMAGE = 100;

    [SerializeField, Tooltip("�e�̓����蔻��͈�")]
    private float _hitClamp;



    //����-------------------------------------------------------------------------------------------------

    void Awake()
    {
        //�v���C���[�̃X�e�[�^�X���擾
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

    }

    private void FixedUpdate()
    {
        if (_targetObj != null)
        {
            //�e�̈ړ�(�Ǐ])
            HomingMoveObj();

            //�^�[�Q�b�g�Ƃ̋������v��
            TargetDistance();

            TimeCount();
        }
    }



    //public�̃��\�b�h�Q-----------------------------------------------------------------------

    /// <summary>
    /// �e�̏����ݒ�
    /// </summary>
    public void TargetStore(GameObject target, int count)
    {

        //�e�̃^�[�Q�b�g��ݒ�
        _targetObj = target;

    }


    //private�̃��\�b�h�Q-----------------------------------------------------------------------

    //�z�[�~���O
    /// <summary>
    /// �e�̈ړ�(�Ǐ])
    /// </summary>
    private void HomingMoveObj()
    {
        //�^�[�Q�b�g�ƒe�̋���
        Vector3 targetDirection = _targetObj.transform.position - this.gameObject.transform.position;

        //�n�_,�I�_,���x * ����,�U��
        Vector3 newDirection = Vector3.RotateTowards(this.gameObject.transform.forward, targetDirection, _rotateSpeed * Time.deltaTime, 0f);

        //�O�������ɒ��i
        this.gameObject.transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);

        //�^�[�Q�b�g�̕����Ɍ�����ς���
        this.gameObject.transform.rotation = Quaternion.LookRotation(newDirection);

    }


    /// <summary>
    /// �^�[�Q�b�g�Ƃ̋������v���ƃq�b�g���o
    /// </summary>
    private void TargetDistance()
    {

        //�e�̈ʒu
        Vector3 thisPos = this.gameObject.transform.position;

        //�^�[�Q�b�g�̈ʒu
        Vector3 enemyPos = _targetObj.transform.position;

        //���̃|�C���g�Ƃ̋������v��
        float distans = Vector3.Distance(thisPos, enemyPos);

        //�e�͈̔͂��^�[�Q�b�g�͈͓̔��ɓ�������
        if (-_hitClamp <= distans && distans <= _hitClamp)
        {

            //�G�Ƀq�b�g�������G���̃��\�b�h�Ăяo��
            HitEnemy();

            //������
            Resetting();

        }

    }

    /// <summary>
    /// ���M��T��
    /// </summary>
    public void HitEnemy()
    {

        //�v���C���[�Ƀ_���[�W
        _playerStatus.Hit(ATTACK_DAMAGE);

    }

    /// <summary>
    /// ������
    /// </summary>
    private void Resetting()
    {

        _count = 0;

        //�^�[�Q�[�b�g������
        _targetObj = null;

        //�e�̈ʒu��������
        this.gameObject.transform.position = default;

        //�A�N�e�B�u�I��
        this.gameObject.SetActive(false);

    }

    /// <summary>
    /// ���Ԍv��
    /// </summary>
    private void TimeCount()
    {

        //���Ԍv��
        _count += Time.deltaTime;

        //���Ԍv����
        if (_count >= _time)
        {
            Resetting();
        }

    }


    /// <summary>
    /// �M�Y���\��
    /// </summary>
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(this.gameObject.transform.position, _hitClamp);
    }

}
