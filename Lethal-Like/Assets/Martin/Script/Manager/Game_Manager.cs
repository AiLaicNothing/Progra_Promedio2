using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance;

    [Header("UI")]
    [SerializeField] private GameObject victoryPanel;

    private Player_Health playerHealth;
    private Quota_Manager quotaManager;

    private bool gameFinished;

    public bool IsGameOver => gameFinished;

    public event Action OnGameWon;
    public event Action OnGameLost;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        gameFinished = false;

        if (victoryPanel != null) victoryPanel.SetActive(false);

        quotaManager = Quota_Manager.instance;

        if (quotaManager != null)
        {
            quotaManager.OnQuotaCompleted += OnVictory;
        }

        playerHealth = FindFirstObjectByType<Player_Health>();

        if (playerHealth != null)
        {
            playerHealth.OnDied += OnDefeat;
        }

        if (quotaManager != null && quotaManager.quotaCompleted)
        {
            OnVictory();
        }
    }

    public void OnVictory()
    {
        if (gameFinished) return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameFinished = true;

        Debug.Log("Victory!");

        OnGameWon?.Invoke();

        if (victoryPanel != null) victoryPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void OnDefeat()
    {
        if (gameFinished) return;

        gameFinished = true;

        Debug.Log("Defeat!");

        OnGameLost?.Invoke();

        Time.timeScale = 1f;

        SceneManager.LoadScene("SampleScene");
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("SampleScene");
    }

    private void OnDestroy()
    {
        if (quotaManager != null) quotaManager.OnQuotaCompleted -= OnVictory;

        if (playerHealth != null) playerHealth.OnDied -= OnDefeat;

        if (Instance == this) Instance = null;
    }
}
