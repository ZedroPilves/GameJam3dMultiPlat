using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] float health;
   


    public void TakeDamage(float dmg)
    {
        health -= dmg;  
        print("Enemy took " + dmg + " damage. Health left: " + health);
    } 
}
