using UnityEngine;



/// <summary>
/// �v���C���[�̃E���g���̉΋�����
/// </summary>
public class UltBoll : MonoBehaviour
{

    #region ����
    [Header("����")]
    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _moveTime;

    [SerializeField]
    private float _exTime;

    [SerializeField]
    private float _waitTime;

    private float _moveCount = 0;

    private GameObject _shutPos;

    //�{�X�ɍU���������ǂ���
    private bool _isBoss = false;

    private GameObject _bossObj;

    private const int ATTACK_DAMAGE = 1000;
    #endregion

    #region �X�N���v�g
    PlayerLinkEnemy _playerLinkEnemy;

    PlayerStatus _playerStatus;

    BossStatus _bossStatus;

    ScoreManager _scoreManager;

    PoolController _exPool;
    #endregion


    Motion _motion = Motion.WAIT;
    enum Motion
    {
        WAIT,
        MOVE,
        STOP
    }
    private void Start()
    {

        //�v���C���[�̉w�Ǘ��z����擾
        _playerLinkEnemy = GameObject.FindGameObjectWithTag("PlayerArray").GetComponent<PlayerLinkEnemy>();

        //�v���C���[�̃X�e�[�^�X���擾
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        //�{�X�X�e�[�^�X���擾
        _bossStatus = GameObject.FindGameObjectWithTag("BossBody").GetComponent<BossStatus>();

        _scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();

        _exPool = GameObject.FindGameObjectWithTag("UltBollEx").GetComponent<PoolController>();

        _shutPos = GameObject.FindGameObjectWithTag("ShutArea").gameObject;

    }


    void FixedUpdate()
    {

        switch (_motion)
        {

            case Motion.WAIT:


                this.gameObject.transform.position = _shutPos.transform.position;

                _moveCount += Time.deltaTime;

                if(_moveCount >= _waitTime)
                {
                    _moveCount = 0;

                    _motion = Motion.MOVE;
                }

                break;


            case Motion.MOVE:

                //�O�������ɒ��i
                this.gameObject.transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);

                _moveCount += Time.deltaTime;

                if (_moveCount >= _moveTime)
                {

                    GameObject obj = _exPool.GetObj();

                    obj.transform.position = this.gameObject.transform.position;

                    obj.SetActive(true);

                    //������
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSE>().ExplosionBigSe();

                    _moveCount = 0;

                    DamageEnemy();

                    _motion = Motion.STOP;

                }

                break;

            case Motion.STOP:

                _moveCount += Time.deltaTime;

                if(_moveCount >= _exTime)
                {

                    _moveCount = 0;

                    //�e�̈ʒu��Ǐ]����Ώۂ�BurretPool�^�O�Ŏw��
                    this.gameObject.transform.position = GameObject.FindGameObjectWithTag("PlayerArray").transform.position;

                    //�A�N�e�B�u�I��
                    this.gameObject.SetActive(false);

                    _motion = Motion.WAIT;

                }

                break;
        
        }

    }

    private void DamageEnemy()
    {

        //�G�i�[�̔z���S�T��
        for(int i = _playerLinkEnemy._targetList.Count - 1; i >= 0;i--)
        {

            PlayerLinkEnemy.Targets targets = _playerLinkEnemy._targetList[i];

            GameObject target = targets._targetObj;

            if(target.tag == "Enemy")
            {

                //�G�̃_���[�W���\�b�h
                if (target.GetComponent<EnemyDeath>()) { target.GetComponent<EnemyDeath>().EnemyDeathController(); }/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/

                //���j���̉��Z
                _scoreManager.SmashEnemyCount(1);

            }

            else if(target.tag == "Boss")
            {

                //�����N�̃��b�N�I����Ԃ�����
                if (targets._targetObj.GetComponent<EnemyCameraView>()) { targets._targetObj.GetComponent<EnemyCameraView>().IsLockFalse(); }

                //�{�X�I�u�W�F�N�g���i�[
                _bossObj = targets._targetObj;

                _isBoss = true;

            }


            if (targets._fireBoll != null)
            {

                targets._fireBoll.GetComponent<FireBollController>().TargetNull();
            }

            if (targets._lockOnCursor != null)
            {

                //�J�[�\��������
                targets._lockOnCursor.GetComponent<CursorController>()._target = null;

            }

            //i�Ԃ̔z��v�f���폜
            _playerLinkEnemy._targetList.RemoveAt(i);

            //�`�F�C�����̉��Z
            _playerStatus.ChainAddition();


        }

        if (_isBoss)
        {

            BossStatus bossStatus = _bossObj.transform.root.gameObject.GetComponent<BossStatus>();

            bossStatus.BossDamage(ATTACK_DAMAGE);
            bossStatus.IsUltTrue();

            _isBoss = false;

        }

    }
}
