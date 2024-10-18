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

    WaitForSeconds _waitTime;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _nextShotTime = 0;

        _waitTime = new WaitForSeconds(0.17f);
    }

    public void Shoot()
    {
        if (Time.time < _nextShotTime)
            return;

        //Bullet bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        //if (bullet != null)
        //{
        //    _audioSource.PlayOneShot(shotClip);
        //    bullet.AddForce(muzzlePoint.transform.forward * shotPower, ForceMode.Impulse);
        //    _nextShotTime = Time.time + shotInterval;
        //}

        StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        _audioSource.PlayOneShot(shotClip);
        yield return _waitTime;

        Bullet bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        bullet.AddForce(muzzlePoint.transform.forward * shotPower, ForceMode.Impulse);

        _nextShotTime = Time.time + shotInterval;
    }
}
