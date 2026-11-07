using UnityEngine;

public class Player_Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0) OnDead();
    }

    private void OnDead()
    {
        Debug.Log("Dead");
    }

}
