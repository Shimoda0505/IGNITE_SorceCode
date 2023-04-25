using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// �{�X�̃X�e�[�^�X�Ǘ�
/// </summary>
public class BossStatus : MonoBehaviour
{

    #region �X�N���v�g
    [SerializeField, Tooltip("�{�X�̃Q�[���I���Ǘ��X�N���v�g")] RaizoGameEnd _raizoGameEnd;/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/
    [SerializeField, Tooltip("�v���C���[�Ǘ��X�N���v�g")] PlayerController _playerController;
    #endregion

    #region Hp�֘A
    [Header("Hp�֘A")]
    [SerializeField, Tooltip("�{�X�̏���HP")]
    protected float _bossFirst_HP = default;

    //�{�X�̌���HP
    private float _boss_HP = default;

    //���S�t���O
    private bool _isDeath = false;

    [SerializeField, Tooltip("Hp�o�[�̃C���[�W�摜")] private Image _hpBar;
    [SerializeField, Tooltip("Hp��UI�I�u�W�F�N�g")] private GameObject _hpUIObj;


    /// <summary>
    /// �{�X�����S�������ǂ���
    /// </summary>
    public bool IsDeath() { return _isDeath; }

    //Hp����
    private bool _isHpHalf;

    /// <summary>
    /// �{�X��Hp������
    /// </summary>
    public bool IsHpHalf() { return _isHpHalf; }

    private bool _isUlt;
    /// <summary>
    /// �E���g�J�n
    /// </summary>
    public void IsUltTrue() { _isUlt = true; }
    /// <summary>
    /// �E���g�I��
    /// </summary>
    public void IsUltFalse() { _isUlt = false; }
    /// <summary>
    /// �E���g�̏�
    /// </summary>
    /// <returns></returns>
    public bool IsUlt() { return _isUlt; }
    #endregion

    //�{�X��J�n
    private bool _isStart = false;
    public Motion _motion = Motion.START;
    public enum Motion
    {
        START,
        PLAY,
        END
    }


    //�����Q---------------------------------------------------------------------------------------------------------------

    private void Start()
    {

        //�{�X��Hp��⊮
        _boss_HP = _bossFirst_HP;

        //Hp�o�[�̏�����
        _hpBar.fillAmount = 0;

        //Hp�̔�\��
        _hpUIObj.SetActive(false);
    }

    private void FixedUpdate()
    {

        //�{�X��J�n����enum���J�n�̎�
        if (_isStart && _motion == Motion.START)
        {
            //3�b�ōő���Z
            _hpBar.fillAmount += Time.deltaTime / 3;

            //Hp�o�[���ő�l�Ȃ�enum����play�ɕύX
            if (_hpBar.fillAmount >= 1) { _motion = Motion.PLAY; }
        }
    }

    //���\�b�h�Q---------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// �{�X�Ƀ_���[�W��^����
    /// </summary>
    public void BossDamage(int damage)
    {

        //�v���C���[���S���͏������Ȃ�
        if (_playerController._playerMotion == PlayerController.PlayerMotion.Death) { return; }

        //�{�X��Hp�����Z
        _boss_HP -= damage;

        //HpBar�̍X�V
        _hpBar.fillAmount = _boss_HP / _bossFirst_HP;

        //Hp�������ȉ��Ȃ�
        if (_boss_HP <= _bossFirst_HP / 2 && !_isHpHalf) { _isHpHalf = true; }

        //Hp��0�ȉ��Ȃ玀�S
        if (_boss_HP <= 0 && !_isDeath)
        {

            //�{�X��I��
            _motion = Motion.END;

            //�{�X��I���J�b�g�C���J�n
            _raizoGameEnd.RaizoCutIn();

            //ho�o�[��\��
            _hpUIObj.SetActive(false);

            //���S�t���O
            _isDeath = true;

        }

    }

    /// <summary>
    /// �{�X��J�n
    /// </summary>
    public void IsStart()
    {

        //Hp�o�[�\��
        _hpUIObj.SetActive(true);

        //�{�X��J�n
        _isStart = true;
    }


}
