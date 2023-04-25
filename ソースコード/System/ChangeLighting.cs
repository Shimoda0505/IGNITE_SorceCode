using UnityEngine;



/// <summary>
/// �X�e�[�W��Lighting�ύX
/// </summary>
public class ChangeLighting : MonoBehaviour
{

    [SerializeField]
    RootNav _rootNav;

    AnyUseMethod _anyUseMethod;

    [Header("�����ݒ�")]
    [SerializeField, Tooltip("�����̐F")]
    private Color _changeColor;

    //�����̊����̐F
    private Color _defColor;

    private Color _color;

    [SerializeField]
    private float _colorSpeed;


    [Header("�t�H�O")]
    [SerializeField, Tooltip("�t�H�O�̋���")]
    private float _changeFogDensity;

    //�����̃t�H�O�̋���
    private float _defFogDensity;

    private float _fog;

    [SerializeField]
    private float _fogSpeed;

    [Header("����")]
    [SerializeField, Tooltip("����")]
    private float _time;
    private float _count = 0;


    [Header("���[�g�֘A")]
    [SerializeField, Tooltip("�ύX�|�C���g")]
    private GameObject[] _changePoints;

    //�ύX�ԍ�
    private int _number = 0;


    [SerializeField]
    Motion _motion = Motion.WAIT;
    enum Motion
    {
        
        WAIT,
        CHANGE
    }


    //������------------------------------------------------------------------------------------------------------------------

    private void Start()
    {

        //�����̊����̐F
        _defColor = RenderSettings.ambientSkyColor;


        //�����̃t�H�O�̋���
        _defFogDensity = RenderSettings.fogDensity;

    }

    private void FixedUpdate()
    {


        switch (_motion)
        {

            case Motion.WAIT:

                //�ԍ��I�u�W�F�N�g�Ȃ珈��
                if (_rootNav.NowPoint() == _changePoints[_number])
                {

                    //�ύX
                    if (_number == 0) { Change(_changeColor, _changeFogDensity); }

                    //������
                    else if (_number == 1) { Change(_defColor, _defFogDensity);}

                    _number++;

                }

                break;


            case Motion.CHANGE:

                //���̕ύX
                RenderSettings.ambientSkyColor = _anyUseMethod.MoveToWardsColorVector3(RenderSettings.ambientSkyColor, _color, _colorSpeed);

                //�t�H�O�̕ύX
                RenderSettings.fogDensity = Mathf.MoveTowards(RenderSettings.fogDensity, _fog, _fogSpeed);


                _count += Time.deltaTime;
                if (_count >= _time)
                {

                    if(_number > _changePoints.Length - 1) { this.gameObject.GetComponent<ChangeLighting>().enabled = false; }

                    _count = 0;
                    _motion = Motion.WAIT;

                }

                break;
        
        }


    }



    //���\�b�h��----------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// �ύX(�F / ����)
    /// </summary>
    private void Change(Color color,float density) { _color = color; _fog = density; _motion = Motion.CHANGE; }

}
