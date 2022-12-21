using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehavior : MonoBehaviour
{
    public GameObject BulletPrefab;//子弹预制体
    public GameObject Bullets;//子弹物体集
    public Camera PlayerCamera;//玩家摄像头
    public int BulletSpeed = 800;//子弹速度
    private int index = 0;//子弹计数
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))//监测鼠标左键是否被点击
        {
            index++;//子弹计数
            GameObject BulletGameObject = Instantiate(BulletPrefab);//加载预制体
            BulletGameObject.name = "Bullet" + index;
            BulletGameObject.transform.parent = Bullets.transform;//设置子弹父对象
            BulletGameObject.transform.localPosition = Bullets.transform.position;//在子弹集合的位置生成子弹
            BulletGameObject.transform.rotation = PlayerCamera.transform.rotation;
            Bullets.transform.DetachChildren();//解除父子关系
            BulletGameObject.GetComponent<Rigidbody>().AddForce(PlayerCamera.transform.forward * BulletSpeed);//给予子弹一个向前的推进力
            Destroy(BulletGameObject, 2);//销毁子弹物体
        }
    }
}
