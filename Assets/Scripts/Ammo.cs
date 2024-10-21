using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] int bulletCount;
    [SerializeField] float maxAngle;
    [SerializeField] Bullet bulletPrefab;

    bool _isUsed;
    public bool IsUsed { get { return _isUsed; } }

    public void Use(Transform muzzlePoint, float power)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float xAngle = Random.Range(-maxAngle, 0);
            float yAngle = Random.Range(-maxAngle, maxAngle);

            // 풀링 필요
            Bullet bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
            bullet.transform.Rotate(xAngle, yAngle, 0);
            bullet.AddForce(bullet.transform.forward * power, ForceMode.Impulse);
        }

        //EditorApplication.isPaused = true;

        _isUsed = true;
        //Debug.Log("Ammo is used, please eject empty cartridge");
    }
}
