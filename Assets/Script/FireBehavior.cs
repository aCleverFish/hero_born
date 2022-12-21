using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehavior : MonoBehaviour
{
    public GameObject BulletPrefab;//�ӵ�Ԥ����
    public GameObject Bullets;//�ӵ����弯
    public Camera PlayerCamera;//�������ͷ
    public int BulletSpeed = 800;//�ӵ��ٶ�
    private int index = 0;//�ӵ�����
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))//����������Ƿ񱻵��
        {
            index++;//�ӵ�����
            GameObject BulletGameObject = Instantiate(BulletPrefab);//����Ԥ����
            BulletGameObject.name = "Bullet" + index;
            BulletGameObject.transform.parent = Bullets.transform;//�����ӵ�������
            BulletGameObject.transform.localPosition = Bullets.transform.position;//���ӵ����ϵ�λ�������ӵ�
            BulletGameObject.transform.rotation = PlayerCamera.transform.rotation;
            Bullets.transform.DetachChildren();//������ӹ�ϵ
            BulletGameObject.GetComponent<Rigidbody>().AddForce(PlayerCamera.transform.forward * BulletSpeed);//�����ӵ�һ����ǰ���ƽ���
            Destroy(BulletGameObject, 2);//�����ӵ�����
        }
    }
}
