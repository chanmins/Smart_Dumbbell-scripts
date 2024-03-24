using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceWalk_Obstacle : MonoBehaviour
{
    private float elapsedTime = 0.0f;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // Lerp() 함수를 사용하여 이동할 거리 계산
        float t = Mathf.Clamp01(elapsedTime);
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);

        // 이동이 완료되면 스크립트 비활성화
        if (transform.position == targetPosition)
        {
            enabled = false;
        }
    }
}