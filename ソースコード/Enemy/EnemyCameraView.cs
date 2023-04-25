using UnityEngine;


/// <summary>
/// �J�������Ɏʂ��Ă邩�̔��ʂ�playerLinkEnemy�X�N���v�g�̔z��Ɋi�[
/// </summary>
public class EnemyCameraView : MonoBehaviour
{

    PlayerLinkEnemy _playerLinkEnemy;

    //�v���C���[�̃X�e�[�^�X
    PlayerStatus _playerStatus;

    GameSystem _gameSystem;

    //���d���b�N�I������
    private bool _isLock = false;

    //�X�N���[�����W
    Rect rect = new Rect(0, 0, 1, 1);

    private const float _disMaxPos = 500;

    //����-------------------------------------------------------------------------------------------------

    private void Start()
    {
        //�v���C���[�̃^�O����PlayerStatus�X�N���v�g���擾
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        //�v���C���[�z��^�O����PlayerLinkEnemy�X�N���v�g���擾
        _playerLinkEnemy = GameObject.FindGameObjectWithTag("PlayerArray").GetComponent<PlayerLinkEnemy>();

        _gameSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameSystem>();
    }

    private void FixedUpdate()
    {

        if (_gameSystem._isEvent) { return; }

        //�J�����̉�ʓ��ɃI�u�W�F�N�g�����邩�ǂ���
        CamView();

    }

    //���\�b�h�Q--------------------------------------------------------------------------------------------


    /// <summary>
    /// �J�����̉�ʓ��ɃI�u�W�F�N�g�����邩�ǂ���
    /// </summary>
    private void CamView()
    {

        //�I�u�W�F�N�g���W�̎擾
        Transform targetPos = this.gameObject.transform;

        //�O�������ɃI�u�W�F�N�g�����邩�ǂ���
        Vector3 upperForward = Camera.main.WorldToScreenPoint(targetPos.position);

        //�X�N���[�����W���A�J����View���W(0~1,0~1)�ɕϊ�
        Vector2 upperCam = Camera.main.WorldToViewportPoint(targetPos.position);

        //�J�����̉�p���Ȃ� && �O�������ɃI�u�W�F�N�g�����邩
        if (rect.Contains(upperCam) && _disMaxPos >= upperForward.z && upperForward.z >= 0)
        {

            //�v���C���[�̓G���i�[����List�ɁA���̃I�u�W�F�N�g���i�[
            InPlayerArray();

        }

        //�J�����̉�p�O�Ȃ�
        else
        {

            //���M��T�����Ĕz��v�f���폜,�e�̃^�[�Q�b�g��Null,�J�[�\�����폜
            OutPlayerArray();

        }

    }

    /// <summary>
    /// �v���C���[�̓G���i�[����List�ɁA���̃I�u�W�F�N�g���i�[
    /// </summary>
    public void InPlayerArray()
    {

        if (!_isLock)
        {

            //List���ɃI�u�W�F�N�g�ƃt���O���i�[
            _playerLinkEnemy._targetList.Add(new PlayerLinkEnemy.Targets { _targetObj = this.gameObject, _isLock = false });

            _isLock = true;

        }

    }

    /// <summary>
    /// ���M��T�����Ĕz��v�f���폜,�e�̃^�[�Q�b�g��Null,�J�[�\�����폜
    /// </summary>
    public void OutPlayerArray()
    {

        for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
        {

            //i�Ԃ�List��ݒ�
            PlayerLinkEnemy.Targets target = _playerLinkEnemy._targetList[i];

            //�^�[�Q�b�g�Ȃ�
            if (target._targetObj == this.gameObject)
            {

                if (target._lockOnCursor != null)
                {

                    //�J�[�\��������
                    target._lockOnCursor.GetComponent<CursorController>()._target = null;

                    //���b�N�I���������Z
                    _playerStatus.LockPrice("���Z");

                }

                if(target._fireBoll != null)
                {

                    //�΋��̃^�[�Q�b�g��null
                    target._fireBoll.GetComponent<FireBollController>().ChangeEnum();

                }

                //i�Ԃ̔z��v�f���폜
                _playerLinkEnemy._targetList.RemoveAt(i);

                break;
            }

        }

        _isLock = false;


    }

    /// <summary>
    /// ���b�N�I���\�ȏ�Ԃɖ߂�
    /// </summary>
    public void IsLockFalse()
    {

        _isLock = false;
    }

}
