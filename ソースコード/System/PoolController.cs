using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// オブジェクトプール
/// </summary>
public class PoolController : MonoBehaviour
{

    //同一のオブジェクトにある、スクリプトを参照
    //PoolController.instans.publicメソッド
    public static PoolController instans;

    #region 変数
    [SerializeField,Tooltip("プールに生成したい弾を代入")]
    private GameObject _poolObj;

    [SerializeField,Tooltip("最初に生成する弾の最大数 ")]
    private int _maxPool;

    //生成した弾用のリスト
    public List<GameObject> _poolObjList;
    #endregion



    //処理-------------------------------------------------------------------------------------------------

    void Awake()
    {

        //最初に固定数プレハブオブジェクトをインスタント
        InitializeObj();

    }



    //メソッド群--------------------------------------------------------------------------------------------

    /// <summary>
    /// 最初に固定数プレハブオブジェクトをインスタント
    /// </summary>
    private void InitializeObj()
    {

        // 最初にある程度の数、オブジェクトを作成してプールに溜めておく処理
        //弾の数を初期化
        _poolObjList = new List<GameObject>();

        //弾を最大数まで生成
        for (int i = 0; i < _maxPool; i++)
        {

            // 弾を生成して
            GameObject newObj = CreateNewObj();

            //生成した弾の物理挙動をfalse
            newObj.SetActive(false);

            // リストに保存しておく
            _poolObjList.Add(newObj);

        }

    }

    /// <summary>
    /// オブジェクトプールから使用するプレハブオブジェクトを取得
    /// </summary>
    /// <returns>プレハブオブジェクト</returns>
    public GameObject GetObj()
    {
        // 未使用があれば使用,なければ生成
        // 使用中でないものを探して返す
        //リスト内にある弾をobjとして返す
        foreach (var obj in _poolObjList)
        {
            //弾のRigidbodyを取得
            bool objrb = obj.activeSelf;

            //弾の物理挙動がfalseのものを探す
            if (objrb == false)
            {

                //弾の物理挙動がfalseのものがあればそれをtrue
                objrb = true;

                //呼び出し元のスクリプトにこのオブジェクトを返す
                return obj;

            }

        }

        // 全て使用中だったら新しく作る
        GameObject newObj = CreateNewObj();

        //リストに保存しておく
        _poolObjList.Add(newObj);

        //新しく作ったオブジェクトの物理挙動をそのままtrueにする
        newObj.SetActive(false);

        //呼び出し元のスクリプトにこのオブジェクトを返す
        return newObj;

    }

    /// <summary>
    /// 新しくプレハブオブジェクトをインスタント
    /// </summary>
    /// <returns>新しくプレハブオブジェクト</returns>
    private GameObject CreateNewObj()
    {

        // 画面外に生成
        Vector3 pos = this.gameObject.transform.position;

        // 新しい弾を生成(生成したい弾を,画面外に,親になるオブジェクトと同じ所に)
        GameObject newObj = Instantiate(_poolObj, pos, Quaternion.identity);

        // 名前に連番付け(リストに追加された順)
        newObj.name = _poolObj.name + (_poolObjList.Count + 1);

        //呼び出し元のスクリプトにこのオブジェクトを返す
        return newObj;

    }

}
