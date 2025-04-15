using System.Collections;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
   [SerializeField] Animator animator;
    [SerializeField] EnemyStats enemyStats;
    [SerializeField] EnemyMovement enemyMovement;
    [SerializeField] GameObject attackZone;

    [SerializeField] bool canAttack;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyStats = GetComponent<EnemyStats>();
        enemyMovement = GetComponent<EnemyMovement>();  
    }

    private void Update()
    {
        if (IsMoving())
        {
            if (!enemyStats.chasing) { 
           animator.SetInteger("Walk", 1);
            }
            else
            {
                animator.SetInteger("Walk", 2);
            }
              
        }
        else
        {
           animator.SetInteger("Walk", 0);  
        }
        
    }
    bool IsMoving()
    {
        return enemyMovement.agent.velocity.magnitude > 0.1f;
    }

    public IEnumerator Attack()
    {
        if (canAttack) { 
        canAttack = false;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.25f);
        attackZone.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        attackZone.SetActive(false);


        yield return new WaitForSeconds(2f);

        canAttack = true;
        }
    }


}
