using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Gun : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtChamber;
    [SerializeField] TextMeshProUGUI txtMagazine;

    private void Awake()
    {
        txtChamber.color = Color.red;
        txtMagazine.text = "X0";
    }

    public void OnEnable()
    {
        GameManager.Instance.Shooter.OnChangedMagazine += UpdateMagazine;
        GameManager.Instance.Shooter.OnChangedChamber += UpdateChamber;
    }

    public void UpdateMagazine(int count)
    {
        txtMagazine.color = count > 0 ? Color.green : Color.red;
        txtMagazine.text = $"X{count}";
    }

    public void UpdateChamber(bool bLoaded)
    {
        txtChamber.color = bLoaded ? Color.green : Color.red;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.Shooter != null)
            {
                GameManager.Instance.Shooter.OnChangedMagazine -= UpdateMagazine;
                GameManager.Instance.Shooter.OnChangedChamber -= UpdateChamber;
            }
        }
    }
}
