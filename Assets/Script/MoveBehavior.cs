using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehavior : MonoBehaviour
{
    public GameObject destination;
    public float moveSpeed = 1f;
    public float smoothTime = 1f;
    private Vector3 force = new Vector3 (10f, 10f, 10f);
    private Vector3 velocity;
    private Vector3 dir;

    public Vector3 vel{get; set;}



    /* һ���������������ֱ�ӱ任
     * 1.ֱ�Ӷ�����������в���
     * Transform.Translate��Vector3 translation��,Space relativeTo��
     * this.transform.Translate(Vector3.left * Time.deltaTime, Space.Self);
     *  this.transform.Translate(new Vector3(1,1,1) * Time.deltaTime, Space.World);
     *
     * ǰһ��������������ƶ��ٶȣ������ٶ���һ��ʸ����������С�ͷ���
     * ��һ���������������ϵ������������ֵ��һ������������ϵһ������������ϵ
     * Time.deltaTime�����һ֡ʱ���ȥǰһ֡ʱ�䣬�൱��ÿһ֡ʱ��ļ��������Time.deltaTime����ÿ֡���ƶ��ٶȣ����˴���ÿ����ƶ��ٶȿ�����ƽ�ⲻͬ���Բ�ͬ֡�����µĲ���
     * update��֡�������������йأ�������������֡���ʱ���йػᵼ�����嶶��
     * ������Fixupdate�н������Ϊupdate֡������̶�����Fixupdate�̶�0.02s��Fixupdate�н���ʹ��Time.fixupdate
     * 
     * 2.ֱ���޸�transform��positionֵ
     * transform.position += (destination.transform.position - gameObject.transform.position) * moveSpeed * Time.deltaTime;
     * transform.position += (destination.transform.position - gameObject.transform.position).normalized * moveSpeed * Time.deltaTime;
     * Vector3.normalized���ǰ�һ������������̵�λ����������������֮��ľ���Ӱ�죬ǿ�������ٶ���Ҫ��moveSpeedӰ��
     * ���������һ�������Ļ�����Ŀ������ԽԶ���ƶ�Խ�죬��֮Խ�������ƶ�Խ��
     * 
     * ����ͨ��һ�������õ���һ֡����Ҫ�����λ�����ƶ�
     * 1.�Թ̶����ٶ��ƶ���Ŀ��λ��
     * public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
     * maxDistanceDelta��ÿ�ε����ƶ��ľ���
     * ����ֵ��current + maxDistanceDelta��������ֵ������target������ֵ����target
     * ����ʵ�������˶���д��update�ж�ε���
     * 
     * 2.ƽ��һ�㵽��Ŀ��λ��
     * public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed)
     * currentVelocity�ǵ�ǰ�ٶȣ�smoothTimeƽ��ʱ�䣬maxSpeed����ٶ�
     * ����ֵ��һ����������������Ҫ�ƶ������λ��
     * 
     * ��������������(�漰��������)
     * �ڸ�������Ӳ�ͬ����������Ӷ�ʵ��������ƶ���������������ķ�ʽ(�ƶ��ٶ������������йأ�������������������)��
     * ��ÿִ��һ��ֻ�����һ������Ҫ������˶����ó������ø÷���
     * public void AddForce(Vector3 force)
     * public void AddForceAtPosition(Vector3 force, Vector3 position,ForceMode mode);
     * 
     * Vector3 force����(ʸ��)
     * Vector3 posotion��ʩ�����ķ���
     * ForceMode mode�����ķ�ʽ
     * public void AddForce(Vector3 force)
     * public void AddForceAtPosition(Vector3 force, Vector3 position,ForceMode mode);
     * ForceModeΪö�����ͣ����������������÷�ʽ�����ĸ�ö�ٳ�Ա
     * ForceMode.Force��Ĭ�Ϸ�ʽ��ʹ�ø�����������㣬��ÿ֡���ʱ��Ϊ��λ���㶯����
     * ForceMode.Acceleration���ڴ������÷�ʽ�»���Ը����ʵ������������Ĭ��ֵ1.0f��ʱ������ϵͳ֡Ƶ������㣨Ĭ��ֵΪ0.02s��
     * ForceMode.Impulse�����ַ�ʽ����˲�������÷�ʽ������ʱ���ֵĬ��Ϊ1�����ٲ���ϵͳ��֡Ƶ�����
     * ForceMode.VelocityChange���������÷�ʽ�½����Ը����ʵ������������Ĭ������1.0��ͬʱҲ����ϵͳ��ʵ��֡Ƶ���������Ĭ�ϼ��1.0
     * 
     * gameObject.GetComponent<Rigidbody>().AddForce(force);
     * 
     * �ġ������ٶȱ任(�漰��������)
     * 1.�Ը����ٶȽ��в���������һ������������ٶ�
     *   �������������û򷵻ظ�����ٶ�ֵ����ʹ�����£�
     *   ���ڽű��������Ǹ����帳��һ��Vector3���͵��ٶ�����v���ǻ�ȡ��ǰ������ٶ�v��v�ķ����������������ϵ���Ե�
     *   ��velocity�ĵ�λ����ÿ�룬������֡ÿ�룬��������UnityĬ�ϵĳ��ȵ�λ
     *   
     *   public Vector3 velocity{get;set;}
     *   r1.velocity=new Vector3(0.0f,0.0f,-15.0f);
     *
     * 2.ʹ��movePosition()��2D�ģ�
     * public void MovePosition(Vector2 position)
     * ��Rigidbody2D��BodyTypeΪKinematic�ǲ����ܵ�������AddForece��������غ���Ӱ��� ��ֱ�Ӱ������ƶ���Ŀ��λ�õ���������Ч��
     * 
     * Vector2 position ��Ŀ��λ��
     * private void FixedUpdate()
     * {
     *     var pos = dir * (speed * Time.fixedTime);		//dir Ŀ�귽��  speed�ٶ�
     *     gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position+ pos);
     * }
     *
     * 
     */



    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<Rigidbody>().AddForce(force);
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(1.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.SmoothDamp(transform.position, destination.transform.position, ref velocity, smoothTime, moveSpeed);
    }

    private void FixedUpdate()
    {
        dir = transform.position;
        var pos = dir * (moveSpeed * Time.fixedDeltaTime);
        gameObject.GetComponent<Rigidbody>().MovePosition(transform.position + pos);
    }
}