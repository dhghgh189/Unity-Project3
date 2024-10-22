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

    WaitForSeconds _waitTime;

    bool _canSpawn;

    private void Awake()
    {
        _waitTime = new WaitForSeconds(waitTime);
        _canSpawn = true;
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
        SoundManager.Instance.Play(Enums.ESoundType.SFX, buzzerClip);
        // 플레이어가 사격을 준비할 수 있도록 잠시 대기
        yield return _waitTime;

        // 풀링 필요?
        GameObject target = Instantiate(targetPrefab, transform.position, transform.rotation);
        target.GetComponent<Rigidbody>().AddForce(transform.forward * launchPower, ForceMode.Impulse);
        SoundManager.Instance.Play(Enums.ESoundType.SFX, launchClip);

        _canSpawn = true;
    }
}
