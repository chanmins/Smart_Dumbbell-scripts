using System.Collections;
using TMPro;
using UnityEngine;


// 장애물 충돌 시 UGUI와 연결시킨 과일의 개수를 감소시키는 내용.
// 코루틴을 이용해 무적 시간 1초를 부여함.
// 또한 바구니의 색상을 반전시켜 무적 시간임을 알림.

public class BalanceWalk_burket : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score;

    public Camera mainCamera;
    public GameObject burket_L;
    public GameObject burket_R;
    bool isUnBeatTime;

    Material burket_L_material;
    Material burket_R_material;

    // Setting
    public GameObject endPanel;

    public TextMeshProUGUI finalScore;

    void Awake()
    {
        isUnBeatTime = false;
        score = 0000;
        scoreText.text = "0000";
        burket_L_material = burket_L.GetComponent<MeshRenderer>().material;
        burket_R_material = burket_R.GetComponent<MeshRenderer>().material;

        StartCoroutine(AddPoint());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && !isUnBeatTime)
        {
            StartCoroutine(UnBeatTime());
        }

        // 여기 수정 .name X
        if(other.name == "EndPoint_Plane")
        {
            StartCoroutine(SlowDownTime());
        }
    }

    // 2초의 무적 시간을 부여함.
    IEnumerator UnBeatTime()
    {
        isUnBeatTime = true;
        score -= 200;
        scoreText.text = score.ToString();
        Color red = new Color(1, 0.5f, 0.5f);
        float BlinkTime = 0f;
        // 매번 GC Allocation 할당 방지
        var wfs = new WaitForSeconds(0.1f);

        // 장애물 피격 시 화면 흔들림을 만듬------------------------------------------------------
        mainCamera.transform.position = mainCamera.transform.position + new Vector3(0.05f, 0, 0);
        yield return wfs;
        mainCamera.transform.position = mainCamera.transform.position + new Vector3(-0.05f, 0, 0);
        yield return wfs;
        mainCamera.transform.position = mainCamera.transform.position + new Vector3(-0.05f, 0, 0);
        yield return wfs;
        mainCamera.transform.position = mainCamera.transform.position + new Vector3(0.05f, 0, 0);

        // 장애물 피격 시 양동이가 깜빡거리는 효과를 만듬-----------------------------------------
        while (BlinkTime < 2)
        {
            burket_L_material.color = red;
            burket_R_material.color = red;

            yield return wfs;
            burket_L_material.color = new Color(1, 1, 1);
            burket_R_material.color = new Color(1, 1, 1);
            yield return wfs;

            BlinkTime += 0.2f;
        }
        isUnBeatTime = false;
    }

    // 게임 종료
    IEnumerator SlowDownTime()
    {
        var wfs = new WaitForSeconds(0.3f);

        // 천천히 종료시킴
        Time.timeScale = Time.timeScale - 0.1f;
        yield return wfs;
        Time.timeScale = Time.timeScale - 0.4f;
        yield return wfs;

        // 점수 저장 및 종료 패널 생성 후 게임 종료
        finalScore.text = $"점수: {score:D4} 점";
        SaveScore();
        endPanel.SetActive(true);
        Time.timeScale = 0f;
        yield return wfs;
    }

    // 매 일정 시간마다 50점씩 점수 추가
    IEnumerator AddPoint()
    {
        while (Time.timeScale != 0)
        {
            yield return new WaitForSeconds(1.0f);
            score += 50;
            scoreText.text = score.ToString();
        }
    }

    // 점수 저장
    private void SaveScore()
    {
        PlayerPrefs.SetInt("CurrentScore", score);
        PlayerPrefs.Save();
    }
}
