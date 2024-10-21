using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] Spawner _spawner;
    [SerializeField] int maxRound;

    // �� ���� �� ��ݰ����� Ƚ��
    [SerializeField] int shootableCount;

    public int MaxRound { get { return maxRound; } }

    // ���� ���� ��
    int _currentRound;
    public int CurrentRound { get { return _currentRound; } private set { _currentRound = value; OnChangedRound?.Invoke(_currentRound, maxRound); } }

    // ��ü round�� ����Ǵ� ���� target�� ���� Ƚ��
    int _hitCount;
    public int HitCount { get { return _hitCount; } private set { _hitCount = value; OnChangedHitCount?.Invoke(_hitCount); } }

    public UnityAction<int, int> OnChangedRound;
    public UnityAction<int> OnChangedHitCount;

    public UnityAction OnStartedRound;
    public UnityAction OnFinishedRound;
    public UnityAction OnFinishedGame;

    public enum EState { Idle, Process }

    EState _curState;
    public EState CurState { get { return _curState; } }

    // ����
    Shooter _shooter;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetShooter(Shooter shooter)
    {
        _shooter = shooter;
    }

    public void IncreaseHitCount()
    {
        HitCount++;
    }

    public void StartGame()
    {
        // �ʿ� ���� �ʱ�ȭ
        CurrentRound = 1;
        HitCount = 0;

        _curState = EState.Idle;
    }

    public void StartRound()
    {
        // round ���� ���� Ȯ�� (���� ����)
        if (_shooter.WholeAmmo < shootableCount)
        {
            Debug.Log("Please Check Ammo Before Start!");
            return;
        }

        _shooter.SetInfo(shootableCount);
        OnStartedRound?.Invoke();
        _curState = EState.Process;

        _spawner.SpawnTarget();
    }

    public void FinishRound()
    {
        if (CurrentRound >= maxRound)
        {
            OnFinishedGame?.Invoke();
        }
        else
        {
            CurrentRound++;
            OnFinishedRound?.Invoke();
        }

        _curState = EState.Idle;
    }
}
