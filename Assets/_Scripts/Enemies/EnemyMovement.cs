using System.Collections;
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
    [SerializeField] float maxDistFromPoint = 5f;

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
        pivot.transform.localPosition = new Vector3(0, 1.5f, 0);

        players = GameObject.FindGameObjectsWithTag("Player");
        enemyStats = GetComponent<EnemyStats>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        agent = GetComponent<NavMeshAgent>();

        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("NavMeshPoint");
        waypoints = new Transform[waypointObjects.Length];
        for (int i = 0; i < waypointObjects.Length; i++)
        {
            waypoints[i] = waypointObjects[i].transform;
        }

        ChooseNextWaypoint();
    }

    private void Update()
    {
        Transform closestPlayer = GetClosestPlayerInRange();

        if (closestPlayer != null)
        {
            float distToPlayer = Vector3.Distance(transform.position, closestPlayer.position);

            if (distToPlayer <= chaseDistance)
            {
                target = closestPlayer;
                agent.SetDestination(target.position);
                enemyStats.chasing = true;
                agent.speed = chaseSpeed;

                if (distToPlayer < attackDistance)
                {
                    agent.speed *= 0.8f;
                    enemyAnimation.StartCoroutine(enemyAnimation.Attack());
                }
                else
                {
                    agent.speed = chaseSpeed;
                }

                return; // Não patrulha enquanto persegue
            }
        }

        if (!enemyStats.chasing && agent.remainingDistance <= waypointThreshold && !agent.pathPending)
        {
            ChooseNextWaypoint();
        }

        if (target != null && enemyStats.chasing)
        {
            pivot.transform.LookAt(target.position);
            lookWeight = Mathf.Lerp(lookWeight, 1f, 3f * Time.deltaTime);
        }
        else
        {
            lookWeight = Mathf.Lerp(lookWeight, 0f, 3f * Time.deltaTime);
        }
    }

    Transform GetClosestPlayerInRange()
    {
        Transform closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < shortestDistance && distance <= chaseDistance)
            {
                shortestDistance = distance;
                closest = player.transform;
            }
        }

        return closest;
    }

    void ChooseNextWaypoint()
    {
        List<Transform> validWaypoints = new List<Transform>();
        Vector3 currentPosition = transform.position;

        foreach (Transform wp in waypoints)
        {
            float dist = Vector3.Distance(currentPosition, wp.position);
            if (dist <= maxDistFromPoint)
            {
                validWaypoints.Add(wp);
            }
        }

        if (validWaypoints.Count == 0)
        {
            Debug.LogWarning("Nenhum waypoint próximo! Usando todos.");
            validWaypoints.AddRange(waypoints);
        }

        Transform chosen = validWaypoints[Random.Range(0, validWaypoints.Count)];
        target = chosen;
        agent.SetDestination(target.position);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator == null || target == null) return;

        if (layerIndex == 2)
        {
            animator.SetLookAtWeight(lookWeight);
            animator.SetLookAtPosition(target.position + Vector3.up * 1.5f);
        }
    }
}
