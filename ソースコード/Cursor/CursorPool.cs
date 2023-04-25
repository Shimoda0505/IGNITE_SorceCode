using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ロックオン時のカーソルをプールに管理
/// </summary>
public class CursorPool : MonoBehaviour
{
    [Header("ロックオンカーソル")]
    [SerializeField]
    private GameObject poolObj; // プールに生成したい弾を代入
    [Header("プール最大値")]
    [SerializeField]
    private int maxCursor; // 最初に生成する弾の最大数 
    public int listBurret()
    {
        return maxCursor;
    }

    public List<GameObject> poolObjList; // 生成した弾用のリスト


    void Awake()
    {
        // 最初にある程度の数、オブジェクトを作成してプールに溜めておく処理
        //弾の数を初期化
        poolObjList = new List<GameObject>();
        //弾を最大数まで生成
        for (int i = 0; i < maxCursor; i++)
        {
            // 弾を生成して
            var newObj = CreateNewBurret();
            //生成した弾の物理挙動をfalse
            newObj.GetComponent<RawImage>().enabled = false;
            // リストに保存しておく
            poolObjList.Add(newObj);
        }
    }

    //----------------------------Burretの返却----------------------------------------
    //PlayerShotBurretのスクリプトに呼び出される部分
    public GameObject GetBurret()
    {
        // 未使用があれば使用,なければ生成
        // 使用中でないものを探して返す
        //リスト内にある弾をobjとして返す
        foreach (var obj in poolObjList)
        {
            //弾のRigidbodyを取得
            var objrb = obj.GetComponent<RawImage>();
            //弾の物理挙動がfalseのものを探す
            if (objrb.enabled == false)
            {
                //弾の物理挙動がfalseのものがあればそれをtrue
                objrb.enabled = true;
                //呼び出し元のスクリプトにこのオブジェクトを返す
                return obj;
            }
        }

        // 全て使用中だったら新しく作る
        var newObj = CreateNewBurret();
        //リストに保存しておく
        poolObjList.Add(newObj);
        //新しく作ったオブジェクトの物理挙動をそのままtrueにする
        newObj.GetComponent<RawImage>().enabled = true;
        //呼び出し元のスクリプトにこのオブジェクトを返す
        return newObj;
    }

    // 新しく弾を作成する処理
    private GameObject CreateNewBurret()
    {
        // 画面外に生成
        var pos = this.gameObject.transform.position;
        // 新しい弾を生成(生成したい弾を親になるオブジェクトと同じ所に)
        var newObj = Instantiate(poolObj, pos, Quaternion.identity);
        // 名前に連番付け(リストに追加された順)
        newObj.name = poolObj.name + (poolObjList.Count + 1);
        //呼び出し元のスクリプトにこのオブジェクトを返す
        return newObj;
    }
}
