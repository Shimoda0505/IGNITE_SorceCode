using UnityEngine;



/// <summary>
/// プレイヤーのエフェクトを管理
/// </summary>
public class PlayerEffect : MonoBehaviour
{

    [SerializeField, Tooltip("バリア")]
    private GameObject _shield;
    /// <summary>
    /// バリアのアクティブ
    /// </summary>
    public void Shild()
    {

        //アクティブ終了
        if(_shield.activeSelf)
        {

            _shield.SetActive(false);
        }
        //アクティブ開始
        else if(!_shield.activeSelf)
        {

            _shield.SetActive(true);
        }
    }


    [SerializeField, Tooltip("バリアのブレイク")]
    private GameObject _shieldBreak;
    /// <summary>
    /// シールドの破壊
    /// </summary>
    public void ShildBreak()
    {

        if(!_shieldBreak.activeSelf)
        {

            //アクティブ
            _shieldBreak.SetActive(true);

        }
    }


    [SerializeField, Tooltip("回復")]
    private GameObject _heal;
    /// <summary>
    /// バリアのアクティブ
    /// </summary>
    public void Heal()
    {

        //アクティブ開始
        if (!_heal.activeSelf)
        {

            _heal.SetActive(true);
        }
    }


    [SerializeField, Tooltip("回避")]
    private GameObject[] _trailsDoage;
    /// <summary>
    /// 回避
    /// </summary>
    public void Doage()
    {

        //アクティブ開始
        if (!_heal.activeSelf)
        {

            for (int i = 0; i < _trailsDoage.Length; i++) { _trailsDoage[i].SetActive(true); }
        }

    }

    [SerializeField, Tooltip("トレール")]
    private GameObject[] _trails;
    /// <summary>
    /// トレール
    /// </summary>
    public void Trayl()
    {
        for (int i = 0; i < _trails.Length; i++)
        {
            _trails[i].SetActive(true);
            _trails[i].GetComponent<PlayerEffectLooping>().ResetCount();
        }
    }
}
