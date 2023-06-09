using UnityEngine;



/// <summary>
/// {XÌseÇ
/// </summary>
public class RaizoSe : MonoBehaviour
{

    [SerializeField]
    AudioSource _audioSource;



    //\bh----------------------------------------------------------------------------------------------------

    [SerializeField, Tooltip("ôK")]
    private AudioClip _roar;
    /// <summary>
    /// ôK
    /// </summary>
    public void RoarSe()
    {
        _audioSource.PlayOneShot(_roar);
    }


    [SerializeField, Tooltip("åôK")]
    private AudioClip _bigRoar;
    /// <summary>
    /// åôK
    /// </summary>
    public void BigRoarSe()
    {
        _audioSource.PlayOneShot(_bigRoar);
    }


    [SerializeField, Tooltip("^bN")]
    private AudioClip _tackle;
    /// <summary>
    /// ^bN
    /// </summary>
    public void TackleSe()
    {
        _audioSource.PlayOneShot(_tackle);
    }


    [SerializeField, Tooltip("SôK")]
    private AudioClip _deathRoar;
    /// <summary>
    /// SôK
    /// </summary>
    public void DeathRoarSe()
    {
        _audioSource.PlayOneShot(_deathRoar);
    }


    [SerializeField, Tooltip("³")]
    private AudioClip _wing;
    /// <summary>
    /// ôK
    /// </summary>
    public void WingSe()
    {
        _audioSource.PlayOneShot(_wing);
    }


    [SerializeField, Tooltip("e")]
    private AudioClip _bullet;
    /// <summary>
    /// e
    /// </summary>
    public void BulletSe()
    {
        _audioSource.PlayOneShot(_bullet);
    }


    [SerializeField, Tooltip("åe")]
    private AudioClip _bigBullet;
    /// <summary>
    /// åe
    /// </summary>
    public void BigBulletSe()
    {
        _audioSource.PlayOneShot(_bigBullet);
    }


    [SerializeField, Tooltip("eqbg")]
    private AudioClip _bulletHit;
    /// <summary>
    /// e
    /// </summary>
    public void BulletHitSe()
    {
        _audioSource.PlayOneShot(_bulletHit);
    }


    [SerializeField, Tooltip("åeqbg")]
    private AudioClip _bigBulletHit;
    /// <summary>
    /// åe
    /// </summary>
    public void BigBulletHitSe()
    {
        _audioSource.PlayOneShot(_bigBulletHit);
    }


    [SerializeField, Tooltip("")]
    private AudioClip _bolt;
    /// <summary>
    /// 
    /// </summary>
    public void BoltSe()
    {
        _audioSource.PlayOneShot(_bolt);
    }
    
}
