using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject targetPrefab;
    [SerializeField] AudioClip buzzerClip;
    [SerializeField] AudioClip launchClip;
    [SerializeField] float waitTime;
    [SerializeField] float launchPower;

    // temp
    AudioSource _audioSource;

    WaitForSeconds _waitTime;

    bool _canSpawn;

    private void Awake()
    {
        _waitTime = new WaitForSeconds(waitTime);

        _canSpawn = true;

        // temp
        _audioSource = GetComponent<AudioSource>();
    }

    public void SpawnTarget()
    {
        if (_canSpawn)
        {
            _canSpawn = false;
            StartCoroutine(SpawnRoutine());
        }
    }

    IEnumerator SpawnRoutine()
    {
        _audioSource.PlayOneShot(buzzerClip);
        // �÷��̾ ����� �غ��� �� �ֵ��� ��� ���
        yield return _waitTime;

        // Ǯ�� �ʿ�?
        GameObject target = Instantiate(targetPrefab, transform.position, transform.rotation);
        target.GetComponent<Rigidbody>().AddForce(transform.forward * launchPower, ForceMode.Impulse);
        _audioSource.PlayOneShot(launchClip);

        _canSpawn = true;
    }
}
