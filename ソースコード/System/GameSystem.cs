using UnityEngine;



/// <summary>
/// ゲーム全体のイベント中かを管理
/// </summary>
public class GameSystem : MonoBehaviour
{

    //カーソルを消す

    private void Start()
    {
        Cursor.visible = false;
    }

    //イベントのフラグ-----------------------------------------------------------------------------------

    public bool _isEvent;

    /// <summary>
    /// Eventの終了
    /// </summary>
    public void FalseIsEvent()
    {

        _isEvent = false;
    }

    /// <summary>
    /// Eventの開始
    /// </summary>
    public void TrueIsEvent()
    {

        _isEvent = true;
    }
}
