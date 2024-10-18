using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShotgunInteractable : MonoBehaviour
{
    [SerializeField] Transform barrelGuardTransform;
    [SerializeField] float barrelGuardZPosLimit;

    Vector3 _startBarrelGuardPos;

    Transform _firstSelectHandTransform;
    Transform _secondSelectHandTransform;

    IXRSelectInteractable _interactable;
    XRInteractionManager _interactionManager;

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
        _startBarrelGuardPos = barrelGuardTransform.position;
    }

    void Update()
    {
        if (!IsReadyToUse)
            return;
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
            Debug.Log($"first hand grab! : {_firstSelectHandTransform.name} ({args.interactorObject.transform.gameObject.name})");
        }
        else
        {
            SetSecondHand(args.interactorObject);
            Debug.Log($"second hand grab! : {_secondSelectHandTransform.name} ({args.interactorObject.transform.gameObject.name})");
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
            Debug.Log($"first hand release! : {_firstSelectHandTransform.name} ({args.interactorObject.transform.gameObject.name})");
            _firstSelectHandTransform = null;
        }
        else
        {
            Debug.Log($"second hand release! : {_secondSelectHandTransform.name} ({args.interactorObject.transform.gameObject.name})");
            _secondSelectHandTransform = null;
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
