using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// プレイヤーの現在位置を補完し、各種イベントを発生させる
/// </summary>
public class RootNav : MonoBehaviour
{

    #region 外部参照
    //現在のポイント位置を格納
    private GameObject _nowPoint;
    public GameObject NowPoint()
    {

        return _nowPoint;
    }

    [SerializeField, Tooltip("ボス本体")]
    private GameObject _bossObj;
    #endregion


    #region イベントポイント関連
    [Header("イベントポイント関連")]
    [SerializeField, Tooltip("イベントポイントの親を設定")]
    private GameObject _playerRootPoints = null;

    [SerializeField, Tooltip("イベントポイントの配列")]
    private List<GameObject> _rootPoints;

    [SerializeField, Tooltip("ゴール地点との冗長距離")]
    private float _distansDistance;

    [SerializeField, Tooltip("ループ地点の配列番号")]
    private int _loopNumber;

    [SerializeField, Tooltip("ボスのアクティブ位置")]
    private int _bossActiveNumber;

    //現在位置のポイント番号
    private int _posNumber = 0;
    #endregion



    //処理-------------------------------------------------------------------------------------------------

    private void Start()
    {

        //イベントの起こるポイントを配列に格納
        RootArrayStoring();
    }


    private void FixedUpdate()
    {

        //現在のポイント位置を格納(更新)
        NextPosUpdate();

    }



    //メソッド群--------------------------------------------------------------------------------------------


    /// <summary>
    /// イベントの起こるポイントを配列に格納
    /// </summary>
    void RootArrayStoring()
    {

        //ボスのアクティブを消す
        for (int i = 0; i <= _bossObj.transform.childCount - 1; i++)
        {
            //子オブジェクトを消す
            _bossObj.transform.GetChild(i).gameObject.SetActive(false);
        }

        //_playerRootPointsの要素を全て探索
        for (int i = 0; i <= _playerRootPoints.transform.childCount - 1; i++)
        {

            for (int j = 0; j <= _playerRootPoints.transform.GetChild(i).transform.childCount - 1; j++)
            {
                //rootPoints[]にrootPointを全て格納
                _rootPoints.Add(_playerRootPoints.transform.GetChild(i).transform.GetChild(j).gameObject);

            }

        }

    }

    /// <summary>
    /// 現在のポイント位置を格納(更新)
    /// </summary>
    void NextPosUpdate()
    {

        //次のポイントとの距離を計測
        float distans = Vector3.Distance(this.gameObject.transform.position, _rootPoints[_posNumber].transform.position);

        //プレイヤーと次のポイントの距離が範囲内なら
        if (-_distansDistance <= distans && distans <= _distansDistance)
        {


            //現在のポイント位置を格納(更新)
            _nowPoint = _rootPoints[_posNumber];

            //次のポイント
            _posNumber++;

            if (_posNumber == _bossActiveNumber)
            {

                //ボスのアクティブ
                for (int i = 0; i <= _bossObj.transform.childCount - 1; i++)
                {
                    //子オブジェクト
                    _bossObj.transform.GetChild(i).gameObject.SetActive(true);
                }

            }

            //ループ地点に達していたら
            if (_rootPoints.Count == _posNumber)
            {

                //ループ開始の配列番号に変更
                _posNumber = _loopNumber;

            }

            return;
        }

    }

}
