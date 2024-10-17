using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShotgunInteractable : MonoBehaviour
{
    [SerializeField] Transform barrelGuardTransform;
    [SerializeField] float barrelGuardZPosLimit;

    Vector3 _startBarrelGuardPos;

    IXRSelectInteractor _firstSelectInteractor;
    IXRSelectInteractor _secondSelectInteractor;

    Transform _firstSelectHandTransform;
    Transform _secondSelectHandTransform;

    public bool IsReadyToUse 
    { 
        get 
        {
            //return (_firstSelectInteractor != null) && (_secondSelectInteractor != null);
            return (_firstSelectHandTransform != null) && (_secondSelectHandTransform != null);
        } 
    }

    void Awake()
    {
        _startBarrelGuardPos = barrelGuardTransform.position;
    }

    void Update()
    {
        if (!IsReadyToUse)
            return;

        //barrelGuardTransform.position = new Vector3(
        //    barrelGuardTransform.position.x,
        //    barrelGuardTransform.position.y,
        //    _secondSelectHandTransform.position.z);
    }

    public void Grab(SelectEnterEventArgs args)
    {
        // 중복처리 방지
        if (args.interactorObject.transform.parent == _firstSelectHandTransform ||
            args.interactorObject.transform.parent == _secondSelectHandTransform)
            return;

        // 첫번째로 select한 손과 두번째로 select한 손의 Interactor를 저장
        //if (_firstSelectInteractor == null)
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
        // 중복처리 방지
        if (args.interactorObject.transform.parent != _firstSelectHandTransform &&
            args.interactorObject.transform.parent != _secondSelectHandTransform)
            return;

        // 버튼을 Release한 Hand가 어느 Hand인지 체크
        if (args.interactorObject.transform.parent == _firstSelectHandTransform)
        {
            Debug.Log($"first hand release! : {_firstSelectHandTransform.name} ({args.interactorObject.transform.gameObject.name})");
            _firstSelectInteractor = null;
            _firstSelectHandTransform = null;
        }
        else
        {
            Debug.Log($"second hand release! : {_secondSelectHandTransform.name} ({args.interactorObject.transform.gameObject.name})");
            _secondSelectInteractor = null;
            _secondSelectHandTransform = null;
        }
    }

    void SetFirstHand(IXRSelectInteractor firstInteractor)
    {
        _firstSelectInteractor = firstInteractor;
        _firstSelectInteractor.transform.GetComponent<XRBaseControllerInteractor>().selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.Sticky;
        _firstSelectHandTransform = firstInteractor.transform.parent;
    }

    void SetSecondHand(IXRSelectInteractor secondInteractor)
    {
        _secondSelectInteractor = secondInteractor;
        _secondSelectInteractor.transform.GetComponent<XRBaseControllerInteractor>().selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.StateChange;
        _secondSelectHandTransform = secondInteractor.transform.parent;
    }
}
