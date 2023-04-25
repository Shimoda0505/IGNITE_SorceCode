using UnityEngine;



/// <summary>
/// ロックオン数やウルト使用に応じて、カーソルの見た目を変更
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
        
        //ウルトレティクルがアクティブなら処理
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
    /// レティクルの変更
    /// </summary>
    public void ChangeRet()
    {

        //ノーマル
        if(_nomalRet.activeSelf)
        {

            _nomalRet.SetActive(false);

            _ultRet.SetActive(true);

            _pointerLockOn.ChangePointerClamp("ウルト");

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
