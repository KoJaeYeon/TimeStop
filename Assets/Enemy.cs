using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 10f; // 총알이 파괴되기까지의 시간
    public float shootInterval = 3f; // 총알 발사 간격

    void Start()
    {
        StartCoroutine(ShootAtPlayer());
    }

    void Update()
    {
        RotateTowardsPlayer();
    }

    IEnumerator ShootAtPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootInterval);
            Shoot();
        }
    }

    void RotateTowardsPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0; // Y축 회전을 유지합니다.

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }

    void Shoot()
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0; // Y축 성분을 제거하여 XZ 평면에서만 이동하도록 합니다.

            // 총알 생성
            GameObject bullet = Instantiate(bulletPrefab, transform.position + direction, Quaternion.LookRotation(direction));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }

            // 일정 시간이 지나면 총알 파괴
            Destroy(bullet, bulletLifetime);
        }
    }
}
