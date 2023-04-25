using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ロックオン時にCursorのカラーを変更
/// </summary>
public class PointerImage : MonoBehaviour
{

    [SerializeField,Tooltip("子オブジェクトで使用されてるポインターのイメージ")]
    private Image[] _pointerImages;


    [SerializeField,Tooltip("弾発射時のカラー")]
    private Color _shotColor;

    [SerializeField,Tooltip("通常時のカラー")]
    private Color _defaultColor;


    /// <summary>
    /// 子オブジェクトで使用されてるポインターを弾発射時のカラーに変更
    /// </summary>
    public void ShotColorChange()
    {

        for (int i = 0; i <= _pointerImages.Length - 1; i++)
        {

            //弾発射時のカラーにする
            _pointerImages[i].GetComponent<Image>().color = _shotColor;
        }

    }

    /// <summary>
    /// 子オブジェクトで使用されてるポインターを通常時のカラーに変更
    /// </summary>
    public void DefaultColorChange()
    {

        for (int i = 0; i <= _pointerImages.Length - 1; i++)
        {

            //弾発射時のカラーにする
            _pointerImages[i].GetComponent<Image>().color = _defaultColor;
        }

    }
}
