using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ゲーム開始演出の管理
/// </summary>
public class GameStart : MonoBehaviour
{

    [Header("スクリプト")]
    [SerializeField, Tooltip("ゲームのイベントやTime.deltaTimeを管理")]
    GameSystem _gameSystem;

    [SerializeField, Tooltip("ゲームのBGM")]
    PlayerBgm _gameBgm;


    [Header("その他")]
    [SerializeField, Tooltip("レティクル")]
    private GameObject _reticle;

    [SerializeField]
    private Image FadeImage;

    [SerializeField]
    private GameObject _eventCam;

    [SerializeField]
    private GameObject _mainCam;


    private void Start()
    {

        //イベントカメラ開始
        _eventCam.SetActive(true);

        _mainCam.GetComponent<Camera>().enabled = false;

        //イベントの開始
        _gameSystem.TrueIsEvent();

        //1番のBgm開始
        _gameBgm.OneBgm();

        //レティクル消す
        _reticle.SetActive(false);

        //フェードイン
        StartCoroutine("FadeIn");

    }


　　public void StartCamEnd()
    {

        //イベントの終了
        _gameSystem.FalseIsEvent();

        _mainCam.GetComponent<Camera>().enabled = true;

        _reticle.SetActive(true);

    }

    //フェードイン
    IEnumerator FadeIn()
    {
        FadeImage.color = new Color(
                                      FadeImage.color.r,
                                      FadeImage.color.g,
                                      FadeImage.color.b,
                                      1);

        for (float i = 1; i > 0; i -= 0.01f)
        {
            FadeImage.color = new Color(
                                      FadeImage.color.r,
                                      FadeImage.color.g,
                                      FadeImage.color.b,
                                      i);
            yield return new WaitForSecondsRealtime(.01f);
        }

    }

}
