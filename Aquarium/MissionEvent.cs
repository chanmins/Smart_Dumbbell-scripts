using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MissionEvent : MonoBehaviour
{
    public Button storeBtn; //상점 버튼
    public Button missionBtn; //미션 버튼
    public Button btn_Main;

    public GameObject missionPanel; //미션 메인 판넬
    public GameObject[] missionList_Lock; //미션 완료시 자물쇠 해제
    public GameObject[] missionList_UnLock;
    public GameObject[] missionList_btn; // 미션 완료시 버튼 비활성화

    public Slider[] missionSlider;

    public TextMeshProUGUI[] progressMission;
    public TextMeshProUGUI currentJew;

    public int[] missionList_point = new int[5]; //미션 완료시 포인트 획득

    private bool attendanceFlag; // 출석 플래그
    private int attendanceCount; //출석 횟수
    private bool mesu;
    private int total_point;

    void Start()
    {
        for (int i = 0; i < missionSlider.Length; i++)
        {
            missionSlider[i].interactable = false;
        }
        missionPanel.SetActive(false);
        currentJew.text = "1000"; // 보석 개수
    }

    // 상점 버튼 클릭시 메인 판넬 활성화
    public void MissionBtnClick()
    {
        btn_Main.interactable = false;
        storeBtn.interactable = false;
        missionBtn.interactable = false;
        missionPanel.SetActive(true);
    }

    // X 버튼 클릭시 판넬 닫기
    public void CloseBtnClick()
    {
        btn_Main.interactable = true;
        storeBtn.interactable = true;
        missionBtn.interactable = true;
        missionPanel.SetActive(false);
    }

    //미션 완료시 미션 바 변경
    public void missionCompleted(int aryNum)
    {
        missionList_Lock[aryNum].SetActive(false);
        missionList_UnLock[aryNum].SetActive(true);
        missionList_btn[aryNum].SetActive(false);
        total_point += missionList_point[aryNum];

        if (aryNum < missionSlider.Length)
        {
            // 누를때마다 미션바 1씩 올라가게 변경
            float increment = 1f / 20f; 
            missionSlider[aryNum].value += increment;
            
            if (aryNum < progressMission.Length)
            {
                int currentValue = Mathf.Clamp(Mathf.RoundToInt(missionSlider[aryNum].value * 10), 1, 20);
                progressMission[aryNum].text = "<color=#72C8FA>" + currentValue.ToString() + "</color><color=#D2D2D2>/20</color>";
            }
        }

        currentJew.text = (int.Parse(currentJew.text) + 200).ToString();
    }

    // 출석일 체크
    public void attendance()
    {
        if (!attendanceFlag)
        {
            switch (attendanceCount)
            {
                case 5:
                    break;
                case 10:
                    break;
                case 15:
                    break;
                case 20:
                    attendanceFlag = true;
                    break;
            }
        }
    }

    public void GoMain()
    {
        SceneManager.LoadScene("Main");
    }
}
