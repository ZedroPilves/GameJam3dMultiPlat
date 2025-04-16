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


    [Header("LookAt")]
    [SerializeField] private Animator animator;
    private GameObject pivot;
    private float lookWeight = 0f;




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
   

    [SerializeField] float chaseSpeed = 5f;

    [SerializeField] float attackDistance = 1.5f;

    private void Start()
    {

        animator = GetComponent<Animator>();

        pivot = new GameObject("EnemyLookPivot");
        pivot.transform.parent = transform;
        pivot.transform.localPosition = new Vector3(0, 1.5f, 0); // Adjust height to head level

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
                agent.speed = chaseSpeed; // Speed when chasing the player
                Debug.Log("Perseguindo o jogador");

                // Compare the distance between the closest player and this object's transform
                float distanceToClosestPlayer = Vector3.Distance(transform.position, closestPlayer.position);
                if(distanceToClosestPlayer < attackDistance)
                {
                    agent.speed = agent.speed * 0.80f; // Slow down when close to the player
                    enemyAnimation.StartCoroutine(enemyAnimation.Attack());
                }else if (distanceToClosestPlayer > attackDistance)
                {
                    agent.speed = chaseSpeed; // Reset speed when not close to the player
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
        if (target != null && enemyStats.chasing)
        {
            // Look at target while chasing
            pivot.transform.LookAt(target.position);
            lookWeight = Mathf.Lerp(lookWeight, 1f, 3f * Time.deltaTime);
        }
        else
        {
            // Fade out look weight
            lookWeight = Mathf.Lerp(lookWeight, 0f, 3f * Time.deltaTime);
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


    private void OnAnimatorIK(int layerIndex)
    {
        if (animator == null || target == null) return;

        if (layerIndex == 2) // Or change to the layer you use for IK
        {
            animator.SetLookAtWeight(lookWeight);
            animator.SetLookAtPosition(target.position + Vector3.up * 1.5f); // Adjust Y offset to match head height
        }
    }

}
