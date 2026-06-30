using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] protected SO_EnemyData enemyData;

    [Header("Combat")]
    [SerializeField] protected float contactDamage = 5f;
    [SerializeField] protected float attackCooldown = 1f;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected float knockbackForce = 4f;

    [Header("Movement")]
    [SerializeField] private float stopDistance = 1.8f;
    [SerializeField] private float pathUpdateInterval = 0.1f;


    protected Transform target;
    protected Health playerHealth;
    protected PlayerMovement playerMovement;

    protected NavMeshAgent agent;

    protected EnemySpawner spawner;

    protected float nextAttackTime;


    private float nextPathUpdate;
    private float sqrStopDistance;
    private float sqrAttackRange;

    private Vector3 lastTargetPosition;

    private static Transform playerReference;


    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (playerReference == null)
        {
            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                playerReference = player.transform;
        }

        sqrStopDistance =
            stopDistance * stopDistance;

        sqrAttackRange =
            attackRange * attackRange;
    }


    public virtual void Initialize(EnemySpawner owner)
    {
        spawner = owner;

        // Reset movement state
        agent.enabled = true;

        agent.ResetPath();

        agent.velocity = Vector3.zero;

        agent.isStopped = false;


        nextAttackTime = 0;
        nextPathUpdate = 0;


        target = playerReference;


        if (target != null)
        {
            playerHealth =
                target.GetComponent<Health>();

            playerMovement =
                target.GetComponent<PlayerMovement>();
        }


        lastTargetPosition =
            target.position;


        float baseSpeed = agent.speed;

        agent.speed =
            Random.Range(
                baseSpeed - 0.2f,
                baseSpeed + 0.2f
            );
    }


    protected virtual void Update()
    {
        if (target == null)
            return;

        HandleMovement();

        HandleAttack();
    }


    protected virtual void HandleMovement()
    {
        float distance =
            (transform.position - target.position)
            .sqrMagnitude;


        if (distance <= sqrStopDistance)
        {
            agent.isStopped = true;
            return;
        }


        agent.isStopped = false;


        bool playerMoved =
            (lastTargetPosition - target.position)
            .sqrMagnitude > 1f;


        if (Time.time < nextPathUpdate && !playerMoved)
            return;


        nextPathUpdate =
            Time.time + pathUpdateInterval;


        lastTargetPosition =
            target.position;


        agent.SetDestination(
            target.position
        );
    }


    protected virtual void HandleAttack()
    {
        float distance =
            (transform.position - target.position)
            .sqrMagnitude;


        if (distance > sqrAttackRange)
            return;


        if (Time.time < nextAttackTime)
            return;


        nextAttackTime =
            Time.time + attackCooldown;


        Attack();
    }


    protected virtual void Attack()
    {
        if (playerHealth != null)
            playerHealth.TakeDamage(contactDamage);


        if (playerMovement != null)
        {
            Vector3 dir =
                (target.position - transform.position)
                .normalized;


            playerMovement.AddKnockback(
                dir * knockbackForce
            );
        }
    }

    public virtual void ResetEnemy()
    {
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        agent.isStopped = false;
        
        nextPathUpdate = 0;
    }
    
    public virtual void Die()
    {
        if (spawner != null)
            spawner.OnEnemyKilled();
        
        EnemyPoolManager.Instance.ReturnEnemy(enemyData, gameObject);
    }
}