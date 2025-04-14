using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] Transform[] waypoints;
    [SerializeField] List<Transform> patrolPoints;
    public bool reachedPoint;
    [SerializeField] int currentPoint = 0;

    private void Start()
    {
        GenerateRandomPatrol(); 
    }
    void GenerateRandomPatrol()
    {
        patrolPoints.Clear();
        for (int i = 0; i < waypoints.Length; i++)
        {
            if(Random.Range(1, 4) == 1)
            {
                patrolPoints.Add(waypoints[i]);
            }
        }
       enemy.target = patrolPoints[currentPoint];

    }
    public void ChangeTarget()
    {

        if (currentPoint >= patrolPoints.Count - 1)
        {
            currentPoint = 0;
            GenerateRandomPatrol();

            enemy.target = patrolPoints[currentPoint];
        }
        else {
            currentPoint++;
            enemy.target = patrolPoints[currentPoint];
        }
    }
   
}
