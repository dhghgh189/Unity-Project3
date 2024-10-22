using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ammo : MonoBehaviour
{
    [SerializeField] int bulletCount;
    [SerializeField] float maxAngle;
    [SerializeField] Bullet bulletPrefab;

    bool _isUsed;
    public bool IsUsed { get { return _isUsed; } }
    WaitForSeconds _destroyTime;

    SelfDestroy _selfDestroy;

    XRGrabInteractable _grabInteractable;

    InteractionLayerMask _originalLayer;

    void Awake()
    {
        _selfDestroy = GetComponent<SelfDestroy>();
        _grabInteractable = GetComponent<XRGrabInteractable>();

        _originalLayer = _grabInteractable.interactionLayers;

        Init();
    }

    public void Init()
    {
        SetInteractionLayer(_originalLayer);
        _isUsed = false;
    }

    public void Use(Transform muzzlePoint, float power)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float xAngle = Random.Range(-maxAngle, maxAngle);
            float yAngle = Random.Range(-maxAngle, maxAngle);

            //Bullet bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
            Bullet bullet = PoolManager.Instance.Pop<Bullet>(bulletPrefab.gameObject);
            bullet.transform.position = muzzlePoint.position;
            bullet.transform.rotation = muzzlePoint.rotation;
            bullet.transform.Rotate(xAngle, yAngle, 0);
            bullet.AddForce(bullet.transform.forward * power, ForceMode.Impulse);
        }

        _isUsed = true;
        //Debug.Log("Ammo is used, please eject empty cartridge");
    }

    public void SetInteractionLayer(InteractionLayerMask layerMask)
    {
        _grabInteractable.interactionLayers = layerMask;
    }

    public void ReserveDestroy()
    {
        _selfDestroy.ReserveDestroy();
    }
}
