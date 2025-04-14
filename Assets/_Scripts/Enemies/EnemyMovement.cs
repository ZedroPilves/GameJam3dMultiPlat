using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] EnemyStats stats;
    [SerializeField] NavMeshAgent agent;
    
    void Start()
    {
        stats = GetComponent<EnemyStats>();     
        agent = GetComponent<NavMeshAgent>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
