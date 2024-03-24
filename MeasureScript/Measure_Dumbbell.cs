using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Measure_Dumbbell : MonoBehaviour
{
    // 측정 타이머
    private float measureTime;
    private float measureTime2;

    // 측정 타이머 시간 표시
    public TextMeshProUGUI measureTimerText;
    public TextMeshProUGUI measureTimerText2;

    // 측정 횟수
    public TextMeshProUGUI countText;
    private int count;

    // 측정 씬 Circular Progress Bar
    public Image fillMeasureTime;
    public Image fillMeasureTime2;

    // Progress Bar 최댓값
    private float measureMax;
    private float measureMax2;

    // 측정 씬 실행조건
    private bool flag2;
    private bool flag3;
    private bool flag4;
    private bool flag5;

    // 측정 버튼
    public GameObject measureLeft;
    public GameObject measureRight;

    // 덤벨 컬 측정 패널
    public GameObject restAndMeasureCurl;

    public GameObject nextButton;

    // 좌측 카운트 다운
    public Button startMeasureLeft;
    public Image leftCounter;
    private float leftCountTime;
    private float leftMaxCountTime;
    public TextMeshProUGUI leftCountTimerText;
    public AudioSource leftSoundClip;
    private bool isPlayingLeft;

    // 우측 카운트 다운
    public Button startMeasureRight;
    public Image rightCounter;
    private float rightCountTime;
    private float rightMaxCountTime;
    public TextMeshProUGUI rightCountTimerText;
    public AudioSource rightSoundClip;
    private bool isPlayingRight;

    // 받아올 그래프
    public GameObject graphScript;

    //측정 1회 종료 시 데이터를 받아올 배열
    public int[] gripStrength = new int[2];

    public GameObject serial;

    void Start()
    {
        serial = GameObject.Find("Serial");
        flag2 = false;
        flag3 = false;
        flag4 = false;
        flag5 = false;
        isPlayingLeft = false;

        leftCountTime = 0;
        rightCountTime = 0;
        count = 0;
        countText.text = "횟수: " + count.ToString() + " /1";

        measureTime = 0;
        measureMax = 30;

        measureTime2 = 0;
        measureMax2 = 30;

    }


    void Update()
    {
        // 좌측 악력 측정
        if (flag2)
        {
            measureTime += Time.deltaTime;
            measureTimerText.text = "" + (int)measureTime;
            fillMeasureTime.fillAmount = measureTime / measureMax;

            // 측정시간 30초가 넘어가면 측정 종료
            if (measureTime >= 30)
            {
                // 타이머 값 초기화
                measureTimerText.text = "시작";
                fillMeasureTime.fillAmount = 0;
                startMeasureRight.interactable = true;
                startMeasureRight.gameObject.SetActive(true);
                measureLeft.gameObject.SetActive(false);
                flag2 = false;
            }
        }

        // 우측 악력 측정
        if (flag3)
        {
            measureTime2 += Time.deltaTime;
            measureTimerText2.text = "" + (int)measureTime2;
            fillMeasureTime2.fillAmount = measureTime2 / measureMax2;

            // 측정시간 30초가 넘어가면 측정 종료
            if (measureTime2 >= 30)
            {
                measureTimerText2.text = "시작";
                fillMeasureTime2.fillAmount = 0;
                startMeasureLeft.gameObject.SetActive(true);
                measureRight.gameObject.SetActive(false);
                flag3 = false;
            }
        }

        // 좌측 덤벨 측정카운트 및 소리 출력
        if (flag4)
        {
            leftCountTime -= Time.deltaTime;
            leftCountTimerText.text = "" + (int)leftCountTime;
            leftCounter.fillAmount = leftCountTime / leftMaxCountTime;

            if (leftCountTime <= 0)
            {
                leftCountTimerText.text = "시작!";
                isPlayingLeft = true;
                // 사운드 출력
                if (isPlayingLeft)
                {
                    leftSoundClip.gameObject.SetActive(true);
                    Invoke("StopSoundLeft", 1.0f);
                }
                flag4 = false;
            }
        }

        // 우측 덤벨 측정카운트 및 소리 출력
        if (flag5)
        {
            rightCountTime -= Time.deltaTime;
            rightCountTimerText.text = "" + (int)rightCountTime;
            rightCounter.fillAmount = rightCountTime / rightMaxCountTime;

            if (rightCountTime <= 0)
            {
                rightCountTimerText.text = "시작!";
                isPlayingRight = true;
                if (isPlayingRight)
                {
                    rightSoundClip.gameObject.SetActive(true);
                    Invoke("StopSoundRight", 1.0f);
                }
                flag5 = false;
            }
        }
    }

    public void CountDownLeft()//첫 덤벨컬 측정.
    {
        startMeasureRight.interactable = false;
        startMeasureLeft.interactable = false;
        // 타이머 값 설정
        leftCountTime = 3;
        leftMaxCountTime = 3;

        StartCoroutine(serial.GetComponent<Serial>().SetGyroMode());

        // 타이머 On
        flag4 = true;

        // 횟수 증가
        count++;
        countText.text = "횟수: " + count.ToString() + " /2";
    }

    public void CountDownRight()
    {
        startMeasureRight.interactable = false;
        startMeasureLeft.interactable = false;

        // 타이머 값 설정
        rightCountTime = 3;
        rightMaxCountTime = 3;

        StartCoroutine(serial.GetComponent<Serial>().SetGyroMode());

        count++;
        countText.text = "횟수: " + count.ToString() + " /2";

        // 타이머 On
        flag5 = true;

        // 측정 횟수가 3회면 덤벨컬로 이동 버튼 생성
        if (count == 2)
            Invoke("ShowNextButton", 34.0f);
    }

    // 좌측 사운드 1초 재생
    private void StopSoundLeft()
    {
        // 소리 끔
        isPlayingLeft = false;
        leftSoundClip.Stop();
        leftSoundClip.gameObject.SetActive(false);

        // 측정 타이머 초기화
        measureTime = 0;
        measureLeft.gameObject.SetActive(true);
        leftCountTimerText.text = "시작";
        leftCounter.fillAmount = 1;
        startMeasureLeft.gameObject.SetActive(false);
        startMeasureLeft.interactable = true;
        flag2 = true;

        if (flag2)
            graphScript.GetComponent<GraphControll_curl>().StartCurlCount();
    }

    // 우측 사운드 1초 재생
    private void StopSoundRight()
    {
        // 소리 끔
        isPlayingRight = false;
        rightSoundClip.Stop();
        rightSoundClip.gameObject.SetActive(false);

        // 측정 타이머 초기화
        measureTime2 = 0;
        measureRight.gameObject.SetActive(true);
        rightCountTimerText.text = "시작";
        rightCounter.fillAmount = 1;
        startMeasureRight.gameObject.SetActive(false);
        flag3 = true;

        if (flag3)
            graphScript.GetComponent<GraphControll_curl>().StartCurlCount();
    }

    // 덤벨컬 이동 버튼
    private void ShowNextButton()
    {
        nextButton.SetActive(true);
    }

    // 덤벨 컬 -> 메인 화면으로 이동
    public void GoToMainPage()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SceneManager.LoadScene("Main");
    }
}
