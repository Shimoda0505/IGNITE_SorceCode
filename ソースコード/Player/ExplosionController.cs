using UnityEngine;



/// <summary>
/// �v���C���[�̒e���������鎞�̃G�t�F�N�g�����Ǘ�
/// </summary>
public class ExplosionController : MonoBehaviour
{

    public bool isMove;
    private float count;

    [SerializeField]
    private float actFalseTime;

    private void FixedUpdate()
    {

        //���Ԍv����A�N�e�B�u�I��
        count += Time.deltaTime;
        if(count >= actFalseTime)
        {
            count = 0;

            this.gameObject.SetActive(false);
        }
    }
}
