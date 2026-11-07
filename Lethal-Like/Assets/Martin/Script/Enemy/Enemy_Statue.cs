using UnityEngine;

public class Enemy_Statue : Enemy_Chaser
{
    [Header("Visibility")]
    [SerializeField] private Camera playerCamera;

    private Collider enemyCollider;

    protected override bool CanAct()
    {
        return !IsBeingLookedAt();
    }

    private bool IsBeingLookedAt()
    {
        if (playerCamera == null) playerCamera = Camera.main;

        if (playerCamera == null) return false;

        if (enemyCollider == null) enemyCollider = GetComponent<Collider>();

        if (enemyCollider == null) return false;

        Vector3 target = enemyCollider.bounds.center;

        Vector3 viewport = playerCamera.WorldToViewportPoint(target);

        bool onScreen = viewport.z > 0f && viewport.x >= 0f && viewport.x <= 1f && viewport.y >= 0f && viewport.y <= 1f;

        if (!onScreen) return false;

        if (IsObstructed(playerCamera.transform.position, target)) return false;

        return true;
    }
}
