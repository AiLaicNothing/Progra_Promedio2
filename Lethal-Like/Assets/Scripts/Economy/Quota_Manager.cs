using System;
using TMPro;
using UnityEngine;

public class Quota_Manager : MonoBehaviour
{
    public static Quota_Manager instance;

    [Header("Quota")]
    public int quota = 500;
    public int collectedMoney = 0;
    public bool quotaCompleted = false;

    [Header("UI")]
    public TextMeshProUGUI quotaText;
    public TextMeshProUGUI progressText;

    public event Action OnQuotaCompleted;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        UpdateQuotaUI();
    }

    public void RegisterSale(int amount)
    {
        if (quotaCompleted || amount <= 0) return;

        collectedMoney += amount;

        if (collectedMoney >= quota)
        {
            quotaCompleted = true;

            UpdateQuotaUI();

            OnQuotaCompleted?.Invoke();
            return;
        }

        UpdateQuotaUI();
    }

    public void SetNextQuota(int amount)
    {
        quota = amount;
        collectedMoney = 0;
        quotaCompleted = false;

        UpdateQuotaUI();
    }

    public int GetRemainingQuota()
    {
        return Mathf.Max(0, quota - collectedMoney);
    }

    public void UpdateQuotaUI()
    {
        if (quotaText != null)
        {
            quotaText.text = "$" + quota;
        }

        if (progressText != null)
        {
            progressText.text = "$" + collectedMoney + " / $" + quota;
        }
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}
