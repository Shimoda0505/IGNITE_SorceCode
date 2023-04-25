using UnityEngine;



/// <summary>
/// �v���C���[�̃G�t�F�N�g���Ǘ�
/// </summary>
public class PlayerEffect : MonoBehaviour
{

    [SerializeField, Tooltip("�o���A")]
    private GameObject _shield;
    /// <summary>
    /// �o���A�̃A�N�e�B�u
    /// </summary>
    public void Shild()
    {

        //�A�N�e�B�u�I��
        if(_shield.activeSelf)
        {

            _shield.SetActive(false);
        }
        //�A�N�e�B�u�J�n
        else if(!_shield.activeSelf)
        {

            _shield.SetActive(true);
        }
    }


    [SerializeField, Tooltip("�o���A�̃u���C�N")]
    private GameObject _shieldBreak;
    /// <summary>
    /// �V�[���h�̔j��
    /// </summary>
    public void ShildBreak()
    {

        if(!_shieldBreak.activeSelf)
        {

            //�A�N�e�B�u
            _shieldBreak.SetActive(true);

        }
    }


    [SerializeField, Tooltip("��")]
    private GameObject _heal;
    /// <summary>
    /// �o���A�̃A�N�e�B�u
    /// </summary>
    public void Heal()
    {

        //�A�N�e�B�u�J�n
        if (!_heal.activeSelf)
        {

            _heal.SetActive(true);
        }
    }


    [SerializeField, Tooltip("���")]
    private GameObject[] _trailsDoage;
    /// <summary>
    /// ���
    /// </summary>
    public void Doage()
    {

        //�A�N�e�B�u�J�n
        if (!_heal.activeSelf)
        {

            for (int i = 0; i < _trailsDoage.Length; i++) { _trailsDoage[i].SetActive(true); }
        }

    }

    [SerializeField, Tooltip("�g���[��")]
    private GameObject[] _trails;
    /// <summary>
    /// �g���[��
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
