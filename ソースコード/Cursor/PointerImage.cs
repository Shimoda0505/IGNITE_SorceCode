using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ���b�N�I������Cursor�̃J���[��ύX
/// </summary>
public class PointerImage : MonoBehaviour
{

    [SerializeField,Tooltip("�q�I�u�W�F�N�g�Ŏg�p����Ă�|�C���^�[�̃C���[�W")]
    private Image[] _pointerImages;


    [SerializeField,Tooltip("�e���ˎ��̃J���[")]
    private Color _shotColor;

    [SerializeField,Tooltip("�ʏ펞�̃J���[")]
    private Color _defaultColor;


    /// <summary>
    /// �q�I�u�W�F�N�g�Ŏg�p����Ă�|�C���^�[��e���ˎ��̃J���[�ɕύX
    /// </summary>
    public void ShotColorChange()
    {

        for (int i = 0; i <= _pointerImages.Length - 1; i++)
        {

            //�e���ˎ��̃J���[�ɂ���
            _pointerImages[i].GetComponent<Image>().color = _shotColor;
        }

    }

    /// <summary>
    /// �q�I�u�W�F�N�g�Ŏg�p����Ă�|�C���^�[��ʏ펞�̃J���[�ɕύX
    /// </summary>
    public void DefaultColorChange()
    {

        for (int i = 0; i <= _pointerImages.Length - 1; i++)
        {

            //�e���ˎ��̃J���[�ɂ���
            _pointerImages[i].GetComponent<Image>().color = _defaultColor;
        }

    }
}
