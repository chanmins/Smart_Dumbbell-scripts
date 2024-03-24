using UnityEngine;

public class Jumping_wheel : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Rotate(15 *Vector3.left);
    }
}
