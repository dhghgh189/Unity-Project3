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

    // 각 라운드 별 사격가능한 횟수
    [SerializeField] int shootableCount;

    public int MaxRound { get { return maxRound; } }

    // 현재 라운드 수
    int _currentRound;
    public int CurrentRound { get { return _currentRound; } private set { _currentRound = value; OnChangedRound?.Invoke(_currentRound, maxRound); } }

    // 전체 round가 진행되는 동안 target을 맞춘 횟수
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

    // 참조
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
        // 필요 변수 초기화
        CurrentRound = 1;
        HitCount = 0;

        _curState = EState.Idle;
    }

    public void StartRound()
    {
        // round 시작 조건 확인 (장전 상태)
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
