using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 2f; // �Ѿ��� �ı��Ǳ������ �ð�

    public Transform shootTrans; // �Ѿ� �߻� ��ġ

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private bool isTimeStopped = false;

    public GameObject UIText;

    void Update()
    {
        // �Է��� �޽��ϴ�.
        float moveX = Input.GetAxisRaw("Horizontal"); // ������ ������
        float moveY = Input.GetAxisRaw("Vertical");   // ���Ʒ��� ������

        // �÷��̾��� ���⿡ ���� �������� ����մϴ�.
        moveInput = new Vector3(moveX, 0, moveY).normalized;
        moveVelocity = transform.TransformDirection(moveInput) * moveSpeed;

        // Transform�� ���� �̵���ŵ�ϴ�.
        transform.position += moveVelocity * Time.unscaledDeltaTime;

        // ���콺 ��ġ�� �������� �÷��̾� ȸ��
        RotatePlayerToMouse();

        // �Ѿ� �߻� ó��
        if (Input.GetButtonDown("Fire1")) // �⺻������ ���콺 ���� ��ư
        {
            Shoot();
        }

        // TŰ�� ���� �ð� ����/�簳
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTimeScale();
        }
    }

    void RotatePlayerToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0; // Y�� ȸ���� �����մϴ�.

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.unscaledDeltaTime * moveSpeed);
            }
        }
    }

    void Shoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;
            Vector3 direction = (targetPosition - shootTrans.position).normalized; // shootTrans ��ġ���� �߻�
            direction.y = 0; // Y�� ������ �����մϴ�.

            // �Ѿ� ����
            GameObject bullet = Instantiate(bulletPrefab, shootTrans.position, Quaternion.LookRotation(direction));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }

            // ���� �ð��� ������ �Ѿ� �ı�
            Destroy(bullet, bulletLifetime);
        }
    }

    void ToggleTimeScale()
    {
        if (isTimeStopped)
        {
            Time.timeScale = 1f;
            UIText.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            UIText.SetActive(true);
        }
        isTimeStopped = !isTimeStopped;
    }
}
