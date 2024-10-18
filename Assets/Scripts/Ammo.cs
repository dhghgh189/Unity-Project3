using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;

    bool _isUsed;
    public bool IsUsed { get { return _isUsed; } }

    void OnEnable()
    {
        _isUsed = false;    
    }

    public void Use(Transform muzzlePoint, float power)
    {
        Bullet bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        bullet.AddForce(muzzlePoint.forward * power, ForceMode.Impulse);

        _isUsed = true;
        Debug.Log("Ammo is used, please eject empty cartridge");
    }
}
