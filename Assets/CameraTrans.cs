using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrans : MonoBehaviour
{
    public Transform playerTrans;    

    private void LateUpdate()
    {
        if (playerTrans != null)
        {
            // 플레이어의 위치를 기준으로 카메라의 위치를 설정합니다.
            Vector3 newPosition = playerTrans.position + new Vector3(10, 10, 10);
            transform.position = newPosition;
        }
    }
}
