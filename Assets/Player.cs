using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 2f; // 총알이 파괴되기까지의 시간

    public Transform shootTrans; // 총알 발사 위치

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private bool isTimeStopped = false;

    public GameObject UIText;

    void Update()
    {
        // 입력을 받습니다.
        float moveX = Input.GetAxisRaw("Horizontal"); // 옆으로 움직임
        float moveY = Input.GetAxisRaw("Vertical");   // 위아래로 움직임

        // 플레이어의 방향에 따른 움직임을 계산합니다.
        moveInput = new Vector3(moveX, 0, moveY).normalized;
        moveVelocity = transform.TransformDirection(moveInput) * moveSpeed;

        // Transform을 직접 이동시킵니다.
        transform.position += moveVelocity * Time.unscaledDeltaTime;

        // 마우스 위치를 기준으로 플레이어 회전
        RotatePlayerToMouse();

        // 총알 발사 처리
        if (Input.GetButtonDown("Fire1")) // 기본적으로 마우스 왼쪽 버튼
        {
            Shoot();
        }

        // T키를 눌러 시간 정지/재개
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
            direction.y = 0; // Y축 회전을 유지합니다.

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
            Vector3 direction = (targetPosition - shootTrans.position).normalized; // shootTrans 위치에서 발사
            direction.y = 0; // Y축 성분을 제거합니다.

            // 총알 생성
            GameObject bullet = Instantiate(bulletPrefab, shootTrans.position, Quaternion.LookRotation(direction));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }

            // 일정 시간이 지나면 총알 파괴
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
