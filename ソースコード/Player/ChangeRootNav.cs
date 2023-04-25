using UnityEngine;



/// <summary>
/// プレイヤーが移動するルートを変更
/// </summary>
public class ChangeRootNav : MonoBehaviour
{
    [SerializeField]
    private GameObject _mountainObj;

    [SerializeField]
    RootNav rootNav;

    [SerializeField, Header("更新位置")]
    private GameObject updatePos;


    private void FixedUpdate()
    {

        //次のスプラインに更新
        if (updatePos == rootNav.NowPoint())
        {

            _mountainObj.SetActive(true);

            this.gameObject.GetComponent<ChangeRootNav>().enabled = false;
        }
    }
}
