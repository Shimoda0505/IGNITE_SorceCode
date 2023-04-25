using System.Collections;
using UnityEngine;


/// <summary>
/// カメラを揺らすためのスクリプト
/// </summary>
public class CameraShake : MonoBehaviour
{

    [SerializeField]
    private Transform _moveObj;

    [SerializeField]
    GameSystem _gameSystem;

    private float xRange;//x軸の揺れ幅
    private float yRange;//Y軸の揺れ幅

    private float shakeX;
    private float shakeY;

    [SerializeField]
    private bool _isShake;

    public void ShakeRaizo()
    {
        Shake(1, 1);
    }

    /// <summary>
    /// カメラの揺れ
    /// </summary>
    /// <param name="time">揺れ時間</param>
    /// <param name="magnitude">揺れ幅</param>
    public void Shake(float time, float magnitude)
    {
        if(!_isShake)
        {
            StartCoroutine(DoShake(time, magnitude));//他スクリプトからの引数

            _isShake = true;
        }
    }

    private IEnumerator DoShake(float time, float magnitude)
    {

        //初期値1
        xRange = 0;
        yRange = 0;

        var elapsed = 0f;//タイマー

        while (elapsed < time)//time時間揺らし続ける
        {

            if(_gameSystem._isEvent)
            {
                _moveObj.localPosition = new Vector3(0, 0, _moveObj.localPosition.z);//x軸y軸を上記分移動

                //初期値1
                xRange = 0;
                yRange = 0;

                yield break;
            }

            //上下左右にRange*magunitude 分移動
            shakeX = Random.Range(-xRange, xRange) * magnitude;
            shakeY = Random.Range(-yRange, yRange) * magnitude;

            _moveObj.localPosition = new Vector3(shakeX, shakeY, _moveObj.localPosition.z);//x軸y軸を上記分移動

            elapsed += Time.deltaTime;//タイマー

            //全体の1/3秒間,揺れを大きくしていく
            if (elapsed < time / 2)
            {
                xRange += Time.deltaTime;
                yRange += Time.deltaTime;
            }
            //1/3秒後,揺れを小さくしていく
            else
            {
                xRange -= Time.deltaTime;

                if(xRange <= 0)
                {
                    xRange = 0;
                }

                yRange -= Time.deltaTime;

                if(yRange <= 0)
                {
                    yRange = 0;
                }

            }

            yield return null;

        }

        _isShake = false;

    }
}
