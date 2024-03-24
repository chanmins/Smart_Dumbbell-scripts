using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;



public class NumberPuzzle : MonoBehaviour
{
    public float LerpTime;
    Vector3 UpPos;//올렸을때 위치
    Vector3 DownPos;//내렸을때 위치
    public Button[] button = new Button[15];
    public TextMeshProUGUI[] block = new TextMeshProUGUI[15];
    List<int> number = new List<int>();
    public int count = 1;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI nextNumber;
    public float timer = 0;


    void Start()
    {
        for (int i = 1; i <= 15; i++)
        {
            number.Add(i);
        }
        print(number[11]);
        for (int i = 1; i <= 15; i++)
        {
            int k = Random.Range(0, number.Count);
            block[i - 1].GetComponent<TextMeshProUGUI>().text = number[k].ToString();
            number.RemoveAt(k);
        }
        timerText.GetComponent<TextMeshProUGUI>().text = timer.ToString();
        nextNumber.GetComponent<TextMeshProUGUI>().text = count.ToString();
    }

    void Update()
    {
        timer += Time.deltaTime;
        float timer2 = Mathf.Floor(timer);
        timerText.GetComponent<TextMeshProUGUI>().text = timer2.ToString();
    }

    IEnumerator moveCo(Vector3 up, Vector3 down, GameObject g, int i)
    {
        // print("moveCo");
        // print(g.transform.GetChild(i).name);
        float moveTime = 0.0f;

        moveTime = 0.0f;
        while (moveTime < LerpTime)
        {
            moveTime += Time.deltaTime * 1.5f;
            // Wall.transform.position
            //     = Vector3.Lerp(DownPos, UpPos, moveTime / LerpTime);
            g.transform.GetChild(i).localPosition = Vector3.Lerp(down, up, moveTime / LerpTime);
            yield return null;
        }

    }

    public void ButtonCheck()
    {

        string ButtonName = EventSystem.current.currentSelectedGameObject.name;
        string but = GameObject.Find(ButtonName).GetComponentInChildren<TextMeshProUGUI>().text;

        print(but);
        if (count == int.Parse(but))
        {

            GameObject g = GameObject.Find(ButtonName + "-");
            // moveWalls(g);
            print("Correct");
            count++;
            GameObject.Find(ButtonName).GetComponentInChildren<TextMeshProUGUI>().text = "V";
            nextNumber.GetComponent<TextMeshProUGUI>().text = count.ToString();
            GameObject.Find(ButtonName).SetActive(false);
        }
        else
        {
            print("False");
        }
    }

    // public void moveWalls(GameObject g)
    // {

    //     print("enter moveWalls");
    //     print(g.transform.childCount);
    //     for(int i = 0; i<g.transform.childCount; i++)
    //     {
    //         UpPos = new Vector3(g.transform.GetChild(i).localPosition.x, g.transform.GetChild(i).localPosition.y + 2.5f, g.transform.GetChild(i).localPosition.z);
    //         DownPos = g.transform.GetChild(i).localPosition;

    //         StartCoroutine(moveCo(UpPos, DownPos, g, i));
    //     }
    // }
}
