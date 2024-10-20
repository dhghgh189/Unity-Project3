using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Shooter : MonoBehaviour
{
    [SerializeField] AudioClip shotClip;
    [SerializeField] Transform muzzlePoint;
    [SerializeField] float shotPower;
    [SerializeField] Transform reloadPoint;
    [SerializeField] Transform ejectPoint;
    [SerializeField] float ejectPower;

    [SerializeField] XRSocketInteractor socketInteractor;

    [Header("Gun Setting")]
    [SerializeField] int maxAmmo;

    AudioSource _audioSource;
    ShotgunInteractable _interactable;

    Queue<Ammo> _magazine;
    int _remainAmmo => _magazine.Where(x => x.IsUsed == false).Count();

    [Header("For Sync")]
    [SerializeField] float waitTimeForSync;
    WaitForSeconds _waitTimeForSync;

    Coroutine _fireRoutine;

    // 약실
    Ammo _chamber;

    XRInteractionManager _interactionManager;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _interactable = GetComponent<ShotgunInteractable>();

        _waitTimeForSync = new WaitForSeconds(waitTimeForSync);

        _magazine = new Queue<Ammo>(maxAmmo);

        _interactionManager = FindAnyObjectByType<XRInteractionManager>();
    }

    public void Shoot()
    {
        // 양손 grab 여부 확인
        if (!_interactable.IsReadyToUse)
            return;

        // 약실 확인
        if (_chamber == null)
            return;

        // 탄피 배출 여부 확인
        if (_chamber.IsUsed)
            return;

        // 연속 발사 방지
        if (_fireRoutine != null)
            return;

        _fireRoutine = StartCoroutine(FireRoutine());
    }

    public void Reload(SelectEnterEventArgs args)
    {
        Ammo ammo = args.interactableObject.transform.gameObject.GetComponent<Ammo>();
        if (ammo == null)
        {
            Debug.LogWarning("can't reload : Invalid");
            _interactionManager.SelectCancel(args.interactorObject, args.interactableObject);
            return;
        }

        _magazine.Enqueue(ammo);
        ammo.gameObject.SetActive(false);

        Debug.Log("Reload!");

        if (_magazine.Count >= maxAmmo)
        {
            // 최대 장탄수만큼 장전 후에는 장전 불가
            reloadPoint.gameObject.SetActive(false);
        }
    }

    public void Eject()
    {
        // 탄이 사용되지 않았어도 eject 가능 한지에 대한 고려 필요
        if (_chamber != null && _chamber.IsUsed)
        {
            Ammo currentAmmo = _chamber;
            currentAmmo.GetComponent<XRGrabInteractable>().interactionLayers = 0;
            currentAmmo.gameObject.SetActive(true);
            currentAmmo.gameObject.transform.position = ejectPoint.position;
            currentAmmo.gameObject.transform.rotation = ejectPoint.rotation;
            currentAmmo.GetComponent<Rigidbody>().AddForce(ejectPoint.right * ejectPower, ForceMode.Impulse);

            // 약실 비움
            _chamber = null;
        }
    }

    public void LoadAmmoToChamber()
    {
        if (_chamber != null)
            return;

        if (_magazine.Count > 0 && _magazine.Peek().IsUsed == false)
        {
            _chamber = _magazine.Dequeue();
            Debug.Log("Load Ammo To Chamber!");

            if (_magazine.Count < maxAmmo)
            {
                // 탄창에 비는 공간이 생기면 다시 장전 가능
                reloadPoint.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator FireRoutine()
    {
        _audioSource.PlayOneShot(shotClip);
        yield return _waitTimeForSync;

        Ammo currentAmmo = _chamber;
        currentAmmo.Use(muzzlePoint, shotPower);

        Debug.Log("Shot!");

        _fireRoutine = null;
    }
}
