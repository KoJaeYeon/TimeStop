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
            // �÷��̾��� ��ġ�� �������� ī�޶��� ��ġ�� �����մϴ�.
            Vector3 newPosition = playerTrans.position + new Vector3(10, 10, 10);
            transform.position = newPosition;
        }
    }
}
