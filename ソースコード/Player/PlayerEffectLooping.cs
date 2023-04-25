using UnityEngine;



/// <summary>
/// �v���C���[�̃G�t�F�N�g�������ŃA�N�e�B�u�I��
/// </summary>
public class PlayerEffectLooping : MonoBehaviour
{
    [SerializeField, Tooltip("������܂ł̎���")]
    private float _activeTime;

    //���Ԃ̌v��
    private float _activeCount = 0;


    private void FixedUpdate()
    {

        //���Ԃ̌v��
        _activeCount += Time.deltaTime;

        //���Ԍv����
        if(_activeCount >= _activeTime)
        {

            //���Ԃ̏�����
            _activeCount = 0;

            //�A�N�e�B�u�I��
            this.gameObject.SetActive(false);

        }
    }

    public void ResetCount() { _activeCount = 0; }
}
