using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //向前or向后的移动速度
    public float moveSpeed = 10f;
    //向左or向右的旋转速度
    public float rotateSpeed = 50f;
    //向上跳跃的速度
    public float jumpVelocity = 5f;
    public float distanceToGround = 0.2f;
    public LayerMask groundLayer;

    
    public GameObject bullet;
    public float bulletSpeed = 50f;

    private float vInput;
    private float hInput;

    public GameObject Bullets;//子弹物体集
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;

    private GameBehaviour gameManager;

    private void Start()
    {
        //这里检测脚本附加的物体上是否包含指定的组件类型(这里是rigidbody)，如果找到了就返回，如果没找到就返回null
        _rigidbody = GetComponent<Rigidbody>();
        //拿到当前脚本对象上的胶囊碰撞体
        _collider = GetComponent<CapsuleCollider>();

        //拿到gameManager下面的GameBehavior脚本
        gameManager = GameObject.Find("GameManager").GetComponent<GameBehaviour>();
    }


    // Update is called once per frame
    void Update()
    {
        //保存垂直轴的输入 Input.GetAxis("Vertical")用于检查上下方向键以及w、s何时被按下
        vInput = Input.GetAxis("Vertical") * moveSpeed;
        //保存水平轴的输入 Input.GetAxis("Horizontal")用于检查左右方向键以及A和D何时被按下，右方向键按下返回1，左方向键按下返回-1
        hInput = Input.GetAxis("Horizontal") * rotateSpeed;

        //只有玩家按下空格键并且已经触底才允许跳跃
        if (isGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            //向量沿着up方向施加力，并乘以jumpVelocity
            //ForceMode是枚举类型，它决定了力是如何施加的，Impulse表示给对象传递考虑了物体质量的即时力
            _rigidbody.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        Vector3 rotation = Vector3.up * hInput;

        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        _rigidbody.MovePosition(this.transform.position + this.transform.forward * vInput * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(_rigidbody.rotation * angleRot);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //this.transform.position
            //当鼠标左键按下时，根据当前胶囊的朝向实例化新的子弹，并强制转换返回GameObject
            GameObject newBullet = Instantiate(bullet, Bullets.transform.position, this.transform.rotation) as GameObject;

            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            //设置rigidbody组件的velocity属性为玩家的transform.forward方向乘子弹速度，
            //通过直接修改velocity而不是addForce方法，可以保证开火时重力不会使弹道下坠而成为弧形
            bulletRB.velocity = this.transform.forward * bulletSpeed;
        }
    }

    private bool isGrounded()
    {
        //创建一个vector3 来保存Player对象的CapsuleCollider组件的底部位置，将使用该位置判定与Ground层级中的对象发生的碰撞
        //碰撞体的底部是指三维空间中的点的坐标(center.x，min.y，center.z)
        Vector3 capsuleBottom = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y, _collider.bounds.center.z);
        /*
         * 这里接收5个参数，胶囊的起始位置 胶囊的结束位置 胶囊的半径 想要用来检查碰撞的层遮罩 
         * 触发器的查询行为决定了CheckCapsule方法是否忽略设置为触发器的碰撞体，因为不需要检查触发器所以使用枚举ignore
         */
        bool grounded = Physics.CheckCapsule(_collider.bounds.center, capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);
        return grounded;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"collision name :{collision.gameObject.name}");

        //这里检测当玩家与敌人碰撞时，设置全局变量的玩家hp减1
        if (collision.gameObject.name == "Enemy")
        {
            Debug.Log($"Player has been attacked! HP remaining ：{gameManager.HP -= 1}");
            gameManager.HP -= 1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(this.transform.position, this.transform.forward * 10);//10是长度
    }

}