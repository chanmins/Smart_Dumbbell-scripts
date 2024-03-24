using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using updataData;

public class Hooray_PlayerRotate : MonoBehaviour
{
    public GameObject waist; //인간의 허리 변수
/*    public Slider slider; // 슬라이더 */
    public int speed; // 허리 돌아가는 속도 설정

    public float rotationSpeed;
    public float targetAngle;

    private Quaternion startRotation;
    private Quaternion targetRotation;
    private Quaternion nextRotation;
    public float currentAngle = 0.0f;
    public bool isRotate;
    public bool isPlay = true;

    public GameObject serial;

    public Hooray_Gamemanager gameManager;

    private void Start()
    {
        targetAngle = 0f;
        serial = GameObject.Find("Serial");
        rotationSpeed = 20f;
        startRotation = waist.transform.rotation;
        targetRotation = Quaternion.Euler(0, 0, 0) * startRotation;
        serial.GetComponent<Serial>().SerialSendinggyro();
    }


    // Update is called once per frame
    void Update()
    {
        if (gameManager.isEnded)
        {
            return;
        }
        RotateControl();
    }

    // 허리 돌리는 함수
    public void Rotate()
    {
        currentAngle += rotationSpeed * Time.deltaTime * 5;
        waist.gameObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, currentAngle / targetAngle);
    }
    //중앙으로 이동하는 함수수
    public void center()
    {
        currentAngle += rotationSpeed * Time.deltaTime * 3;
        waist.gameObject.transform.rotation = Quaternion.Lerp(targetRotation, startRotation, currentAngle / targetAngle);
    }

    //완전히 돌아갔는지 체크
    public bool finishRotate()
    {
        if (currentAngle >= targetAngle - 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //왼쪽으로 돌리는 함수
    IEnumerator leftRotate()
    {

        targetRotation = Quaternion.Euler(-targetAngle, 0, 0) * startRotation;

        while (currentAngle <= targetAngle)
        {
            Rotate();
            yield return null;
        }

        isPlay = true;
    }

    //오른쪽으로 돌리는 함수
    IEnumerator rightRotate()
    {

        targetRotation = Quaternion.Euler(targetAngle, 0, 0) * startRotation;

        while (currentAngle <= targetAngle)
        {
            Rotate();
            yield return null;
        }

        isPlay = true;
    }
    //중앙으로 이동하는 함수
    IEnumerator centerRotate()
    {

        while (currentAngle <= targetAngle)
        {
            center();
            yield return null;
        }
    }


    //회전 제어 함수
    public void RotateControl()
    {

        if (isPlay)
        {

            if (contentData.y >= 0 && contentData.y < 10)
            { //슬라이더 바가 왼쪽으로 갔을 경우
                isPlay = false;
                if (finishRotate() && !isRotate)
                {
                    currentAngle = 0;
                }

                StartCoroutine(leftRotate());


                isRotate = true;

            }
            else if (contentData.y >= 30 && contentData.y < 100)
            { //슬라이더 바가 오른쪽으로 갔을 경우
                isPlay = false;
                if (finishRotate() && !isRotate)
                {
                    currentAngle = 0;
                }

                StartCoroutine(rightRotate());

                isRotate = true;
            }
            else
            {                      //슬라이더 바가 중앙으로 갔을 경우

                if (finishRotate() && isRotate)
                {
                    currentAngle = 0;
                }

                StartCoroutine(centerRotate());

                isRotate = false;
            }

        }

    }
}