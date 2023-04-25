using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// �Q�[���J�n���o�̊Ǘ�
/// </summary>
public class GameStart : MonoBehaviour
{

    [Header("�X�N���v�g")]
    [SerializeField, Tooltip("�Q�[���̃C�x���g��Time.deltaTime���Ǘ�")]
    GameSystem _gameSystem;

    [SerializeField, Tooltip("�Q�[����BGM")]
    PlayerBgm _gameBgm;


    [Header("���̑�")]
    [SerializeField, Tooltip("���e�B�N��")]
    private GameObject _reticle;

    [SerializeField]
    private Image FadeImage;

    [SerializeField]
    private GameObject _eventCam;

    [SerializeField]
    private GameObject _mainCam;


    private void Start()
    {

        //�C�x���g�J�����J�n
        _eventCam.SetActive(true);

        _mainCam.GetComponent<Camera>().enabled = false;

        //�C�x���g�̊J�n
        _gameSystem.TrueIsEvent();

        //1�Ԃ�Bgm�J�n
        _gameBgm.OneBgm();

        //���e�B�N������
        _reticle.SetActive(false);

        //�t�F�[�h�C��
        StartCoroutine("FadeIn");

    }


�@�@public void StartCamEnd()
    {

        //�C�x���g�̏I��
        _gameSystem.FalseIsEvent();

        _mainCam.GetComponent<Camera>().enabled = true;

        _reticle.SetActive(true);

    }

    //�t�F�[�h�C��
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
