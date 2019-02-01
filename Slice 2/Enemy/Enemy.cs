using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour {

    [SerializeField]
    protected PlayerDeath player;

    public float maxHealth;
    protected float health;

    [SerializeField]
    private float attackCooldownInSeconds;

    private bool isAttackOnCooldown;

    [SerializeField]
    protected float attackRange;
    protected float walkingDistance;

    [SerializeField]
    protected float moveSpeed;

    protected EnemyState state;

    private Animator animator;

    [SerializeField]
    protected float baseDamage;

    public event Action<float, float> enemyDamageAction;

    private List<Node> path;

    private GridManager gridManager;

    private SpawnManager spawnManager;

    private IEnumerator followEnumerator;

    public abstract void Attack();

    private void SetState(EnemyState enemyState)
    {
        state = enemyState;

        //TODO EDIT ANIMATION STATE VARIABLE
    }

    void Start()
    {
        //  animator = GetComponent<Animator>();

        health = maxHealth;
        player = FindObjectOfType<PlayerDeath>();

        state = EnemyState.IDLE;

        gridManager = FindObjectOfType<GridManager>();

        spawnManager = FindObjectOfType<SpawnManager>();

        animator = GetComponent<Animator>();
    }

    protected void Move(Vector2 position)
    {
        state = EnemyState.MOVE;
        // transform.position = Vector2.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
        
        Vector2 tilePos = AStarPath.GetTilePosition(transform.position);
        Vector2 playerTilePos = AStarPath.GetTilePosition(player.transform.position);
        path = AStarPath.FindPath(tilePos, playerTilePos);

        if (path != null)
        {
            path.Reverse();

            List<Vector2> pathWorldPosition = new List<Vector2>();
            for (int i = 0; i < path.Count; i++) {
                pathWorldPosition.Add(AStarPath.GetTileWorldPosition(path[i].position));
            }

            if (followEnumerator != null)
            {
                StopCoroutine(followEnumerator);
            }

            followEnumerator = FollowPath(pathWorldPosition, player.transform.position);

            StartCoroutine(followEnumerator);
        }
    }

    IEnumerator FollowPath(List<Vector2> _path, Vector3 playerPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position = _path[0], moveSpeed * Time.deltaTime);

        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = _path[targetWaypointIndex];

        while ((Vector2)transform.position != _path[_path.Count - 1]) {
            if (playerPosition != player.transform.position)
            {
                yield return null;
            }
            
           // Debug.Log(transform.position.x - targetWaypoint.x);
           
            transform.position = Vector2.MoveTowards(transform.position, targetWaypoint, moveSpeed * Time.deltaTime);
            if (transform.position == targetWaypoint) {
                if (targetWaypointIndex < _path.Count - 1) {
                    targetWaypointIndex++;
                    targetWaypoint = _path[targetWaypointIndex];
                }
            }
            yield return null;
        }
    }

    protected bool IsPlayerClose()
    {
        if (player != null)
        {
            bool isClose = Vector2.Distance(player.transform.position, gameObject.transform.position) < attackRange;

            if (isClose)
            {
                if (followEnumerator != null)
                {
                    StopCoroutine(followEnumerator);
                    followEnumerator = null;
                }
            }

            return isClose;
        }

        return false;
    }
    
    protected bool CanAttack()
    {
        if (!isAttackOnCooldown)
        {
            StartCoroutine(AttackCooldown());

            // We gaan ervan uit dat wanneer de enemy kan attacken dat hij dat ook gaat doen,
            // zorg er daarom voor dat je eerst checked of de enemy dichtbij genoeg is en daarna pas checked of hij kan attacken.

            state = EnemyState.ATTACK;

            return true;
        }

        return false;
    }

    private IEnumerator AttackCooldown()
    {
        isAttackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldownInSeconds);
        isAttackOnCooldown = false;
    }

    public void Damage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;

            spawnManager.EnemyDeath();

            Destroy(gameObject);
        }

        if (enemyDamageAction != null)
            enemyDamageAction(health, damage);


    }

    public enum EnemyState
    {
        MOVE, ATTACK, IDLE, DEATH
    }

    private void OnDrawGizmos()
    {
        if (path != null) {
            Gizmos.color = Color.blue;
            foreach (Node n in path) {
                Gizmos.DrawSphere(new Vector2(n.position.x - 11.5f, n.position.y - 5.5f), 0.1f);
            }
        }
    }

    public enum Direction
    {
        UP, DOWN, LEFT, RIGHT
    }
}