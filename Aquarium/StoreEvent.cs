using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreEvent : MonoBehaviour
{
    public Button storeBtn; //상점 버튼
    public Button missionBtn; //미션 버튼
    public Button btn_Main;

    public GameObject mainPanel; //상점 메인 판넬
    public GameObject rockPanel; //돌 판넬
    public GameObject clamPanel; //조개 판넬
    public GameObject coralPanel; //산호 판넬
    public GameObject kelpPanel; //켈프 판넬
    public GameObject grassPanel; //수초 판넬
    public GameObject starfishPanel; //불가사리 판넬
    public GameObject confirmPanel; //확인 판넬
    public GameObject noRockBuyPanel;//돌 판넬
    public GameObject noMoneyPanel; // 돈 없어 판넬

    public GameObject GrassGroup1; //수초1
    public GameObject GrassGroup2; //수초2

    private Vector3 OrignalScale; //이벤트 트리거 기존 스케일

    public int totalCoin; //현재 코인

    public TextMeshProUGUI[] Cointext = new TextMeshProUGUI[6];

    public GameObject[] ObjectAry = new GameObject[40]; //아쿠아리움 오브젝트
    public GameObject[] UIAry = new GameObject[40]; // 오브젝트 구매 말풍선
    private bool[] BoolAry = new bool[40]; //db에 들어갈 불린 배열
    public int[] CoinAry = new int[40]; //구매 가격
    public int aryNum;

    public void Awake()
    {
        totalCoin = 9999;
    }

    public void Start()
    {
        TextChange();
        storeBtn.gameObject.SetActive(true);
        mainPanel.SetActive(false);
        rockPanel.SetActive(false);
        clamPanel.SetActive(false);
        coralPanel.SetActive(false);
        kelpPanel.SetActive(false);
        grassPanel.SetActive(false);
        starfishPanel.SetActive(false);
        confirmPanel.SetActive(false);
        GrassGroup1.SetActive(true);
        GrassGroup2.SetActive(false);
        noRockBuyPanel.SetActive(false);
        noMoneyPanel.SetActive(false);
    }

    public void TextChange()
    {
        for (int i = 0; i < 6; i++)
        {
            Cointext[i].text = totalCoin.ToString();
        }
    }

    // 상점 버튼 클릭시 메인 판넬 활성화
    public void StoreBtnClick()
    {
        btn_Main.interactable = false;
        storeBtn.interactable = false;
        missionBtn.interactable = false;
        mainPanel.SetActive(true);
    }

    //해당 상점 판넬로 이동
    public void StoreBtnMove(GameObject panel)
    {
        panel.SetActive(true);
    }

    // X 버튼 클릭시 판넬 닫기
    public void CloseBtnClick()
    {
        btn_Main.interactable = true;
        storeBtn.interactable = true;
        missionBtn.interactable = true;
        mainPanel.SetActive(false);
        rockPanel.SetActive(false);
        clamPanel.SetActive(false);
        coralPanel.SetActive(false);
        kelpPanel.SetActive(false);
        grassPanel.SetActive(false);
        starfishPanel.SetActive(false);
        confirmPanel.SetActive(false);
    }

    // 이전 버튼 클릭시 판넬 닫기
    public void BackBtnClick(GameObject panel)
    {
        panel.SetActive(false);
    }

    // 상품을 구매했다는 걸 보여주는 말풍선
    public void BuyObjectBtnClick(int Num)
    {
        aryNum = Num;
        confirmPanel.SetActive(true);
    }

    // 확인패널 Yes 눌렀을때 현재 잔액을 수정
    public void ConfirmPanelYes()
    {
        if (CoinAry[aryNum] < totalCoin)
        {
            ObjectAry[aryNum].SetActive(true);
            UIAry[aryNum].SetActive(false);
            confirmPanel.SetActive(false);
            totalCoin -= CoinAry[aryNum];
            TextChange();
        }
        else
        {
            noMoneyPanel.SetActive(true);
            confirmPanel.SetActive(false);
        }
    }

    // 확인판넬 No 눌렀을때
    public void ConfirmPanelNo()
    {
        confirmPanel.SetActive(false);
    }

    // 돌 안사고 돌에 붙어있는 물품 사려고 할때 못산다는 것을 보여줌
    public void RockBuy(int Num)
    {
        if (ObjectAry[0].activeSelf && ObjectAry[1].activeSelf && ObjectAry[2].activeSelf)
        {
            BuyObjectBtnClick(Num);
        }
        else
        {
            noRockBuyPanel.SetActive(true);
        }
    }

    // 구매 가격(말풍선)에 커서 가져다 놨을 시 scale 커짐
    public void onPointerEnter(GameObject obj)
    {
        OrignalScale = obj.transform.localScale;
        obj.transform.localScale = new Vector3(OrignalScale.x * 1.2f, OrignalScale.y * 1.2f, 1f);
    }

    // 구매 가격(말풍선)에 커서가 해제됐을 시 원상 복구
    public void onPointerExit(GameObject obj)
    {
        obj.transform.localScale = new Vector3(OrignalScale.x, transform.localScale.y, 1f);
    }

    public void OnDisable()
    {
        for (int i = 0; i < 40; i++)
        {
            BoolAry[i] = ObjectAry[i].activeSelf;
        }
    }
}