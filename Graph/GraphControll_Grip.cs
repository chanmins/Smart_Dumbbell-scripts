using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using ChartAndGraph;
using SerialData;
using TMPro;
using Data;

//데이터 저장할 클래스, 악력
public class MaxPowerData
{
    public float time { get; set; }
    public float data { get; set; }

    public MaxPowerData(float t, float d)
    {
        time = t;
        data = d;
    }
}

public class TrackingData
{
    public float time { get; set; }
    public float data { get; set; }
    public float guide { get; set; }

    public TrackingData(float t, float d, float g)
    {
        time = t;
        data = d;
        guide = g;
    }
}

public class GraphControll_Grip : MonoBehaviour
{
    GameObject inputField;
    GameObject legend;
    Text title;
    Text result;
    public GraphChart chart;
    public HorizontalAxis horizontalAxis;
    public VerticalAxis verticalAxis;

    public GameObject canvas;
    public GameObject startPanel;
    public int power = 0;
    public float halfPower = 0f;
    float timer;
    float startOffset = 2f;
    string measurementIndex = "";
    public List<MaxPowerData> maxPowerDatas = new List<MaxPowerData>();
    public List<TrackingData> trackingDatas = new List<TrackingData>();
    public AudioSource beep;
    public AudioClip beepClip;
    float rmsetmp = 0f;

    //0502
    public float gripMax;
    public int count;
    public GameObject MeasureScript;

    //0530

    [SerializeField]
    GameObject[] TextArray = new GameObject[8];


    //0731
    public MeasureGrip gripDTO;
    public WebRequest Server;

    void Start()
    {
        power = 0;
        halfPower = 0f;
        startOffset = 2f;
        count = -1;                             
        gripMax = -1f;

        legend = canvas.transform.GetChild(0).GetChild(5).gameObject;

        //0530
        TextArray[0] = GameObject.Find("First_Left");
        TextArray[1] = GameObject.Find("First_Right");
        TextArray[2] = GameObject.Find("Second_Left");
        TextArray[3] = GameObject.Find("Second_Right");
        TextArray[4] = GameObject.Find("Third_Left");
        TextArray[5] = GameObject.Find("Third_Right");
        TextArray[6] = GameObject.Find("Avg_Left");
        TextArray[7] = GameObject.Find("Avg_Right");
        for (int i = 0; i < 8; i++)
        {
            TextArray[i].GetComponent<TMP_Text>().text = "";
        }

        gripDTO = new MeasureGrip();
        Server = GameObject.Find("Server").GetComponent<WebRequest>();
    }

