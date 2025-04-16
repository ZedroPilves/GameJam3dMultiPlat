using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] public bool chasing;


    public void TakeDamage(float dmg)
    {
        health -= dmg;  
       if(health <= 0)
        {
            Destroy(gameObject);
        }       
    } 
}
