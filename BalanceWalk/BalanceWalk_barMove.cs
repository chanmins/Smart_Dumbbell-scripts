using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using updataData;

public class BalanceWalk_barMove : MonoBehaviour
{
    public Slider slider_barRotate;
    public GameObject player;

    public float lerpSpeed_bar;
    public float playerSpeed;


    public GameObject settingScript;
    public Serial Serial;
    public bool flag;

    void Awake()
    {
        lerpSpeed_bar = 1f;
        playerSpeed = 2.5f;
        Serial = GameObject.Find("Serial").GetComponent<Serial>();
        flag = false;
    }

    void Start()
    {
        Time.timeScale = 0f;
    }

    // 플레이어의 속도를 조절할 수 있도록함.
    void Update()
    {
        if (!flag)
        {
            Serial.SerialSendinggyro();
            flag = true;
        }

        // 준비 카운트가 끝나면 플레이어 움직임 활성화
        if (Time.timeScale == 1)
        {
            player.transform.position = player.transform.position + new Vector3(0, 0, -playerSpeed * Time.deltaTime);
            rotate_bar();
        }

    }

    // 덤벨의 움직임에 따라서 GameObject bar를 회전시키는 함수
    public void rotate_bar()
    {
        float targetRotation_bar = Mathf.Lerp(70, 110, contentData.x/15);
        Quaternion targetQuaternion_bar = Quaternion.Euler(0f, 0f, targetRotation_bar);
        transform.rotation = Quaternion.Lerp(transform .rotation, targetQuaternion_bar, lerpSpeed_bar);
    }

}
