using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquatHopping_Launcher : MonoBehaviour
{
    public float charging;
    public float total;
    // Start is called before the first frame update
    void Start()
    {
        charging = 0.03f;
        total = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Hold();
    }
    void Hold()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            this.transform.localScale -= new Vector3(0.0f, charging, 0.0f);
            total += charging;
            if (total >= 1.0f)
                charging = 0.0f;
        }
            
        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.transform.localScale = new Vector3(5.0f, 2.5f, 5.0f);
            charging = 0.03f;
            total = 0.0f;
        }
            
    }
}
