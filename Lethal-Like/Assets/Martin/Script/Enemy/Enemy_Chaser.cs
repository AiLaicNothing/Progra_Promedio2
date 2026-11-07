using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Chaser : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stopDistance = 1.2f;
    [SerializeField] private float pathUpdateInterval = 0.2f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.7f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Detection")]
    [SerializeField] private LayerMask obstacleLayers;

    protected NavMeshAgent agent;

    private Player_Health player;
    private float nextAttackTime;
    private float nextPathUpdateTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
        agent.stoppingDistance = stopDistance;
    }

    private void Start()
    {
        player = FindFirstObjectByType<Player_Health>();
    }

    private void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        bool canAct = CanAct();

        agent.isStopped = !canAct;
        agent.updateRotation = canAct;

        if (!canAct) return;

        if (Time.time >= nextPathUpdateTime)
        {
            agent.SetDestination(player.transform.position);

            nextPathUpdateTime = Time.time + pathUpdateInterval;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= attackRange && HasLineOfSight())
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (Time.time < nextAttackTime) return;

        player.TakeDamage(damage);

        nextAttackTime = Time.time + attackCooldown;

        Debug.Log("Enemy attacked player");
    }

    private bool HasLineOfSight()
    {
        Vector3 origin = transform.position + Vector3.up;
        Vector3 target = player.transform.position + Vector3.up;

        return !IsObstructed(origin, target);
    }

    protected bool IsObstructed(Vector3 start, Vector3 end)
    {
        return Physics.Linecast(start, end, obstacleLayers, QueryTriggerInteraction.Ignore);
    }

    protected virtual bool CanAct()
    {
        return true;
    }
}
