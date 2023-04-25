using UnityEngine;



/// <summary>
/// Cursorをロックオン対象に追従
/// </summary>
public class PointerMovePos : MonoBehaviour
{

    #region スクリプト
    [Header("スクリプト")]
    [SerializeField, Tooltip("ゲームのイベントやTime.deltaTimeを管理")]
    GameSystem _gameSystem;

    [SerializeField, Tooltip("プレイヤーのコントローラー")]
    PlayerController _playerController;
    #endregion


    #region ポインター
    [Header("ポインター")]
    [SerializeField, Tooltip("ポインターの移動制限")]
    private Vector2 _moveClamp;
    #endregion


    private void FixedUpdate()
    {

        //時間0もしくは,イベント中なら何もしない
        if (Time.timeScale == 0 || _gameSystem._isEvent)
        {

            return;
        }

        //ロックオンポインターの移動計算
        PointerProcess();

    }


    /// <summary>
    /// ロックオンポインターの移動計算
    /// </summary>
    void PointerProcess()
    {

        //プレイヤーの実際の移動量と最大の移動量から、どれだけの割合移動したかを計算
        Vector2 playerMovePercentage = new Vector2(_playerController.InputMove().x / _playerController.MoveClamp().x,
                                                   _playerController.InputMove().y / _playerController.MoveClamp().y);

        //ポインターの最大移動量に上記の割合を乗算
        Vector2 movePos = new Vector2(_moveClamp.x * playerMovePercentage.x, _moveClamp.y * playerMovePercentage.y);

        //レティクルの移動速度
        this.gameObject.GetComponent<RectTransform>().localPosition = new Vector2(movePos.x, movePos.y);

    }
}
