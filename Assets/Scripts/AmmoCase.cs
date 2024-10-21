using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AmmoCase : MonoBehaviour
{
    [SerializeField] Ammo ammoPrefab;

    XRInteractionManager _interactionManager;

    private void Awake()
    {
        _interactionManager = FindAnyObjectByType<XRInteractionManager>();
    }

    public void GrabAmmo(SelectEnterEventArgs args)
    {
        _interactionManager.SelectCancel(args.interactorObject, args.interactableObject);

        Ammo ammo = Instantiate(ammoPrefab);
        _interactionManager.SelectEnter(args.interactorObject, ammo.GetComponent<IXRSelectInteractable>());
    }
}
