using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Data;
using System;

public class Login_PanelChange : MonoBehaviour
{
    // 배경화면
    public Image background;

    private float delay;
    public GameObject load_Panel;
    public GameObject characterX;

    // 로그인 페이지 패널
    public GameObject panel_SignIn;
    public GameObject[] panel_SignUp;
    
    public GameObject survey_Start;
    public GameObject survey_Panel;
    public GameObject survey_End;

    public RectTransform SI;
    public RectTransform[] SU;

    // 설문지 패널
    public GameObject[] surveyPanels;

    // 설문지 도움말
    public GameObject[] helpers;

    // 설문지 버튼
    public Button btn_Next;
    public Button[] btn_Cancel;
    
    // 설문지 1~5번 화면에 3개씩 위치한 설문 버튼 
    public Button[] button_1;
    public Button[] button_2;
    public Button[] button_3;
    public Button[] button_4;
    public Button[] button_5;

    // 설문지 선택란 3개
    private Dictionary<string, int> selectedChoices = new Dictionary<string, int>();

    // 설문지 패널 1~6
    public RectTransform[] surveyPanelMovement;

    // 아이디 비밀번호 찾기
    public GameObject panel_ID;
    public GameObject panel_PW;
    public GameObject showID;
    public GameObject showPW;

    // ImageChanger
    public Image[] imageBars;
    public Color fillIn;
    public Color fillOut;

    // Server object
    public WebRequest Server;
    public UserInfo userins;

    //sarc 점수
    public int sarcScore;

    //Tab 배열
    public TMP_InputField[] sign;

    // Wrong password Effect & Finder
    public TextMeshProUGUI wrongPassword;
    private float fadeDuration;
    public CanvasGroup cg;

    // Connect Disease and Checkbox
    public Dictionary<string, Toggle> diseaseToCheckBox;

    // Save diseases
    public List<string> selectedDiseases;
    public Text resultText;

