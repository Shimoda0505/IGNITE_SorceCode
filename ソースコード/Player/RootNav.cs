using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �v���C���[�̌��݈ʒu��⊮���A�e��C�x���g�𔭐�������
/// </summary>
public class RootNav : MonoBehaviour
{

    #region �O���Q��
    //���݂̃|�C���g�ʒu���i�[
    private GameObject _nowPoint;
    public GameObject NowPoint()
    {

        return _nowPoint;
    }

    [SerializeField, Tooltip("�{�X�{��")]
    private GameObject _bossObj;
    #endregion


    #region �C�x���g�|�C���g�֘A
    [Header("�C�x���g�|�C���g�֘A")]
    [SerializeField, Tooltip("�C�x���g�|�C���g�̐e��ݒ�")]
    private GameObject _playerRootPoints = null;

    [SerializeField, Tooltip("�C�x���g�|�C���g�̔z��")]
    private List<GameObject> _rootPoints;

    [SerializeField, Tooltip("�S�[���n�_�Ƃ̏璷����")]
    private float _distansDistance;

    [SerializeField, Tooltip("���[�v�n�_�̔z��ԍ�")]
    private int _loopNumber;

    [SerializeField, Tooltip("�{�X�̃A�N�e�B�u�ʒu")]
    private int _bossActiveNumber;

    //���݈ʒu�̃|�C���g�ԍ�
    private int _posNumber = 0;
    #endregion



    //����-------------------------------------------------------------------------------------------------

    private void Start()
    {

        //�C�x���g�̋N����|�C���g��z��Ɋi�[
        RootArrayStoring();
    }


    private void FixedUpdate()
    {

        //���݂̃|�C���g�ʒu���i�[(�X�V)
        NextPosUpdate();

    }



    //���\�b�h�Q--------------------------------------------------------------------------------------------


    /// <summary>
    /// �C�x���g�̋N����|�C���g��z��Ɋi�[
    /// </summary>
    void RootArrayStoring()
    {

        //�{�X�̃A�N�e�B�u������
        for (int i = 0; i <= _bossObj.transform.childCount - 1; i++)
        {
            //�q�I�u�W�F�N�g������
            _bossObj.transform.GetChild(i).gameObject.SetActive(false);
        }

        //_playerRootPoints�̗v�f��S�ĒT��
        for (int i = 0; i <= _playerRootPoints.transform.childCount - 1; i++)
        {

            for (int j = 0; j <= _playerRootPoints.transform.GetChild(i).transform.childCount - 1; j++)
            {
                //rootPoints[]��rootPoint��S�Ċi�[
                _rootPoints.Add(_playerRootPoints.transform.GetChild(i).transform.GetChild(j).gameObject);

            }

        }

    }

    /// <summary>
    /// ���݂̃|�C���g�ʒu���i�[(�X�V)
    /// </summary>
    void NextPosUpdate()
    {

        //���̃|�C���g�Ƃ̋������v��
        float distans = Vector3.Distance(this.gameObject.transform.position, _rootPoints[_posNumber].transform.position);

        //�v���C���[�Ǝ��̃|�C���g�̋������͈͓��Ȃ�
        if (-_distansDistance <= distans && distans <= _distansDistance)
        {


            //���݂̃|�C���g�ʒu���i�[(�X�V)
            _nowPoint = _rootPoints[_posNumber];

            //���̃|�C���g
            _posNumber++;

            if (_posNumber == _bossActiveNumber)
            {

                //�{�X�̃A�N�e�B�u
                for (int i = 0; i <= _bossObj.transform.childCount - 1; i++)
                {
                    //�q�I�u�W�F�N�g
                    _bossObj.transform.GetChild(i).gameObject.SetActive(true);
                }

            }

            //���[�v�n�_�ɒB���Ă�����
            if (_rootPoints.Count == _posNumber)
            {

                //���[�v�J�n�̔z��ԍ��ɕύX
                _posNumber = _loopNumber;

            }

            return;
        }

    }

}
