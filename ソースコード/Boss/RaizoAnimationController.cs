using UnityEngine;



/// <summary>
/// ボスのアニメーション管理
/// </summary>
public class RaizoAnimationController : MonoBehaviour
{

    [SerializeField,Tooltip("ボスのアニメーター")]
    private Animator _anim;


    public void FlyHorizontalAnim(float hor) { _anim.SetFloat("Horizontal", hor); }

    /// <summary>
    /// プレイヤー直視
    /// </summary>
    public void LookPlayerAnim() { _anim.SetBool("LookPlayer", true);
                                   _anim.SetBool("LookForward", false); _anim.SetBool("Tackle", false); _anim.SetBool("Attack", false); }

    /// <summary>
    /// 前方直視
    /// </summary>
    public void LookForwardAnim()  {_anim.SetBool("LookForward",true);
                                    _anim.SetBool("LookPlayer", false); _anim.SetBool("Tackle", false); _anim.SetBool("Attack", false); }

    /// <summary>
    /// 攻撃
    /// </summary>
    public void AttackAnim() { /*_anim.SetBool("Attack",true);*/ }

    /// <summary>
    /// ダメージ
    /// </summary>
    public void DamageAnim() { _anim.SetTrigger("Damage"); }

    /// <summary>
    /// タックル
    /// </summary>
    public void TackleAnimT() { _anim.SetBool("Tackle", true); }
}
