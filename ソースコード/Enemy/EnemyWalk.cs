using UnityEngine;


/// <summary>
/// �G(�n�ʕ��s�֘A)�̋���
/// </summary>
public class EnemyWalk : MonoBehaviour
{

    #region �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField, Tooltip("�ړ��̃��[�g�i�r")]
    RootNav _rootNav;

    //�J�����r���[
    EnemyCameraView _enemyCameraView;
    #endregion

    #region �ʒu
    [Header("�ʒu")]
    [SerializeField, Tooltip("�o���|�C���g")]
    private GameObject _popPos;

    [SerializeField, Tooltip("�n�_")]
    private Transform _firstPos;

    [SerializeField, Tooltip("�I�_")]
    private Transform _endPos;
    #endregion

    #region �ړ�
    [Header("�ړ�")]
    [SerializeField, Tooltip("�ړ����x")]
    private float _moveSpeed;

    [SerializeField, Tooltip("�ړ��I������")]
    private float _endDistance;
    #endregion

    #region ����
    [Header("����")]
    [SerializeField, Tooltip("�ړ��J�n����")]
    private float _startTime;

    [SerializeField, Tooltip("���񂾂��Ə�����܂ł̎���")]
    private float _endTime;

    //���Ԍv��
    private float _count = 0;
    #endregion


    //�A�j���[�V����
    Animator _anim;

    Motion _motion = Motion.Wait;
    enum Motion
    {
        Wait,
        Move,
        Death
    }




    //������----------------------------------------------------------------------------------------------------------------

    private void Start()
    {

        //�J�����r���[
        _enemyCameraView = this.gameObject.GetComponent<EnemyCameraView>();

        //�J�����r���[false
        _enemyCameraView.enabled = false;

        //�A�j���[�V����
        _anim = this.gameObject.GetComponent<Animator>();

        //�A�j���[�V����false
        _anim.enabled = false;

        //�ړ��J�n�ʒu�Ɉړ�
        this.gameObject.transform.position = _firstPos.position;

    }


    private void FixedUpdate()
    {

        switch (_motion)
        {

            case Motion.Wait:

                Wait();

                break;


            case Motion.Move:

                Move();

                break;


            case Motion.Death:

                Death();

                break;

        }

    }



    //���\�b�h��----------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// �ҋ@
    /// </summary>
    private void Wait()
    {

        if (_rootNav.NowPoint() == _popPos)
        {

            //���Ԍv��
            _count += Time.deltaTime;

            //�v����
            if (_count >= _startTime)
            {

                //���ԏ�����
                _count = 0;

                //�A�j���[�V����true
                _anim.enabled = true;

                //�J�����r���[true
                _enemyCameraView.enabled = true;

                //�ړ�Enum
                _motion = Motion.Move;

            }

        }

    }

    /// <summary>
    /// �ړ�
    /// </summary>
    private void Move()
    {

        //�ړ�����������
        this.gameObject.transform.LookAt(_endPos);

        //�O�������Ɉړ�
        this.gameObject.transform.position += this.gameObject.transform.forward * _moveSpeed;

        //�I�_�Ƃ̋���
        float _endPosDirection = (_endPos.position - transform.position).magnitude;

        //�I�_�ɋ߂Â�����
        if (_endPosDirection <= _endDistance)
        {

            //�A�N�e�B�ufalse
            this.gameObject.SetActive(false);

        }

    }

    /// <summary>
    /// ���S
    /// </summary>
    private void Death()
    {
        //���Ԍv��
        _count += Time.deltaTime;

        //�v����
        if (_endTime <= _count)
        {

            //�A�N�e�B�ufalse
            this.gameObject.SetActive(false);

        }

    }

    /// <summary>
    /// ���S
    /// </summary>
    public void EnemyDeath()
    {

        _anim.SetBool("Death", true);

        _motion = Motion.Death;
    }

}
