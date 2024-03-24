using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Reflection;
using Data;
using System;

public class Setting : MonoBehaviour
{
    private bool isSettingOpen;
    private bool isTimerRunning;

    public GameObject button;
    public GameObject panel;
    public Button btnSetting;

    //시간 초
    public float time;

    // 백그라운드 뮤직
    public AudioSource bgm;

    // 효과음 Example
    public AudioSource effectSource;
    private bool hasEffectSourceStarted;

    public Slider bgmController;
    public Slider effectController;

    public TMP_Text timerText;

    private WebRequest server;

    public Serial Serial;

    void Start()
    {
        // 설정 값
        isSettingOpen = false;
        isTimerRunning = false;
        button.SetActive(true);
        time = 0f;

        Serial = GameObject.Find("Serial").GetComponent<Serial>();

        server = GameObject.Find("Server").GetComponent<WebRequest>();

        // 백그라운드 뮤직
        bgmController.value = bgm.volume;
        bgm.Stop();

        // 사운드 이펙트
        effectSource.volume = 1.0f;
        effectSource.Stop();
        hasEffectSourceStarted = false;
    }

    private void Update()
    {
        // 게임시작 버튼 누르면 실행
        if (isTimerRunning)
        {
            timerText.gameObject.SetActive(true);
            time += Time.deltaTime;
            timerText.text = ((int)time % 60).ToString("경과 시간: 00 초");
        }

        if (Time.timeScale == 0)
        {
            isTimerRunning = false;
            btnSetting.interactable = false;
        }
        else
        {
            isTimerRunning = true;
            btnSetting.interactable = true;
        }
    }

    // 설정화면
    public void OpenSetting()
    {
        if (isSettingOpen)
        {
            panel.gameObject.SetActive(true);
            Time.timeScale = 0;
            isSettingOpen = false;
        }
        else
        {
            panel.gameObject.SetActive(false);
            Time.timeScale = 1;
            isSettingOpen = true;
        }
    }

    // 메인화면으로 돌아가기
    public void GoMain()
    {
        panel.gameObject.SetActive(false);
        Time.timeScale = 1;

        PlayContentData dto = new PlayContentData();
        PlayContentCount countDTO = new PlayContentCount();

        dto.userId = server.baseData.id;
        dto.date = DateTime.Now.ToString("yyyy-MM-01");
        dto.score = Int32.Parse(GameObject.Find("Score").GetComponent<TMP_Text>().text);

        countDTO.userId = server.baseData.id;
        countDTO.date = DateTime.Now.ToString("yyyy-MM-01");

        string content = SceneManager.GetActiveScene().name;

        switch (content)
        {
            case "HangGlide":
                dto.contentNumber = 1;
                countDTO.contentNumber = 1;
                break;

            case "BalanceWalk_MainGame":
                dto.contentNumber = 2;
                countDTO.contentNumber = 2;
                break;

            case "Hooray_InGame":
                dto.contentNumber = 3;
                countDTO.contentNumber = 3;
                break;

            case "Jump":
                dto.contentNumber = 4;
                countDTO.contentNumber = 4;
                break;

            case "Jumping_test":
                dto.contentNumber = 5;
                countDTO.contentNumber = 5;
                break;
        }


        string urlScore = "https://port-0-api-7xwyjq992llj595n50.sel4.cloudtype.app/contents/updateScore";
        string urlCount = "https://port-0-api-7xwyjq992llj595n50.sel4.cloudtype.app/contents/updateCount";
        string json = JsonUtility.ToJson(dto);
        string jsonCount = JsonUtility.ToJson(countDTO);

        //StartCoroutine(server.SendJsonData(urlScore, json));
        //StartCoroutine(server.SendJsonData(urlCount, jsonCount));

        SceneManager.LoadScene("Main");
        isSettingOpen = true;
    }
    
    // 게임화면으로 돌아가기
    public void BackToGame()
    {
        panel.gameObject.SetActive(false);
        Time.timeScale = 1;
        isSettingOpen = true;
    }
    
    // 게임 다시하기
    public void PushRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    // 타이머
    public void StartTimer()
    {
        Time.timeScale = 1;
        bgm.Play();
        button.SetActive(false);
    }

    // 설정창에서 BGM 소리 조절
    public void BGMController(float value)
    {
        bgm.volume = value;
    }

    // 설정창에서 이펙트 소리 조절
    public void EffectController()
    {
        effectSource.volume = effectController.value;
    }

    public void EffectPlayer()
    {
        // 점프동작 있으면 충돌감지 함수에 박으면됨
        hasEffectSourceStarted = true;
        if (hasEffectSourceStarted)
        {
            effectSource.Play();
            hasEffectSourceStarted = false;
        }
        else effectSource.Stop();
    }
}