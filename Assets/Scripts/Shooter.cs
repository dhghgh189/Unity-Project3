using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] AudioClip shotClip;
    [SerializeField] Transform muzzlePoint;
    [SerializeField] float shotPower;
    [SerializeField] float shotInterval;

    AudioSource _audioSource;
    float _nextShotTime;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _nextShotTime = 0;
    }

    public void Shoot()
    {
        if (Time.time < _nextShotTime)
            return;

        Bullet bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        if (bullet != null)
        {
            bullet.AddForce(muzzlePoint.transform.forward * shotPower, ForceMode.Impulse);
            _audioSource.PlayOneShot(shotClip);
            _nextShotTime = Time.time + shotInterval;
        }
    }
}
