using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    //存储MainCamera和Player之间的偏移距离
    public Vector3 camOffset = new Vector3(0, 1.2f, -2.6f);
    //保存player对象的变换信息
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        //在场景种按名称查找Player对象，并获取player对象的属性
        target = GameObject.Find("Player").transform;
    }


    //①LateUpdate也是内置的方法，但在Update方法之后执行
    //这里希望这个脚本在移动操作完成之后执行，保证target引用的是最新位置
    //每帧更新相机位置以实现跟随效果
    void LateUpdate()
    {
        this.transform.position = target.TransformPoint(camOffset);
        this.transform.LookAt(target);
    }
}
