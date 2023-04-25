using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;/*�ySpline��Editor�g�����K�v�ł��z*/
using System;



/// <summary>
/// �{�X��Spline��̈ړ�����
/// </summary>
public class BossMoveSpline : MonoBehaviour
{

    #region �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField]
    RootNav _rootNav;

    [SerializeField]
    RizouSystem _rizouSystem;/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/

    [SerializeField]
    GameSystem _gameSystem;

    [SerializeField]
    BossStatus _bossStatus;

    [SerializeField]
    RaizoAnimationController _raizoAnim;

    [SerializeField]
    PlayerController _playerController;

    [SerializeField]
    PlayerStatus _playerStatus;

    [SerializeField]
    CameraShake _cameraShake;

    [SerializeField]
    RaizoSe _raizoSe;

    #endregion

    #region �v���C���[�֘A
    [Header("�v���C���[�֘A")]
    [SerializeField, Tooltip("�v���C���[")]
    private GameObject _playerObj;
    #endregion

    #region �U���֘A
    [Header("�U���֘A")]
    [SerializeField, Tooltip("�ːi�̔���")]
    private float _tackleDis;

    //�ːi�����ǂ���
    private bool _istackle;

    [SerializeField, Tooltip("�^�b�N���G�t�F�N�g")]
    private GameObject _tackleEff;

    [SerializeField, Tooltip("�J�����̐U������/�U����")]
    private Vector2 _shakeTimeMag;

    //�^�b�N���̍U����
    private int _tackleAttack = 250;
    #endregion

    #region �X�v���C���֘A
    //���[�g���x
    private float _rootSpeed;

    //��Ԃ̊���(0~1�̊Ԃ��n�_^�I�_�ňړ�)
    private float _percentage;

    //�O�t���[���̃��[���h�ʒu
    private Vector3 _prevPos;

    //�O���NowRoot��⊮
    private GameObject _lastPoint = null;

    //�ݒ肳�ꂽ�X�v���C��
    private SplineContainer _settingSpline;/*�ySpline��Editor�g�����K�v�ł��z*/
    #endregion

    #region �s�����
    [Header("�s�����")]
    [SerializeField, Tooltip("�ύX�|�C���g�Ƃ̋���")]
    private float _pointDistance;

    //���݂̃��[�V�����f�[�^�̔ԍ�
    private int _motionNumber = 0;

    //�����ύX�ԍ�
    private int _changeNumber = 0;

    [Serializable, Tooltip("�^�[�Q�b�g�֘A�̊i�[�v�f")]
    public class MotionDatas
    {

        [SerializeField, Tooltip("�v���C���[�̈ړ����")]
        public GameObject _playerRoot;

        [SerializeField, Tooltip("�����̃X�v���C�����")]
        public SplineContainer _moveSpline;/*�ySpline��Editor�g�����K�v�ł��z*/

        [SerializeField, Tooltip("�����̈ړ����x")]
        public float[] _moveSpeed;

        [SerializeField, Tooltip("�����̃��[�V�����ω��|�C���g")]
        public GameObject[] _motionChange;

        [SerializeField, Tooltip("�e�|�C���g�̃E���g���[�V����")]
        public ActiveMotion[] _activeMotion;
        public enum ActiveMotion
        {
            SHOT,
            HOMING,
            BIG,
            TACKLE,
            Roar,
            CHANGE_LOOK,
            CAMERA_SHAKE
        }

    }

    //�^�[�Q�b�g��List
    public List<MotionDatas> _motionDatas = new List<MotionDatas>();
    #endregion


    /// <summary>
    /// �s�������ǂ���
    /// </summary>
    Motion _motion = Motion.WAIT;
    enum Motion
    {
        WAIT,//�ҋ@
        MOVE,//�ړ�
        Death
    }

    /// <summary>
    /// ���_�̑I��
    /// </summary>
    LookMotion _lookMotion = LookMotion.LOOK_FOWARD;
    enum LookMotion
    {

        LOOK_FOWARD,
        LOOK_PLAYER
    }




    //�����Q--------------------------------------------------------------------------------------------------------------------

