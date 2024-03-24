using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelChanger : MonoBehaviour
{
    public GameObject userGraphPanel;
    public GameObject userScorePanel;

    public GameObject avgScoreGraph;
    public GameObject gripGraph;
    public GameObject curlGraph;

    private void Start()
    {
        userGraphPanel.SetActive(true);
        userScorePanel.SetActive(false);
    }
    public void TurnOnScore()
    {
        userGraphPanel.SetActive(false);
        userScorePanel.SetActive(true);
    }

    public void TurnOnGraph()
    {
        userScorePanel.SetActive(false);
        userGraphPanel.SetActive(true);
    }

    public void TurnOnAvgScoreGraph()
    {
        avgScoreGraph.SetActive(true);
        gripGraph.SetActive(false);
        curlGraph.SetActive(false);
    }

    public void TurnOnGripGraph()
    {
        avgScoreGraph.SetActive(false);
        gripGraph.SetActive(true);
        curlGraph.SetActive(false);
    }
    public void TurnOnCurlGraph()
    {
        avgScoreGraph.SetActive(false);
        gripGraph.SetActive(false);
        curlGraph.SetActive(true);
    }

    public void GoMain()
    {
        SceneManager.LoadScene("Main");
    }
}
