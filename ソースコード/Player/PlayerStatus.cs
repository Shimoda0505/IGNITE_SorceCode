using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// �v���C���[�̃X�e�[�^�X�Ǘ�
/// </summary>
public class PlayerStatus : MonoBehaviour
{

    #region �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField, Tooltip("�v���C���[�̃R���g���[���[")]
    PlayerController _playerController;

    [SerializeField, Tooltip("�v���C���[�A�j���[�V����")]
    PlayerAnimator _playerAnimator;

    [SerializeField, Tooltip("���b�N�I���̃V�X�e��")]
    PointerLockOn _pointerLockOn;

    [SerializeField, Tooltip("�E���g�̃J�b�g�V�[��")]
    GENSHIN_CutIn _cutIn;/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/

    [SerializeField, Tooltip("�`�F�C��UI")]
    ComboSystem _comboSystem;/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/

    [SerializeField, Tooltip("�G�t�F�N�g�R���g���[���[")]
    PlayerEffect _playerEffect;

    [SerializeField]
    ScoreManager _scoreManager;

    [SerializeField]
    ContinueController _continueController;/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/

    [SerializeField]
    PointerChange _pointerChange;

    [SerializeField]
    PlayerLinkEnemy _playerLinkEnemy;

    [SerializeField]
    GameSystem _gameSystem;

    [SerializeField]
    PoolManager _poolManager;

    PoolController _ultPool;
    #endregion


    #region HP
    [Header("HP")]
    [SerializeField, Tooltip("����HP")]
    private float _playerHp;

    //�ő�Hp
    private float _maxHp;

    [SerializeField, Tooltip("�c�@")]
    private int _playerRemaining;
    public int PlayerRemaining() { return _playerRemaining; }

    [SerializeField, Tooltip("�_���[�W�p�l��")]
    private GameObject _damagePanel;
    #endregion

    #region �o���A
    [Header("�o���A")]
    [SerializeField, Tooltip("�ő�o���A��")]
    private float _maxShild;

    //�o���A�c��
    private float _playerShild;
    public float ShildVolume() { return _playerShild / _maxShild; }

    [SerializeField, Tooltip("�o���A�����n�߂̎���")]
    private float _startBariaoutTime;
    private float _startBariaOutCount = 0;
    #endregion

    #region �Q�[�W
    [Header("�Q�[�W")]
    [SerializeField]
    private GameObject _lockOnUi;

    [SerializeField, Tooltip("Hp��Ui")]
    private Image _hpImage;

    [SerializeField, Tooltip("Skill��Ui")]
    private Image _skillImage;

    [SerializeField, Tooltip("Hp�}�e���A��")]
    private Material _hpM;

    [SerializeField, Tooltip("Skill�}�e���A��")]
    private Material _skillM;

    //�}�e���A��ID
    private int _fillAmount;
    #endregion

    #region �X�L��
    [Header("�X�L��")]
    [SerializeField, Tooltip("�ő�X�L����")]
    private float _maxSkillVol;
    public float MaxSkillVol() { return _maxSkillVol; }

    //���݂̃X�L����
    private float _skillVol = 0;
    public float SkillVol() { return _skillVol; }

    //�X�L�����ő�l���ǂ���
    private bool _isSkillMax = false;
    public bool IsSkillMax() { return _isSkillMax; }

    [SerializeField, Tooltip("�񕜗�")]
    private int _heelVolume;

    [SerializeField, Tooltip("�E���g�̔��ˈʒu")]
    private Transform _shotPos;

    private GameObject _ultTarget;

    #endregion

    #region ���b�N�I��
    [Header("���b�N�I��")]
    [SerializeField, Tooltip("�ő働�b�N�I����")]
    private int _maxLock;

    [SerializeField, Tooltip("���e�B�N���̕ύX�l")]
    private Vector2 _changeLock;

    //���݂̃��b�N�I����
    private int _lockCount;
    #endregion

    #region �ϐ�
    [Header("�`�F�C��")]
    [SerializeField, Tooltip("�X�L���̉��Z�{��UI")]
    private Text _skillText;

    [SerializeField, Tooltip("�X�L���̉��Z�{��UI�̍ő�t�H���g")]
    private int _maxChainFontSize;

    //�X�L���̉��Z�{��UI�̃f�t�H���g�t�H���g
    private int _defChainFontSize;

