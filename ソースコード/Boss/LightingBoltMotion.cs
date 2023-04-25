using System.Collections.Generic;
using UnityEngine;
using System;



/// <summary>
/// 落雷の挙動管理
/// </summary>
public class LightingBoltMotion : MonoBehaviour
{
    [SerializeField,Tooltip("プレイヤーのルート管理スクリプト")] RootNav _rootNav;
    [SerializeField,Tooltip("ボスのse管理スクリプト")] RaizoSe _raizoSe;
    [SerializeField,Tooltip("ボスのステータス管理スクリプト")] BossStatus _bossStatus;

    [Serializable,Tooltip("落雷データ")]
    private class BoltDatas
    {

        [SerializeField, Tooltip("落雷インターバル")]
        public float _interval;

        [SerializeField, Tooltip("落雷ポイント")]
        public GameObject _point;

        [SerializeField, Tooltip("落雷")]
        public GameObject[] _bolt;
    }
    [SerializeField,Tooltip("落雷データのリスト")] private List<BoltDatas> _boltDatas = new List<BoltDatas>();


    //時間のカウント
    private float _count = 0;

    //配列番号
    private int _numberDatas = 0;

    //落雷番号
    private int _numberBolt = 0;

    //前回のポイント
    private GameObject _lastPoint = null;

    Motion _motion = Motion.WAIT; 
    enum Motion
    {
        WAIT,
        MOVE
    }


    private void Start()
    {

        //全ての落雷オブジェクトにダメージ用スクリプトをAdd
        for (int i = 0; i < _boltDatas.Count; i++)
        {
            for (int j = 0; j < _boltDatas[i]._bolt.Length; j++)
            {
                _boltDatas[i]._bolt[j].AddComponent<BoltDamage>();
            }
        }
    }

    private void FixedUpdate()
    {

        //ボスのhpが半分になるまで使用しない
        if (!_bossStatus.IsHpHalf()) { return; }

        switch (_motion)
        {

            case Motion.WAIT:

                //前回と同じポイントなら処理しない
                if(_lastPoint == _rootNav.NowPoint()) { return; }

                for (int i = 0; i < _boltDatas.Count; i++)
                {

                    //範囲内なら処理
                    if (_rootNav.NowPoint() == _boltDatas[i]._point)
                    {

                        //ポイントを補完
                        _lastPoint = _rootNav.NowPoint();

                        //配列番号の補完
                        _numberDatas = i;

                        //落雷enumに遷移
                        _motion = Motion.MOVE;
                    }
                }
                break;


            case Motion.MOVE:

                //時間計測後に処理
                _count += Time.deltaTime;
                if(_count >= _boltDatas[_numberDatas]._interval)
                {

                    //時間初期化
                    _count = 0;

                    //配列最大
                    if (_numberBolt > _boltDatas[_numberDatas]._bolt.Length - 1)
                    {

                        //落雷番号の初期化
                        _numberBolt = 0;

                        //待機enumに遷移
                        _motion = Motion.WAIT;
                    }

                    //落雷Active
                    _boltDatas[_numberDatas]._bolt[_numberBolt].SetActive(true);

                    //落雷se
                    _raizoSe.BoltSe();

                    //落雷番号の加算
                    _numberBolt++;
                }
                break;
        }
    }
}
