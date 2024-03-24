using SerialData;
using System;
using System.Collections;
using UnityEngine;
using updataData;
public class Jumping_Player : MonoBehaviour
{
    Rigidbody rigid;

    // 점프 관련
    public bool _isJump;
    float JumpPower;
    public float JumpSpeed;
    public float strength;

    // 슬로우 점프 관련
    public bool _isLimit;
    public float jumpTime;

    //중력 관련
    public float gravity;
    public float currentGravity;
    public Boolean isfall;
    public int respawnTime;

    public GameObject gameManager;

    public bool jumpFlag;
    public Serial Serial;

    void Start()
    {
        Serial = GameObject.Find("Serial").GetComponent<Serial>();
        rigid = GetComponent<Rigidbody>();
        _isJump = true;
        _isLimit = true;
        isfall = false;
        jumpFlag = true;
        Serial.SerialSendinggyro();
    }

    void FixedUpdate()
    {
        // 리스폰시간
        if (isfall)
        {
            if (respawnTime == 50)
            {
                respawnTime = 0;
                isfall = false;
                rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                respawnTime++;
            }
            return;
        }

        Player();

        // 일정 높이 넘어가는거 제한함수
        if (rigid.velocity.y > 12)
        {
            rigid.velocity = new Vector3(transform.GetComponent<Rigidbody>().velocity.x, 12, transform.GetComponent<Rigidbody>().velocity.z);
        }

        // 죽었을시
        if (transform.position.y < -12)
        {
            transform.position = new Vector3(transform.position.x, 5, transform.position.z);
            rigid.velocity = new Vector3(transform.GetComponent<Rigidbody>().velocity.x, -4, transform.GetComponent<Rigidbody>().velocity.z);
            isfall = true;
            rigid.constraints = RigidbodyConstraints.FreezeAll;
        }

        // 일정속도 넘어가는거 제한함수
        if ((GetComponent<Transform>().position.y > 3) && _isLimit)
        {
            _isLimit = false;
            StartCoroutine(SlowJump());
        }

        // 슬로우 점프 코루틴
        IEnumerator SlowJump()
        {
            rigid.velocity = new Vector3(transform.GetComponent<Rigidbody>().velocity.x, 6, transform.GetComponent<Rigidbody>().velocity.z);
            yield return new WaitForSeconds(jumpTime);
            rigid.velocity = new Vector3(transform.GetComponent<Rigidbody>().velocity.x, 3, transform.GetComponent<Rigidbody>().velocity.z);
        }

    }

    // 플레이어가 전체 작동 함수
    void Player()
    {
        if (contentData.y >= 30 && contentData.y < 100 && jumpFlag)
        {
            jumpFlag = false;
            JumpPower = JumpSpeed * strength * strength * strength;
            currentGravity = gravity * 0.03f;
            Jump();
        }
        else if (contentData.y >= 0 && contentData.y < 10 && !jumpFlag)
        {
            jumpFlag = true;
            currentGravity = gravity;
            JumpPower = 0;
        }

        rigid.AddForce(Vector3.down * currentGravity, ForceMode.Impulse);
    }

    // 점프 함수
    void Jump()
    {
        if (_isJump == false)
        {
            rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
            print("jump");
            _isJump = true;
        }
    }

    // 바닥과 닿으면 발생
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            _isLimit = true;
            _isJump = false;
        }
    }
}