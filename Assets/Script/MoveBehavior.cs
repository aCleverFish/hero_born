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



    /* 一、对物体坐标进行直接变换
     * 1.直接对物体坐标进行操作
     * Transform.Translate（Vector3 translation）,Space relativeTo）
     * this.transform.Translate(Vector3.left * Time.deltaTime, Space.Self);
     *  this.transform.Translate(new Vector3(1,1,1) * Time.deltaTime, Space.World);
     *
     * 前一个变量是物体的移动速度，这里速度是一个矢量，包含大小和方向
     * 后一个变量是相对坐标系，这里有两个值，一个是世界坐标系一个是自身坐标系
     * Time.deltaTime代表后一帧时间减去前一帧时间，相当于每一帧时间的间隔，不加Time.deltaTime代表每帧的移动速度，加了代表每秒的移动速度可用于平衡不同电脑不同帧数导致的差异
     * update的帧间隔与电脑性能有关，而物体受力和帧间隔时间有关会导致物体抖动
     * 可以在Fixupdate中解决，因为update帧间隔不固定，而Fixupdate固定0.02s在Fixupdate中建议使用Time.fixupdate
     * 
     * 2.直接修改transform的position值
     * transform.position += (destination.transform.position - gameObject.transform.position) * moveSpeed * Time.deltaTime;
     * transform.position += (destination.transform.position - gameObject.transform.position).normalized * moveSpeed * Time.deltaTime;
     * Vector3.normalized就是把一个方向向量编程单位向量，忽略两个点之间的距离影响，强调方向，速度主要受moveSpeed影响
     * 不加这个归一化向量的画，当目标离我越远我移动越快，反之越靠近我移动越慢
     * 
     * 二、通过一个函数得到下一帧物体要到达的位置再移动
     * 1.以固定的速度移动到目标位置
     * public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
     * maxDistanceDelta是每次调用移动的距离
     * 返回值是current + maxDistanceDelta，如果这个值超过了target，返回值就是target
     * 这里实现匀速运动，写在update中多次调用
     * 
     * 2.平滑一点到达目标位置
     * public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed)
     * currentVelocity是当前速度，smoothTime平滑时间，maxSpeed最大速度
     * 返回值是一个向量，把它赋给要移动物体的位置
     * 
     * 三、刚体力的添(涉及物理引擎)
     * 在刚体上添加不同方向的力，从而实现物体的移动，还可以添加力的方式(移动速度与物理特性有关，如质量、阻力、重力)，
     * 但每执行一次只能添加一次力，要项持续运动还得持续调用该方法
     * public void AddForce(Vector3 force)
     * public void AddForceAtPosition(Vector3 force, Vector3 position,ForceMode mode);
     * 
     * Vector3 force：力(矢量)
     * Vector3 posotion：施加力的方向
     * ForceMode mode：力的方式
     * public void AddForce(Vector3 force)
     * public void AddForceAtPosition(Vector3 force, Vector3 position,ForceMode mode);
     * ForceMode为枚举类型，用来控制力的作用方式，有四个枚举成员
     * ForceMode.Force：默认方式，使用刚体的质量计算，以每帧间隔时间为单位计算动量。
     * ForceMode.Acceleration：在此种作用方式下会忽略刚体的实际质量而采用默认值1.0f，时间间隔以系统帧频间隔计算（默认值为0.02s）
     * ForceMode.Impulse：此种方式采用瞬间力作用方式，即把时间的值默认为1，不再采用系统的帧频间隔。
     * ForceMode.VelocityChange：此种作用方式下将忽略刚体的实际质量，采用默认质量1.0，同时也忽略系统的实际帧频间隔，采用默认间隔1.0
     * 
     * gameObject.GetComponent<Rigidbody>().AddForce(force);
     * 
     * 四、刚体速度变换(涉及物理引擎)
     * 1.对刚体速度进行操作，赋予一个持续不变的速度
     *   此属性用于设置或返回刚体的速度值，其使用如下：
     *   ①在脚本中无论是给刚体赋予一个Vector3类型的速度向量v还是获取当前刚体的速度v，v的方向都是相对世界坐标系而言的
     *   ②velocity的单位是米每秒，而不是帧每秒，其中米是Unity默认的长度单位
     *   
     *   public Vector3 velocity{get;set;}
     *   r1.velocity=new Vector3(0.0f,0.0f,-15.0f);
     *
     * 2.使用movePosition()（2D的）
     * public void MovePosition(Vector2 position)
     * 在Rigidbody2D的BodyType为Kinematic是不会受到重力和AddForece（）等相关函数影响的 ，直接把物体移动到目标位置但是有物理效果
     * 
     * Vector2 position ：目标位置
     * private void FixedUpdate()
     * {
     *     var pos = dir * (speed * Time.fixedTime);		//dir 目标方向  speed速度
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