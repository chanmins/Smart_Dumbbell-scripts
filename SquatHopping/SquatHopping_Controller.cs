using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SerialData;


public class SquatHopping_Controller: MonoBehaviour
{
    public Slider slider;

    private Rigidbody rigid;
    public float globalGravity;
    public float gravityScale;

    public int maxPower;
    public int power;
    public float chargingTime;

    public bool isJumping;
    Animator anim;

    public GameObject[] gaugeListL;
    public GameObject[] gaugeListR;
    public float gaugeValue;
    bool gaugeFlag;

    public bool SerialJumpFlag;
    GameObject serial;
    private float maxYaxis;

    void Start()
    {
        maxYaxis = 17.3f;
        serial = GameObject.Find("Serial");

        Time.timeScale = 0;
        maxPower = 35;
        power = 3;
        chargingTime = 0;

        SerialJumpFlag = false;

        globalGravity = -9.81f;
        gravityScale = 3.0f;
        for (int i = 0; i < 10; i++)
        {
            gaugeListL[i].SetActive(false);
            gaugeListR[i].SetActive(false);
        }
        gaugeFlag = false;
        isJumping = false;

        serial.GetComponent<Serial>().SerialSendingGrip();
    }
    //커스텀 중력
    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        rigid.useGravity = false;
    }
    private void FixedUpdate()
    {
        if (Time.timeScale == 1)
        {
            Vector3 gravity = globalGravity * gravityScale * Vector3.up;
            rigid.AddForce(gravity, ForceMode.Acceleration);
            Jump();

            // Y값이 최대치를 넘지 않도록 제한
            if(transform.position.y > maxYaxis)
            {
                Vector3 newPosition = transform.position;
                newPosition.y = maxYaxis;
                transform.position = newPosition;
                rigid.velocity = Vector3.zero;  // Y값을 제한하면 속도를 초기화하여 뭉치지 않게된다
            }
        }
    }


    void Update()
    {
        if (Time.timeScale == 1)
        {
            slider.value = (chargingTime * power) / maxPower;
            gauge();
        }
    }
    void gauge()
    {
        gaugeValue = (chargingTime * power) / maxPower*8;

        if (gaugeValue > 10)
            gaugeValue = 10;
        
        if(gaugeFlag)
        {
            
            for(int i=0; i<gaugeValue;i++)
            {
                gaugeListL[i].SetActive(true);
                gaugeListR[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                gaugeListL[i].SetActive(false);
                gaugeListR[i].SetActive(false);
            }
        }

    }
    void Jump()
    {
        if (InputData.force > 10)
        {
            SerialJumpFlag = true;
            if (isJumping)
            {
                Debug.Log("점프 중");
                return;
            }
            else
            {
                gaugeFlag = true;
                chargingTime += Time.deltaTime * 10;
                anim.SetBool("doHold",true);
            }

        }


        if (SerialJumpFlag && InputData.force < 5)
        {
            SerialJumpFlag = true;
            if(isJumping)
            {
                Debug.Log("점프 중");
                return;
            }
            else
            {
               
                float tempPower = chargingTime * power;
                if (tempPower >= maxPower)
                    tempPower = maxPower;
                rigid.AddForce(Vector3.up * (tempPower), ForceMode.Impulse);
                chargingTime = 0;
                isJumping = true;
                anim.SetBool("isJump",true);
                anim.SetTrigger("doJump");
                anim.SetBool("doHold", false);
               
            }

            SerialJumpFlag = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            Debug.Log("땅에 닿음");
            isJumping = false;
            anim.SetBool("isJump",false);
            gaugeFlag = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gold"))
        {   
            SquatHopping_GameManager.instance.GetScore(100);
        }
        if (other.CompareTag("silver"))
        {
            Debug.Log("Get silver");
            SquatHopping_GameManager.instance.GetScore(50);
        }
        if (other.CompareTag("bomb"))
        {
            Debug.Log("Bomb!");
            SquatHopping_GameManager.instance.GetScore(-30);
        }
    }
}
