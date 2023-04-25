using UnityEngine;
using System;


/// <summary>
/// 1種類のプレイヤーや敵が、複数のオブジェクトプールを使用する際に、それら全ての管理
/// </summary>
public class PoolManager : MonoBehaviour
{

    [Serializable]
    public class PoolName
    {
        [SerializeField, Tooltip("Pool生成スクリプト")]
        public PoolController _poolControllers;

        [SerializeField, Tooltip("Poolの名前(検索用)")]
        public string _poolName;
    }

    public PoolName[] _poolArrays;
}
