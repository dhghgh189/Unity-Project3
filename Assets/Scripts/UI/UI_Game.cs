using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Game : MonoBehaviour
{
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject resultPanel;

    [SerializeField] TextMeshProUGUI txtRound;
    [SerializeField] TextMeshProUGUI txtHitCount;
    [SerializeField] TextMeshProUGUI txtResult;

    void Awake()
    {
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    void Start()
    {
        GameManager.Instance.OnStartedRound += RoundStart;
        GameManager.Instance.OnFinishedRound += FinishedRound;
        GameManager.Instance.OnFinishedGame += FinishedGame;

        GameManager.Instance.OnChangedRound += UpdateRound;
        GameManager.Instance.OnChangedHitCount += UpdateHitCount;
    }

    public void GameStart()
    {
        startPanel.SetActive(false);
        resultPanel.SetActive(false);
        gamePanel.SetActive(true);

        GameManager.Instance.StartGame();
    }

    public void RoundStart()
    {
        gamePanel.SetActive(false);
    }

    public void FinishedRound()
    {
        gamePanel.SetActive(true);
    }

    public void FinishedGame()
    {
        txtResult.text = $"Hit Count : {GameManager.Instance.HitCount}";
        resultPanel.SetActive(true);
    }

    public void UpdateRound(int currentRound, int maxRound)
    {
        txtRound.text = $"{currentRound} / {maxRound}";
    }

    public void UpdateHitCount(int hitCount)
    {
        txtHitCount.text = $"Hit Count : {hitCount}";
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStartedRound -= RoundStart;
            GameManager.Instance.OnFinishedRound -= FinishedRound;
            GameManager.Instance.OnFinishedGame -= FinishedGame;

            GameManager.Instance.OnChangedRound -= UpdateRound;
            GameManager.Instance.OnChangedHitCount -= UpdateHitCount;
        }
    }
}
