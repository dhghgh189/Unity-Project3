using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public void Hit()
    {
        GameManager.Instance.IncreaseHitCount();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.Instance.FinishRound();
    }
}
