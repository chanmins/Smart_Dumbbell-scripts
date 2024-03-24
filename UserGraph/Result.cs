using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChartAndGraph;

public class Result : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Dropdown dropdown;
    public GraphChart chart;
    public HorizontalAxis horizontalAxis;
    public VerticalAxis verticalAxis;
    DateTime now = DateTime.Now;
    public TextMeshProUGUI date1, date2, date3, date4, date5;           //날짜 처음부터
    void Start()
    {
        chart.AutoScrollVertically = false;

        Change_Graph(dropdown.value);

        //주석주석
            // SM.SetActive(false);
            // VA.SetActive(false);
            // VS.SetActive(false);
            // VR.SetActive(false);
            // DA.SetActive(false);

            // //값 넣기
            // chart.DataSource.AddPointToCategory("Player", 0, 42);
            // chart.DataSource.AddPointToCategory("Player", 1, 45);
            // chart.DataSource.AddPointToCategory("Player", 2, 37);
            // chart.DataSource.AddPointToCategory("Player", 3, 41);

            // chart.DataSource.AddPointToCategory("Standard", 0, 40);
            // chart.DataSource.AddPointToCategory("Standard", 4, 40);

            //db에서 시간값 받아오면 이걸로 하면 됨.
            // string text1 = "2022-11-24 PM 2:12:30";
            // string[] text2 = text1.Split(' ');
            // string text3 = text2[0].Replace('-', '/');


            //여기서부터 디비에 있는 값들 담아서 x축에 넣어주는거
            

            // date1.GetComponent<TextMeshProUGUI>().text = text3;

            // print(text3);

            // chart.HorizontalScrolling = ChartDateUtility.TimeSpanToValue(TimeSpan.FromDays(5));
            // chart.HorizontalScrolling = ChartDateUtility.DateToValue(DateTime.Today);
            // chart.HorizontalScrolling = ChartDateUtility.DateToValue(now.AddDays(1));
            // chart.HorizontalScrolling = 1546300800;

            // chart.DataSource.VerticalViewSize = 50;  // 단위조정
            // chart.DataSource.HorizontalViewSize = 5;

            // verticalAxis.MainDivisions.Total = 5;
            // verticalAxis.SubDivisions.Total = 0;
            // horizontalAxis.MainDivisions.Total = 5;
            // horizontalAxis.SubDivisions.Total = 0;
            // horizontalAxis


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Change_Graph(int v)
    {
        //날짜 배열
        TextMeshProUGUI[] date = {date1, date2, date3, date4, date5};

        //그래프 초기화
        chart.DataSource.ClearCategory("Player");
        chart.DataSource.ClearCategory("Standard");
        for(int j = 0; j<date.Length; j++)
        {   
            date[j].GetComponent<TextMeshProUGUI>().text = " ";
        }
           
           
        //db에서 날짜 받아오는거, 일단은 전체다 받아오는 걸로 생각함.
        string[][] dateArray = new string[][]
        {
            new string[] {"2022-11-24 PM 2:12:30", "2022-11-26 PM 2:12:30", "2022-11-28 PM 2:12:30"},
            new string[] {"2022-11-24 PM 2:12:30", "2022-11-26 PM 2:12:30", "2022-11-28 PM 2:12:30", "2022-11-30 PM 2:12:30"},
            new string[] {"2022-11-24 PM 2:12:30", "2022-11-26 PM 2:12:30", "2022-11-28 PM 2:12:30"},
            new string[] {"2022-11-24 PM 2:12:30", "2022-11-26 PM 2:12:30", "2022-11-28 PM 2:12:30"},
            new string[] {"2022-11-24 PM 2:12:30", "2022-11-26 PM 2:12:30", "2022-11-28 PM 2:12:30", "2022-11-30 PM 2:12:30"},
            new string[] {"2022-11-24 PM 2:12:30", "2022-11-26 PM 2:12:30", "2022-11-28 PM 2:12:30", "2022-11-30 PM 2:12:30","2022-12-01 PM 2:12:30"}
        };
        
        //점수 받아오기. 위와 마찬가지.
        string[][] resultData = new string[][]
        {
            new string[] {"32.8","25.4","44.9"},    //해머링
            new string[] {"4","5","6","8"},         //도어락
            new string[] {"8.8","5.8","12.3"},    //사이먼
            new string[] {"13","25","17"},    //넘버퍼즐
            new string[] {"0.8","0.95","1.3", "1.5"},    //신호등
            new string[] {"83.6","75.2","79.0","62.7", "58.4"},    //엔백
        };

        //축 설정해주기
        switch(v){
            //Hammering(Working Memory)
            case 0:
                //기준치 설정
                chart.DataSource.AddPointToCategory("Standard", 0, 40);
                chart.DataSource.AddPointToCategory("Standard", 4, 40);
                //축 사이즈 설정
                verticalAxis.MainDivisions.Total = 1;
                verticalAxis.SubDivisions.Total = 5;
                chart.DataSource.VerticalViewSize = 50;
                break;
            //Doorlock(Short-Term-Memory)
            case 1:
                chart.DataSource.AddPointToCategory("Standard", 0, 6);
                chart.DataSource.AddPointToCategory("Standard", 4, 6);
                verticalAxis.MainDivisions.Total = 1;
                verticalAxis.SubDivisions.Total = 3;
                chart.DataSource.VerticalViewSize = 15;
                break;
            //Simon(Visual Attention)
            case 2:
                chart.DataSource.AddPointToCategory("Standard", 0, 7); //75퍼센트 기준(50개)
                chart.DataSource.AddPointToCategory("Standard", 4, 7);
                verticalAxis.MainDivisions.Total = 1;
                verticalAxis.SubDivisions.Total = 4;
                chart.DataSource.VerticalViewSize = 20;
                break;
            //Number Puzzle(Visual Searching)
            case 3:
                chart.DataSource.AddPointToCategory("Standard", 0, 20); //75퍼센트 기준(50개)
                chart.DataSource.AddPointToCategory("Standard", 4, 20);
                verticalAxis.MainDivisions.Total = 1;
                verticalAxis.SubDivisions.Total = 7;
                chart.DataSource.VerticalViewSize = 35;
                break;
            //Traffic Light(Visual Reaction)
            case 4:
                chart.DataSource.AddPointToCategory("Standard", 0, 1); //75퍼센트 기준(50개)
                chart.DataSource.AddPointToCategory("Standard", 4, 1);
                verticalAxis.MainDivisions.Total = 1;
                verticalAxis.SubDivisions.Total = 10;
                chart.DataSource.VerticalViewSize = 2;
                break;
            //N-Back(Devided Attention) 2Back
            case 5:
                chart.DataSource.AddPointToCategory("Standard", 0, 75); //75퍼센트 기준(50개)
                chart.DataSource.AddPointToCategory("Standard", 4, 75);
                verticalAxis.MainDivisions.Total = 1;
                verticalAxis.SubDivisions.Total = 10;
                chart.DataSource.VerticalViewSize = 100;
                break;
        }


        //db에서 배열로 받아오기
        int changeScore = 0;

        int[] showDataInt = new int[resultData[v].Length];
        float[] showDataFloat = new float[resultData[v].Length];

        if((v == 1 )^ (v == 3))
        {
            foreach(string s in resultData[v])
            {
                int score = int.Parse(s);
                showDataInt[changeScore] = score;
                changeScore++;
            }
            // print(showDataInt[0]);
        }
        else
        {
            foreach(string s in resultData[v])
            {
                float score = float.Parse(s);
                showDataFloat[changeScore] = score;
                changeScore++;
            }
            // print(showDataFloat[0]);
        }

         //날짜 + 값 넣기
        int i = 0;
        
        foreach(string s in dateArray[v])
        {
            //x축 날짜 넣기
            string text1 = s;
            string[] text2 = text1.Split(' ');
            string text3 = text2[0].Replace('-', '/');
            date[i].GetComponent<TextMeshProUGUI>().text = text3;

            //데이터 값 넣기
            if((v == 1 )^(v == 3))
            {
                chart.DataSource.AddPointToCategory("Player", i, showDataInt[i]);     //넣을 카테고리 이름, x축 위치, y축 위치
            }
            else
            {
                chart.DataSource.AddPointToCategory("Player", i, showDataFloat[i]);     //넣을 카테고리 이름, x축 위치, y축 위치
            }

            i++;
        }

        //주석주석
            //각각 컨텐츠별 그래프 y축 기준 바꾸기
            // if(v==0)
            // {
            //     verticalAxis.MainDivisions.Total = 1;
            //     verticalAxis.SubDivisions.Total = 5;
            // }
        }

    public void Function_Dropdown()
    {
        Change_Graph(dropdown.value);
    }
}
