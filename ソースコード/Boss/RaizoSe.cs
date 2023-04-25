using UnityEngine;



/// <summary>
/// ƒ{ƒX‚ÌseŠÇ—
/// </summary>
public class RaizoSe : MonoBehaviour
{

    [SerializeField]
    AudioSource _audioSource;



    //ƒƒ\ƒbƒh•”----------------------------------------------------------------------------------------------------

    [SerializeField, Tooltip("™ôšK")]
    private AudioClip _roar;
    /// <summary>
    /// ™ôšK
    /// </summary>
    public void RoarSe()
    {
        _audioSource.PlayOneShot(_roar);
    }


    [SerializeField, Tooltip("‘å™ôšK")]
    private AudioClip _bigRoar;
    /// <summary>
    /// ‘å™ôšK
    /// </summary>
    public void BigRoarSe()
    {
        _audioSource.PlayOneShot(_bigRoar);
    }


    [SerializeField, Tooltip("ƒ^ƒbƒNƒ‹")]
    private AudioClip _tackle;
    /// <summary>
    /// ƒ^ƒbƒNƒ‹
    /// </summary>
    public void TackleSe()
    {
        _audioSource.PlayOneShot(_tackle);
    }


    [SerializeField, Tooltip("€–S™ôšK")]
    private AudioClip _deathRoar;
    /// <summary>
    /// €–S™ôšK
    /// </summary>
    public void DeathRoarSe()
    {
        _audioSource.PlayOneShot(_deathRoar);
    }


    [SerializeField, Tooltip("•—ˆ³")]
    private AudioClip _wing;
    /// <summary>
    /// ™ôšK
    /// </summary>
    public void WingSe()
    {
        _audioSource.PlayOneShot(_wing);
    }


    [SerializeField, Tooltip("—‹’e")]
    private AudioClip _bullet;
    /// <summary>
    /// —‹’e
    /// </summary>
    public void BulletSe()
    {
        _audioSource.PlayOneShot(_bullet);
    }


    [SerializeField, Tooltip("‘å—‹’e")]
    private AudioClip _bigBullet;
    /// <summary>
    /// ‘å—‹’e
    /// </summary>
    public void BigBulletSe()
    {
        _audioSource.PlayOneShot(_bigBullet);
    }


    [SerializeField, Tooltip("—‹’eƒqƒbƒg")]
    private AudioClip _bulletHit;
    /// <summary>
    /// —‹’e
    /// </summary>
    public void BulletHitSe()
    {
        _audioSource.PlayOneShot(_bulletHit);
    }


    [SerializeField, Tooltip("‘å—‹’eƒqƒbƒg")]
    private AudioClip _bigBulletHit;
    /// <summary>
    /// ‘å—‹’e
    /// </summary>
    public void BigBulletHitSe()
    {
        _audioSource.PlayOneShot(_bigBulletHit);
    }


    [SerializeField, Tooltip("——‹")]
    private AudioClip _bolt;
    /// <summary>
    /// ——‹
    /// </summary>
    public void BoltSe()
    {
        _audioSource.PlayOneShot(_bolt);
    }
    
}
