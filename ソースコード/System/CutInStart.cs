using UnityEngine;


/// <summary>
/// ボス戦時のカットイン管理
/// </summary>
public class CutInStart : MonoBehaviour
{

    [SerializeField]
    RootNav _rootNav;

    [SerializeField]
    Cutin _cutin;/*【他メンバーが制作したため添付してません】*/

    [SerializeField]
    PlayerMoveSpline _playerMoveSpline;

    [SerializeField]
    GameSystem _gameSystem;

    [SerializeField]
    BossStatus _bossStatus;

    [SerializeField]
    PlayerBgm _playerBgm;

    [SerializeField]
    private GameObject _startPosObj;


    [SerializeField]
    private float _time;
    private float _count = 0;


    Motion _motion = Motion.WAIT;
    enum Motion
    {
        WAIT,
        EVENT
    }

    private void FixedUpdate()
    {
        switch (_motion)
        {

            case Motion.WAIT:

                if (_rootNav.NowPoint() == _startPosObj)
                {

                    _playerMoveSpline.ChangeMoveSpeed("停止");

                    _gameSystem.TrueIsEvent();

                    _playerBgm.SecondBgm();

                    //カットインの開始
                    _cutin.RaizoCutIn();

                    _motion = Motion.EVENT;

                }

                break;


            case Motion.EVENT:

                _count += Time.deltaTime;
                if(_count >= _time) { _playerMoveSpline.ChangeMoveSpeed("移動"); }

                if(_cutin.IsCutIn())
                {
                    _count = 0;

                    //ボス戦闘開始
                    _bossStatus.IsStart();

                    _gameSystem.FalseIsEvent();

                    this.gameObject.SetActive(false);
                }

                break;
        
        }

    }
}