    [SerializeField, Tooltip("�`�F�C���{��")]
    private Vector2 _chainMag;

    //�`�F�C����
    private int _chainCount = 0;
    #endregion

    #region�@���S
    [Header("���S�֘A")]
    [SerializeField, Tooltip("���[�g�i�r�I�u�W�F�N�g")]
    private GameObject _rootNav;

    [SerializeField, Tooltip("���S���ɗ�������x���W")]
    private float _fallPos;

    [SerializeField, Tooltip("�������x")]
    private float _fallSpeed;

    [SerializeField, Tooltip("�߂葬�x")]
    private float _reviveSpeed;

    [SerializeField, Tooltip("���S��̃C���^�[�o��")]
    private float _deathIntervalTime;
    private float _deathIntervalCount = 0;

    //���S�t���O
    private bool _isDeath = false;

    //���S���̈ʒu�⊮
    private float _deathPosY;

    //���S���Motion
    DeathMotion _deathMotion = DeathMotion.Wait;
    enum DeathMotion
    {
        Wait,
        Death,
        ReBorn
    }
    #endregion





    //�����֘A-------------------------------------------------------------------------------------------------

    private void Start()
    {
        _fillAmount = Shader.PropertyToID("_FillAmount");

        _defChainFontSize = _comboSystem.MathSize;

        //�ő�Hp�̐ݒ�
        _maxHp = _playerHp;

        //�v���C���[�̃v�[���Ǘ�����v�[���ݒ�
        for (int i = 0; i <= _poolManager._poolArrays.Length - 1; i++)
        {

            //�v�[�������擾
            string poolName = _poolManager._poolArrays[i]._poolName;

            //�X�N���v�g���擾
            PoolController poolScript = _poolManager._poolArrays[i]._poolControllers;

            //���O��v�̃v�[����T��������
            if (poolName == "�K�E�Z")
            {

                _ultPool = poolScript;
            }

        }


    }

    private void FixedUpdate()
    {

        print(_lockCount);

        //�X�L����
        if (_skillVol >= _maxSkillVol) { _isSkillMax = true; }
        else { _isSkillMax = false; }


        //Hp�o�[
        _hpImage.fillAmount = _playerHp / _maxHp;
        _hpM.SetFloat(_fillAmount, _playerHp / _maxHp);

        //Skill�o�[
        _skillImage.fillAmount = _skillVol / _maxSkillVol;
        _skillM.SetFloat(_fillAmount, _skillVol / _maxSkillVol);


        //�o���A�����Ԍo�ߌ�ɏ���
        if (_playerShild > 0)
        {

            //���Ԍv��
            _startBariaOutCount += Time.deltaTime;

            //���Ԍv����
            if (_startBariaOutCount >= _startBariaoutTime) { _playerShild -= Time.deltaTime * 200; }

        }



        //���S���̍��W
        switch (_deathMotion)
        {

            case DeathMotion.Wait:

                //���S��̖��G����
                if (_isDeath)
                {
                    _deathIntervalCount += Time.deltaTime;
                    if (_deathIntervalCount >= _deathIntervalTime) { _isDeath = false; }
                }

                break;


            case DeathMotion.Death:


                if (_rootNav.transform.localPosition.y >= _fallPos)
                {

                    //����
                    _rootNav.transform.localPosition = new Vector3(_rootNav.transform.localPosition.x,
                                                                   _rootNav.transform.localPosition.y - Time.deltaTime * _fallSpeed,
                                                                   _rootNav.transform.localPosition.z);

                }

                break;


            case DeathMotion.ReBorn:

                //����
                _rootNav.transform.localPosition = new Vector3(_rootNav.transform.localPosition.x,
                                                               _rootNav.transform.localPosition.y + Time.deltaTime * _reviveSpeed,
                                                               _rootNav.transform.localPosition.z);

                if (_rootNav.transform.localPosition.y >= _deathPosY)
                {

                    _rootNav.transform.localPosition = new Vector3(_rootNav.transform.localPosition.x,
                                                               _deathPosY,
                                                               _rootNav.transform.localPosition.z);
                    _deathMotion = DeathMotion.Wait;
                }

                break;


        }



        //�`�F�C�����Z��
        if (_chainCount < _chainMag.x) { _skillText.text = "�~" + 1; }
        else if (_chainMag.y > _chainCount && _chainCount >= _chainMag.x) { _skillText.text = "�~" + 2; }
        else if (_chainCount >= _chainMag.y) { _skillText.text = "�~" + 3; }

        //���b�N�I�����ɉ����ă��e�B�N���̕ύX
        LockChange();

    }



