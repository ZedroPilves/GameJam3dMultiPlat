using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] public bool chasing;


    public void TakeDamage(float dmg)
    {
        health -= dmg;  
        print("Enemy took " + dmg + " damage. Health left: " + health);
        if(health <= 0)
        {
            Destroy(gameObject);
        }   
    } 
}
