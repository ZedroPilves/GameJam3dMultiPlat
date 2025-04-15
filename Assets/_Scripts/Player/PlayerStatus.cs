using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] PlayerUIManager playerUIManager;




    public bool canMove = true;
    public bool isInvincible = false;   



    public float basicAtackDamage = 10f;
    public float basicAtackSpeed = 1f;


    public float health; 
    public float maxHealth = 100f;  


     
    public bool usingController = false;

    public bool isLocked = false;


    private void Start()
    {
        health = maxHealth;
        playerUIManager = GetComponent<PlayerUIManager>();
    }
    public void TakeDamage(float dmg)
    {
        health -= dmg;
       playerUIManager.DeplenishHealth();
         
    }

}
