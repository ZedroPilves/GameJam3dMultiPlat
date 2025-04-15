using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] EnemyStats enemyStats;

    [Header("Navigation")]
    public NavMeshAgent agent;
    [SerializeField] Transform[] waypoints;
    [SerializeField] EnemyAnimation enemyAnimation;

    [Header("Patrolling")]
    [SerializeField] float waypointThreshold = 0.2f;
    [SerializeField] List<Transform> patrolPoints = new List<Transform>();
    [SerializeField] int currentPoint = 0;
    private float distanceFromPatrol;
    [SerializeField] float maxDistFromPoint;

    [Header("Target")]
    public Transform target;
    [SerializeField] GameObject[] players;

    [Header("Chase")]
    [SerializeField] float chaseDistance = 10f;
    [SerializeField] Transform chaseTarget;

    [SerializeField] float chaseSpeed = 0f;

    [SerializeField] float attackDistance = 1.5f;

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        enemyStats = GetComponent<EnemyStats>();
        enemyAnimation = GetComponent<EnemyAnimation>();    
        agent = GetComponent<NavMeshAgent>();

        // Fill waypoints with all objects tagged "NavMeshPoint"
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("NavMeshPoint");
        waypoints = new Transform[waypointObjects.Length];
        for (int i = 0; i < waypointObjects.Length; i++)
        {
            waypoints[i] = waypointObjects[i].transform;
        }

        GenerateRandomPatrol();
        MoveToCurrentPatrolPoint();
    }

    private void Update()
    {
        Transform closestPlayer = GetClosestPlayerInRange();

        if (closestPlayer != null)
        {
            distanceFromPatrol = Vector3.Distance(transform.position, closestPlayer.position);

            if (distanceFromPatrol <= chaseDistance)
            {
                target = closestPlayer;
                agent.SetDestination(target.position);
                enemyStats.chasing = true;
                agent.speed = 5f; // Speed when chasing the player
                Debug.Log("Perseguindo o jogador");

                // Compare the distance between the closest player and this object's transform
                float distanceToClosestPlayer = Vector3.Distance(transform.position, closestPlayer.position);
                if(distanceToClosestPlayer < attackDistance)
                {
                    enemyAnimation.StartCoroutine(enemyAnimation.Attack()); 
                }
            }
            else
            {
                if (target != null)
                {
                    agent.SetDestination(target.position);
                }
            }
        }
        else
        {
            if (target != null)
            {
                agent.SetDestination(target.position);
            }
        }

        if (agent.remainingDistance <= waypointThreshold && !agent.pathPending)
        {
            if (Vector3.Distance(transform.position, target.position) < waypointThreshold)
            {
                Debug.Log("Chegou no ponto final");
                ChangeTarget();
            }
            else
            {
                ChangeTarget();
            }
        }
    }

    void GenerateRandomPatrol()
    {
        patrolPoints.Clear();

        foreach (Transform point in waypoints)
        {
            distanceFromPatrol = Vector3.Distance(transform.position, point.position);

            if (Random.Range(1, 4) == 1 && distanceFromPatrol <= maxDistFromPoint)
            {
                patrolPoints.Add(point);
            }
        }

        if (patrolPoints.Count == 0)
        {
            patrolPoints.AddRange(waypoints);
        }

        currentPoint = 0;
        target = patrolPoints[currentPoint];
    }

    Transform GetClosestPlayerInRange()
    {
        Transform closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            Transform playerTransform = player.transform;
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance < shortestDistance && distance <= chaseDistance)
            {
                shortestDistance = distance;
                closest = playerTransform;
            }
        }

        return closest;
    }

    void ChangeTarget()
    {
        if (patrolPoints.Count == 0) return;

        if (currentPoint >= patrolPoints.Count - 1)
        {
            GenerateRandomPatrol();
        }
        else
        {
            currentPoint++;
        }

        target = patrolPoints[currentPoint];
        MoveToCurrentPatrolPoint();
    }

    void MoveToCurrentPatrolPoint()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}
