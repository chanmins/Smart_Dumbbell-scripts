using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquatHopping_SpawnerManager : MonoBehaviour
{
    public GameObject[] Coin;

    List<GameObject>[] spawner;

    private void Awake()
    {
        spawner = new List<GameObject>[Coin.Length];

        for (int i = 0; i < spawner.Length; i++)
        {
            spawner[i] = new List<GameObject>();
        }
    }
    public GameObject Get(int index)
    {
        GameObject select = null;
        if (select == null)
        {
            select = Instantiate(Coin[index], transform);
            spawner[index].Add(select);
        }
        foreach (GameObject item in spawner[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        return select;
    }
}
