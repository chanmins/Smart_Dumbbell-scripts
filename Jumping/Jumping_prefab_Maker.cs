using System;
using UnityEngine;

public class Jumping_prefab_Maker : MonoBehaviour
{
    // 맵 오브젝트 모음
    public GameObject basic;
    public GameObject before;
    public GameObject after;
    public GameObject gate;
    public GameObject ascend;

    // 맵 만드는거 관련 함수
    public Boolean _isMakeBefore;
    public int freqBasic;           // 100보다 작은 값 아래로 갈수록 더 커야함
    public int freqbefore;

    // 만드는 시간 관련 변수
    public int time;
    public int makeTime;

    // 한번 만들지 여부
    public Boolean onlyOne;

    void Start()
    {
        // prefab 속도 0.5f -> makeTime = 64
        makeTime = 129;
        _isMakeBefore = false;
        time = makeTime;
        if (!onlyOne)
        {
            Instantiate(gate, transform.position, transform.rotation);
        }
    }

    void FixedUpdate()
    {
        // 한번만 생성하는 함수
        if (onlyOne && (time == makeTime))
        {
            Instantiate(basic, transform.position, transform.rotation);
            makeTime = -1;
        }

        // 계속 만들어내는 함수
        if (time == makeTime)
        {
            time = 0;
            if (_isMakeBefore)
            {
                Instantiate(after, transform.position, transform.rotation);
                _isMakeBefore = false;
                return;
            }

            int randMap = UnityEngine.Random.Range(0, 101);
            if (randMap < freqBasic)
            {
                Instantiate(basic, transform.position, transform.rotation);
            }
            else if (randMap < freqbefore)
            {
                _isMakeBefore = true;
                Instantiate(before, transform.position, transform.rotation);
            }
        }
        else
        {
            time++;
        }
    }
}
