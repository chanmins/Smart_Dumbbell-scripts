using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using ChartAndGraph;
using SerialData;
using TMPro;
using updataData;
using Data;

public class CurlData
{
    public float time { get; set; }
    public float data { get; set; }

    public CurlData(float t, float d)
    {
        time = t;
        data = d;
    }
}



public class GraphControll_curl : MonoBehaviour
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
    public List<CurlData> CurlData = new List<CurlData>();
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
    GameObject[] TextArray = new GameObject[3];

    public GameObject serial;
    public WebRequest Server;


    void Start()
    {
        serial = GameObject.Find("Serial");

        power = 0;
        halfPower = 0f;
        startOffset = 2f;
        count = -1;
        gripMax = -1f;

        legend = canvas.transform.GetChild(0).GetChild(5).gameObject;

        //0530
        TextArray[0] = GameObject.Find("Result_Left");
        TextArray[1] = GameObject.Find("Result_Right");
        TextArray[2] = GameObject.Find("Count");

        for (int i = 0; i < 2; i++)
        {
            TextArray[i].GetComponent<TMP_Text>().text = "";
        }

        Server = GameObject.Find("Server").GetComponent<WebRequest>();
    }

    //그래프 설정


    public void setPower()
    {
        startPanel.SetActive(true);
        power = int.Parse(startPanel.transform.GetChild(0).GetComponent<InputField>().text);
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
        serial.GetComponent<Serial>().offsetflag = true;
        yield return new WaitForSeconds(0.5f);
        serial.GetComponent<Serial>().offsetflag = false;
        yield return new WaitForSeconds(2.2f);
        beep.PlayOneShot(beepClip);
        yield return new WaitForSeconds(3f);
        beep.PlayOneShot(beepClip);
        yield return null;
    }




    IEnumerator MeasureCurlCount()
    {
        serial.GetComponent<Serial>().curlflag = true;

        //UserData 그래프 
        while (timer <= 30)//측정 시간
        {

            CurlData.Add(new CurlData(timer, contentData.y));
            chart.DataSource.AddPointToCategoryRealtime("Data", timer, contentData.y);    // 이부분이 차트 데이터 구간.
            timer += Time.deltaTime;
            yield return null;
        }

        GameObject.Find("Serial").GetComponent<Serial>().curlflag = false;

        chart.DataSource.StartBatch();

        chart.DataSource.EndBatch();


        TextArray[count].GetComponent<TMP_Text>().text = "14";

        if (count == 1)
        {
            MeasureCount countDTO = new MeasureCount();

            countDTO.id = Server.baseData.id;
            countDTO.date = DateTime.Now.ToString("yyyy-MM-dd");
            countDTO.count_left = Int32.Parse(TextArray[0].GetComponent<TMP_Text>().text);
            countDTO.count_right = Int32.Parse(TextArray[1].GetComponent<TMP_Text>().text);
           
            string url = "https://port-0-api-7xwyjq992llj595n50.sel4.cloudtype.app/Measure/Count";
            string json = JsonUtility.ToJson(countDTO);

            StartCoroutine(Server.SendJsonData(url, json));
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


    //---------------------------------------------------------------------------
    //밑으로 추가

    public void StartCurlCount()
    {
        serial.GetComponent<Serial>().SerialSendinggyro();
        count++;
        CurlData.Clear();
        chart.DataSource.ClearCategory("Data");
        StartCoroutine(ControllBeep());
        timer = 0;
        StartCoroutine(MeasureCurlCount());
    }

}