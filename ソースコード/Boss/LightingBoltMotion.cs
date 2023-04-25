using System.Collections.Generic;
using UnityEngine;
using System;



/// <summary>
/// �����̋����Ǘ�
/// </summary>
public class LightingBoltMotion : MonoBehaviour
{
    [SerializeField,Tooltip("�v���C���[�̃��[�g�Ǘ��X�N���v�g")] RootNav _rootNav;
    [SerializeField,Tooltip("�{�X��se�Ǘ��X�N���v�g")] RaizoSe _raizoSe;
    [SerializeField,Tooltip("�{�X�̃X�e�[�^�X�Ǘ��X�N���v�g")] BossStatus _bossStatus;

    [Serializable,Tooltip("�����f�[�^")]
    private class BoltDatas
    {

        [SerializeField, Tooltip("�����C���^�[�o��")]
        public float _interval;

        [SerializeField, Tooltip("�����|�C���g")]
        public GameObject _point;

        [SerializeField, Tooltip("����")]
        public GameObject[] _bolt;
    }
    [SerializeField,Tooltip("�����f�[�^�̃��X�g")] private List<BoltDatas> _boltDatas = new List<BoltDatas>();


    //���Ԃ̃J�E���g
    private float _count = 0;

    //�z��ԍ�
    private int _numberDatas = 0;

    //�����ԍ�
    private int _numberBolt = 0;

    //�O��̃|�C���g
    private GameObject _lastPoint = null;

    Motion _motion = Motion.WAIT; 
    enum Motion
    {
        WAIT,
        MOVE
    }


    private void Start()
    {

        //�S�Ă̗����I�u�W�F�N�g�Ƀ_���[�W�p�X�N���v�g��Add
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

        //�{�X��hp�������ɂȂ�܂Ŏg�p���Ȃ�
        if (!_bossStatus.IsHpHalf()) { return; }

        switch (_motion)
        {

            case Motion.WAIT:

                //�O��Ɠ����|�C���g�Ȃ珈�����Ȃ�
                if(_lastPoint == _rootNav.NowPoint()) { return; }

                for (int i = 0; i < _boltDatas.Count; i++)
                {

                    //�͈͓��Ȃ珈��
                    if (_rootNav.NowPoint() == _boltDatas[i]._point)
                    {

                        //�|�C���g��⊮
                        _lastPoint = _rootNav.NowPoint();

                        //�z��ԍ��̕⊮
                        _numberDatas = i;

                        //����enum�ɑJ��
                        _motion = Motion.MOVE;
                    }
                }
                break;


            case Motion.MOVE:

                //���Ԍv����ɏ���
                _count += Time.deltaTime;
                if(_count >= _boltDatas[_numberDatas]._interval)
                {

                    //���ԏ�����
                    _count = 0;

                    //�z��ő�
                    if (_numberBolt > _boltDatas[_numberDatas]._bolt.Length - 1)
                    {

                        //�����ԍ��̏�����
                        _numberBolt = 0;

                        //�ҋ@enum�ɑJ��
                        _motion = Motion.WAIT;
                    }

                    //����Active
                    _boltDatas[_numberDatas]._bolt[_numberBolt].SetActive(true);

                    //����se
                    _raizoSe.BoltSe();

                    //�����ԍ��̉��Z
                    _numberBolt++;
                }
                break;
        }
    }
}
