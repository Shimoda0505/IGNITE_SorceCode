using UnityEngine;



/// <summary>
/// プレイヤーのシールドエフェクトをディゾルブ
/// </summary>
public class ShildEffect : MonoBehaviour
{

    //シェーダー
    [SerializeField]
    Material _material;

    [SerializeField]
    PlayerStatus _playerStatus;

    [SerializeField]
    private int _dis = 0;

    private void Start()
    {

        _dis = Shader.PropertyToID("_Dissolve");
    }

    void FixedUpdate()
    {

        float disVol = 1 - _playerStatus.ShildVolume();

        _material.SetFloat(_dis, disVol);

        if (disVol >= 1)
        {

            this.gameObject.SetActive(false);
        }
    }
}
