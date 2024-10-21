using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class ShotgunInteractable : MonoBehaviour
{
    [SerializeField] Transform barrelGuardTransform;
    [SerializeField] float barrelGuardZPosMin;

    [SerializeField] Vector3 _startBarrelGuardPos;

    Transform _firstSelectHandTransform;
    Transform _secondSelectHandTransform;

    IXRSelectInteractable _interactable;
    XRInteractionManager _interactionManager;
    XRGeneralGrabTransformer _grabTransformer;

    Shooter _shooter;
    bool _isActivatedPumpAction;

    public bool IsReadyToUse 
    { 
        get 
        {
            return (_firstSelectHandTransform != null) && (_secondSelectHandTransform != null);
        } 
    }

    void Awake()
    {
        _interactable = GetComponent<IXRSelectInteractable>();
        _interactionManager = FindAnyObjectByType<XRInteractionManager>();
        _startBarrelGuardPos = barrelGuardTransform.localPosition;
        _shooter = GetComponent<Shooter>();
        _grabTransformer = GetComponent<XRGeneralGrabTransformer>();
    }

    void Update()
    {
        if (!IsReadyToUse)
            return;

        if (_isActivatedPumpAction)
        {
            float amount = _secondSelectHandTransform.position.z - barrelGuardTransform.position.z;
            float zValue = Mathf.Clamp(barrelGuardTransform.localPosition.z + amount, barrelGuardZPosMin, _startBarrelGuardPos.z);
            barrelGuardTransform.localPosition = new Vector3(_startBarrelGuardPos.x, _startBarrelGuardPos.y, zValue);
        }
    }

    public void Grab(SelectEnterEventArgs args)
    {
        // ���� Interactor �ߺ� ����
        if (args.interactorObject.transform.parent == _firstSelectHandTransform ||
            args.interactorObject.transform.parent == _secondSelectHandTransform)
        {
            _interactionManager.SelectCancel(args.interactorObject, _interactable);
            return;
        }

        // ù��°�� select�� �հ� �ι�°�� select�� ���� Interactor�� ����
        if (_firstSelectHandTransform == null)
        {
            SetFirstHand(args.interactorObject);
            //Debug.Log($"first hand grab! : {_firstSelectHandTransform.name} ({args.interactorObject.transform.gameObject.name})");
        }
        else
        {
            SetSecondHand(args.interactorObject);
            //Debug.Log($"second hand grab! : {_secondSelectHandTransform.name} ({args.interactorObject.transform.gameObject.name})");
        }
    }

    public void Release(SelectExitEventArgs args)
    {
        // ���� Interactor �ߺ� ����
        if (args.interactorObject.transform.parent != _firstSelectHandTransform &&
            args.interactorObject.transform.parent != _secondSelectHandTransform)
        {
            return;
        }

        // ��ư�� Release�� Hand�� ��� Hand���� üũ
        if (args.interactorObject.transform.parent == _firstSelectHandTransform)
        {
            //Debug.Log($"first hand release! : {_firstSelectHandTransform.name} ({args.interactorObject.transform.gameObject.name})");
            _firstSelectHandTransform = null;
        }
        else
        {
            //Debug.Log($"second hand release! : {_secondSelectHandTransform.name} ({args.interactorObject.transform.gameObject.name})");
            _secondSelectHandTransform = null;

            // ���� �׼� ����϶� second hand�� ���� trigger�� �����ʰ� ���� ������ pump action��尡 �������� �ʴ� ���� ����
            _isActivatedPumpAction = false;
        }
    }

    public void TryReload(SelectEnterEventArgs args)
    {
        Ammo ammo = args.interactableObject.transform.gameObject.GetComponent<Ammo>();
        if (ammo == null)
        {
            Debug.LogWarning("can't reload : Invalid");
            _interactionManager.SelectCancel(args.interactorObject, args.interactableObject);
            return;
        }

        _shooter.Reload(ammo);
    }

    public void Activate(ActivateEventArgs args)
    {
        if (!IsReadyToUse)
            return;

        if (args.interactorObject.transform.parent == _firstSelectHandTransform)
        {
            _shooter.Shoot();
        }
        else
        {
            _isActivatedPumpAction = true;
        }
    }

    public void DeActivate(DeactivateEventArgs args)
    {
        if (args.interactorObject.transform.parent == _secondSelectHandTransform)
        {
            _isActivatedPumpAction = false;
        }
    }

    void SetFirstHand(IXRSelectInteractor firstInteractor)
    {
        _firstSelectHandTransform = firstInteractor.transform.parent;
    }

    void SetSecondHand(IXRSelectInteractor secondInteractor)
    {
        _secondSelectHandTransform = secondInteractor.transform.parent;
    }
}
