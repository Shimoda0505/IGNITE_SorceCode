using UnityEngine;



/// <summary>
/// ボスのロックオン部位にスクリプトをセット
/// </summary>
public class RaizoLockOnPos : MonoBehaviour
{

    [SerializeField]
    private GameObject[] _lockPosObjs;


    private void Start()
    {
        
        //ロックオンポイントすべてにスクリプトをつける
        for(int i = 0; i <= _lockPosObjs.Length - 1; i++)
        {

            _lockPosObjs[i].AddComponent<EnemyCameraView>();
        }
    }
}