    private void FixedUpdate()
    {

        //���S������
        if (_bossStatus.IsDeath()) { _raizoAnim.DamageAnim(); this.gameObject.SetActive(false); }

        //�E���g�����������
        if (_bossStatus.IsUlt()) { _raizoAnim.DamageAnim(); _bossStatus.IsUltFalse(); }

        //�C�x���g�� || �v���C���[���S���͒�~
        if (_gameSystem._isEvent || _playerController._playerMotion == PlayerController.PlayerMotion.Death) { return; }

        //Hp�������ɂȂ�����
        if (_bossStatus.IsHpHalf()) { _tackleAttack = 500; }

        //�U�����[�V����
        switch (_motion)
        {

            //�ҋ@
            case Motion.WAIT:

                //���[�V�����f�[�^�̒T��
                MotionArray();

                break;


            //�ړ�
            case Motion.MOVE:

                //Spline����ړ�
                MoveSpline();

                //���_
                switch (_lookMotion)
                {

                    case LookMotion.LOOK_FOWARD:

                        //�i�s�����𒼎�
                        LookFoward();

                        break;

                    case LookMotion.LOOK_PLAYER:

                        //�v���C���[�𒼎�
                        LockPlayer();

                        break;

                }

                //�U���p�̃��[�V�����̕ύX
                ActivePoint();

                //�ːi���̏���
                TackleProcess();

                break;

        }

    }



    //���\�b�h�Q--------------------------------------------------------------------------------------------------------------------

    //�T��
    /// <summary>
    /// ���[�V�����f�[�^�̒T��
    /// </summary>
    private void MotionArray()
    {

        //�O�񂩂�v���C���[�̈ʒu��񂪍X�V����Ă���Ȃ�
        if (_lastPoint != _rootNav.NowPoint())
        {

            //���[�V�����f�[�^��T��
            for (int i = 0; i <= _motionDatas.Count - 1; i++)
            {

                //���[�V�����f�[�^�̕⊮
                MotionDatas data = _motionDatas[i];

                //�v���C���[�̈ʒu����⊮
                GameObject _motionPos = data._playerRoot;

                //�v���C���[�̌��݈ʒu���ƈ�v�����Ȃ�
                if (_motionPos == _rootNav.NowPoint())
                {

                    //�A�j���[�V�����̏�����
                    _raizoAnim.LookForwardAnim();

                    //���_�̏�����
                    _lookMotion = LookMotion.LOOK_FOWARD;

                    //�ːi��Ԃ̏�����
                    _istackle = false;
                    _tackleEff.SetActive(false);

                    //���[�V�����f�[�^�̔ԍ���⊮
                    _motionNumber = i;

                    //�ʒu�����X�V
                    _lastPoint = _motionPos;

                    //�X�v���C�����̍X�V
                    _settingSpline = data._moveSpline;

                    //�ړ����x�̍X�V
                    _rootSpeed = data._moveSpeed[0];

                    //�ړ��J�n
                    _motion = Motion.MOVE;

                }

            }

        }

    }


    //�ړ�
    /// <summary>
    /// Spline����ړ�
    /// </summary>
    private void MoveSpline()
    {

        //���������Ԃŉ��Z
        _percentage += Time.deltaTime * _rootSpeed;

        // �v�Z�����ʒu�i���[���h���W�j���^�[�Q�b�g�ɑ��
        this.gameObject.transform.position = _settingSpline.EvaluatePosition(_percentage);

        //�ݒ肳��Ă���Spline�̏I�_
        if (_percentage >= 1)
        {

            //�����̏�����
            _percentage = 0;

            //�ړ����̃��[�V�����ύX�Í���������
            _changeNumber = 0;

            //�ҋ@Enum
            _motion = Motion.WAIT;

        }

    }


    //���_
    /// <summary>
    /// �i�s�����𒼎�
    /// </summary>
    private void LookFoward()
    {

        //���_�̌���------------------------------------------------------------------------
        // ���݃t���[���̃t���[���ʒu
        Vector3 position = this.gameObject.transform.position;

        // �ړ��ʂ��v�Z
        Vector3 moveVolume = position - _prevPos;

        // ����Update�Ŏg�����߂̑O�t���[���ʒu�⊮
        _prevPos = position;

        // �i�s�����Ɋp�x��ύX
        this.gameObject.transform.rotation = Quaternion.LookRotation(moveVolume, Vector3.up);

    }

