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

    [Header("Gun Setting")]
    [SerializeField] int maxAmmo;

    AudioSource _audioSource;
    float _nextShotTime;
    ShotgunInteractable _interactable;

    WaitForSeconds _waitTime;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _nextShotTime = 0;
        _interactable = GetComponent<ShotgunInteractable>();

        _waitTime = new WaitForSeconds(0.17f);
    }

    public void Shoot()
    {
        // 양손 grab 여부 확인
        if (!_interactable.IsReadyToUse)
            return;

        if (Time.time < _nextShotTime)
            return;

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
