using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Shooter : MonoBehaviour
{
    [SerializeField] Transform muzzlePoint;
    [SerializeField] float shotPower;
    [SerializeField] Transform reloadPoint;
    [SerializeField] Transform ejectPoint;
    [SerializeField] float ejectPower;

    [SerializeField] XRSocketInteractor socketInteractor;

    [Header("Gun Setting")]
    [SerializeField] int maxAmmo;
    [SerializeField] AudioClip shotClip;
    [SerializeField] AudioClip reloadClip;
    [SerializeField] AudioClip ejectClip;
    [SerializeField] AudioClip loadToChamberClip;
    [SerializeField] GameObject shotEffect;

    AudioSource _audioSource;
    ShotgunInteractable _interactable;

    Queue<Ammo> _magazine;
    int _remainAmmo => _magazine.Where(x => x.IsUsed == false).Count();

    [Header("For Sync")]
    [SerializeField] float waitTimeForSync;
    WaitForSeconds _waitTimeForSync;

    Coroutine _fireRoutine;

    // ���
    Ammo _chamber;

    XRInteractionManager _interactionManager;

    // ���� �� ������ Ƚ����ŭ�� �߻� ����
    int _shootableCount;

    public int MaxAmmo { get { return maxAmmo; } }
    public int WholeAmmo { get { return _chamber != null ? (_magazine.Count + 1) : (_magazine.Count); } }

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _interactable = GetComponent<ShotgunInteractable>();

        _waitTimeForSync = new WaitForSeconds(waitTimeForSync);

        _magazine = new Queue<Ammo>(maxAmmo);

        _interactionManager = FindAnyObjectByType<XRInteractionManager>();
    }

    void Start()
    {
        GameManager.Instance.SetShooter(this);
    }

    public void SetInfo(int shootableCount)
    {
        _shootableCount = shootableCount;
    }

    public void Shoot()
    {
        // �ش� ���忡�� �߻��� �� �ִ� Ƚ���� ���Ҵ��� Ȯ��
        if (_shootableCount <= 0)
            return;

        // ��� grab ���� Ȯ��
        if (!_interactable.IsReadyToUse)
            return;

        // ��� Ȯ��
        if (_chamber == null)
            return;

        // ź�� ���� ���� Ȯ��
        if (_chamber.IsUsed)
            return;

        // ���� �߻� ����
        if (_fireRoutine != null)
            return;

        _fireRoutine = StartCoroutine(FireRoutine());
    }

    public void Reload(Ammo ammo)
    {
        _audioSource.PlayOneShot(reloadClip);
        _magazine.Enqueue(ammo);
        ammo.gameObject.SetActive(false);

        //Debug.Log("Reload!");

        if (_magazine.Count >= maxAmmo)
        {
            // �ִ� ��ź����ŭ ���� �Ŀ��� ���� �Ұ�
            reloadPoint.gameObject.SetActive(false);
        }
    }

    public void Eject()
    {
        // ź�� ������ �ʾҾ eject ���� ������ ���� ��� �ʿ�
        if (_chamber != null && _chamber.IsUsed)
        {
            _audioSource.PlayOneShot(ejectClip);

            Ammo currentAmmo = _chamber;
            currentAmmo.GetComponent<XRGrabInteractable>().interactionLayers = 0;
            currentAmmo.gameObject.SetActive(true);
            currentAmmo.gameObject.transform.position = ejectPoint.position;
            currentAmmo.gameObject.transform.rotation = ejectPoint.rotation;
            currentAmmo.GetComponent<Rigidbody>().AddForce(ejectPoint.right * ejectPower, ForceMode.Impulse);

            // ��� ���
            _chamber = null;
        }
    }

    public void LoadAmmoToChamber()
    {
        if (_chamber != null)
            return;

        if (_magazine.Count > 0 && _magazine.Peek().IsUsed == false)
        {
            _audioSource.PlayOneShot(loadToChamberClip);

            _chamber = _magazine.Dequeue();
            //Debug.Log("Load Ammo To Chamber!");

            if (_magazine.Count < maxAmmo)
            {
                // źâ�� ��� ������ ����� �ٽ� ���� ����
                reloadPoint.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator FireRoutine()
    {
        _audioSource.PlayOneShot(shotClip);

        // Ǯ�� �ʿ�?
        GameObject effect = Instantiate(shotEffect, muzzlePoint);
        effect.transform.position = muzzlePoint.position;
        effect.transform.rotation = muzzlePoint.rotation;

        yield return _waitTimeForSync;

        Ammo currentAmmo = _chamber;
        currentAmmo.Use(muzzlePoint, shotPower);

        //Debug.Log("Shot!");

        _shootableCount--;

        _fireRoutine = null;
    }
}
