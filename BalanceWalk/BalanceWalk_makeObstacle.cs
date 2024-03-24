using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BalanceWalk_makeObstacle : MonoBehaviour
{
    public GameObject Player;
    public GameObject Obstacle;

    public float createObstacle;
    public int countObstacle;

    void Start()
    {
        createObstacle = 0;
        countObstacle = 0;
    }

    void Update()
    {
        createObstacle += Time.deltaTime;

        // 3초마다 프리펩 생성
        if(createObstacle >= 3.0f)
            SpawnObstacle();
    }

    // 장애물 랜덤으로 좌,우 생성
    public void SpawnObstacle()
    {
        int direction = Random.Range(0, 2);

        // 왼쪽에 장애물 생성
        if (direction == 0)
        {
            Vector3 position = Player.transform.position;
            // obstacle 생성
            GameObject objectLeft = (GameObject)Instantiate(Obstacle, position + new Vector3(-1.5f, -1, -5), Quaternion.Euler(new Vector3(0, 180, 0)));
        }

        // 오른쪽에 장애물 생성
        else if (direction == 1)
        {
            Vector3 position = Player.transform.position;
            GameObject objectRight = (GameObject)Instantiate(Obstacle, position + new Vector3(1.5f, -1, -5), Quaternion.Euler(new Vector3(0, 180, 0)));
        }

        // 장애물이 한 번 생성되면 0으로 초기화 시킴
        createObstacle = 0;
    }
}