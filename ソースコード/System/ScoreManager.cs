using UnityEngine;



/// <summary>
/// ゲームクリア時にスコアを計算
/// </summary>
public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    InGameResultSystem _inGameResultSystem;/*【他メンバーが制作したため添付してません】*/

    [SerializeField]
    GameSystem _gameSystem;

    [SerializeField]
    PlayerStatus _playerStatus;

    SelectSystem _selectSystem = new SelectSystem();/*【他メンバーが制作したため添付してません】*/
    ParamSet _paramSet = new ParamSet();/*【他メンバーが制作したため添付してません】*/

    [SerializeField, Header("ステージ番号")]
    private int _stageNumber;



    //処理部--------------------------------------------------------------------------------------------------------------------

    private void FixedUpdate()
    {
        _gameTime += Time.deltaTime;
        _gameTime.ToString("n2");
    }



    //メソッド部--------------------------------------------------------------------------------------------------------------------

    //時間
    [SerializeField, Header("時間計測(確認用)")]
    private float _gameTime = 0;


    //撃破数
    [SerializeField, Header("撃破数(確認用)")]
    private float _destroyingEnemy = 0;
    [SerializeField, Header("撃破数(確認用)")]
    private float _destroyingMulti = 0;
    /// <summary>
    /// 撃破数の加算
    /// </summary>
    public void SmashEnemyCount(int lockCount)
    {
        _destroyingEnemy++;

        if(lockCount >= 8) { _destroyingMulti++; }
    }


    //チェイン数
    [SerializeField, Header("最大チェイン数(確認用)")]
    private int _maxChain;
    /// <summary>
    /// 最大チェイン数を更新
    /// </summary>
    public void ScoreUpdate(int newChain)
    {
        if (_maxChain <= newChain) { _maxChain = newChain; }
    }


    //スコア表示
    [SerializeField,Header("S.A.B.C")]
    private float[] _scoreMag;
    /// <summary>
    /// スコアの表示
    /// </summary>
    public void ScoreShowing()
    {

        //ステージ1クリア
        //_selectSystem.ClearStage1();
        ParamSet._isStage1Clear = 1;

        //イベント開始
        _gameSystem.TrueIsEvent();

        //ランク初期値
        string rank = "";

        //残機数に応じてスコア減算
        float death = _playerStatus.PlayerRemaining();
        if(death == 3) { death = 1; }
        else if(death == 2) { death = 0.5f; }
        else if(death == 1) { death = 0.35f; }
        else if(death == 0) { death = 0.2f; }

        //スコア計算
        float points = _destroyingEnemy + _maxChain + _destroyingMulti * 2;
        points = points / death;

        print(points);

        //ランク番号の初期値
        // S,4 A,3 B,2 C,1
        int rankNumber = 0;

        //ランク計算
        if(points >= _scoreMag[0]) { rank = "S"; rankNumber = 4; }
        else if (_scoreMag[0] > points && points >= _scoreMag[1]) { rank = "A"; rankNumber = 3; }
        else if (_scoreMag[1] > points && points >= _scoreMag[2]) { rank = "B"; rankNumber = 2; }
        else if (_scoreMag[2] > points) { rank = "C"; rankNumber = 1; }

        //スコアの表示
        _inGameResultSystem.Score(_destroyingEnemy, _maxChain, _gameTime, rank);

        //スコアの更新
        _paramSet.UpdateScore(_stageNumber, rankNumber, _destroyingEnemy);
    }































    //未使用部--------------------------------------------------------------------------------------------------------------------


    //撃破数の加算
    public void DestroyingEnemy()
    {

        //過去の加算用
    }

}