    //���\�b�h��-------------------------------------------------------------------------------------------------

    //HP
    /// <summary>
    /// �_���[�W����
    /// </summary>
    public bool Hit(int damage)
    {

        //��sEnum�ȊO�̎� || ���G����
        if (_playerController._playerMotion != PlayerController.PlayerMotion.Fly || _playerController.IsInvincible() || _gameSystem._isEvent) { return false; }

        //�E���g�g�p���Ȃ�
        if (_playerController.IsUlt())
        {

            //��
            _playerHp += damage;

            //�ő�l�𒴂�����
            if (_playerHp >= _maxHp)
            {

                _playerHp = _maxHp;
            }

            //�񕜃G�t�F�N�g
            _playerEffect.Heal();

            return true;

        }

        //�o���A�ʂ��c���Ă���Ȃ�
        if (_playerShild > 0)
        {

            //�o���A�Ƀ_���[�W
            _playerShild -= damage;

            //�o���A�j��G�t�F�N�g
            _playerEffect.ShildBreak();

            return true;

        }

        //Hp�Ƀ_���[�W
        else if ((_playerShild <= 0))
        {

            //�_���[�W���󂯂��Ƃ��Ƀ��b�N�I���z���T�����ă��b�N�I������
            _pointerLockOn.DamageResetting();

            //�`�F�C���̏�����
            ChainResetting();

            //���b�N�I�����̏�����
            LockPrice("������");

            //Hp�̌��Z
            _playerHp = _playerHp - damage;

            _damagePanel.SetActive(true);

            //Hp��0�ȉ��Ȃ�
            if (_playerHp <= 0)
            {

                //���S�t���O
                _isDeath = true;

                //�c�@0�ȉ��Ȃ�Q�[���I�[�o�[
                if (_playerRemaining <= 0)
                {

                    //�Q�[���I�[�o�[###############################
                    _continueController.GameOver();

                    return true;

                }

                _deathPosY = this.gameObject.transform.localPosition.y;

                //���SEnum
                _deathMotion = DeathMotion.Death;

                //���S�ƒ���̏���
                _playerController.DeathStart();

                //�R���e�j���[UI
                _continueController.IsDeath();

                _lockOnUi.SetActive(false);

                return true;

            }

            //�_���[�W�A�j���[�V�����Đ�
            _playerAnimator.DamageAnim();

            //�v���C���[��Enum���_���[�W�ɑJ��
            _playerController._playerMotion = PlayerController.PlayerMotion.Damage;

        }

        return true;

    }


    //���S
    /// <summary>
    /// �����J�n�̏���
    /// </summary>
    public void ReviveStart()
    {

        //����Enum
        _deathMotion = DeathMotion.ReBorn;

        //�c�@���Z
        _playerRemaining--;

        _playerHp = _maxHp;

        //�����A�j���[�V�����Đ�
        _playerAnimator.ReviveAnim();

        //����Enum�ɑJ��
        _playerController._playerMotion = PlayerController.PlayerMotion.Revive;

        _lockOnUi.SetActive(true);


    }


    //�X�L��
    /// <summary>
    /// �X�L���̎g�p(��/�o���A/�E���g)
    /// </summary>
    public bool UseSkill(string skill)
    {

        //�� && �񕜐���1��ȏ�
        if (skill == "��" && _skillVol >= _maxSkillVol / 4)
        {

            //�X�L���̌��Z
            _skillVol -= _maxSkillVol / 4;

            //��
            _playerHp += _heelVolume;

            //�ő�l�𒴂�����
            if (_playerHp >= _maxHp)
            {

                _playerHp = _maxHp;
            }

            //�񕜃G�t�F�N�g
            _playerEffect.Heal();

            return true;

        }

        //�o���A && �o���A�񐔂�1��ȏ�
        else if (skill == "�o���A" && _skillVol >= _maxSkillVol / 2)
        {

            //�X�L���̌��Z
            _skillVol -= _maxSkillVol / 2;

            //�o���A�ʂ��ő�l�ɕύX
            _playerShild = _maxShild;

            //�V�[���h�̃A�N�e�B�u
            _playerEffect.Shild();

            _startBariaOutCount = 0;

            return true;

        }

        //�E���g && �E���g�g�p�񐔂�1��ȏ�
        else if (skill == "�E���g" && _skillVol >= _maxSkillVol)
        {

            //�X�L���̌��Z
            _skillVol -= _maxSkillVol;

            //�G�̊i�[�z���T��
            for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
            {

                PlayerLinkEnemy.Targets targets = _playerLinkEnemy._targetList[i];

                //���b�N�I���� && �e���ł���Ă��Ȃ��G������Ȃ�
                if (targets._isLock && targets._fireBoll == null)
                {

                    _ultTarget = targets._targetObj;

                    //�E���g�̃J�b�g�V�[��
                    _cutIn.CutIn();

                    break;

                }

            }

            //���b�N�I�����̏�����
            LockPrice("������");

            //���e�B�N���̕ύX
            _pointerChange.ChangeRet();

            return true;

        }

        return false;

    }

