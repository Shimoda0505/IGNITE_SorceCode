using System.Collections.Generic;
using UnityEngine;
using System;



/// <summary>
/// �X�v���C����̈ړ����x��ύX
/// </summary>
public class ChangeSpeedSpline : MonoBehaviour
{

    [Header("�X�N���v�g")]
    [SerializeField]
    RootNav _rootNav;

    [SerializeField]
    PlayerMoveSpline _playerMoveSpline;

    [SerializeField]
    GameSystem _gameSystem;

    [Serializable, Tooltip("�s���f�[�^")]
    private class ActionDatas
    {

        [SerializeField,Tooltip("�ύX�ʒu")]
        public GameObject _changePos;

        [SerializeField,Tooltip("�ύX���x")]
        public float _speed;
    }

    [SerializeField]
    private List<ActionDatas> _actionDatas = new List<ActionDatas>();

    //���݂̔z��ԍ�
    private int _listNumber = 0;


    private void FixedUpdate()
    {
        if(_listNumber <= _actionDatas.Count - 1)
        {

            //�͈͓��ɓ�������
            if (_rootNav.NowPoint() == _actionDatas[_listNumber]._changePos)
            {

                //���x�̕ύX
                _playerMoveSpline.ChangeSplineSpeed(_actionDatas[_listNumber]._speed);

                //���X�g�̍X�V
                _listNumber++;
            }
        }
    }
}
