using UnityEngine;
using System;


/// <summary>
/// 1��ނ̃v���C���[��G���A�����̃I�u�W�F�N�g�v�[�����g�p����ۂɁA�����S�Ă̊Ǘ�
/// </summary>
public class PoolManager : MonoBehaviour
{

    [Serializable]
    public class PoolName
    {
        [SerializeField, Tooltip("Pool�����X�N���v�g")]
        public PoolController _poolControllers;

        [SerializeField, Tooltip("Pool�̖��O(�����p)")]
        public string _poolName;
    }

    public PoolName[] _poolArrays;
}
