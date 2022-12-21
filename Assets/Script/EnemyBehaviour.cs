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
         remainingDistance ��ʾ����NavMeshAgent�ĵ�ǰλ����Ŀ��λ��destination֮��ľ���
         ���Unity����ΪNavMesh����·�ߣ���ôpathPending��ֵΪtrue����֮Ϊfalse
         */

        if (agent.remainingDistance < 0.2f && !agent.pathPending)
        {
            //���agent�ǳ��ӽ�Ŀ�꣬�Ҳ������������ڼ����·������ô����ֱ�ӵ���MoveToNextPatrolLocation����
            MoveToNextPatrolLocation();
        }
    }
    void MoveToNextPatrolLocation()
    {
        //���λ������û��Ԫ�أ���ֱ�ӷ���
        if (locations.Count == 0)
        {
            return;
        }
        agent.destination = locations[locationIndex].position;

        //�����ȡģ��Ϊ��ѭ��ȡֵ
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
            //����ҽ��빥����Χ�����ͻᳯ����ƶ�
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
