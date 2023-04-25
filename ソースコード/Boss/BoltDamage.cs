using UnityEngine;


/// <summary>
/// ボスのHpが半分になった際、城(ステージ)に発生する落雷
/// </summary>
public class BoltDamage : MonoBehaviour
{

    //プレイヤー
    private GameObject _player;//オブジェクト
    PlayerStatus _playerStatus;//ステータス
    PoolController _hitBigPool;//攻撃ヒット時のエフェクト

    [SerializeField,Tooltip("ボスのSe")]RaizoSe _raizoSe;
    [SerializeField, Tooltip("距離")] private float _dis;

    //攻撃力
    private const int _attack = 350;


    //処理部-----------------------------------------------------------------------------------------------------

    private void Start()
    {

        //プレイヤー取得
        _player = GameObject.FindGameObjectWithTag("Player").gameObject;
        _playerStatus = _player.GetComponent<PlayerStatus>();

        //攻撃ヒット時のエフェクト
        _hitBigPool = GameObject.FindGameObjectWithTag("HitBigPool").GetComponent<PoolController>();

    }

    private void FixedUpdate()
    {

        //2点間の距離
        Vector2 player = new Vector2(_player.transform.position.x, _player.transform.position.z);
        Vector2 obj = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.z);
        float dis = Vector2.Distance(player, obj);

        //範囲内なら処理
        if (-_dis <= dis && dis <= _dis) { HitEffect(_playerStatus.Hit(_attack)); }

    }

    /// <summary>
    /// 爆発の呼び出し
    /// </summary>
    private void HitEffect(bool flag)
    {

        //isHitがfalseなら処理しない
        if (!flag) { return; }

        //爆発の呼び出し
        GameObject hitObj = _hitBigPool.GetObj();
        hitObj.SetActive(true);
        hitObj.transform.position = _player.transform.position;
        hitObj.transform.parent = _player.transform;

        //爆発seの再生
        _raizoSe.BigBulletHitSe();

    }


    /// <summary>
    /// ギズモ表示
    /// </summary>
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(this.gameObject.transform.position, _dis);
    }

}
