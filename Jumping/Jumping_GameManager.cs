using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Jumping_GameManager : MonoBehaviour
{
    public Slider slider;
    public float value;
    public int score;
    public TextMeshProUGUI scoreText;
    public float speed;
    public int probability; // boob, coin 비율
    public int makeProbability; // boob, coin 생성 비율

    public GameObject setting;
    public GameObject endPanel;
    private Setting settingScript;

    public TextMeshProUGUI finalScore;

    public TextMeshProUGUI countdownText;
    private float timer;

    void Start()
    {
        timer = 3f;

        settingScript = setting.GetComponent<Setting>();

        Time.timeScale = 0;
        StartCoroutine(Count());
        speed = 0.25f;
        score = 0000;
        scoreText.text = "0000";
        value = 0;
        probability = 2; // max = 10 낮을수록 코인생성 높으면 폭탄생성
        makeProbability = 2; // max = 10 낮을수록 조금 생성
    }

    private void Update()
    {
        EndGame();
        scoreText.text = score.ToString("D4");
    }

    private void EndGame()
    {
        if (settingScript != null && settingScript.time != null && Time.timeScale == 1)
        {
            if (settingScript.time >= 60f)
            {
                endPanel.SetActive(true);
                Time.timeScale = 0;

                finalScore.text = $"점수: {score:D4} 점";
                SaveScore();
            }
        }
    }
    private void SaveScore()
    {
        PlayerPrefs.SetInt("CurrentScore", score);
        PlayerPrefs.Save();
    }

    private IEnumerator Count()
    {
        float currentTime = timer;
        var wfs = new WaitForSeconds(1.0f);
        while (currentTime > 0)
        {
            countdownText.text = currentTime.ToString("0");
            yield return wfs;
            countdownText.gameObject.SetActive(true);
            currentTime--;
        }
        countdownText.text = "Start!";
        yield return wfs;
        countdownText.gameObject.SetActive(false);
    }
}