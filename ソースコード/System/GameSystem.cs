using UnityEngine;



/// <summary>
/// �Q�[���S�̂̃C�x���g�������Ǘ�
/// </summary>
public class GameSystem : MonoBehaviour
{

    //�J�[�\��������

    private void Start()
    {
        Cursor.visible = false;
    }

    //�C�x���g�̃t���O-----------------------------------------------------------------------------------

    public bool _isEvent;

    /// <summary>
    /// Event�̏I��
    /// </summary>
    public void FalseIsEvent()
    {

        _isEvent = false;
    }

    /// <summary>
    /// Event�̊J�n
    /// </summary>
    public void TrueIsEvent()
    {

        _isEvent = true;
    }
}
