using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] Skills skills;
    public bool canMove = true;
    public bool isInvincible = false;   

    public float basicAtackDamage = 10f;
    public float basicAtackSpeed = 1f;



    public float maxHealth = 100f;  


    public bool usingController = false;


    public Action skill1;
    public float skill1Cooldown;


    public Action skill2;
    public float skill2Cooldown;



}
