using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBehaviour : MonoBehaviour
{

    public string labelText = "�ռ������ĸ���Ʒ�����ʤ��";
    public int maxItems = 4;

    public bool showWinScreen = false;
    public bool showLossScreen = false;

    private int itemsCollected = 0;
    private int playerHP = 10;

    //�����˽�б����ṩ��ȡ����get set����
    public int Items { 
        get { return itemsCollected; } 
        set { itemsCollected = value;
              Debug.LogFormat("Item: {0}", itemsCollected);
              if (itemsCollected >= maxItems)
              {
                 labelText = "���Ѿ�����������Ʒ��";
                 showWinScreen = true;
                 //��timeScaleΪ0ʱ����Ϸ��ͣ��ֹͣ�κ�����
                 Time.timeScale = 0f;
              }
              else
              {
                 labelText = $"���ҵ�{itemsCollected}����Ʒ����ʣ{maxItems - itemsCollected}����Ʒ���������ͣ�";
              }
            }
    }

    public int HP
    {
        get { return playerHP; }
        set { 
                playerHP = value;
                Debug.Log($"Lives��{playerHP}");
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
        GUI.Box(new Rect(20,20,150,25), "���������" + HP);
        GUI.Box(new Rect(20, 50, 150, 25), "���ռ���Ʒ" + itemsCollected);

        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 300, 50), labelText);

        if (showWinScreen)
        {
            if (GUI.Button(new Rect(Screen.width/2 -100, Screen.height/2 -50, 200, 100), "��Ӯ�ˣ�"))
            {
                //����һ����������ʾ��ǰ����������Ŀǰֻ��һ���������Բ���Ϊ0����ʾ���¿�ʼ��ǰ����
                SceneManager.LoadScene(0);
                //���´򿪳����󣬰Ѹ�ֵ����Ϊ1����ʾ���Խ�������
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
