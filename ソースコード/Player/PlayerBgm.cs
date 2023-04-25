using UnityEngine;



/// <summary>
/// ゲーム全体のBgm管理
/// </summary>
public class PlayerBgm : MonoBehaviour
{

    #region 全てで使用
    [SerializeField,Tooltip("BgmのAudioSource")]
    AudioSource _audioSource;

    [SerializeField, Tooltip("Bgmの音量速度")]
    private float _bgmChangeSpeed;

    //Bgmの現在の状態
    private BgmMotion _bgmMotion = BgmMotion.WAIT;
    enum BgmMotion
    { 
        WAIT,//再生中
        START,//開始
        STOP,//終了   
    }

    //ボリュームの最低値
    private const int _minVolume = 0;

    //ボリュームの最大値
    private float _maxVolume = 1;


    private void Start()
    {
        _maxVolume = ParamSet.BGM_Volume;/*【他メンバーが制作したため添付してません】*/

        _audioSource.loop = true;

    }

    private void FixedUpdate()
    {

        switch (_bgmMotion)
        {

            //再生中
            case BgmMotion.WAIT:

                return;


            //開始
            case BgmMotion.START:

                //オーディオの音量を上げる
                _audioSource.volume += Time.deltaTime * _bgmChangeSpeed;

                if (_audioSource.volume >= _maxVolume)
                {

                    _bgmMotion = BgmMotion.WAIT;
                }


                break;


            //終了 
            case BgmMotion.STOP:

                //オーディオの音量を下げる
                _audioSource.volume -= Time.deltaTime * _bgmChangeSpeed;

                if (_audioSource.volume <= _minVolume)
                {
                    _audioSource.Stop();
                }

                break;
        
        }

    }
    #endregion

    [Header("StageのBgm")]
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

    [SerializeField, Tooltip("クリア")]
    AudioClip _clearBgm;
    public void ClearBgm()
    {
        _audioSource.Stop();

        _audioSource.clip = _clearBgm;
        _audioSource.Play();

    }

    [SerializeField, Tooltip("ゲームオーバー")]
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
