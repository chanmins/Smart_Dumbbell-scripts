using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooray_ObjectMove : MonoBehaviour
{
    public int score;
    void Start()
    {
        
        Destroy(this.gameObject, 3f); // 5초뒤에 자동삭제
    }

    void Update()
    {
        if(Time.timeScale == 1)
        move();
    }

    private void move(){
        transform.position += new Vector3(0.2f,0,0);  //앞으로 날라옴
    }
}