    //그래프 설정
    /*public void SetGraph()
    {
        if (measurementIndex == "maxPower")
        {
            //메인패널 끄고 그래프 패널 켜기
            canvas.transform.GetChild(0).gameObject.SetActive(false);
            canvas.transform.GetChild(1).gameObject.SetActive(true);
            //레전드 세팅
            SetlLegend();
            title.text = "Max. Strength";
            inputField.SetActive(false);
            chart.DataSource.ClearCategory("GuideLine");
            chart.DataSource.ClearCategory("Data");
            *//*            chart.DataSource.ClearCategory("RisingTIme1");
                        chart.DataSource.ClearCategory("RisingTIme2");*//*
            timer = 0f;
            maxPowerDatas.Clear();
            chart.DataSource.VerticalViewSize = 45;  // 세로축
            chart.DataSource.HorizontalViewSize = 15; // 가로축

            // x,y축 설정
            verticalAxis.MainDivisions.Total = 5;
            verticalAxis.SubDivisions.Total = 2;
            horizontalAxis.MainDivisions.Total = 9;
            horizontalAxis.SubDivisions.Total = 0;
            //StartCoroutine(MeasureMaxPower());
            //StartCoroutine(ControllBeep());
        }

        else if (measurementIndex == "DCTracking")
        {
            //메인패널 끄고 그래프 패널 켜기
            canvas.transform.GetChild(0).gameObject.SetActive(false);
            canvas.transform.GetChild(1).gameObject.SetActive(true);
            //레전드 필요없는거 끄기
            SetlLegend();
            title.text = "Static endurance";
            inputField.SetActive(false);
            chart.DataSource.StartBatch();
            chart.DataSource.ClearCategory("GuideLine");
            chart.DataSource.ClearCategory("Data");
            timer = 0f;
            rmsetmp = 0f;
            trackingDatas.Clear();
            chart.DataSource.VerticalViewSize = power + 2; //halfPower;
            chart.DataSource.HorizontalViewSize = 20 + startOffset;

            //x,y축 설정
            verticalAxis.MainDivisions.Total = power + 2;
            verticalAxis.SubDivisions.Total = 0;
            horizontalAxis.MainDivisions.Total = 20 + (int)startOffset;
            horizontalAxis.SubDivisions.Total = 0;

            //가이드라인 그리기
            chart.DataSource.StartBatch();
            chart.DataSource.AddPointToCategory("GuideLine", 2, power * 0.5f);
            chart.DataSource.AddPointToCategory("GuideLine", 20 + startOffset, power * 0.5f);
            chart.DataSource.EndBatch();

            //StartCoroutine(MeasureDCTracking());
        }

        else if (measurementIndex == "SINTracking")
        {
            canvas.transform.GetChild(0).gameObject.SetActive(false);
            canvas.transform.GetChild(1).gameObject.SetActive(true);
            //레전드 세팅
            SetlLegend();
            title.text = "Force control";
            inputField.SetActive(false);
            chart.DataSource.ClearCategory("GuideLine");
            chart.DataSource.ClearCategory("Data");
            chart.DataSource.ClearCategory("RisingTIme1");
            chart.DataSource.ClearCategory("RisingTIme2");
            timer = 0f;
            rmsetmp = 0f;
            trackingDatas.Clear();
            chart.DataSource.VerticalViewSize = power; //halfPower;
            chart.DataSource.HorizontalViewSize = 20 + startOffset;

            //x,y축 설정
            verticalAxis.MainDivisions.Total = power;
            verticalAxis.SubDivisions.Total = 0;
            horizontalAxis.MainDivisions.Total = 20 + (int)startOffset;
            horizontalAxis.SubDivisions.Total = 0;

            chart.DataSource.StartBatch();
            for (float i = startOffset; i < 20 + startOffset; i += 0.1f)
            {
                chart.DataSource.AddPointToCategory("GuideLine", i, Mathf.Sin((0.2f * (Mathf.PI)) * (i - startOffset) - (0.5f * (Mathf.PI))) * (power / 4f) + (power / 4f));//chart.DataSource.AddPointToCategory("GuideLine",i,Mathf.Sin((0.2f * (Mathf.PI)) * i-(0.5f* (Mathf.PI))) * (halfPower / 4f) + (halfPower / 4f));
            }
            chart.DataSource.EndBatch();
            //StartCoroutine(MeasureSinTracking());
        }
    }*/

    public void setPower()
    {
        startPanel.SetActive(true);
        power = int.Parse(startPanel.transform.GetChild(0).GetComponent<InputField>().text);
    }

