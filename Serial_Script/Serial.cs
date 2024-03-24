using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using UnityEngine.UI;
using SerialData;
using updataData;
/*using TempData;*/
using System.Collections;


namespace SerialData
{
    [SerializeField]
    public static class InputData // 자이로 센서값
    {
        public static float battery;
        public static float x;
        public static float y;
        public static float z;
        public static float force;
    }
}

namespace updataData
{
    [SerializeField]
    public static class contentData
    {
        public static float x;
        public static float y;
        public static float z;
    }
}


public class Serial : MonoBehaviour
{
    /*
        serial 코드는 싱글톤 패턴을 이용해 지속적으로 모든 씬에서 사용
        컨텐츠 내에서 싱글톤으로 유지되어야 하는 코드
            Serial.c
            Class_All
                Class_All : 해당 스크립트는 모든 씬에서 유지되어 각 클래스에 저장함
                            싱글톤을 사용하지 않고 저장 할 경우 서버로 전송 전 유니티 생명주기 내에서 초기화
                            전송 시 Null
            
            Class_All 내부 클래스들은 dto 클래스 대신 LinQ를 사용하여 데이터 처리
       
     
     */
    string str = "";
    string[] str2;

    [SerializeField]
    float hx, hy, hz, hf;

    public float tempx;
    public float tempy;
    public float tempz;

    public GameObject cube;

    public bool sendingFlag, flag, loginflag;

    int count;
    public SerialPort sp;

    public int[] data;
    public string user_nickname;
    string[] tempstr;
    public Button measureBtt;

    Queue<string> queue = new Queue<string>();

    public static Serial instance;
    public GameObject gm; // 찾는걸로 변경.

    //0612
    public bool curlStartflag;
    public bool curlflag;
    public int curlCount;
    public bool offsetflag;

    public GameObject debugPannel;

    public float modeTime;

    private void Awake()
    {


        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        tempx = 0;
        tempy = 0;
        tempz = 0;

        curlCount = 0;

        loginflag = false;
        curlflag = false;
        sendingFlag = false;
        offsetflag = false;
        modeTime = 0;

        ConnectSerial();

/*        StartCoroutine(HandGripOffset());*/
    }

    public void ConnectSerial() // 검색해서 자동 연결
    {
        string[] ports = SerialPort.GetPortNames();
        foreach (string p in ports)
        {
            sp = new SerialPort(p, 115200, Parity.None, 8, StopBits.One); // 초기화

            try
            {
                sp.WriteTimeout = 1000;
                sp.Open(); // 프로그램 시작시 포트 열기
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                continue;
            }

            if (sp.ReadExisting().Equals(""))
            {
                continue;
            }
            else break;
        }

        sp.Write("AB5");
    }
    void Update()
    {


        if (sendingFlag)
        {
            MySerialReceived();
        }

        /*if (Input.GetKeyDown(KeyCode.D))//시리얼 디버거
        {
            SerialDebuger();
        }*/


        if (Input.GetKey(KeyCode.O))
        {
            SerialSendingOffset();
        }

        if (Input.GetKeyDown(KeyCode.E))//시작
        {
            SerialSendinggyro();
        }

        if (Input.GetKeyDown(KeyCode.T))//정지
        {
            SerialSendingStop();
        }

        if (Input.GetKeyDown(KeyCode.H))//악략일때
        {
            SerialSendingGrip();
            print("악력값 시작");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            curlStartflag = !curlStartflag;
        }


        //print(InputData.y);

        contentData.x = InputData.x - tempx;
        contentData.y = InputData.y - tempy;
        contentData.z = InputData.z - tempz;

        hx = contentData.x;
        hy = contentData.y;
        hz = contentData.z;
    }
    private void MySerialReceived()  //자이로 센서값(x ,y) 가공
    {
        // 인큐 과정
        string tmp = sp.ReadExisting(); //업데이트 마다 현재 입력 버퍼에서 가져옴
        //print(tmp);
        str2 = tmp.Split('\n');

        foreach (string s in str2)
        {
            if (s.Replace("\r", "").Replace("\n", "").Equals(""))
                continue;
            queue.Enqueue(s);
            GetDataFromQueue();
        }

    }


