using UnityEngine;



/// <summary>
/// �Q�[���N���A���ɃX�R�A���v�Z
/// </summary>
public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    InGameResultSystem _inGameResultSystem;/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/

    [SerializeField]
    GameSystem _gameSystem;

    [SerializeField]
    PlayerStatus _playerStatus;

    SelectSystem _selectSystem = new SelectSystem();/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/
    ParamSet _paramSet = new ParamSet();/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/

    [SerializeField, Header("�X�e�[�W�ԍ�")]
    private int _stageNumber;



    //������--------------------------------------------------------------------------------------------------------------------

    private void FixedUpdate()
    {
        _gameTime += Time.deltaTime;
        _gameTime.ToString("n2");
    }



    //���\�b�h��--------------------------------------------------------------------------------------------------------------------

    //����
    [SerializeField, Header("���Ԍv��(�m�F�p)")]
    private float _gameTime = 0;


    //���j��
    [SerializeField, Header("���j��(�m�F�p)")]
    private float _destroyingEnemy = 0;
    [SerializeField, Header("���j��(�m�F�p)")]
    private float _destroyingMulti = 0;
    /// <summary>
    /// ���j���̉��Z
    /// </summary>
    public void SmashEnemyCount(int lockCount)
    {
        _destroyingEnemy++;

        if(lockCount >= 8) { _destroyingMulti++; }
    }


    //�`�F�C����
    [SerializeField, Header("�ő�`�F�C����(�m�F�p)")]
    private int _maxChain;
    /// <summary>
    /// �ő�`�F�C�������X�V
    /// </summary>
    public void ScoreUpdate(int newChain)
    {
        if (_maxChain <= newChain) { _maxChain = newChain; }
    }


    //�X�R�A�\��
    [SerializeField,Header("S.A.B.C")]
    private float[] _scoreMag;
    /// <summary>
    /// �X�R�A�̕\��
    /// </summary>
    public void ScoreShowing()
    {

        //�X�e�[�W1�N���A
        //_selectSystem.ClearStage1();
        ParamSet._isStage1Clear = 1;

        //�C�x���g�J�n
        _gameSystem.TrueIsEvent();

        //�����N�����l
        string rank = "";

        //�c�@���ɉ����ăX�R�A���Z
        float death = _playerStatus.PlayerRemaining();
        if(death == 3) { death = 1; }
        else if(death == 2) { death = 0.5f; }
        else if(death == 1) { death = 0.35f; }
        else if(death == 0) { death = 0.2f; }

        //�X�R�A�v�Z
        float points = _destroyingEnemy + _maxChain + _destroyingMulti * 2;
        points = points / death;

        print(points);

        //�����N�ԍ��̏����l
        // S,4 A,3 B,2 C,1
        int rankNumber = 0;

        //�����N�v�Z
        if(points >= _scoreMag[0]) { rank = "S"; rankNumber = 4; }
        else if (_scoreMag[0] > points && points >= _scoreMag[1]) { rank = "A"; rankNumber = 3; }
        else if (_scoreMag[1] > points && points >= _scoreMag[2]) { rank = "B"; rankNumber = 2; }
        else if (_scoreMag[2] > points) { rank = "C"; rankNumber = 1; }

        //�X�R�A�̕\��
        _inGameResultSystem.Score(_destroyingEnemy, _maxChain, _gameTime, rank);

        //�X�R�A�̍X�V
        _paramSet.UpdateScore(_stageNumber, rankNumber, _destroyingEnemy);
    }































    //���g�p��--------------------------------------------------------------------------------------------------------------------


    //���j���̉��Z
    public void DestroyingEnemy()
    {

        //�ߋ��̉��Z�p
    }

}
