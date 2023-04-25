using UnityEngine;



/// <summary>
/// Cursor�̍��W���ړ�������
/// </summary>
public class CursorController : MonoBehaviour
{

    //�Ǐ]����^�[�Q�b�g
    public GameObject _target;

    //�q�I�u�W�F�N�g�̃J�[�\�����Ǘ�
    [SerializeField]
    PointerImage _pointerImage;

    [SerializeField]
    GameSystem _gameSystem;

    [SerializeField]
    private RectTransform _rect;

    private Vector2 _rectPos;

    private Vector2 _upRect = new Vector2(0, 20);

    //����-------------------------------------------------------------------------------------------------

    private void FixedUpdate()
    {

        //�^�[�Q�b�g��Bull�ɂȂ�����
        if(_target ==null)
        {

            //�J�[�\���̃J���[�ύX
            _pointerImage.DefaultColorChange();

            //�J�[�\���̃A�N�e�B�u������
            this.gameObject.SetActive(false);

            return;
        }


        if (_gameSystem._isEvent)
        {

            return;
        }

        //�^�[�Q�b�g�̃X�N���[�����W�ɕϊ�
        Vector2 enemyPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.transform.position);

        //�X�N���[�����W��RectTransfirm�ɕϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, enemyPos, Camera.main, out _rectPos);

        //�^�[�Q�b�g�̍��W�Ɉړ�
        this.gameObject.GetComponent<RectTransform>().localPosition = _rectPos + _upRect;
    }
}
