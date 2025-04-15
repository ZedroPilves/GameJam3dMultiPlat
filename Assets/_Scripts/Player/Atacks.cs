using UnityEngine;

public class Atacks : MonoBehaviour
{
    [SerializeField] float damage;





    private void OnCollisionEnter(Collision collision)
    {
        print("collidied");
        if(collision.gameObject.tag == "Enemy") // Assuming layer 6 is the enemy layer
        {
            Debug.Log("Collided with enemy");
            EnemyStats enemyStats = collision.gameObject.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(damage);
                print("Player dealt " + damage + " damage to the enemy.");
            }
            else { print("No EnemyStats component found on the collided object."); }
            Destroy(gameObject); // Destroy the arrow after collision   
        }


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) // Use CompareTag for better performance
        {
            Debug.Log("Collided with enemy");
            EnemyStats enemyStats = other.gameObject.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(damage);
                print("Player dealt " + damage + " damage to the enemy.");
            }
            else
            {
                print("No EnemyStats component found on the collided object.");
            }
        }

        if (other.gameObject.CompareTag("Player")) // Use CompareTag for better performance
        {
            Debug.Log("Collided with player");
            PlayerStatus playerStats = other.gameObject.GetComponent<PlayerStatus>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                print("Enemy dealt " + damage + " damage to the player.");
            }
            else
            {
                print("No PlayerStats component found on the collided object.");
            }
        }
    }
}
