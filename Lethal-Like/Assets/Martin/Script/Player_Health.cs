using System;
using UnityEngine;

public class Player_Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private bool isDead;

    public event Action OnDied;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead || amount <= 0) return;

        currentHealth -= amount;

        if (currentHealth <= 0) OnDead();
    }

    private void OnDead()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("Player died");

        OnDied?.Invoke();
    }
}
