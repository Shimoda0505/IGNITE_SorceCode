using UnityEngine;



/// <summary>
/// �v���C���[���ړ����郋�[�g��ύX
/// </summary>
public class ChangeRootNav : MonoBehaviour
{
    [SerializeField]
    private GameObject _mountainObj;

    [SerializeField]
    RootNav rootNav;

    [SerializeField, Header("�X�V�ʒu")]
    private GameObject updatePos;


    private void FixedUpdate()
    {

        //���̃X�v���C���ɍX�V
        if (updatePos == rootNav.NowPoint())
        {

            _mountainObj.SetActive(true);

            this.gameObject.GetComponent<ChangeRootNav>().enabled = false;
        }
    }
}
