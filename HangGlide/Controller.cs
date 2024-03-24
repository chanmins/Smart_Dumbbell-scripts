using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;
using SerialData;
using updataData;

public class HanglideController : MonoBehaviour
{
    public Animator isPlaying;
    public GameObject runner;
    public Slider slider;
    public GameObject endPanel;
    public TextMeshProUGUI counter;

    [SerializeField]
    private int score;
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI countdownText;
    private float timer;
    private bool flag;
    private Rigidbody rigid;
    public Serial Serial;
    public TextMeshProUGUI finalScore;

    void Start()
    {
        Serial = GameObject.Find("Serial").GetComponent<Serial>();
        // Mem, GC Allocation 할당 방지
        if (runner.TryGetComponent(out rigid))
        {
            flag = false;
            timer = 3f;
            Time.timeScale = 0;
            score = 0;

            StartCoroutine(Count());
        }
    }

    private void FixedUpdate()
    {

        if (Time.timeScale == 1)
        {
            counter.gameObject.SetActive(true);
            runner.transform.Translate(Vector3.forward * 0.26f);

            if (flag)
            {
                counter.gameObject.SetActive(false);
                // 덤벨 y값이 40이상인 경우 올라감
                if (contentData.y >= 40)
                {
                    isPlaying.SetBool("isUp", true);
                    isPlaying.SetBool("isDown", false);
                    runner.transform.Translate(Vector3.up * 0.08f);
                }
                // 덤벨 y값이 15이하인 경우 내려감
                if (contentData.y <= 15)
                {
                    isPlaying.SetBool("isDown", true);
                    isPlaying.SetBool("isUp", false);
                    runner.transform.Translate(Vector3.down * 0.08f);
                }
            }
        }
    }

    // 게임 시작하기 전 3초 대기 시간
    private IEnumerator Count()
    {
        float currentTime = timer;

        Serial.SerialSendinggyro();

        while (currentTime > 0)
        {
            countdownText.text = currentTime.ToString("0");
            yield return new WaitForSeconds(1.0f);
            currentTime--;
        }
        countdownText.text = "Start!";
        yield return new WaitForSeconds(1.0f);
        countdownText.gameObject.SetActive(false);

        flag = true;
    }

    void OnCollisionEnter(Collision col)
    {
        // 가비지 생성 방지를 위해 CompareTag 사용
        // Obstacle 장애물을 통과하면 100점을 추가함
        if (col.gameObject.CompareTag("Obstacle"))
        {
            score += 100;
            scoreText.text = score.ToString("0000");
        }
        // Final 게임 오브젝트를 통과하면 게임이 종료됨
        if (col.gameObject.CompareTag("Final"))
        {
            rigid.useGravity = true;
            rigid.isKinematic = false;
            flag = false;
            isPlaying.SetBool("isLanding", true);
            Invoke("FinishGame", 2f);
        }

        // 바닥, 바다에 닿을 경우 게임 종료
        if (col.gameObject.CompareTag("GameOver"))
        {
            FinishGame();
        }
    }

    private void FinishGame()
    {
        endPanel.SetActive(true);
        Time.timeScale = 0;

        finalScore.text = $"점수: {score:D4} 점";
        SaveScore();
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt("CurrentScore", score);
        PlayerPrefs.Save();
    }
}