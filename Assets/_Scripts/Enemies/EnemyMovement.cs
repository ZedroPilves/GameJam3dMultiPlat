using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] EnemyStats enemyStats;


    [Header("Navigation")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform[] waypoints;


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
    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        enemyStats = GetComponent<EnemyStats>();
        agent = GetComponent<NavMeshAgent>();    
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
                enemyStats.chasing = true; // Set chasing to true when a player is close      
                Debug.Log("Perseguindo o jogador");
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
            // fallback to patrol behavior if no player is close
            if (target != null)
            {
                agent.SetDestination(target.position);
            }
        }

        // Patrolling behavior
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

            if (Random.Range(1, 4) == 1 && distanceFromPatrol <= maxDistFromPoint) // 1 in 3 chance
            {
                patrolPoints.Add(point);
            }
        }

        // Fallback: if none selected randomly, pick all
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

        foreach (GameObject player in players) // Fix: Changed Transform to GameObject
        {
            Transform playerTransform = player.transform; // Fix: Access the Transform component of the GameObject
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