    /// <summary>
    /// �E���g�g�p���̃^�[�Q�b�g
    /// </summary>
    public GameObject UltTarget() { return _ultTarget; }

    /// <summary>
    /// �A�j���[�V����Events�Ŏg�p
    /// </summary>
    public void UltShot()
    {

        GameObject obj = _ultPool.GetObj();

        obj.SetActive(true);

        //�e�̔��ˈʒu��Player�̏e���ɐݒ�
        obj.transform.position = _shotPos.position;

        //�e�̔��ˈʒu��Player�̏e���ɐݒ�
        obj.transform.rotation = _shotPos.rotation;

    }


    //�`�F�C��
    /// <summary>
    /// �`�F�C���̉��Z�Ăяo��
    /// </summary>
    public void ChainAddition()
    {

        //�`�F�C�����̉��Z
        _chainCount++;

        //�ő�`�F�C�����̍X�V
        _scoreManager.ScoreUpdate(_chainCount);

        //���Z��̃`�F�C������\��
        _comboSystem.IncreaseCombo(_chainCount);

        _comboSystem.MathSize = _defChainFontSize + _chainCount;

        if (_comboSystem.MathSize >= _maxChainFontSize) { _comboSystem.MathSize = _maxChainFontSize; }

        //�X�L���ʂ̉��Z
        if (_skillVol <= _maxSkillVol)
        {

            //�`�F�C�����Z��
            if (_chainCount < _chainMag.x) { _skillVol++; }
            else if (_chainMag.y > _chainCount && _chainCount >= _chainMag.x) { _skillVol += 2; }
            else if (_chainCount >= _chainMag.y) { _skillVol += 3; }
        }

        //�ő�l�𒴂��Ă�����
        if (_skillVol > _maxSkillVol) { _skillVol = _maxSkillVol; }

    }

    /// <summary>
    /// �`�F�C���̏�����
    /// </summary>
    public void ChainResetting()
    {

        //�R���{���̏�����
        _comboSystem.RessetingCombo();

        //�`�F�C�����̏�����
        _chainCount = 0;

    }


    //���b�N�I��
    /// <summary>
    /// �ő働�b�N�I��������return
    /// </summary>
    public bool MaxLockCount()
    {

        if (_lockCount >= _maxLock) { return true; }

        return false;
    }

    /// <summary>
    /// ���b�N�I������ԋp
    /// </summary>
    public int LockCount() { return _lockCount; }

    /// <summary>
    /// ���b�N�I��(���Z/���Z/������)
    /// </summary>
    /// <param name="motion"></param>
    public void LockPrice(string motion)
    {
        if (motion == "���Z") { if (_lockCount < 10) { _lockCount++; } }
        else if (motion == "���Z") { if (_lockCount > 0) { _lockCount--; } }
        else if (motion == "������") { _lockCount = 0; }
    }

    /// <summary>
    /// ���b�N�I�����ɉ����ă��e�B�N���̕ύX
    /// </summary>
    private void LockChange()
    {

        //�E���g���͏������Ȃ�
        if (_playerController.IsUlt()) { return; }


        if (_lockCount < _changeLock.x) { _pointerLockOn.ChangePointerClamp("1"); }

        else if (_changeLock.x <= _lockCount && _lockCount < _changeLock.y) { _pointerLockOn.ChangePointerClamp("2"); }

        else if (_changeLock.y <= _lockCount) { _pointerLockOn.ChangePointerClamp("3"); }

    }

}
