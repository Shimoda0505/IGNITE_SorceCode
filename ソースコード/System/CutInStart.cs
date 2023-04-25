using UnityEngine;


/// <summary>
/// �{�X�펞�̃J�b�g�C���Ǘ�
/// </summary>
public class CutInStart : MonoBehaviour
{

    [SerializeField]
    RootNav _rootNav;

    [SerializeField]
    Cutin _cutin;/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/

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

                    _playerMoveSpline.ChangeMoveSpeed("��~");

                    _gameSystem.TrueIsEvent();

                    _playerBgm.SecondBgm();

                    //�J�b�g�C���̊J�n
                    _cutin.RaizoCutIn();

                    _motion = Motion.EVENT;

                }

                break;


            case Motion.EVENT:

                _count += Time.deltaTime;
                if(_count >= _time) { _playerMoveSpline.ChangeMoveSpeed("�ړ�"); }

                if(_cutin.IsCutIn())
                {
                    _count = 0;

                    //�{�X�퓬�J�n
                    _bossStatus.IsStart();

                    _gameSystem.FalseIsEvent();

                    this.gameObject.SetActive(false);
                }

                break;
        
        }

    }
}
