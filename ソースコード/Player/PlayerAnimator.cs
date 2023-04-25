using UnityEngine;


/// <summary>
/// �v���C���[�̃A�j���[�V�����Ǘ�
/// </summary>
public class PlayerAnimator : MonoBehaviour
{

    #region �S�ĂŎg�p
    PlayerEffect _playerEffect;
    _InputSystemController _inputController;

    private Animator _anim;//�v���C���[��animator�̎擾


    void Start()
    {
        _anim = this.gameObject.GetComponent<Animator>();//animator�̎擾
        _playerEffect = this.gameObject.GetComponent<PlayerEffect>();
    }
    #endregion


    #region �H�΂������̕ύX
    [Header("�H�΂������̕ύX")]
    [SerializeField, Tooltip("���s��s�̊J�n����")]
    private float _parallelFlyTime;

    [SerializeField, Tooltip("�H�΂�����s�̊J�n����")]
    private float _flappingFlyTime;

    //��s�ύX�̌v��
    private float _changeFlyCount;

    [SerializeField, Tooltip("��s�ύX�̃X�s�[�h")]
    private float _changeVerocitySpeed;

    //�H�΂����ɑ��(0~1)
    private Vector2 _flyAnimTree = new Vector2(0, 0);

    //��s�ύX�̑J�ڃt���O
    private bool _isFlyVertical = false;

    [SerializeField, Tooltip("�H�΂�����s�̃A�j���[�V�����ő�l(0~1)")]
    private float _maxFlappingFly;


    /// <summary>
    /// �H�΂����ƕ��s��s�̃A�j���[�V�����؂�ւ�
    /// </summary>
    public void ChangeFlyVerticalAnim()
    {

        _changeFlyCount += Time.deltaTime;

        if (!_isFlyVertical)
        {

            //���s��s�ɑJ�ڂ���܂ł̎���
            if (_changeFlyCount >= _parallelFlyTime)
            {

                //���s��s�Ɉڍs
                _flyAnimTree.y += Time.deltaTime * _changeVerocitySpeed;

                if (_flyAnimTree.y >= _maxFlappingFly)
                {
                    _playerEffect.Trayl();
                    _isFlyVertical = true;
                    _changeFlyCount = 0;
                }
            }
        }

        else if (_isFlyVertical)
        {

            //�H�΂�����s�ɑJ�ڂ���܂ł̎���
            if (_changeFlyCount >= _flappingFlyTime)
            {

                //�H�΂�����s�Ɉڍs
                _flyAnimTree.y -= Time.deltaTime * _changeVerocitySpeed;

                if (_flyAnimTree.y <= 0)
                {

                    _isFlyVertical = false;
                    _changeFlyCount = 0;
                }
            }
        }

        //1�ɂȂ������_�ŉH�΂����J�E���g�Ɉڍs
        _flyAnimTree.y = Mathf.Clamp(_flyAnimTree.y, 0, _maxFlappingFly);

        //�H�΂����ƕ��s��s�̃A�j���[�V����(BrendTree)
        _anim.SetFloat("Vertical", _flyAnimTree.y);
    }


    #endregion


    #region ���E��s�̕ύX
    [Header("���E��s�̕ύX")]
    [SerializeField, Tooltip("���E��s�̕ύX���x")]
    private float _changeHorizontalSpeed;

    [SerializeField, Tooltip("���E��s�̖߂葬�x")]
    private float _returnHorizontalSpeed;

    //���E��s�̃A�j���[�V�����ɑ��(0~1)
    private float _animValue;

    [SerializeField, Tooltip("���E��s�̃A�j���[�V�����ő�l(0~1)")]
    private float _maxHorizontalFly;


    /// <summary>
    /// ���E�ړ����̃A�j���[�V�����؂�ւ�
    /// </summary>
    public void ChangeFlyHorizontalAnim(float moveInputX)
    {

        //���E�U��ނ��A�j���[�V��������͂ɍ��킹�ē�����
        _animValue += moveInputX * Time.deltaTime * _changeHorizontalSpeed;

        //_animValue�ɔ͈͐��������Č��������Ȃ��悤�ɂ���
        _animValue = Mathf.Clamp(_animValue, -_maxHorizontalFly, _maxHorizontalFly);

        //���E��s�A�j���[�V����(BrendTree)
        _anim.SetFloat("Horizontal", _animValue);

    }

    /// <summary>
    /// ���E�ړ����̃A�j���[�V�����߂�
    /// </summary>
    public void ReturnFlyHorizontalAnim()
    {

        //���E�U��ނ��A�j���[�V�����𐳖ʂɖ߂�
        _animValue = Mathf.Lerp(_animValue, 0, Time.deltaTime * _returnHorizontalSpeed);

        //���E��s�A�j���[�V����(BrendTree)
        _anim.SetFloat("Horizontal", _animValue);

    }

    /// <summary>
    /// ���E�ړ����̃A�j���[�V����������
    /// </summary>
    public void ResetFlyHorizontalAnim()
    {

        //���E�U��ނ��A�j���[�V�����𐳖ʂɖ߂�
        _animValue = 0;

        //���E��s�A�j���[�V����(BrendTree)
        _anim.SetFloat("Horizontal", _animValue);

    }
    #endregion


    #region ���
    /// <summary>
    /// ����A�j���[�V����
    /// </summary>
    public int DodgeAnim(float inputHorizontalValue)
    {

        //���͂ɉ�����������
        if (inputHorizontalValue >= 0)
        {
            _anim.SetBool("DoageR", true);

            //�E����̕ԋp
            return 1;

        }
        else if (inputHorizontalValue < 0)
        {
            _anim.SetBool("DoageL", true);

            //������̕ԋp
            return -1;

        }

        //�ԋp�l�Ȃ�
        return 0;
    }

    /// <summary>
    /// Animation��Event�ł̍Đ����I��
    /// </summary>
    public void DoageFalse()
    {

        _anim.SetBool("DoageR", false);
        _anim.SetBool("DoageL", false);
    }
    #endregion


    #region �_���[�W
    /// <summary>
    /// �_���[�W���̃A�j���[�V����
    /// </summary>
    public void DamageAnim()
    {

        if (_inputController.LeftStickValue().x >= 0)
        {
            _anim.SetBool("DamageR", true);

        }
        else if (_inputController.LeftStickValue().x < 0)
        {
            _anim.SetBool("DamageL", true);

        }

    }

    /// <summary>
    /// �_���[�W�A�j���[�V�������~
    /// </summary>
    public void DamageFalse()
    {
        _anim.SetBool("DamageR", false);
        _anim.SetBool("DamageL", false);

    }
    #endregion


    #region ���S,����
    /// <summary>
    /// ���S�A�j���[�V����
    /// </summary>
    public void DeathAnim()
    {

        _anim.SetTrigger("Death");
    }

    /// <summary>
    /// �����A�j���[�V����
    /// </summary>
    public void ReviveAnim()
    {

        _anim.SetTrigger("ReBorn");
    }


    #endregion


    #region �E���g
    public void UltAnim()
    {
        _anim.SetTrigger("Ult");
    }
    #endregion

    public void GameClearAnim()
    {
        _anim.SetTrigger("GameClear");
    }

}
