using UnityEngine;



/// <summary>
/// �v���C���[�̃J���[���E���g���ɐɕύX
/// �v���C���[�̓����x���J�[�\�����d�Ȃ��Ă��邩�ɂ��ύX
/// </summary>
public class PlayerTransparent : MonoBehaviour
{

    #region �X�N���v�g
    [SerializeField]
    GameSystem _gameSystem;

    [SerializeField]
    PlayerController _playerController;

    [SerializeField]
    PlayerStatus _playerStatus;
    #endregion

    #region �J�[�\��
    [Header("�J�[�\���֘A")]
    [SerializeField, Tooltip("Cursor")]
    private Transform _cursorPos;

    [SerializeField]
    private float _distance;

    //Ui���W�̍ő�l
    private Vector2 _maxRectTr = new Vector2(800, 450);

    //�J�������W�̒l��(-0.5~0.5)����(0~1)�ɕϊ�
    private const float _valueChange = 0.5f;

    [SerializeField]
    private Camera _mainCamera;
    #endregion

    #region �}�e���A��
    [Header("�}�e���A���֘A")]
    [SerializeField, Tooltip("���b�V��")]
    private SkinnedMeshRenderer _mesh;

    [SerializeField, Tooltip("���߃}�e���A��")]
    private Material[] _transMaterials;

    [SerializeField, Tooltip("�񓧉߃}�e���A��")]
    private Material[] _opaMaterials;

    [SerializeField, Tooltip("���߃}�e���A��(ULT)")]
    private Material[] _transMaterialsUlt;

    [SerializeField, Tooltip("�񓧉߃}�e���A��(ULT)")]
    private Material[] _opaMaterialsUlt;

    //�E���g�����ǂ���
    private bool _isUlt = false;
    #endregion

    #region �f�B�]���u
    [Header("�f�B�]���u�֘A")]
    [SerializeField, Tooltip("�����ւ����x")]
    private float _changeDisSpeed;

    //�f�B�]���uID
    private int _dis = 0;

    //�f�B�]���u�̌��ݒl
    private float _disMove = 0;

    //�����ւ�
    private bool _isDis = false;
    #endregion

    #region �u�[���A��
    [Header("�u�[���A���֘A")]
    [SerializeField, Tooltip("�u�[���A�����x")]
    private float _changeBospeed;

    //�u�[���A��ID
    private int _bo;

    //�����p���[ID
    private int _rim;

    //�u�[���A���̌��ݒl
    [SerializeField]
    private float _rimMove;

    //�u�[���A���̍ő�ŏ�
    private Vector2 _rimClamp = new Vector2(1, 0.4f);
    #endregion

    #region �G�t�F�N�g
    [Header("�G�t�F�N�g")]
    [SerializeField, Tooltip("�ԉ�")]
    private GameObject _redFire;

    [SerializeField, Tooltip("��")]
    private GameObject _blueFire;
    #endregion


    Motion _motion = Motion.OPAQUA;
    enum Motion
    {
        CHANGE,
        TRANSPARENT,
        OPAQUA
    }



    //������---------------------------------------------------------------------------------------------------------

    private void Start()
    {

        //�G�t�F�N�g��Active��false
        _redFire.SetActive(false);
        _blueFire.SetActive(false);

        //�u�[���A��ID�擾
        _bo = Shader.PropertyToID("_Boolean");

        //�����p���[ID�擾
        _rim = Shader.PropertyToID("_RimPower");

        //�����l�̏����l
        _rimMove = _rimClamp.x;

        //�����̕ύX
        //ChangeFloat(_transMaterials, _rim, _rimMove);
        ChangeFloat(_opaMaterials, _rim, _rimMove);

        //�u�[���A���̕ύX
        ChangeFloat(_transMaterials, _bo, 0);
        ChangeFloat(_opaMaterials, _bo, 0);


        //Opaqua�ɏ����ݒ�
        ChangeMaterial(_opaMaterials);


    }

    private void FixedUpdate()
    {

        //�C�x���g�� / �E���g�}�e���A�������ւ� / �񓧉߂ɑJ��
        if (_gameSystem._isEvent)
        {
            if (_playerController.IsUlt()) { ChangeMaterial(_opaMaterialsUlt); _redFire.SetActive(false); _blueFire.SetActive(true); }
            else { ChangeMaterial(_opaMaterials); }

            _motion = Motion.OPAQUA;

            return;
        }


        //�E���g�g�p�\���̃v���C���[����
        ChangeGloss();

        //���e�B�N���ƃv���C���[�̈ʒu���v�Z/enum�̑J��
        ReticleDistance();

        //����Active
        EffectActive();

        //����
        switch (_motion)
        {

            //���ߒ�
            case Motion.TRANSPARENT:

                //�񓧉�
                if (!_playerController.IsUlt()) { ChangeMaterial(_transMaterials); }
                //�񓧉�Ult��
                else if (_playerController.IsUlt()) { ChangeMaterial(_transMaterialsUlt); }

                break;


            //�񓧉ߒ�
            case Motion.OPAQUA:

                //�񓧉�
                if (!_playerController.IsUlt()) { ChangeMaterial(_opaMaterials); }
                //�񓧉�Ult��
                else if (_playerController.IsUlt()) { ChangeMaterial(_opaMaterialsUlt); }

                break;

        }

    }



