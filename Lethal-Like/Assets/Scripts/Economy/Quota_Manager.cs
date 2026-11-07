using TMPro;
using UnityEngine;

public class Quota_Manager : MonoBehaviour
{
    public static Quota_Manager instance;

    public int quota = 500;
    public int collectedMoney = 0;

    public TextMeshProUGUI quotaText;
    public TextMeshProUGUI progressText;

    public bool quotaCompleted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateQuotaUI();
    }

    public void RegisterSale(int amount)
    {
        collectedMoney += amount;

        if (collectedMoney >= quota)
        {
            quotaCompleted = true;
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
        int remaining = quota - collectedMoney;

        if (remaining < 0)
        {
            remaining = 0;
        }

        return remaining;
    }

    public void UpdateQuotaUI()
    {
        if (quotaText != null)
        {
            quotaText.text = "  $" + quota.ToString();
        }

        if (progressText != null)
        {
            progressText.text = "$" + collectedMoney.ToString()
                + " / $" + quota.ToString();
        }
    }
}
