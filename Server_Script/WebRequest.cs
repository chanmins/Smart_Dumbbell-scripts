using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Data;

public class WebRequest : MonoBehaviour
{

    public static WebRequest instance;
    public string response;
    public Login baseData;


    //싱글톤 해야함.
    private void Awake()
    {
        if(instance == null)
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
        response = "";
        baseData = new Login();
    }

    public void sendButton()
    {
        //Debug.Log("아이디 : " + id.text + " 비밀번호 : " + pswd.text);


        try
        {
            //StartCoroutine(SendJsonData());
        }
        catch (Exception e)
        {
            print(e);
        }

    }

    public IEnumerator SendJsonData(string url, string json)
    {
        /*MyData data = new MyData();//클래스 객체 생성

        

        //객체 타입에 맞게 저장
        data.id = id.text.ToString();
        data.password = pswd.text.ToString();
        

        //중요
        string url = "http://localhost:8080/postMethod";//상황에 맞는 api 호출 -> 매핑된 url을 호출
        string json = JsonUtility.ToJson(data);//유니티에서 일반적인 클래스를 json으로 바꿔준다.*/

        UnityWebRequest request = UnityWebRequest.Post(url, json);

        request.SetRequestHeader("Content-Type", "application/json");

        //*******중요******
        byte[] jsonToSend = new UTF8Encoding().GetBytes(json);//바이트 배열 생성 후 json을 다시 인코딩
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            response = request.downloadHandler.text;
            
            Debug.Log(response);
        }
    }


}


namespace Data
{
    public class Login
    {
        public string id;
        public string password;
    }

    public class UserInfo
    {
        public string name;
        public string id;
        public string password;
        public string weight;
        public string height;
        public string birth;
        public string gender;
        public string forgetText;
        public string arm;
    }

    public class sarc
    {
        public string id;
        public int sarc_f;
        public string disease;
    }

    public class forgetId
    {
        public string name;
        public string birth;
    }
    
    public class forgetPW
    {
        public string name;
        public string id;
        public string forgetText;
    }
    
    public class CreateMeasureRow
    {
        public string id;
        public string date;
    }

    public class MeasureGrip
    {
        public string id;
        public string date;
        public float grip_left;
        public float grip_right;
    }

    public class MeasureCount
    {
        public string id;
        public string date;
        public int count_left;
        public int count_right;        
    }

    public class PlayContentData
    {
        public string userId;
        public string date;
        public int contentNumber;
        public int score;
    }
    
    public class PlayContentCount
    {
        public string userId;
        public string date;
        public int contentNumber;
    }
}