using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// �v���C���[�̃X�L��UI���A�X�L���̎g�p�󋵂ɂ���ĕύX
/// </summary>
public class SkillUi : MonoBehaviour
{

    [SerializeField]
    PlayerController _playerController;

    [SerializeField]
    PlayerStatus _playerStatus;


    [Header("�X�L��Ui")]
    [SerializeField]
    private Image _heal;

    [SerializeField]
    private Image _shield;

    [SerializeField]
    private Image _ult;

    private bool _isHeal = false;
    private bool _isShield = false;
    private bool _isUlt = false;

    private float _maxSkillVol;

    private void Start()
    {
        //�X�L���̍ő�l
        _maxSkillVol = _playerStatus.MaxSkillVol();
    }

    private void FixedUpdate()
    {

        //���݂̃X�L����
        float skillVol = _playerStatus.SkillVol();

        //�X�L���̃C���^�[�o��
        float healPar = _playerController.SkillCount().x / _playerController.SkillTime().x;
        float shieldPar = _playerController.SkillCount().y / _playerController.SkillTime().y;
        float ultPar = _playerController.SkillCount().z / _playerController.SkillTime().z;

        //��
        if (healPar >= 1) { _isHeal = true; }
        else { _isHeal = false; }

        _heal.fillAmount = healPar;

        if (skillVol >= _maxSkillVol / 4 && _isHeal) { _heal.color = new Color(255, 255, 255, 1); }
        else { _heal.color = new Color(255, 255, 255, 0.2f); }


        //�V�[���h
        if (shieldPar >= 1) { _isShield = true; }
        else { _isShield = false; }

        _shield.fillAmount = shieldPar;

        if (skillVol >= _maxSkillVol / 2 && _isShield) { _shield.color = new Color(255, 255, 255, 1); }
        else { _shield.color = new Color(255, 255, 255, 0.2f); }


        //�E���g
        if (ultPar >= 1) { _isUlt = true; }
        else { _isUlt = false; }

        _ult.fillAmount = ultPar;

        if (skillVol >= _maxSkillVol && _isUlt) { _ult.color = new Color(255, 255, 255, 1); }
        else { _ult.color = new Color(255, 255, 255, 0.2f); }

    }
}