    /// <summary>
    /// �v���C���[�𒼎�
    /// </summary>
    private void LockPlayer()
    {

        //�v���C���[�𒼎�
        this.gameObject.transform.LookAt(_playerObj.transform);

    }


    //�s���֘A
    /// <summary>
    /// �����̕ύX
    /// </summary>
    private void ActivePoint()
    {

        //�f�[�^������Ȃ�
        if (_changeNumber <= _motionDatas[_motionNumber]._motionChange.Length - 1)
        {

            //�{�X�̈ʒu
            Vector3 thisPos = this.gameObject.transform.position;

            //�ύX�|�C���g�̈ʒu
            Vector3 changePos = _motionDatas[_motionNumber]._motionChange[_changeNumber].transform.position;

            //2�_�̋���
            float distance = Vector3.Distance(thisPos, changePos);

            //���͈͓��Ȃ�
            if (-_pointDistance <= distance && distance <= _pointDistance)
            {

                //�ړ����x�̕ύX
                _rootSpeed = _motionDatas[_motionNumber]._moveSpeed[_changeNumber + 1];

                //���[�V�����f�[�^�̊i�[
                MotionDatas.ActiveMotion activeMotion = _motionDatas[_motionNumber]._activeMotion[_changeNumber];

                //���[�V�����̕ύX
                if (activeMotion == MotionDatas.ActiveMotion.SHOT) { _rizouSystem.Barrage(); _raizoAnim.AttackAnim(); _raizoSe.BulletSe(); }//�ʏ�e
                else if (activeMotion == MotionDatas.ActiveMotion.HOMING) { _rizouSystem.Homing(); _raizoAnim.AttackAnim(); _raizoSe.BulletSe(); }//�z�[�~���O�e
                else if (activeMotion == MotionDatas.ActiveMotion.BIG) { _rizouSystem.LargeFireBall(); _raizoAnim.AttackAnim(); _raizoSe.BigBulletSe(); }//��e
                else if (activeMotion == MotionDatas.ActiveMotion.TACKLE) { _istackle = true; _raizoAnim.TackleAnimT(); _raizoSe.TackleSe(); _tackleEff.SetActive(true); }//�ːi
                else if (activeMotion == MotionDatas.ActiveMotion.Roar) { _raizoSe.RoarSe(); }//���K
                else if (activeMotion == MotionDatas.ActiveMotion.CHANGE_LOOK) //���_�ύX
                {
                    _raizoSe.RoarSe();
                    _raizoSe.WingSe();

                    if (_lookMotion == LookMotion.LOOK_FOWARD) { _lookMotion = LookMotion.LOOK_PLAYER; _raizoAnim.LookPlayerAnim(); }//�v���C���[�����ɕύX
                    else if (_lookMotion == LookMotion.LOOK_PLAYER) { _lookMotion = LookMotion.LOOK_FOWARD; _raizoAnim.LookForwardAnim(); }//�O�������ɕύX  

                }
                else if (activeMotion == MotionDatas.ActiveMotion.CAMERA_SHAKE) { _cameraShake.Shake(_shakeTimeMag.x, _shakeTimeMag.y); _raizoSe.WingSe(); _raizoSe.BigRoarSe(); }//�J�����h��

                //�n�_�X�V
                _changeNumber++;

            }

        }

    }

    /// <summary>
    /// �ːi���̏���
    /// </summary>
    private void TackleProcess()
    {

        if (_istackle)
        {
            //�v���C���[�Ƃ̋���
            float dis = Vector3.Distance(this.gameObject.transform.position, _playerObj.transform.position);

            //�͈͓��ɓ�������
            if (-_tackleDis <= dis && dis <= _tackleDis)
            {

                //�v���C���[�Ƀ_���[�W
                _playerStatus.Hit(_tackleAttack);

                //�J�����h��
                _cameraShake.Shake(_shakeTimeMag.x, _shakeTimeMag.y);

            }

        }

    }




    /// <summary>
    /// �M�Y���\��
    /// </summary>
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(this.gameObject.transform.position, _tackleDis);
    }

}
