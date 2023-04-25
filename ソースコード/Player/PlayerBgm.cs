using UnityEngine;



/// <summary>
/// �Q�[���S�̂�Bgm�Ǘ�
/// </summary>
public class PlayerBgm : MonoBehaviour
{

    #region �S�ĂŎg�p
    [SerializeField,Tooltip("Bgm��AudioSource")]
    AudioSource _audioSource;

    [SerializeField, Tooltip("Bgm�̉��ʑ��x")]
    private float _bgmChangeSpeed;

    //Bgm�̌��݂̏��
    private BgmMotion _bgmMotion = BgmMotion.WAIT;
    enum BgmMotion
    { 
        WAIT,//�Đ���
        START,//�J�n
        STOP,//�I��   
    }

    //�{�����[���̍Œ�l
    private const int _minVolume = 0;

    //�{�����[���̍ő�l
    private float _maxVolume = 1;


    private void Start()
    {
        _maxVolume = ParamSet.BGM_Volume;/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/

        _audioSource.loop = true;

    }

    private void FixedUpdate()
    {

        switch (_bgmMotion)
        {

            //�Đ���
            case BgmMotion.WAIT:

                return;


            //�J�n
            case BgmMotion.START:

                //�I�[�f�B�I�̉��ʂ��グ��
                _audioSource.volume += Time.deltaTime * _bgmChangeSpeed;

                if (_audioSource.volume >= _maxVolume)
                {

                    _bgmMotion = BgmMotion.WAIT;
                }


                break;


            //�I�� 
            case BgmMotion.STOP:

                //�I�[�f�B�I�̉��ʂ�������
                _audioSource.volume -= Time.deltaTime * _bgmChangeSpeed;

                if (_audioSource.volume <= _minVolume)
                {
                    _audioSource.Stop();
                }

                break;
        
        }

    }
    #endregion

    [Header("Stage��Bgm")]
    [SerializeField, Tooltip("1Stage")]
    AudioClip _oneBgm;
    public void OneBgm()
    {
        _audioSource.clip = _oneBgm;
        _audioSource.Play();

        _bgmMotion = BgmMotion.START;

    }

    [SerializeField, Tooltip("2Stage")]
    AudioClip _secondBgm;
    public void SecondBgm()
    {
        _audioSource.clip = _secondBgm;
        _audioSource.Play();

        _bgmMotion = BgmMotion.START;

    }

    [SerializeField, Tooltip("3Stage")]
    AudioClip _thirdBgm;
    public void ThirdBgm()
    {
        _audioSource.clip = _thirdBgm;
        _audioSource.Play();

        _bgmMotion = BgmMotion.START;

    }

    [SerializeField, Tooltip("�N���A")]
    AudioClip _clearBgm;
    public void ClearBgm()
    {
        _audioSource.Stop();

        _audioSource.clip = _clearBgm;
        _audioSource.Play();

    }

    [SerializeField, Tooltip("�Q�[���I�[�o�[")]
    AudioClip _gameOverBgm;
    public void GameOverBgm()
    {
        _audioSource.Stop();

        _audioSource.loop = false;
        _audioSource.clip = _gameOverBgm;
        _audioSource.Play();

    }


    public void BgmStops()
    {
        _bgmMotion = BgmMotion.STOP;
    }

}
