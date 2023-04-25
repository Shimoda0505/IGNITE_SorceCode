using UnityEngine;



/// <summary>
/// �{�X�̃A�j���[�V�����Ǘ�
/// </summary>
public class RaizoAnimationController : MonoBehaviour
{

    [SerializeField,Tooltip("�{�X�̃A�j���[�^�[")]
    private Animator _anim;


    public void FlyHorizontalAnim(float hor) { _anim.SetFloat("Horizontal", hor); }

    /// <summary>
    /// �v���C���[����
    /// </summary>
    public void LookPlayerAnim() { _anim.SetBool("LookPlayer", true);
                                   _anim.SetBool("LookForward", false); _anim.SetBool("Tackle", false); _anim.SetBool("Attack", false); }

    /// <summary>
    /// �O������
    /// </summary>
    public void LookForwardAnim()  {_anim.SetBool("LookForward",true);
                                    _anim.SetBool("LookPlayer", false); _anim.SetBool("Tackle", false); _anim.SetBool("Attack", false); }

    /// <summary>
    /// �U��
    /// </summary>
    public void AttackAnim() { /*_anim.SetBool("Attack",true);*/ }

    /// <summary>
    /// �_���[�W
    /// </summary>
    public void DamageAnim() { _anim.SetTrigger("Damage"); }

    /// <summary>
    /// �^�b�N��
    /// </summary>
    public void TackleAnimT() { _anim.SetBool("Tackle", true); }
}
