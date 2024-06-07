using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 10f; // �Ѿ��� �ı��Ǳ������ �ð�
    public float shootInterval = 3f; // �Ѿ� �߻� ����

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
            direction.y = 0; // Y�� ȸ���� �����մϴ�.

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
            direction.y = 0; // Y�� ������ �����Ͽ� XZ ��鿡���� �̵��ϵ��� �մϴ�.

            // �Ѿ� ����
            GameObject bullet = Instantiate(bulletPrefab, transform.position + direction, Quaternion.LookRotation(direction));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }

            // ���� �ð��� ������ �Ѿ� �ı�
            Destroy(bullet, bulletLifetime);
        }
    }
}
