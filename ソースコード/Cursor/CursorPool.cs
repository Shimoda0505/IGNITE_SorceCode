using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ���b�N�I�����̃J�[�\�����v�[���ɊǗ�
/// </summary>
public class CursorPool : MonoBehaviour
{
    [Header("���b�N�I���J�[�\��")]
    [SerializeField]
    private GameObject poolObj; // �v�[���ɐ����������e����
    [Header("�v�[���ő�l")]
    [SerializeField]
    private int maxCursor; // �ŏ��ɐ�������e�̍ő吔 
    public int listBurret()
    {
        return maxCursor;
    }

    public List<GameObject> poolObjList; // ���������e�p�̃��X�g


    void Awake()
    {
        // �ŏ��ɂ�����x�̐��A�I�u�W�F�N�g���쐬���ăv�[���ɗ��߂Ă�������
        //�e�̐���������
        poolObjList = new List<GameObject>();
        //�e���ő吔�܂Ő���
        for (int i = 0; i < maxCursor; i++)
        {
            // �e�𐶐�����
            var newObj = CreateNewBurret();
            //���������e�̕���������false
            newObj.GetComponent<RawImage>().enabled = false;
            // ���X�g�ɕۑ����Ă���
            poolObjList.Add(newObj);
        }
    }

    //----------------------------Burret�̕ԋp----------------------------------------
    //PlayerShotBurret�̃X�N���v�g�ɌĂяo����镔��
    public GameObject GetBurret()
    {
        // ���g�p������Ύg�p,�Ȃ���ΐ���
        // �g�p���łȂ����̂�T���ĕԂ�
        //���X�g���ɂ���e��obj�Ƃ��ĕԂ�
        foreach (var obj in poolObjList)
        {
            //�e��Rigidbody���擾
            var objrb = obj.GetComponent<RawImage>();
            //�e�̕���������false�̂��̂�T��
            if (objrb.enabled == false)
            {
                //�e�̕���������false�̂��̂�����΂����true
                objrb.enabled = true;
                //�Ăяo�����̃X�N���v�g�ɂ��̃I�u�W�F�N�g��Ԃ�
                return obj;
            }
        }

        // �S�Ďg�p����������V�������
        var newObj = CreateNewBurret();
        //���X�g�ɕۑ����Ă���
        poolObjList.Add(newObj);
        //�V����������I�u�W�F�N�g�̕������������̂܂�true�ɂ���
        newObj.GetComponent<RawImage>().enabled = true;
        //�Ăяo�����̃X�N���v�g�ɂ��̃I�u�W�F�N�g��Ԃ�
        return newObj;
    }

    // �V�����e���쐬���鏈��
    private GameObject CreateNewBurret()
    {
        // ��ʊO�ɐ���
        var pos = this.gameObject.transform.position;
        // �V�����e�𐶐�(�����������e��e�ɂȂ�I�u�W�F�N�g�Ɠ�������)
        var newObj = Instantiate(poolObj, pos, Quaternion.identity);
        // ���O�ɘA�ԕt��(���X�g�ɒǉ����ꂽ��)
        newObj.name = poolObj.name + (poolObjList.Count + 1);
        //�Ăяo�����̃X�N���v�g�ɂ��̃I�u�W�F�N�g��Ԃ�
        return newObj;
    }
}
