using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPage : MonoBehaviour
{
    public Color[] colors;
    public GameObject scrollbar, imageContent;
    public GameObject mainPanel;
    public GameObject miniGame1_panel;
    public GameObject miniGame2_panel;
    public GameObject miniGame3_panel;

    private float scroll_pos = 0;
    private float[] pos;
    private float distance;
    private float time;
    private Button takeTheBtn;
    private int btnNumber;
    private bool runIt = false;

    void Update()
    {
        pos = new float[transform.childCount];      // 스크롤 뷰 내에 있는 버튼 수 위치 저장에 사용
        distance = 1f / (pos.Length - 1f);    // 버튼 간의 거리 계산 변수

        if (runIt)
        {
            GecisiDuzenle(distance, pos, takeTheBtn);   // 버튼 위치 및 크기 조절
            time += Time.deltaTime;

            if (time > 1f)
            {
                time = 0;
                runIt = false;
            }
        }

        // pos 배열 초기화 및 거리 계산
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;  //  distance -> 이미지 간 거리
        }

        // 현재 스크롤 위치 계산
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value; // 마우스 클릭 이벤트가 발생하면 스크롤바 값(위치) 변경
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance * 0.5) && scroll_pos > pos[i] - (distance * 0.5))   // pos 배열의 각 요소와 scroll_pos 값 비교
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);    // 범위 안에 있으면 스크롤바 위치를 해당 이미지 위치로 이동
                }
            }
        }

        // 선택된 페이지 강조시켜줌
        for (int i = 0; i < pos.Length; i++)
        {
            // 현재 스크롤 위치가 선택된 레벨 위치에서 절반만큼 넘어왔다면 실행
            if (scroll_pos < pos[i] + (distance * 0.5) && scroll_pos > pos[i] - (distance * 0.5))
            {
                // 현재 선택된 페이지면 이미지 수정 및 색 변경
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(0.9f, 0.9f), 1f);
                imageContent.transform.GetChild(i).localScale = Vector2.Lerp(imageContent.transform.GetChild(i).localScale, new Vector2(1.0f, 1.5f), 0.1f);
                imageContent.transform.GetChild(i).GetComponent<Image>().color = colors[1];
                // 모든 페이지에 대해 다시 반복함
                for (int j = 0; j < pos.Length; j++)
                {
                    if (j != i)
                    {
                        // 현재 선택된 페이지가 아니면 이미지 축소 및 색 변경
                        imageContent.transform.GetChild(j).GetComponent<Image>().color = colors[0];
                        imageContent.transform.GetChild(j).localScale = Vector2.Lerp(imageContent.transform.GetChild(j).localScale, new Vector2(1.0f, 1.5f), 0.1f);
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }

    private void GecisiDuzenle(float distance, float[] pos, Button btn)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance * 0.5) && scroll_pos > pos[i] - (distance * 0.5))
            {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[btnNumber], 1f * Time.deltaTime);

            }
        }

        for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
        {
            btn.transform.name = ".";
        }
    }

    public void WhichBtnClicked(Button btn)
    {
        btn.transform.name = "clicked";
        for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
        {
            if (btn.transform.parent.transform.GetChild(i).transform.name == "clicked")
            {
                btnNumber = i;
                takeTheBtn = btn;
                time = 0;
                scroll_pos = (pos[btnNumber]);
                runIt = true;
            }
        }  
    }

    // 팔 운동 화면으로 이동
    public void WrestWorkOut()
    {
          miniGame1_panel.SetActive(true);
          mainPanel.SetActive(false);
    }

    // 가슴 운동 화면으로 이동
    public void ChestWorkOut()
    {
        miniGame2_panel.SetActive(true);
        mainPanel.SetActive(false);
    }

    // 어깨 운동 화면으로 이동
    public void ShoulderWorkOut()
    {
        miniGame3_panel.SetActive(true);
        mainPanel.SetActive(false);
    }    

    // 팔 운동 미니 게임 1
    public void GoToHooray()
    {
        SceneManager.LoadScene("Hooray_InGame");
    }

    // 팔 운동 미니게임 2
    public void GoToBalanceWalk()
    {
        SceneManager.LoadScene("BalanceWalk_MainGame");
    }

    public void GoToGlider()
    {
        SceneManager.LoadScene("HangGlide");
    }

    public void GoToJump()
    {
        SceneManager.LoadScene("Jump");
    }

    public void GoToJumping()
    {
        SceneManager.LoadScene("Jumping_test");
    }

    // 추천시스템 화면으로 이동
    public void Recommendation()
    {
        SceneManager.LoadScene("Recommend");
    }

    // 아쿠아리움
    public void Aquarium()
    {
        SceneManager.LoadScene("Aquarium");
    }

    // 사용자의 결과를 종합한 그래프 화면으로 이동
    public void ShowResults()
    {
        SceneManager.LoadScene("UserGraph");
    }

    // 측정 씬으로 이동
    public void Measurement()
    {
        SceneManager.LoadScene("Handgrip_M");
    }

    // 미니게임 패널에서 메인화면으로 이동
    public void GoMain()
    {
        mainPanel.SetActive(true);
        miniGame1_panel.SetActive(false);
        miniGame2_panel.SetActive(false);
        miniGame3_panel.SetActive(false);
    }
}