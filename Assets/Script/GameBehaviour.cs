using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBehaviour : MonoBehaviour
{

    public string labelText = "收集所有四个物品来获得胜利";
    public int maxItems = 4;

    public bool showWinScreen = false;
    public bool showLossScreen = false;

    private int itemsCollected = 0;
    private int playerHP = 10;

    //这里给私有变量提供置取方法get set方法
    public int Items { 
        get { return itemsCollected; } 
        set { itemsCollected = value;
              Debug.LogFormat("Item: {0}", itemsCollected);
              if (itemsCollected >= maxItems)
              {
                 labelText = "你已经集齐所有物品了";
                 showWinScreen = true;
                 //当timeScale为0时，游戏暂停，停止任何输入
                 Time.timeScale = 0f;
              }
              else
              {
                 labelText = $"已找到{itemsCollected}个物品，还剩{maxItems - itemsCollected}个物品，继续加油！";
              }
            }
    }

    public int HP
    {
        get { return playerHP; }
        set { 
                playerHP = value;
                Debug.Log($"Lives：{playerHP}");
                if (playerHP <= 0)
                {
                    labelText = "You want another life with that ?";
                    showLossScreen = true;
                    Time.timeScale = 0;
                }
                else
                {
                    labelText = "Ouch... that's got hurt.";
                }
            }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(20,20,150,25), "玩家生命：" + HP);
        GUI.Box(new Rect(20, 50, 150, 25), "已收集物品" + itemsCollected);

        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 300, 50), labelText);

        if (showWinScreen)
        {
            if (GUI.Button(new Rect(Screen.width/2 -100, Screen.height/2 -50, 200, 100), "你赢了！"))
            {
                //接收一个参数来表示当前场景索引，目前只有一个场景所以参数为0，表示重新开始当前场景
                SceneManager.LoadScene(0);
                //重新打开场景后，把该值设置为1，表示可以进行输入
                Time.timeScale = 1.0f;
            }
        }

        if (showLossScreen)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 100), "You lose ..."))
            {
                SceneManager.LoadScene(0);
                Time.timeScale = 1.0f;
            }
        }
    }
}
