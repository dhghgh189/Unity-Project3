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

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _interactable = GetComponent<ShotgunInteractable>();

        _waitTimeForSync = new WaitForSeconds(waitTimeForSync);

        _magazine = new Queue<Ammo>(maxAmmo);
    }

    public void Shoot()
    {
        // ��� grab ���� Ȯ��
        if (!_interactable.IsReadyToUse)
            return;

        // ���� ź �� Ȯ��
        if (_remainAmmo <= 0)
            return;

        // ź�� ���� ���� Ȯ��
        if (_magazine.Peek().IsUsed)
            return;

        // ���� �߻� ����
        if (_fireRoutine != null)
            return;

        _fireRoutine = StartCoroutine(FireRoutine());
    }

    public void Reload(SelectEnterEventArgs args)
    {
        Ammo ammo = args.interactableObject.transform.gameObject.GetComponent<Ammo>();
        if (ammo == null)
        {
            Debug.LogError("Reload Error !");
            return;
        }

        _magazine.Enqueue(ammo);
        ammo.gameObject.SetActive(false);

        Debug.Log("Reload!");

        if (_magazine.Count >= maxAmmo)
        {
            // �ִ� ��ź����ŭ ���� �Ŀ��� ���� �Ұ�
            reloadPoint.gameObject.SetActive(false);
        }
    }

    IEnumerator FireRoutine()
    {
        _audioSource.PlayOneShot(shotClip);
        yield return _waitTimeForSync;

        Ammo currentAmmo = _magazine.Peek();
        currentAmmo.Use(muzzlePoint, shotPower);

        Debug.Log("Shot!");

        _fireRoutine = null;
    }
}
