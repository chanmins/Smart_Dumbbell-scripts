using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooray_ObjectCreate : MonoBehaviour
{
    public GameObject coin;
    public GameObject boom;
    public GameObject parent;

    public int objectcount; // 오브젝트 연속 생성 개수

    public float longWaiting; // 주기 설정
    public float shortWaition; // 물체 사이의 간격

    public Hooray_Gamemanager gameManager;
    
    void Start()
    {
        objectcount = 7;
        longWaiting = 2;
        shortWaition = 0.3f;
    }

    //오브젝트 생성 함수
    public IEnumerator SpawnObject()
    {
        while (true)
        {
            var wfsl = new WaitForSeconds(longWaiting);
            var wfss = new WaitForSeconds(shortWaition);
            if (gameManager.isEnded) break;
            int rand = Random.Range(1, 3);
            for (int i = 0; i < objectcount; i++)
            {
                if (rand == 1)
                {
                    if (i == objectcount - 1)
                    {
                        parent = Instantiate(boom, transform.position + new Vector3(0, 0, 2.5f), transform.rotation);
                        parent.transform.parent = this.transform;
                        yield return wfss;
                        continue;
                    }
                    parent = Instantiate(coin, transform.position + new Vector3(0, 0, 2.5f), transform.rotation);
                    parent.transform.parent = this.transform;
                }
                else if (rand == 2)
                {
                    if (i == objectcount - 1)
                    {
                        parent = Instantiate(boom, transform.position + new Vector3(0, 0, -2.5f), transform.rotation);
                        parent.transform.parent = this.transform;
                        yield return wfss;
                        continue;
                    }
                    parent = Instantiate(coin, transform.position + new Vector3(0, 0, -2.5f), transform.rotation);
                    parent.transform.parent = this.transform;

                }
                yield return wfss;
            }
            yield return wfsl;
        }
    }
}