    //���\�b�h��------------------------------------------------------------------------------------------------------

    //���e�B�N��
    /// <summary>
    /// ���e�B�N���ƃv���C���[�̈ʒu���v�Z/enum�̑J��
    /// </summary>
    private void ReticleDistance()
    {

        //�v���C���[�̍��W���A�J����View���W(0~1,0~1)�ɕϊ�
        Vector2 thisPos = Camera.main.WorldToViewportPoint(this.gameObject.transform.position);

        //�|�C���^�[�̃J����View���W(0~1,0~1)
        //Vector(0~800 , 0~460)��(0~1 , 0~1)�ɕϊ�
        Vector2 uiPos = new Vector2(_cursorPos.localPosition.x / _maxRectTr.x + _valueChange,
                                    _cursorPos.localPosition.y / _maxRectTr.y + _valueChange);

        //�|�C���^�[�ƃG�l�~�[�̋������v��
        float distans = Vector2.Distance(uiPos, thisPos);

        //���߂ɑJ��
        if (-_distance <= distans && distans <= _distance) { _motion = Motion.TRANSPARENT; }
        //�񓧉߂ɑJ��
        else if (-_distance > distans || distans > _distance) { _motion = Motion.OPAQUA; }

    }

    //�G�t�F�N�g
    /// <summary>
    /// ����Active
    /// </summary>
    private void EffectActive()
    {

        if (_playerController.IsUlt() && !_blueFire.activeSelf)
        {
            //�ԉ���Stop
            _redFire.SetActive(false);

            //����Active
            _blueFire.SetActive(true);
        }
        else if (!_playerController.IsUlt() && _blueFire.activeSelf)
        {

            //����Stop
            _blueFire.SetActive(false);
        }

    }

    //�}�e���A��
    /// <summary>
    /// �}�e���A�������ւ�
    /// </summary>
    private void ChangeMaterial(Material[] material)
    {

        Material[] tmp = _mesh.materials;
        tmp[0] = material[0];
        tmp[1] = material[1];
        _mesh.materials = tmp;

    }

    /// <summary>
    /// �E���g�g�p�\���̃v���C���[����
    /// </summary>
    private void ChangeGloss()
    {

        if (_playerStatus.IsSkillMax())
        {

            //���l�Ȃ珈�����Ȃ�
            if (_rimMove <= _rimClamp.y) { return; }

            //�ԉ���Active
            _redFire.SetActive(true);

            //�u�[���A���̕\��
            _rimMove = Mathf.MoveTowards(_rimMove, _rimClamp.y, Time.deltaTime * _changeBospeed);

            //�u�[���A���̕ύX
            ChangeFloat(_transMaterials, _bo, 1);
            ChangeFloat(_opaMaterials, _bo, 1);

            //�����̕ύX
            //ChangeFloat(_transMaterials, _rim, _rimMove);
            ChangeFloat(_opaMaterials, _rim, _rimMove);

        }
        else
        {

            //���l�Ȃ珈�����Ȃ�
            if (_rimMove >= _rimClamp.x) { return; }

            //�ԉ���Stop
            _redFire.SetActive(false);

            //�u�[���A���̔�\��
            _rimMove = Mathf.MoveTowards(_rimMove, _rimClamp.x, Time.deltaTime * _changeBospeed);

            //�u�[���A���t���O�̕ύX
            ChangeFloat(_transMaterials, _bo, 0);
            ChangeFloat(_opaMaterials, _bo, 0);

            //�����̕ύX
            //ChangeFloat(_transMaterials, _rim, _rimMove);
            ChangeFloat(_opaMaterials, _rim, _rimMove);

        }

    }

    /// <summary>
    /// �}�e���A���l�̕ύX(Dissolve / Rim)
    /// </summary>
    private void ChangeFloat(Material[] materials, int id, float speed)
    {
        //�}�e���A���̃f�B�]���u
        materials[0].SetFloat(id, speed);
        materials[1].SetFloat(id, speed);

    }

    /// <summary>
    /// �f�B�]���u�ύX
    /// </summary>
    private void ChangeDissolve(Material[] plusMaterial, Material[] minusMaterial)
    {

        //����
        if (!_isDis)
        {

            //�f�B�]���u���Z
            _disMove += Time.deltaTime * _changeDisSpeed;

            if (_disMove >= 1)
            {

                _disMove = 1;

                //�}�e���A���̕ύX
                ChangeMaterial(minusMaterial);

                _isDis = true;

            }

            //�}�e���A���̃f�B�]���u
            ChangeFloat(plusMaterial, _dis, _disMove);

        }
        //�o��
        else if (_isDis)
        {

            //�f�B�]���u���Z
            _disMove -= Time.deltaTime * _changeDisSpeed;

            if (_disMove <= 0)
            {

                _disMove = 0;

                //�E���g���̕ύX
                if (_isUlt) { _isUlt = false; }
                else if (!_isUlt) { _isUlt = true; }

                _isDis = false;

                _motion = Motion.OPAQUA;

            }

            //�}�e���A���̃f�B�]���u
            ChangeFloat(minusMaterial, _dis, _disMove);

        }

    }

}
