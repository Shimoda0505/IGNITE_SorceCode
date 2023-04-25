using UnityEngine;



/// <summary>
/// プレイヤーのSE管理スクリプト
/// </summary>
public class PlayerSE : MonoBehaviour
{

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip _endTime;
    public void EndTime()
    {
        audioSource.PlayOneShot(_endTime);
    }
    [SerializeField, Header("ロックオン")]
    AudioClip rockOn;

    public void RockOnSe()
    {
        audioSource.PlayOneShot(rockOn);
    }


    [SerializeField,Header("火球")]
    AudioClip fireBollSe;
    public void FireBollSe()
    {
        if(!_isFireBollSe)
        {
            audioSource.PlayOneShot(fireBollSe);

            _isFireBollSe = true;
        }
    }

    [SerializeField, Header("爆発(小)")]
    AudioClip explosionMini;
    public void ExplosionMiniSe()
    {
        if(!_isExplosionMini)
        {
            audioSource.PlayOneShot(explosionMini);

            _isExplosionMini = true;

        }
    }

    [SerializeField, Header("爆発(大)")]
    AudioClip explosionBig;
    public void ExplosionBigSe()
    {
        audioSource.PlayOneShot(explosionBig);
    }

    [SerializeField, Header("水音")]
    AudioClip mizu;
    public void Mizuse()
    {
        audioSource.PlayOneShot(mizu);
    }
    
    [SerializeField, Header("柱")]
    AudioClip hasira;
    public void HashiraSe()
    {
        audioSource.PlayOneShot(hasira);
        _isHasira = true;
    }

    [SerializeField, Header("Crystal")]
    AudioClip crystal;
    public void CrystalBreakSe()
    {
        audioSource.PlayOneShot(crystal);
    }

    [SerializeField, Header("風キリ音")]
    AudioClip windBgm;
    public void WindBgm()
    {
        audioSource.clip = windBgm;
        audioSource.Play();
    }

    [SerializeField, Header("回復")]
    AudioClip heal;
    public void HealSe()
    {
        audioSource.PlayOneShot(heal);

    }

    [SerializeField, Header("回避")]
    AudioClip brink;
    public void BrinkSe()
    {
        audioSource.PlayOneShot(brink);

    }



    public void StopBgm()
    {
        audioSource.Stop();

    }

    //各Seが重複してならないようにする
    private bool _isExplosionMini = false;
    private float _exTime = 0.8f;
    private float _exCount = 0;

    private bool _isFireBollSe;
    private float _fiTime = 0.5f;
    private float _fiCount = 0;

    private bool _isHasira;
    private float _HasiraTime = 2.5f;
    private float _HasiraCount = 0;

    private void FixedUpdate()
    {
        
        if(_isExplosionMini)
        {
            _exCount += Time.deltaTime;
            if(_exCount >= _exTime)
            {
                _isExplosionMini = false;
                _exCount = 0;
            }
        }

        if(_isFireBollSe)
        {
            _fiCount += Time.deltaTime;
            if(_fiCount >= _fiTime)
            {
                _isFireBollSe = false;
                _fiCount = 0;
            }
        }

        if(_isHasira)
        {
            _HasiraCount += Time.deltaTime;
            if(_HasiraCount >= _HasiraTime)
            {
                _isHasira = false;
                _HasiraCount = 0;
            }
        }

    }
}
