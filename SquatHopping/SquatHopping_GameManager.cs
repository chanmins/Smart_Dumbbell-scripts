using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SquatHopping_GameManager : MonoBehaviour
{
    public static SquatHopping_GameManager instance;
    public SquatHopping_SpawnerManager spawnerManager;
    public SquatHopping_CoinSpawner spawner;
    public SquatHopping_Coin coin;
    public TextMeshProUGUI score;
    public static bool isGameOver;
    public GameObject endPanel;
    public GameObject setting;

    private Setting settingScript;

    public TextMeshProUGUI finalScore;
    private int currentScore;

    private void Awake()
    {
        isGameOver = false;
        instance = this;
    }

    void Start()
    {
        settingScript = setting.GetComponent<Setting>();
        Time.timeScale = 1;
        score.text = "0000";
    }

    private void Update()
    {
        if (settingScript != null && settingScript.time != null &&  Time.timeScale == 1)
        {
            if (settingScript.time >= 60f)
            {
                endPanel.SetActive(true);
                Time.timeScale = 0;
                SaveScore();
            }
        }
    }

    public void GetScore(int scoreToAdd)
    {
        currentScore = int.Parse(score.text);

        int newScore = currentScore + scoreToAdd;
        string formattedScore = newScore.ToString("D4");

        score.text = formattedScore;
        finalScore.text = $"점수 :{currentScore:D4} 점";
        Debug.Log("현재 점수 : " + formattedScore);
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        PlayerPrefs.Save();
    }

}