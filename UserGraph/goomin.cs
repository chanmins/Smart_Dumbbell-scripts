using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goomin : MonoBehaviour
{

    public GameObject findgoomin;

    // Start is called before the first frame update
    void Start()
    {
        findgoomin = GameObject.FindWithTag("Goomin");

        Debug.Log(findgoomin.name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
