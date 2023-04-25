using UnityEngine;



/// <summary>
/// Cursor�����b�N�I���ΏۂɒǏ]
/// </summary>
public class PointerMovePos : MonoBehaviour
{

    #region �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField, Tooltip("�Q�[���̃C�x���g��Time.deltaTime���Ǘ�")]
    GameSystem _gameSystem;

    [SerializeField, Tooltip("�v���C���[�̃R���g���[���[")]
    PlayerController _playerController;
    #endregion


    #region �|�C���^�[
    [Header("�|�C���^�[")]
    [SerializeField, Tooltip("�|�C���^�[�̈ړ�����")]
    private Vector2 _moveClamp;
    #endregion


    private void FixedUpdate()
    {

        //����0��������,�C�x���g���Ȃ牽�����Ȃ�
        if (Time.timeScale == 0 || _gameSystem._isEvent)
        {

            return;
        }

        //���b�N�I���|�C���^�[�̈ړ��v�Z
        PointerProcess();

    }


    /// <summary>
    /// ���b�N�I���|�C���^�[�̈ړ��v�Z
    /// </summary>
    void PointerProcess()
    {

        //�v���C���[�̎��ۂ̈ړ��ʂƍő�̈ړ��ʂ���A�ǂꂾ���̊����ړ����������v�Z
        Vector2 playerMovePercentage = new Vector2(_playerController.InputMove().x / _playerController.MoveClamp().x,
                                                   _playerController.InputMove().y / _playerController.MoveClamp().y);

        //�|�C���^�[�̍ő�ړ��ʂɏ�L�̊�������Z
        Vector2 movePos = new Vector2(_moveClamp.x * playerMovePercentage.x, _moveClamp.y * playerMovePercentage.y);

        //���e�B�N���̈ړ����x
        this.gameObject.GetComponent<RectTransform>().localPosition = new Vector2(movePos.x, movePos.y);

    }
}
