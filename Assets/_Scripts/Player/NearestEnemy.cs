using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NearestEnemy : MonoBehaviour
{
    public Transform target;
    [SerializeField] GameObject[] enemies;
    [SerializeField] float distance;
    [SerializeField] float maxRange;



    void Start()
    {
              StartCoroutine(SearchForEnemies());
    }
    IEnumerator SearchForEnemies()
    {
        while (true)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
            {
                distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < maxRange)
                {
                
                    target = enemy.transform;
                }
            }
            yield return new WaitForSeconds(0.75f);
        }
    }
}
