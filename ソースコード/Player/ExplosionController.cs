using UnityEngine;



/// <summary>
/// プレイヤーの弾が爆発する時のエフェクト挙動管理
/// </summary>
public class ExplosionController : MonoBehaviour
{

    public bool isMove;
    private float count;

    [SerializeField]
    private float actFalseTime;

    private void FixedUpdate()
    {

        //時間計測後アクティブ終了
        count += Time.deltaTime;
        if(count >= actFalseTime)
        {
            count = 0;

            this.gameObject.SetActive(false);
        }
    }
}
