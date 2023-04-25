using UnityEngine;



/// <summary>
/// プレイヤーのエフェクトを自動でアクティブ終了
/// </summary>
public class PlayerEffectLooping : MonoBehaviour
{
    [SerializeField, Tooltip("消えるまでの時間")]
    private float _activeTime;

    //時間の計測
    private float _activeCount = 0;


    private void FixedUpdate()
    {

        //時間の計測
        _activeCount += Time.deltaTime;

        //時間計測後
        if(_activeCount >= _activeTime)
        {

            //時間の初期化
            _activeCount = 0;

            //アクティブ終了
            this.gameObject.SetActive(false);

        }
    }

    public void ResetCount() { _activeCount = 0; }
}
