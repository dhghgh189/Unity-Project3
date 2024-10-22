using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelGuard : MonoBehaviour
{
    [SerializeField] Shooter shooter;
    [SerializeField] ShotgunInteractable shotgunInteractable;

    private void OnTriggerEnter(Collider other)
    {
        if (shotgunInteractable.IsActivatedPumpAction == false)
            return;

        if (other.gameObject.name == "LoadTrigger")
        {
            //Debug.Log("LoadTrigger Enter");
            shooter.LoadAmmoToChamber();
        }
        else if (other.gameObject.name == "EjectTrigger")
        {
            //Debug.Log("EjectTrigger Enter");
            shooter.Eject();
        }
    }
}
