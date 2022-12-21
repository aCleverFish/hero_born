using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    //�洢MainCamera��Player֮���ƫ�ƾ���
    public Vector3 camOffset = new Vector3(0, 1.2f, -2.6f);
    //����player����ı任��Ϣ
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        //�ڳ����ְ����Ʋ���Player���󣬲���ȡplayer���������
        target = GameObject.Find("Player").transform;
    }


    //��LateUpdateҲ�����õķ���������Update����֮��ִ��
    //����ϣ������ű����ƶ��������֮��ִ�У���֤target���õ�������λ��
    //ÿ֡�������λ����ʵ�ָ���Ч��
    void LateUpdate()
    {
        this.transform.position = target.TransformPoint(camOffset);
        this.transform.LookAt(target);
    }
}
