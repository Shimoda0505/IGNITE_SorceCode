using UnityEngine;


/// <summary>
/// 敵が初めて画面に映った多岐にCursorで知らせる際の、Cursor挙動管理
/// </summary>
public class InViewImage : MonoBehaviour
{
    //追従するターゲット
    public GameObject _target;

    [SerializeField]
    GameSystem _gameSystem;

    [SerializeField]
    private RectTransform _rect;

    private Vector2 _rectPos;
    private Vector2 _upRect = new Vector2(0, 20);

    private float _time = 1f;
    private float _count = 0;

    //処理-------------------------------------------------------------------------------------------------

    private void FixedUpdate()
    {

        if (_gameSystem._isEvent) { return; }

        //時間計測後に停止
        _count += Time.deltaTime;
        if(_count >= _time) { _count = 0; this.gameObject.SetActive(false); }

        //ターゲットのスクリーン座標に変換
        Vector2 enemyPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.transform.position);

        //スクリーン座標をRectTransfirmに変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, enemyPos, Camera.main, out _rectPos);

        //ターゲットの座標に移動
        this.gameObject.GetComponent<RectTransform>().localPosition = _rectPos + _upRect;
    }

}
