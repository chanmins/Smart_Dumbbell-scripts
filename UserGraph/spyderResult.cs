using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChartAndGraph;
using Data;

public class spyderResult : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_Dropdown dropdown;
    public GraphChart RadarChart;
    public HorizontalAxis horizontalAxis;
    public VerticalAxis verticalAxis;
    DateTime now = DateTime.Now;
    public RadarChart radar;
    //public TextMeshProUGUI m1, m2, m3, m4, m5, m6;

    public WebRequest server;

    void Start()
    {
        radar= GameObject.Find("Radar").GetComponent<RadarChart>();

        server = GameObject.Find("Server").GetComponent<WebRequest>();

        Change_Graph(dropdown.value);

        
    }

    public void Change_Graph(int v)
    {

        //날짜 배열
        //TextMeshProUGUI[] date = { m1, m2, m3, m4, m5, m6 };

        //그래프 초기화

/*        GraphDataServer q = JsonUtility.FromJson<GraphDataServer>(server.GetComponent<WebRequest>().response);*/

/*        string[][] dateArray = new string[][]
        {
            new string[] {"content1", "content2","content3", "content4","content5","content6"},
            new string[] {"content1", "content2","content3", "content4","content5","content6"},
            new string[] {"content1", "content2","content3", "content4","content5","content6"},
            new string[] {"content1", "content2","content3", "content4","content5","content6"},
            new string[] {"content1", "content2","content3", "content4","content5","content6"},
            new string[] {"content1", "content2","content3", "content4","content5","content6"}
        };

        string[][] resultData = new string[][]
        {
            new string[] { q.content1Count.ToString(),q.content2Count.ToString(),q.content3Count.ToString(),q.content4Count.ToString(),q.content5Count.ToString(),q.content6Count.ToString()},    //해머링
            new string[] {"4","5","6","8"},         //도어락
            new string[] {"8.8","5.8","12.3"},    //사이먼
            new string[] {"13","25","17"},    //넘버퍼즐
            new string[] {"0.8","0.95","1.3", "1.5"},    //신호등
            new string[] {"83.6","75.2","79.0","62.7", "58.4"},    //엔백
        };*/


        switch (v)
        {
            //Hammering(Working Memory)
            case 0:
                radar.DataSource.SetValue("Player 1", "HangGlide", 14);
                radar.DataSource.SetValue("Player 1", "TongNamu", 17);
                radar.DataSource.SetValue("Player 1", "Horray",22);
                radar.DataSource.SetValue("Player 1", "Jump", 27);
                radar.DataSource.SetValue("Player 1", "CarJump", 20);
                break;
            //Doorlock(Short-Term-Memory)
            case 1:
                radar.DataSource.SetValue("Player 1", "HangGlide", 24);
                radar.DataSource.SetValue("Player 1", "TongNamu", 18);
                radar.DataSource.SetValue("Player 1", "Horray", 2);
                radar.DataSource.SetValue("Player 1", "Jump", 29);
                radar.DataSource.SetValue("Player 1", "CarJump", 10);
                break;
            //Simon(Visual Attention)
            case 2:
                radar.DataSource.SetValue("Player 1", "HangGlide", 22);
                radar.DataSource.SetValue("Player 1", "TongNamu", 3);
                radar.DataSource.SetValue("Player 1", "Horray", 8);
                radar.DataSource.SetValue("Player 1", "Jump", 24);
                radar.DataSource.SetValue("Player 1", "CarJump", 14);
                break;
            //Number Puzzle(Visual Searching)
            case 3:
                radar.DataSource.SetValue("Player 1", "HangGlide", 11);
                radar.DataSource.SetValue("Player 1", "TongNamu", 12);
                radar.DataSource.SetValue("Player 1", "Horray", 29);
                radar.DataSource.SetValue("Player 1", "Jump", 13);
                radar.DataSource.SetValue("Player 1", "CarJump", 10);
                break;
            //Traffic Light(Visual Reaction)
            case 4:
                radar.DataSource.SetValue("Player 1", "HangGlide", 27);
                radar.DataSource.SetValue("Player 1", "TongNamu", 12);
                radar.DataSource.SetValue("Player 1", "Horray", 13);
                radar.DataSource.SetValue("Player 1", "Jump", 9);
                radar.DataSource.SetValue("Player 1", "CarJump", 9);
                break;
            //N-Back(Devided Attention) 2Back
            case 5:
                radar.DataSource.SetValue("Player 1", "HangGlide", 25);
                radar.DataSource.SetValue("Player 1", "TongNamu", 13);
                radar.DataSource.SetValue("Player 1", "Horray", 8);
                radar.DataSource.SetValue("Player 1", "Jump", 4);
                radar.DataSource.SetValue("Player 1", "CarJump", 11);
                break;
        }

       
    }

    public void Function_Dropdown()
    {
        Change_Graph(dropdown.value);    
    }
}

