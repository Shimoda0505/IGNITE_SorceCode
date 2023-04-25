using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// ボスのステータス管理
/// </summary>
public class BossStatus : MonoBehaviour
{

    #region スクリプト
    [SerializeField, Tooltip("ボスのゲーム終了管理スクリプト")] RaizoGameEnd _raizoGameEnd;/*【他メンバーが制作したため添付してません】*/
    [SerializeField, Tooltip("プレイヤー管理スクリプト")] PlayerController _playerController;
    #endregion

    #region Hp関連
    [Header("Hp関連")]
    [SerializeField, Tooltip("ボスの初期HP")]
    protected float _bossFirst_HP = default;

    //ボスの現在HP
    private float _boss_HP = default;

    //死亡フラグ
    private bool _isDeath = false;

    [SerializeField, Tooltip("Hpバーのイメージ画像")] private Image _hpBar;
    [SerializeField, Tooltip("HpのUIオブジェクト")] private GameObject _hpUIObj;


    /// <summary>
    /// ボスが死亡したかどうか
    /// </summary>
    public bool IsDeath() { return _isDeath; }

    //Hp半分
    private bool _isHpHalf;

    /// <summary>
    /// ボスのHpが半分
    /// </summary>
    public bool IsHpHalf() { return _isHpHalf; }

    private bool _isUlt;
    /// <summary>
    /// ウルト開始
    /// </summary>
    public void IsUltTrue() { _isUlt = true; }
    /// <summary>
    /// ウルト終了
    /// </summary>
    public void IsUltFalse() { _isUlt = false; }
    /// <summary>
    /// ウルトの状況
    /// </summary>
    /// <returns></returns>
    public bool IsUlt() { return _isUlt; }
    #endregion

    //ボス戦開始
    private bool _isStart = false;
    public Motion _motion = Motion.START;
    public enum Motion
    {
        START,
        PLAY,
        END
    }


    //処理群---------------------------------------------------------------------------------------------------------------

    private void Start()
    {

        //ボスのHpを補完
        _boss_HP = _bossFirst_HP;

        //Hpバーの初期化
        _hpBar.fillAmount = 0;

        //Hpの非表示
        _hpUIObj.SetActive(false);
    }

    private void FixedUpdate()
    {

        //ボス戦開始かつenumが開始の時
        if (_isStart && _motion == Motion.START)
        {
            //3秒で最大加算
            _hpBar.fillAmount += Time.deltaTime / 3;

            //Hpバーが最大値ならenumえおplayに変更
            if (_hpBar.fillAmount >= 1) { _motion = Motion.PLAY; }
        }
    }

    //メソッド群---------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// ボスにダメージを与える
    /// </summary>
    public void BossDamage(int damage)
    {

        //プレイヤー死亡中は処理しない
        if (_playerController._playerMotion == PlayerController.PlayerMotion.Death) { return; }

        //ボスのHpを減算
        _boss_HP -= damage;

        //HpBarの更新
        _hpBar.fillAmount = _boss_HP / _bossFirst_HP;

        //Hpが半分以下なら
        if (_boss_HP <= _bossFirst_HP / 2 && !_isHpHalf) { _isHpHalf = true; }

        //Hpが0以下なら死亡
        if (_boss_HP <= 0 && !_isDeath)
        {

            //ボス戦終了
            _motion = Motion.END;

            //ボス戦終了カットイン開始
            _raizoGameEnd.RaizoCutIn();

            //hoバー非表示
            _hpUIObj.SetActive(false);

            //死亡フラグ
            _isDeath = true;

        }

    }

    /// <summary>
    /// ボス戦開始
    /// </summary>
    public void IsStart()
    {

        //Hpバー表示
        _hpUIObj.SetActive(true);

        //ボス戦開始
        _isStart = true;
    }


}
