using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Measurement_M : MonoBehaviour
{
    // 측정 타이머
    private float restTime;
    public GameObject timer;
    private float measureTime;
    private float measureTime2;

    // 측정 타이머 시간 표시
    public TextMeshProUGUI restTimerText;
    public TextMeshProUGUI measureTimerText;
    public TextMeshProUGUI measureTimerText2;

    // 측정 횟수
    public TextMeshProUGUI countText;
    private int count;

    // 측정 씬 Circular Progress Bar
    public Image fillRestTime;
    public Image fillMeasureTime;
    public Image fillMeasureTime2;

    // Progress Bar 최댓값
    private float restMax;
    private float measureMax;
    private float measureMax2;

    // 측정 씬 실행조건
    private bool flag;
    private bool flag2;
    private bool flag3;
    private bool flag4;
    private bool flag5;

    // 측정 버튼
    public GameObject measureLeft;
    public GameObject measureRight;

    // 악력 측정 패널
    public GameObject restAndMeasureGrip;

    // 덤벨 컬 측정 패널
/*    public GameObject restAndMeasureCurl;
    public RectTransform measureDumbbellCurl;*/

    public GameObject nextButton;

    // 좌측 카운트 다운
    public Button countDownLeft;
    public Image leftCounter;
    private float leftCountTime;
    private float leftMaxCountTime;
    public TextMeshProUGUI leftCountTimerText;
    public AudioSource leftSoundClip;
    private bool isPlayingLeft;

    // 우측 카운트 다운
    public Button countDownRight;
    public Image rightCounter;
    private float rightCountTime;
    private float rightMaxCountTime;
    public TextMeshProUGUI rightCountTimerText;
    public AudioSource rightSoundClip;
    private bool isPlayingRight;

    // 받아올 그래프
    public GameObject graphScript;

    //측정 1회 종료 시 데이터를 받아올 배열
    public float[] gripStrength = new float[6];

    GameObject serial;

    void Start()
    {
        serial = GameObject.Find("Serial");
        flag = false;
        flag2 = false;
        flag3 = false;
        flag4 = false;
        flag5 = false;
        isPlayingLeft = false;

        leftCountTime = 0;
        rightCountTime = 0;
        count = 0;        
        countText.text = "횟수: " + count.ToString() + " /3";

        measureTime = 0;
        measureMax = 15;

        measureTime2 = 0;
        measureMax2 = 15;
    }

    // CountDownLeft -> flag4 -> StopSoundLeft -> flag2 -> CountDownRight -> flag5 -> flag
    void Update()
    {
        // 휴식 타이머
        if (flag && !flag3)
        {
            countDownLeft.gameObject.SetActive(false);
            restTime -= Time.deltaTime;
            restTimerText.text = "" + (int)restTime;
            fillRestTime.fillAmount = restTime / restMax;

            if (restTime < 0)
            {
                // 타이머 값 초기화
                restTime = 0;
                restTimerText.text = "30";
                fillRestTime.fillAmount = 1;
                // 버튼 활성화
                countDownLeft.gameObject.SetActive(true);
                countDownLeft.interactable = true;
                measureRight.gameObject.SetActive(false);
                timer.SetActive(false);
                flag = false;
            }
        }

        // 좌측 악력 측정
        if (flag2)
        {
            measureTime += Time.deltaTime;
            measureTimerText.text = "" + (int)measureTime;
            fillMeasureTime.fillAmount = measureTime / measureMax;

            // 측정시간 15초가 넘어가면 측정 종료
            if (measureTime >= 15)
            {
                // 타이머 값 초기화
                measureTimerText.text = "시작";
                fillMeasureTime.fillAmount = 0;
                countDownRight.interactable = true;
                countDownRight.gameObject.SetActive(true);
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

            // 측정시간 15초가 넘어가면 측정 종료
            if (measureTime2 >= 15)
            {
                measureTimerText2.text = "시작";
                fillMeasureTime2.fillAmount = 0;
                countDownRight.interactable = true;
                measureRight.gameObject.SetActive(false);
                timer.SetActive(true);
                flag3 = false;
            }
        }

        // 좌측 악력 측정카운트 및 소리 출력
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

        // 우측 악력 측정카운트 및 소리 출력
        if (flag5)
        {
            rightCountTime -= Time.deltaTime;
            rightCountTimerText.text = "" + (int)rightCountTime;
            rightCounter.fillAmount = rightCountTime / rightMaxCountTime;

            if(rightCountTime <= 0)
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
        countDownLeft.gameObject.SetActive(false);
        flag2 = true;
        if (flag2) graphScript.GetComponent<GraphControll_Grip>().StartGripStrength();
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
        countDownRight.gameObject.SetActive(false);
        flag3 = true;
        if(flag3) graphScript.GetComponent<GraphControll_Grip>().StartGripStrength();
    }

    // 좌측 카운트 다운 누르면 3초 타이머 후 삐~ 소리 1초동안 나온 후에 자동으로 측정 시작.
    public void CountDownLeft()
    {
        countDownLeft.interactable = false;
        StartCoroutine(serial.GetComponent<Serial>().SetGripMode());
        // 타이머 값 설정
        leftCountTime = 3;
        leftMaxCountTime = 3;

        // 타이머 On
        flag4 = true;
    }

    // 우측 카운트 다운 누르면 3초 타이머 후 삐~ 소리 1초동안 나온 후에 자동으로 측정 시작.
    public void CountDownRight()
    {
        countDownRight.interactable = false;
        StartCoroutine(serial.GetComponent<Serial>().SetGripMode());
        // 횟수 증가
        count++;
        countText.text = "횟수: " + count.ToString() + " /3";
        // 타이머 값 설정
        rightCountTime = 3;
        rightMaxCountTime = 3;

        // 타이머 On
        flag5 = true;

        // 측정 횟수가 3회 미만이면 휴식 타이머 실행 
        if (count < 3)
            Invoke("Rest", 15.0f);
        
        // 측정 횟수가 3회면 덤벨컬로 이동 버튼 생성
        if (count == 3)
            Invoke("ShowNextButton", 19.0f);
    }

    // 휴식 시작
    private void Rest()
    {
        restTime = 5;
        restMax = 5;
        measureTimerText.text = "시작";
        flag = true;
    }

    // 덤벨 컬 패널로 이동
    public void GoToNextPanel()
    {
        restAndMeasureGrip.SetActive(false);
        SceneManager.LoadScene("Dumbellcurl_M");
    }

    // 덤벨 컬 -> 메인 화면으로 이동
    public void GoToMainPage()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SceneManager.LoadScene("Main_M");
    }
}