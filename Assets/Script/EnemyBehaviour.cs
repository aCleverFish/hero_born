using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform player;
    public Transform patrolRoute;
    public List<Transform> locations;

    private int locationIndex = 0;
    private NavMeshAgent agent;

    private int _lives = 3;
    public int EnemyLives
    {
        get { return _lives; }
        set {
            if(_lives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy Down");
            }
            _lives = value; 
        }
    }


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        InitializePatrolRoute();
        MoveToNextPatrolLocation();
    }

    private void Update()
    {
        /*
         remainingDistance 表示的是NavMeshAgent的当前位置与目标位置destination之间的距离
         如果Unity正在为NavMesh计算路线，那么pathPending的值为true，反之为false
         */

        if (agent.remainingDistance < 0.2f && !agent.pathPending)
        {
            //如果agent非常接近目标，且不存在其他正在计算的路径，那么将会直接调用MoveToNextPatrolLocation方法
            MoveToNextPatrolLocation();
        }
    }
    void MoveToNextPatrolLocation()
    {
        //如果位置数组没有元素，则直接返回
        if (locations.Count == 0)
        {
            return;
        }
        agent.destination = locations[locationIndex].position;

        //这里的取模是为了循环取值
        locationIndex = (locationIndex + 1) % locations.Count;
    }
    void InitializePatrolRoute()
    {
        foreach (Transform child in patrolRoute)
        {
            locations.Add(child);
        }
    }
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            //当玩家进入攻击范围，它就会朝玩家移动
            agent.destination = player.position;
            Debug.Log("Enemy detected!");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player out of range, resume patrol");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            EnemyLives -= 1;
            Debug.Log("Critical Hit");
        }
    }

}
