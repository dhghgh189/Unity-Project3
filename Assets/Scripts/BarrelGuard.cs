using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelGuard : MonoBehaviour
{
    [SerializeField] Shooter _shooter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "LoadTrigger")
        {
            Debug.Log("LoadTrigger Enter");
            _shooter.LoadAmmoToChamber();
        }
        else if (other.gameObject.name == "EjectTrigger")
        {
            Debug.Log("EjectTrigger Enter");
            _shooter.Eject();
        }
    }
}
