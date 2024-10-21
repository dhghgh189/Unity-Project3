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

        // 라운드 진행중일 땐 새 탄약 수급 불가
        if (GameManager.Instance.CurState == GameManager.EState.Process)
            return;

        Ammo ammo = Instantiate(ammoPrefab);
        _interactionManager.SelectEnter(args.interactorObject, ammo.GetComponent<IXRSelectInteractable>());
    }
}
