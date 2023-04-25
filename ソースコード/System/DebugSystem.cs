using UnityEngine;


/// <summary>
/// デバック
/// </summary>
public class DebugSystem : MonoBehaviour
{
    [Header("Q/倍速")]
    [Header("W/等速")]
    [Header("A/ダメージ(100)")]
    [Header("C/カメラ揺れ")]
    [Header("D/死亡")]
    [Header("P/カメラview")]
    [SerializeField]
    PlayerStatus _playerStatus;

    [SerializeField]
    CameraShake _cameraShake;

    [SerializeField]
    PlayerController _playerController;

    [SerializeField]
    PlayerBgm _playerBgm;

    [SerializeField]
    ScoreManager _scoreManager;

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = 50;
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _playerStatus.Hit(100);
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            _cameraShake.Shake(3,1);
        }
    }
}