    public void StartMeasurement()
    {
        if (measurementIndex == "maxPower")
        {
            StartCoroutine(ControllBeep());
            StartCoroutine(MeasureMaxPower());
        }

        else if (measurementIndex == "DCTracking")
        {
            //power = int.Parse(inputField.GetComponent<InputField>().text);
            //halfPower = power / 2f;
            StartCoroutine(MeasureDCTracking());
        }

        else if (measurementIndex == "SINTracking")
        {
            //power = int.Parse(inputField.GetComponent<InputField>().text);
            //halfPower = power / 2f;
            StartCoroutine(MeasureSinTracking());
        }
    }
    //레전드 세팅
    void SetlLegend()
    {
        if (measurementIndex == "maxPower")
        {
            legend.transform.GetChild(0).gameObject.SetActive(false);
            legend.transform.GetChild(1).gameObject.SetActive(true);
            legend.transform.GetChild(2).gameObject.SetActive(true);
            legend.transform.GetChild(3).gameObject.SetActive(true);
        }

        else if (measurementIndex == "DCTracking")
        {
            legend.transform.GetChild(0).gameObject.SetActive(true);
            legend.transform.GetChild(1).gameObject.SetActive(true);
            legend.transform.GetChild(2).gameObject.SetActive(false);
            legend.transform.GetChild(3).gameObject.SetActive(false);
        }

        else if (measurementIndex == "SINTracking")
        {
            legend.transform.GetChild(0).gameObject.SetActive(true);
            legend.transform.GetChild(1).gameObject.SetActive(true);
            legend.transform.GetChild(2).gameObject.SetActive(false);
            legend.transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    //Beep사운드 https://blog.naver.com/PostView.nhn?blogId=ckdduq2507&logNo=222113891105&parentCategoryNo=&categoryNo=74&viewDate=&isShowPopularPosts=true&from=search
    IEnumerator ControllBeep()
    {
        yield return new WaitForSeconds(2.7f);
        beep.PlayOneShot(beepClip);
        yield return new WaitForSeconds(3f);
        beep.PlayOneShot(beepClip);
        yield return null;
    }

    //최대악력 측정 함수
    IEnumerator MeasureMaxPower()
    {
        //UserData 그래프 
        while (timer <= 15)//측정 시간
        {
            maxPowerDatas.Add(new MaxPowerData(timer, InputData.force));
            chart.DataSource.AddPointToCategoryRealtime("Data", timer, InputData.force);    // 이부분이 차트 데이터 구간.
            timer += Time.deltaTime;
            yield return null;
        }

        //risingTime 그려주기
        var risingStart = maxPowerDatas.Max(x => x.data) * 0.9;
        var risingEnd = maxPowerDatas.Max(x => x.data) * 0.1;
        gripMax = maxPowerDatas.Max(x => x.data);

        chart.DataSource.StartBatch();

        float i = 0;
        while (i < 10)
        {
            chart.DataSource.AddPointToCategory("RisingTime1", i, risingStart);     // 단위조정
            chart.DataSource.AddPointToCategory("RisingTime2", i, risingEnd);
            i += 0.18f;
        }

        var risingTIme = maxPowerDatas
            .Where(x => x.data >= risingEnd)
            .Min(x => x.time) - maxPowerDatas
            .Where(x => x.data >= risingStart)
            .Min(x => x.time);


        chart.DataSource.EndBatch();

        //releaseTime 계산
        var tmpList = maxPowerDatas.Where(x => x.time >= 6).ToList();


        TextArray[count].GetComponent<TMP_Text>().text = gripMax.ToString();
        MeasureScript.GetComponent<Measurement_M>().gripStrength[count] = gripMax;

        yield return new WaitForSeconds(0.3f);

        
        if (count == 5)
        {
            float left = ((MeasureScript.GetComponent<Measurement_M>().gripStrength[0] +
                                                           MeasureScript.GetComponent<Measurement_M>().gripStrength[2] +
                                                           MeasureScript.GetComponent<Measurement_M>().gripStrength[4]) / 3);

            float right = ((MeasureScript.GetComponent<Measurement_M>().gripStrength[1] +
                                                           MeasureScript.GetComponent<Measurement_M>().gripStrength[3] +
                                                           MeasureScript.GetComponent<Measurement_M>().gripStrength[5]) / 3);

            gripDTO.id = Server.baseData.id;
            gripDTO.grip_left = left;
            gripDTO.grip_right = right;
            gripDTO.date = DateTime.Now.ToString("yyyy-MM-dd");

            
            string url = "https://port-0-api-7xwyjq992llj595n50.sel4.cloudtype.app/Measure/Grip";
            string json = JsonUtility.ToJson(gripDTO);

            print(json);

            StartCoroutine(Server.SendJsonData(url, json));

            TextArray[6].GetComponent<TMP_Text>().text = left.ToString();

            TextArray[7].GetComponent<TMP_Text>().text = right.ToString();
        }

        yield return new WaitForSeconds(0.3f);


    }

    // DC 트래킹 측정 (static endurance)
    IEnumerator MeasureDCTracking()
    {
        //User Data 그래프
        while (timer >= 0 && timer < startOffset)
        {
            var fingertmp = InputData.force;
            chart.DataSource.AddPointToCategoryRealtime("Data", timer, fingertmp);
            timer += Time.deltaTime;
            Debug.Log("timer : " + timer);
            yield return null;
        }
        while (timer >= startOffset && timer <= 20 + startOffset)
        {
            var DCtmp = power * 0.5f;
            var fingertmp = InputData.force;
            var rmsePow = Mathf.Pow(DCtmp - fingertmp, 2f);

            trackingDatas.Add(new TrackingData(timer, fingertmp, DCtmp));
            chart.DataSource.AddPointToCategoryRealtime("Data", timer, fingertmp);
            Debug.Log("DC : " + DCtmp + ", finger : " + fingertmp + " RMSE : " + rmsePow);

            rmsetmp += rmsePow;
            timer += Time.deltaTime;
            yield return null;
        }

        canvas.transform.GetChild(2).gameObject.SetActive(true);    // 결과 패널
        var RMSE = Mathf.Sqrt(rmsetmp / trackingDatas.Count()) * 2f;
        result.text = "RMSE \n" + RMSE; //Mathf.Sqrt(rmsetmp / trackingDatas.Count());  
                                        //Debug.Log("평균 : " + RMSE); //Mathf.Sqrt(rmsetmp / trackingDatas.Count()));

        /*      Data.instance.rmseValue = RMSE;
              string query = "INSERT INTO measurement (date,userID,gameID,rmse) VALUES ('" + DateTime.Now.ToString("yyyy년 MM-dd일 HH시 mm분 ss초") + "','" + Data.instance.userID + "','" + "04M" + "','" + Data.instance.rmseValue + "')";
              DB.DatabaseInsert(query);*/
    }

    //사인파 트래킹 측정 
    IEnumerator MeasureSinTracking()
    {
        while (timer >= 0 && timer < startOffset)
        {
            var fingertmp = InputData.force;
            chart.DataSource.AddPointToCategoryRealtime("Data", timer, fingertmp);
            timer += Time.deltaTime;
            yield return null;
        }
        while (timer >= startOffset && timer <= 20 + startOffset)
        {
            var sintmp = Mathf.Sin((0.2f * (Mathf.PI)) * (timer - startOffset) - (0.5f * (Mathf.PI))) * (power / 4f) + (power / 4f);//var sintmp = Mathf.Sin((0.2f * (Mathf.PI)) * timer - (0.5f * (Mathf.PI))) * (halfPower / 4f) + (halfPower / 4f);
            var fingertmp = InputData.force;
            var rmsePow = Mathf.Pow(sintmp - fingertmp, 2f);
            trackingDatas.Add(new TrackingData(timer, fingertmp, sintmp));
            chart.DataSource.AddPointToCategoryRealtime("Data", timer, fingertmp);

            //Debug.Log("sin : " + sintmp + ", finger : " + fingertmp + " RMSE : " + rmsePow);

            rmsetmp += rmsePow;
            timer += Time.deltaTime;
            yield return null;
        }

        canvas.transform.GetChild(2).gameObject.SetActive(true);
        var RMSE = Mathf.Sqrt(rmsetmp / trackingDatas.Count()) * 2f;
        result.text = "RMSE\n" + RMSE;
        // Debug.Log( "평균 : " + RMSE);

        //Data.instance.rmseValue = RMSE;
        //string query = "INSERT INTO measurement (date,userID,gameID,rmse) VALUES ('" + DateTime.Now.ToString("yyyy년 MM-dd일 HH시 mm분 ss초") + "','" + Data.instance.userID + "','" + "04M" + "','" + Data.instance.rmseValue + "')";
        //DB.DatabaseInsert(query);
    }

    //버튼으로 모드 변경
/*    public void SetMeasurementIndex(string s)
    {
        measurementIndex = s;
        if (s == "maxPower")
        {
            SetGraph();
        }
    }*/

    //---------------------------------------------------------------------------
    //밑으로 추가

    public void StartGripStrength()
    {
        count++;
        maxPowerDatas.Clear();
        chart.DataSource.ClearCategory("Data");
        chart.DataSource.ClearCategory("RisingTime1");
        chart.DataSource.ClearCategory("RisingTime2");
        StartCoroutine(ControllBeep());
        timer = 0;
        StartCoroutine(MeasureMaxPower());
    }

}
