using UnityEngine;


/// <summary>
/// �f�o�b�N
/// </summary>
public class DebugSystem : MonoBehaviour
{
    [Header("Q/�{��")]
    [Header("W/����")]
    [Header("A/�_���[�W(100)")]
    [Header("C/�J�����h��")]
    [Header("D/���S")]
    [Header("P/�J����view")]
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