    void Start()
    {
        delay = 2f;
        Screen.orientation = ScreenOrientation.Portrait;

        Server = GameObject.Find("Server").GetComponent<WebRequest>();
        //로그인 객체
        userins = new UserInfo();

        wrongPassword.gameObject.SetActive(true);
        cg.alpha = 0f;
        fadeDuration = 1.5f;

        selectedDiseases = new List<string>();
        diseaseToCheckBox = new Dictionary<string, Toggle>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Shift키 눌림 여부
            bool shiftKey = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            // 현재 필드값
            TMP_InputField currentField = null;

            // foreach를 이용하여 isFocused로 설정된 입력필드 찾음
            foreach (TMP_InputField field in sign)
            {
                if (field.isFocused)
                {
                    currentField = field;
                    break;
                }
            }

            if (currentField != null)
            {
                // 현재 필드값 위치 확인
                int currentIndex = System.Array.IndexOf(sign, currentField);

                // shift를 누르면 currentIndext에서 1을 빼고, 그렇지 않으면 1을 더함 
                int nextIndex = currentIndex + (shiftKey ? -1 : 1);

                //mathf.clamp를 이용하여 배열의 범위 내에 있는지 확인
                nextIndex = Mathf.Clamp(nextIndex, 0, sign.Length - 1);

                // 다음 필드로 이동시킴
                TMP_InputField nextField = sign[nextIndex];
                nextField.Select();
                nextField.ActivateInputField();
            }
        }
    }


    public void OnCheckboxValueChanged(string disease)
    {
        // Check if the key exists in the dictionary
        if (diseaseToCheckBox.ContainsKey(disease))
        {
            // The key exists, so proceed with your logic
            if (diseaseToCheckBox[disease].isOn)
            {
                selectedDiseases.Add(disease);
            }
            else
            {
                selectedDiseases.Remove(disease);
            }

            // Update the result text
            UpdateResultText();
        }
        else
        {
            // The key does not exist in the dictionary, handle this case accordingly
            Debug.LogError("Key not found in the diseaseToCheckBox dictionary: " + disease);
        }
    }

    private void UpdateResultText()
    {
        resultText.text = "선택한 병명: " + string.Join(", ", selectedDiseases.ToArray());
    }

    // 로그인 -> 회원가입 페이지 이동
    public void SignUp()
    {
        panel_SignUp[0].SetActive(true);
        SI.DOAnchorPos(new Vector2(0, 0), 0.3f);
        SU[0].DOAnchorPos(new Vector2(30, 280), 0.3f);
        SI.DOAnchorPos(new Vector2(1000, 250), 0.3f);
        panel_SignIn.SetActive(false);
    }

    // 회원가입 1 -> 2 이동
    public void SignUp_Next_1()
    {
        string name = GameObject.Find("NAME_SU").GetComponent<TMP_InputField>().text.ToString();
        string id = GameObject.Find("ID_SU").GetComponent<TMP_InputField>().text.ToString();
        string password = GameObject.Find("PASSWORD_SU").GetComponent<TMP_InputField>().text.ToString();
        string correct = GameObject.Find("CONFIRM PASSWORD_SU").GetComponent<TMP_InputField>().text.ToString();

        if (!password.Equals(correct))
        {
            FadeIn();
            Invoke("FadeOut", 1f);
            return;
        }

        userins.name = name;
        userins.id = id;
        userins.password = password;

        panel_SignUp[1].SetActive(true);
        SU[0].DOAnchorPos(new Vector2(0, 0), 0.3f);
        SU[1].DOAnchorPos(new Vector2(30, 280), 0.3f);
        SU[0].DOAnchorPos(new Vector2(-1000, 280), 0.3f);
        panel_SignUp[0].SetActive(false);
    }

    private void FadeIn()
    {
        cg.DOFade(1f, fadeDuration);
    }

    private void FadeOut()
    {
        cg.DOFade(0f, fadeDuration);
    }

    public void SignUp_Next_2()
    {
        string weight = GameObject.Find("Weight_SU").GetComponent<TMP_InputField>().text.ToString();
        string height = GameObject.Find("Height_SU").GetComponent<TMP_InputField>().text.ToString();
        string birth = GameObject.Find("Birth_SU").GetComponent<TMP_InputField>().text.ToString();
        string gender = GameObject.Find("Gender_SU").GetComponent<TMP_Dropdown>().name.ToString();

        userins.weight = weight;
        userins.height = height;
        userins.birth = birth;
        userins.gender = gender;

        panel_SignUp[2].SetActive(true);
        SU[1].DOAnchorPos(new Vector2(0, 0), 0.3f);
        SU[2].DOAnchorPos(new Vector2(30, 280), 0.3f);
        SU[1].DOAnchorPos(new Vector2(-1000, 280), 0.3f);
        panel_SignUp[1].SetActive(false);
    }

    // 회원가입 1 -> 로그인
    public void SignUpBack()
    {
        panel_SignIn.SetActive(true);
        SI.DOAnchorPos(new Vector2(50, 250), 0.3f);
        SU[0].DOAnchorPos(new Vector2(30, 280), 0.3f);
        panel_SignUp[0].SetActive(false);
    }

    // 회원가입 2 -> 회원가입 1
    public void SignUpBack_1()
    {
        panel_SignUp[0].SetActive(true);
        SU[1].DOAnchorPos(new Vector2(1000, 280), 0.3f);
        SU[0].DOAnchorPos(new Vector2(30, 280), 0.3f); 
        panel_SignUp[1].SetActive(false);
    }

    // 회원가입 3 -> 회원가입 2
    public void SignUpBack_2()
    {
        panel_SignUp[1].SetActive(true);
        SU[2].DOAnchorPos(new Vector2(1000, 280), 0.3f);
        SU[1].DOAnchorPos(new Vector2(30, 280), 0.3f);
        panel_SignUp[2].SetActive(false);
    }

    // 회원가입 -> 로그인 페이지 이동
    public void SignUp_Fin()
    {
        string arm = GameObject.Find("ArmLength").GetComponent<TMP_InputField>().text.ToString();
        string forget = GameObject.Find("Answer").GetComponent<TMP_InputField>().text.ToString();

        userins.arm = arm;
        userins.forgetText = forget;

        string url = "https://port-0-api-7xwyjq992llj595n50.sel4.cloudtype.app/Register/SignUp";
        string json = JsonUtility.ToJson(userins);

        StartCoroutine(SignUpCoroutine(url, json));
    }

    private IEnumerator SignUpCoroutine(string url, string json)
    {
        StartCoroutine(Server.SendJsonData(url, json));

        yield return new WaitForSeconds(2f); // 1초 딜레이

        if (Server.response.Equals("SignUp Complete"))
        {

            panel_SignIn.SetActive(true);
            SU[0].DOAnchorPos(new Vector2(1000, 280), 0.3f);
            SU[1].DOAnchorPos(new Vector2(1000, 280), 0.3f);
            SU[2].DOAnchorPos(new Vector2(1000, 280), 0.3f);
            SI.DOAnchorPos(new Vector2(40, 250), 0.3f);
            panel_SignUp[2].SetActive(false);
        }
        else
        {
            Debug.Log("회원가입 불가");
        }

    }


    // 비밀번호 찾기 -> 로그인 페이지
    public void FoundPassword()
    {
        panel_SignIn.SetActive(true);
        showPW.SetActive(false);
    }

    // 로그인 -> 설문 동의 페이지 이동
    public void SignIn()
    {
        string id = GameObject.Find("ID_SI").GetComponent<TMP_InputField>().text.ToString();
        string password = GameObject.Find("PW_SI").GetComponent<TMP_InputField>().text.ToString();

        Login lg = new Login();

        userins.id = id;
        lg.id = id;
        lg.password = password;

        string url = "https://port-0-api-7xwyjq992llj595n50.sel4.cloudtype.app/Register/SignIn";
        string json = JsonUtility.ToJson(lg);

        StartCoroutine(SignInCoroutine(url, json, id));
    }

    private IEnumerator SignInCoroutine(string url, string json, string id)
    {
        StartCoroutine(Server.SendJsonData(url, json));

        yield return new WaitForSeconds(1f); // 1초 딜레이

        if (Server.response.Equals("Success"))
        {
            Server.baseData.id = id;
            background.gameObject.SetActive(false);

            CreateMeasureRow dto = new CreateMeasureRow();
            dto.id = id;
            dto.date = DateTime.Now.ToString("yyyy-MM-dd");


            string curl = "https://port-0-api-7xwyjq992llj595n50.sel4.cloudtype.app/Measure/CreateRow";
            string cjosn = JsonUtility.ToJson(dto);

            print(cjosn);

            StartCoroutine(Server.SendJsonData(curl, cjosn));
            survey_Start.SetActive(true);
            panel_SignIn.SetActive(false);
        }

    }

    // 설문 동의 -> 설문 1 이동
    public void SurveyStart()
    {
        survey_Panel.SetActive(true);
        surveyPanels[0].SetActive(true);
        survey_Start.SetActive(false);
    }

    // 설문 1 -> 2
    public void Survey1()
    {//00FF09
        SaveSelectedValues("Survey1");
        ShowSavedValues("Survey1");

        imageBars[0].color = fillIn;
        surveyPanels[1].SetActive(true);
        surveyPanelMovement[0].DOAnchorPos(new Vector2(-1476, -1376), 0.3f);  // 얘가 빠지고
        surveyPanelMovement[1].DOAnchorPos(new Vector2(0, -1376), 0.3f);   // 얘가 들어감
        surveyPanels[0].SetActive(false);
    }

    // 설문 2 -> 3
    public void Survey2()
    {
        SaveSelectedValues("Survey2");
        ShowSavedValues("Survey2");

        imageBars[1].color = fillIn;
        surveyPanels[2].SetActive(true);
        surveyPanelMovement[1].DOAnchorPos(new Vector2(-1476, -1376), 0.3f);  // 얘가 빠지고
        surveyPanelMovement[2].DOAnchorPos(new Vector2(0, -1376), 0.3f);   // 얘가 들어감
        surveyPanels[1].SetActive(false);
    }

    // 설문 3 -> 4
    public void Survey3()
    {
        SaveSelectedValues("Survey3");
        ShowSavedValues("Survey3");

        imageBars[2].color = fillIn;
        surveyPanels[3].SetActive(true);
        surveyPanelMovement[2].DOAnchorPos(new Vector2(-1476, -1376), 0.3f);  // 얘가 빠지고
        surveyPanelMovement[3].DOAnchorPos(new Vector2(0, -1376), 0.3f);   // 얘가 들어감
        surveyPanels[2].SetActive(false);
    }

    // 설문 4 -> 5
    public void Survey4()
    {
        SaveSelectedValues("Survey4");
        ShowSavedValues("Survey4");

        imageBars[3].color = fillIn;
        surveyPanels[4].SetActive(true);
        surveyPanelMovement[3].DOAnchorPos(new Vector2(-1476, -1376), 0.3f);  // 얘가 빠지고
        surveyPanelMovement[4].DOAnchorPos(new Vector2(0, -1376), 0.3f);   // 얘가 들어감
        surveyPanels[3].SetActive(false);
    }

    // 설문 5 -> 6
    public void Survey5()
    {
        SaveSelectedValues("Survey5");
        ShowSavedValues("Survey5");

        imageBars[4].color = fillIn;
        surveyPanels[5].SetActive(true);
        surveyPanelMovement[4].DOAnchorPos(new Vector2(-1476, -1376), 0.3f);  // 얘가 빠지고
        surveyPanelMovement[5].DOAnchorPos(new Vector2(0, -1376), 0.3f);   // 얘가 들어감
        surveyPanels[4].SetActive(false);
    }

    // 설문 6 -> 종료 패널
    public void Survey6()
    {
        survey_End.SetActive(true);

        // 병명과 체크박스를 딕셔너리에 추가
        diseaseToCheckBox.Add("골다공증", GameObject.Find("Toggle_1   ").GetComponent<Toggle>());
        diseaseToCheckBox.Add("당뇨병", GameObject.Find("Toggle_2").GetComponent<Toggle>());
        diseaseToCheckBox.Add("비만", GameObject.Find("Toggle_3").GetComponent<Toggle>());
        diseaseToCheckBox.Add("관절염", GameObject.Find("Toggle_4").GetComponent<Toggle>());
        diseaseToCheckBox.Add("심혈관 질환 및 고혈압", GameObject.Find("Toggle_5").GetComponent<Toggle>());

        survey_Panel.gameObject.SetActive(false);
        surveyPanels[5].SetActive(false);
    }

    // 설문 2 -> 1
    public void GoToQ1()
    {
        imageBars[0].color = fillOut;
        surveyPanels[0].SetActive(true);
        surveyPanelMovement[1].DOAnchorPos(new Vector2(-1476, -1376), 0.3f);  // 얘가 빠지고
        surveyPanelMovement[0].DOAnchorPos(new Vector2(0, -1376), 0.3f);   // 얘가 들어감
        surveyPanelMovement[1].DOAnchorPos(new Vector2(1239, -1376), 0.3f);
        surveyPanels[1].SetActive(false);
    }

    // 설문 3 -> 2
    public void GoToQ2()
    {
        imageBars[1].color = fillOut;
        surveyPanels[1].SetActive(true);
        surveyPanelMovement[2].DOAnchorPos(new Vector2(-1476, -1376), 0.3f);  // 얘가 빠지고
        surveyPanelMovement[1].DOAnchorPos(new Vector2(0, -1376), 0.3f);   // 얘가 들어감
        surveyPanelMovement[2].DOAnchorPos(new Vector2(1239, -1376), 0.3f);
        surveyPanels[2].SetActive(false);
    }

    // 설문 4 -> 3
    public void GoToQ3()
    {
        imageBars[2].color = fillOut;
        surveyPanels[2].SetActive(true);
        surveyPanelMovement[3].DOAnchorPos(new Vector2(-1476, -1376), 0.3f);  // 얘가 빠지고
        surveyPanelMovement[2].DOAnchorPos(new Vector2(0, -1376), 0.3f);   // 얘가 들어감
        surveyPanelMovement[3].DOAnchorPos(new Vector2(1239, -1376), 0.3f);
        surveyPanels[3].SetActive(false);
    }

    // 설문 5 -> 4
    public void GoToQ4()
    {
        imageBars[3].color = fillOut;
        surveyPanels[3].SetActive(true);
        surveyPanelMovement[4].DOAnchorPos(new Vector2(-1476, -1376), 0.3f);  // 얘가 빠지고
        surveyPanelMovement[3].DOAnchorPos(new Vector2(0, -1376), 0.3f);   // 얘가 들어감
        surveyPanelMovement[4].DOAnchorPos(new Vector2(1239, -1376), 0.3f);
        surveyPanels[4].SetActive(false);
    }

    // 설문 6 -> 5
    public void GoToQ5()
    {
        imageBars[4].color = fillOut;
        surveyPanels[4].SetActive(true);
        surveyPanelMovement[5].DOAnchorPos(new Vector2(-1476, -1376), 0.3f);  // 얘가 빠지고
        surveyPanelMovement[4].DOAnchorPos(new Vector2(0, -1376), 0.3f);   // 얘가 들어감
        surveyPanelMovement[5].DOAnchorPos(new Vector2(1239, -1376), 0.3f);
        surveyPanels[5].SetActive(false);
    }

    // 종료 패널 -> 측정 패널
    public void End()
    {
        load_Panel.SetActive(true);
        characterX.gameObject.SetActive(true);

        // 값 저장
        SaveSelectedValues("Survey1");
        SaveSelectedValues("Survey2");
        SaveSelectedValues("Survey3");
        SaveSelectedValues("Survey4");
        SaveSelectedValues("Survey5");

        int totalsum = SumSavedValues();
        sarc data = new sarc();

        sarcScore = totalsum;

        data.id = userins.id;
        data.sarc_f = sarcScore;

        Debug.Log(sarcScore);
        string url = "https://port-0-api-7xwyjq992llj595n50.sel4.cloudtype.app/Register/sarc";
        string json = JsonUtility.ToJson(data);

        StartCoroutine(Server.SendJsonData(url, json));


        StartCoroutine(LoadingScene());
    }

    // LoadScene() -> Measurement
    IEnumerator LoadingScene()
    {
        yield return new WaitForSeconds(delay);
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        survey_End.SetActive(false);
        SceneManager.LoadScene("Handgrip_M");
    }

    public void Survey1_Choices(int choice)
    {
        selectedChoices["Survey1"] = choice;
    }

    public void Survey2_Choices(int choice)
    {
        selectedChoices["Survey2"] = choice;
    }

    public void Survey3_Choices(int choice)
    {
        selectedChoices["Survey3"] = choice;
    }

    public void Survey4_Choices(int choice)
    {
        selectedChoices["Survey4"] = choice;
    }

    public void Survey5_Choices(int choice)
    {
        selectedChoices["Survey5"] = choice;
    }

    private void SaveSelectedValues(string surveyName)
    {
        if (selectedChoices.TryGetValue(surveyName, out int choice))
        {
            PlayerPrefs.SetInt(surveyName, choice);
            PlayerPrefs.Save();

            Debug.Log(surveyName + " Choice " + choice + " Value has stored.");
        }
    }

    private void ShowSavedValues(string surveyName)
    {
        if (PlayerPrefs.HasKey(surveyName))
        {
            int savedChoice = PlayerPrefs.GetInt(surveyName);
            Debug.Log(surveyName + " Saved Value: " + savedChoice);
        }
        else
        {
            Debug.Log(surveyName + " has no saved value.");
        }
    }

    public int SumSavedValues()
    {
        int sum = 0;
        foreach (string key in selectedChoices.Keys)
        {
            if (PlayerPrefs.HasKey(key))
            {
                int savedValue = PlayerPrefs.GetInt(key);
                sum += savedValue;
            }
        }

        return sum;
    }

    // 도움말
    public void Helper_1()
    {
        helpers[0].SetActive(true);
    }
    public void Helper_2()
    {
        helpers[1].SetActive(true);
    }

    public void Helper_3()
    {
        helpers[2].SetActive(true);
    }

    public void Helper_4()
    {
        helpers[3].SetActive(true);
    }

    public void Helper_5()
    {
        helpers[4].SetActive(true);
    }
    public void Helper_6()
    {
        helpers[5].SetActive(true);
    }

    private void HideAllHelpers()
    {
        foreach (GameObject helper in helpers)
        {
            helper.SetActive(false);
        }
    }

    // 도움말 나가기
    public void GoBack()
    {
        HideAllHelpers();
    }

    // 로그인 -> 아이디 찾기
    public void FindID()
    {
        panel_ID.SetActive(true);
        panel_PW.SetActive(false);
    }

    // 아이디 찾기 -> 비밀번호 찾기
    public void FindPW()
    {
        panel_PW.SetActive(true);
        showID.SetActive(false);
        panel_ID.SetActive(false);
    }

    public void ShowID()
    {
        string name = GameObject.Find("NAME").GetComponent<TMP_InputField>().text.ToString();
        string birth = GameObject.Find("Birth").GetComponent<TMP_InputField>().text.ToString();

        forgetId dto = new forgetId();

        dto.name = name;
        dto.birth = birth;

        string url = "https://port-0-api-7xwyjq992llj595n50.sel4.cloudtype.app/Register/ForgetID";
        string json = JsonUtility.ToJson(dto);

        StartCoroutine(Server.SendJsonData(url, json));
        StartCoroutine(ReadForgetID());

    }

    IEnumerator ReadForgetID()
    {
        yield return new WaitForSeconds(1.5f);
        showID.SetActive(true);
        GameObject.Find("forgetIDText").GetComponent<TMP_Text>().text = "아이디 : " + Server.response;
        panel_ID.SetActive(false);
    }

    public void ShowPW()
    {
        string name = GameObject.Find("PW_NAME").GetComponent<TMP_InputField>().text.ToString();
        string id = GameObject.Find("PW_ID").GetComponent<TMP_InputField>().text.ToString();
        string forgetText = GameObject.Find("PW_FOR").GetComponent<TMP_InputField>().text.ToString();

        forgetPW dto = new forgetPW();

        dto.name = name;
        dto.id = id;
        dto.forgetText = forgetText;

        string url = "https://port-0-api-7xwyjq992llj595n50.sel4.cloudtype.app/Register/ForgetPassword";
        string json = JsonUtility.ToJson(dto);

        print(json);

        StartCoroutine(Server.SendJsonData(url, json));
        StartCoroutine(ReadForgetPW());
    }

    IEnumerator ReadForgetPW()
    {
        yield return new WaitForSeconds(1.5f);
        showPW.SetActive(true);
        GameObject.Find("for_Password").GetComponent<TMP_InputField>().text = "비밀번호 : " + Server.response;
        panel_PW.SetActive(false);
    }

    public void GoToLogin()
    {
        panel_SignIn.SetActive(true);
        panel_ID.SetActive(false);
        panel_PW.SetActive(false);
        showID.SetActive(false);
    }
}