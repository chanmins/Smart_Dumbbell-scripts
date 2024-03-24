using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquatHopping_Coin2 : MonoBehaviour
{
    public float turnSpeed;
    public float speed;
    Vector3 moveVec;
    // Start is called before the first frame update
    void Start()
    {
        turnSpeed = 100.0f;
        speed = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        SpinCoin();
        MoveCoin();
    }
    public void SpinCoin()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * turnSpeed);
    }
    public void MoveCoin()
    {
        moveVec = new Vector3(-1, 0, 0);
        transform.position += moveVec * Time.deltaTime * speed;
        if (transform.position.x <= -30)
            this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("player"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
