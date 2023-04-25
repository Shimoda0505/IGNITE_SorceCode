using UnityEngine;


/// <summary>
/// �{�X��Hp�������ɂȂ����ہA��(�X�e�[�W)�ɔ������闎��
/// </summary>
public class BoltDamage : MonoBehaviour
{

    //�v���C���[
    private GameObject _player;//�I�u�W�F�N�g
    PlayerStatus _playerStatus;//�X�e�[�^�X
    PoolController _hitBigPool;//�U���q�b�g���̃G�t�F�N�g

    [SerializeField,Tooltip("�{�X��Se")]RaizoSe _raizoSe;
    [SerializeField, Tooltip("����")] private float _dis;

    //�U����
    private const int _attack = 350;


    //������-----------------------------------------------------------------------------------------------------

    private void Start()
    {

        //�v���C���[�擾
        _player = GameObject.FindGameObjectWithTag("Player").gameObject;
        _playerStatus = _player.GetComponent<PlayerStatus>();

        //�U���q�b�g���̃G�t�F�N�g
        _hitBigPool = GameObject.FindGameObjectWithTag("HitBigPool").GetComponent<PoolController>();

    }

    private void FixedUpdate()
    {

        //2�_�Ԃ̋���
        Vector2 player = new Vector2(_player.transform.position.x, _player.transform.position.z);
        Vector2 obj = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.z);
        float dis = Vector2.Distance(player, obj);

        //�͈͓��Ȃ珈��
        if (-_dis <= dis && dis <= _dis) { HitEffect(_playerStatus.Hit(_attack)); }

    }

    /// <summary>
    /// �����̌Ăяo��
    /// </summary>
    private void HitEffect(bool flag)
    {

        //isHit��false�Ȃ珈�����Ȃ�
        if (!flag) { return; }

        //�����̌Ăяo��
        GameObject hitObj = _hitBigPool.GetObj();
        hitObj.SetActive(true);
        hitObj.transform.position = _player.transform.position;
        hitObj.transform.parent = _player.transform;

        //����se�̍Đ�
        _raizoSe.BigBulletHitSe();

    }


    /// <summary>
    /// �M�Y���\��
    /// </summary>
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(this.gameObject.transform.position, _dis);
    }

}