    void GetDataFromQueue() //디큐 과정
    {

        if (queue.Any()) // 큐안에 값이 존재한다면.
        {
            if (str == "")
            {
                str = queue.Dequeue();
            }
            else if (!str.Contains('a'))
            {
                str = "";
            }

            if (str.Contains('a') && str.Contains('b'))
            {
                dataSave();
            }
            else if (queue.Any())
            {
                str += queue.Dequeue();
            }
        }

    }


    //x 랑 y랑 바뀐거 같은데
    void dataSave()
    {
        tempstr = str.Split(','); //저장전 가공
        try
        {

            InputData.battery = float.Parse(tempstr[1]);
            /*InputData.y = (Mathf.Sqrt(Mathf.Pow(float.Parse(tempstr[2]),2)));
            InputData.x = (Mathf.Sqrt(Mathf.Pow(float.Parse(tempstr[3]), 2)));*/
            InputData.y = float.Parse(tempstr[2]);
            InputData.x = float.Parse(tempstr[3]);
            InputData.z = (float.Parse(tempstr[4]));
            InputData.force = float.Parse(tempstr[5]);
        }
        catch (Exception ex)
        {

        }

        str = "";
    }

    //동작이 시작할 때 오프셋 잡아야함.
    //***********시리얼 센서제어 및 센서 값 Debug*********************
    public void SerialSendinggyro()//유니티 -> Serial (E)전송
    {
        print("Send-Gyro");
        sp.Write("ABE");
        sendingFlag = true;
        flag = true;
    }

    public void SerialSendingGrip()//악력값 받기
    {
        print("Send-Grip");
        sp.Write("ABH");
        sp.Write("ABO");
        sendingFlag = true;
        flag = false;
    }

    public void SerialSendingStop()//멈추기
    {
        sp.Write("ABT");
        //Debug.Log("정지");
        sendingFlag = false;

    }

    public void SerialSendingOffset()//값 리셋
    {
        tempx = InputData.x;
        tempy = InputData.y;
        tempz = InputData.z;

        print("Send-Offset");

        sendingFlag = true;
    }

    public IEnumerator SetGripMode()
    {
        while(modeTime <= 0.8)
        {
            SerialSendingGrip();
            modeTime += Time.deltaTime;
            yield return null;
        }
        modeTime = 0;
    }

    public IEnumerator SetGyroMode()
    {
        while (modeTime <= 0.8)
        {
            SerialSendinggyro();
            modeTime += Time.deltaTime;
            yield return null;
        }


        while (modeTime <= 1.2)
        {
            modeTime += Time.deltaTime;
            yield return null;
        }

        while (modeTime <= 1.7)
        {
            SerialSendingOffset();
            modeTime += Time.deltaTime;
            yield return null;
        }

        modeTime = 0;
    }

    //***********시리얼 센서제어 및 센서 값 Debug*********************

    private void OnApplicationQuit()
    {
        sp.Close(); // 프로그램 종료시 포트 닫기
    }

    public IEnumerator HandGripOffset()
    {
        float time = 0;

        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            if (time > 0.5)
                break;

            sp.Write("ABH");
            time += Time.deltaTime;
            yield return null;

        }
    }

    public void CurlCount()
    {
        //오프셋 잡고
        //SerialSendingOffset();

        if (contentData.y > 70 && curlflag)
        {
            curlCount++;
            curlflag = false;
            print(curlCount);
        }
        else if (contentData.y < 10 && !curlflag)
        {
            curlflag = true;
            print(curlCount);
        }

    }

    public void SerialDebuger()
    {
        debugPannel.SetActive(true);

        debugPannel.transform.GetChild(1).GetComponent<Text>().text = InputData.x.ToString();
        debugPannel.transform.GetChild(2).GetComponent<Text>().text = InputData.y.ToString();
        debugPannel.transform.GetChild(3).GetComponent<Text>().text = InputData.z.ToString();
        debugPannel.transform.GetChild(4).GetComponent<Text>().text = InputData.force.ToString();
    }
}