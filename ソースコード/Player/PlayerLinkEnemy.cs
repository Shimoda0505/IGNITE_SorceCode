using System.Collections.Generic;
using UnityEngine;
using System;



/// <summary>
/// ��ʓ��ɕ`�悳��Ă���G�͂����Ɋi�[
/// �G���Ƃ̃��b�N�I�����Ȃǂ��⊮
/// </summary>
public class PlayerLinkEnemy : MonoBehaviour
{

    [Serializable, Tooltip("�^�[�Q�b�g�֘A�̊i�[�v�f")]
    public class Targets
    {
        [SerializeField,Tooltip("�i�[����Ă���^�[�Q�b�g")]
        public GameObject _targetObj;

        [SerializeField, Tooltip("�^�[�Q�b�g�����b�N�I������Ă��邩�ǂ���")]
        public bool _isLock;

        [SerializeField, Tooltip("���b�N�I���̃J�[�\��")]
        public GameObject _lockOnCursor;

        [SerializeField, Tooltip("�^�[�Q�b�g�ɒǏ]����΋�")]
        public GameObject _fireBoll;

        [SerializeField, Tooltip("�z��ɏ��߂ē��������ǂ���")]
        public bool _isInView;

    }

    //�^�[�Q�b�g��List
    [Header("�^�[�Q�b�g��List")]
    public  List<Targets> _targetList = new List<Targets>();


    [Header("�J�[�\��")]
    [SerializeField, Tooltip("InView�J�[�\��")]
    private GameObject _inViewCursor;

    //InViewCursor
    private List<GameObject> _cursors = new List<GameObject>();

    private bool _isBoss = false;

    //������--------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //�J�[�\����z��i�[
        for (int i = 0; i <= _inViewCursor.transform.childCount - 1; i++)
        {

            _cursors.Add(_inViewCursor.transform.GetChild(i).gameObject);

            _cursors[i].SetActive(false);

        }
    }

    private void FixedUpdate()
    {

        if (_isBoss) { return; }


        //Enemy�z�����S�T��
        for (int i = 0; i <= _targetList.Count - 1; i++)
        {

            if(_targetList[i]._targetObj.tag == "Boss") { _isBoss = true;return; }

            //�z��ɏ��߂ē����� / Enemy�^�O 
            if (!_targetList[i]._isInView && _targetList[i]._targetObj.tag == "Enemy")
            {

                //Cursor�z�����S�T��
                for (int j = 0; j <= _cursors.Count - 1; j++)
                {

                    //�g�p���ĂȂ����̂������
                    if (!_cursors[j].activeSelf)
                    {

                        _targetList[i]._isInView = true;

                        //�^�[�Q�b�g�̐ݒ�
                        _cursors[j].GetComponent<InViewImage>()._target = _targetList[i]._targetObj;

                        //Active
                        _cursors[j].SetActive(true);

                        break;

                    }

                }

            }

        }
    }
}