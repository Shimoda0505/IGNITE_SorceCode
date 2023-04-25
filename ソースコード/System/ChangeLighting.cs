using UnityEngine;



/// <summary>
/// ステージのLighting変更
/// </summary>
public class ChangeLighting : MonoBehaviour
{

    [SerializeField]
    RootNav _rootNav;

    AnyUseMethod _anyUseMethod;

    [Header("環境光設定")]
    [SerializeField, Tooltip("環境光の色")]
    private Color _changeColor;

    //初期の環境光の色
    private Color _defColor;

    private Color _color;

    [SerializeField]
    private float _colorSpeed;


    [Header("フォグ")]
    [SerializeField, Tooltip("フォグの距離")]
    private float _changeFogDensity;

    //初期のフォグの距離
    private float _defFogDensity;

    private float _fog;

    [SerializeField]
    private float _fogSpeed;

    [Header("時間")]
    [SerializeField, Tooltip("時間")]
    private float _time;
    private float _count = 0;


    [Header("ルート関連")]
    [SerializeField, Tooltip("変更ポイント")]
    private GameObject[] _changePoints;

    //変更番号
    private int _number = 0;


    [SerializeField]
    Motion _motion = Motion.WAIT;
    enum Motion
    {
        
        WAIT,
        CHANGE
    }


    //処理部------------------------------------------------------------------------------------------------------------------

    private void Start()
    {

        //初期の環境光の色
        _defColor = RenderSettings.ambientSkyColor;


        //初期のフォグの距離
        _defFogDensity = RenderSettings.fogDensity;

    }

    private void FixedUpdate()
    {


        switch (_motion)
        {

            case Motion.WAIT:

                //番号オブジェクトなら処理
                if (_rootNav.NowPoint() == _changePoints[_number])
                {

                    //変更
                    if (_number == 0) { Change(_changeColor, _changeFogDensity); }

                    //初期化
                    else if (_number == 1) { Change(_defColor, _defFogDensity);}

                    _number++;

                }

                break;


            case Motion.CHANGE:

                //光の変更
                RenderSettings.ambientSkyColor = _anyUseMethod.MoveToWardsColorVector3(RenderSettings.ambientSkyColor, _color, _colorSpeed);

                //フォグの変更
                RenderSettings.fogDensity = Mathf.MoveTowards(RenderSettings.fogDensity, _fog, _fogSpeed);


                _count += Time.deltaTime;
                if (_count >= _time)
                {

                    if(_number > _changePoints.Length - 1) { this.gameObject.GetComponent<ChangeLighting>().enabled = false; }

                    _count = 0;
                    _motion = Motion.WAIT;

                }

                break;
        
        }


    }



    //メソッド部----------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 変更(色 / 距離)
    /// </summary>
    private void Change(Color color,float density) { _color = color; _fog = density; _motion = Motion.CHANGE; }

}
