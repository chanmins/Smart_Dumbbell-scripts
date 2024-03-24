using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquatHopping_CoinSpawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public float SpawnTime;
    float timer;
    public static bool direct;

    Vector3 moveVec;
    public float turnSpeed;
    public float speed;
    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        foreach (Transform child in spawnPoint)
        {
            if (child.name == transform.name)
            {
                return;
            }
        }
    }
    void Start()
    {
        SpawnTime = 3.0f;
        direct = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        int random = Random.Range(0, 2);
        if (timer > SpawnTime)
        {
            if(random==0)
                leftSpawn();
            if(random==1)
                rightSpawn();
            timer = 0;
        } 
    }
   
    void leftSpawn()
    {
        int start = Random.Range(1, 3);
        int finish = Random.Range(start, 6);
        int isBomb = Random.Range(0, 2);
        if(isBomb==0)
        {
            for(int i = start; i<=finish;i++)
            {   
                GameObject coin = SquatHopping_GameManager.instance.spawnerManager.Get(Random.Range(0, 2));
                coin.transform.position = spawnPoint[i].position;
            }
        }
        if(isBomb==1)
        { 
            GameObject bomb = SquatHopping_GameManager.instance.spawnerManager.Get(2);
            bomb.transform.position = spawnPoint[start].position;
            for (int i = start+1; i <= finish; i++)
            {
                GameObject coin = SquatHopping_GameManager.instance.spawnerManager.Get(Random.Range(0, 2));
                coin.transform.position = spawnPoint[i].position;
            }
           
        }
        
    }

    void rightSpawn()
    {
        int start = Random.Range(7, 9);
        int finish = Random.Range(start, 12);
        int isBomb = Random.Range(0, 2);
        if (isBomb == 0)
        {
            for (int i = start; i <= finish; i++)
            {
                GameObject coin = SquatHopping_GameManager.instance.spawnerManager.Get(Random.Range(3, 5));
                coin.transform.position = spawnPoint[i].position;
            }
        }
        if (isBomb == 1)
        {
            
            for (int i = start; i < finish; i++)
            {
                GameObject coin = SquatHopping_GameManager.instance.spawnerManager.Get(Random.Range(3, 5));
                coin.transform.position = spawnPoint[i].position;
            }
            GameObject bomb = SquatHopping_GameManager.instance.spawnerManager.Get(5);
            bomb.transform.position = spawnPoint[finish].position;
        }
    }
    int RandomSpawn()
    {
        return 0;
    }
}
