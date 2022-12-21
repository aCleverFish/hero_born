
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    public GameBehaviour gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameBehaviour>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Destroy(this.transform.parent.gameObject);

            Debug.Log("Item collected!");

            //当对象被销毁后，gameManager中增加Items属性值
            gameManager.Items += 1;
        }
    }
}
