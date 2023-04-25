using UnityEngine;



/// <summary>
/// ���b�N�I������E���g�g�p�ɉ����āA�J�[�\���̌����ڂ�ύX
/// </summary>
public class PointerChange : MonoBehaviour
{

    [SerializeField]
    PointerLockOn _pointerLockOn;

    [SerializeField]
    private GameObject _nomalRet;

    [SerializeField]
    private GameObject _nomalRet2;

    [SerializeField]
    private GameObject _nomalRet3;


    [SerializeField]
    private GameObject _ultRet;

    [SerializeField]
    private Animator _ultRetAnim;

    private bool _isUlt = false;

    [SerializeField]
    private float _time;

    private float _count = 0;



    private void FixedUpdate()
    {
        
        //�E���g���e�B�N�����A�N�e�B�u�Ȃ珈��
        if(_isUlt)
        {

            _count += Time.deltaTime;
            if(_count >= _time)
            {

                _count = 0;

                _isUlt = false;

                _nomalRet.SetActive(true);

                _ultRet.SetActive(false);

            }

        }

    }


    /// <summary>
    /// ���e�B�N���̕ύX
    /// </summary>
    public void ChangeRet()
    {

        //�m�[�}��
        if(_nomalRet.activeSelf)
        {

            _nomalRet.SetActive(false);

            _ultRet.SetActive(true);

            _pointerLockOn.ChangePointerClamp("�E���g");

            _ultRetAnim.SetTrigger("Move");

        }
        else if (!_nomalRet.activeSelf)
        {

            _isUlt = true;

            _ultRetAnim.SetTrigger("ReMove");
        }

    }


    public void Change1()
    {
        _nomalRet.transform.localScale = new Vector2(1, 1);

        _nomalRet2.SetActive(false);

        _nomalRet3.SetActive(false);
    }
    public void Change2()
    {

        _nomalRet.transform.localScale = new Vector2(1.4f, 1.4f);

        _nomalRet2.SetActive(true);

        _nomalRet3.SetActive(false);
    }
    public void Change3()
    {

        _nomalRet.transform.localScale = new Vector2(1.8f, 1.8f);

        _nomalRet2.SetActive(true);

        _nomalRet3.SetActive(true);
    }

}
