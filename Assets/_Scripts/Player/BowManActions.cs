using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowManActions : MonoBehaviour
{
    [Header("Basic References")]
    private PlayerInput playerInputs;
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] PlayerUIManager playerUIManager;
    [SerializeField] PlayerAnimation playerAnimation;
    [Header("Basic Atack")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject fireArrowPrefab;
    [SerializeField] Transform shootPos;
    [SerializeField] bool teste;
    [SerializeField] float basicAtackCooldown;
    private bool basickAtackisCooldown;


    [Header("Secondary Atack")]
    [SerializeField] float secondaryAtackCooldown;
    [SerializeField] GameObject secondaryAtackHitbox;
    private bool secondaryAtackisCooldown;


    [Header("Skill 1")]
    [SerializeField] bool skill1Atcive;
    [SerializeField] float skill1Cooldown;
    [SerializeField] float skill1Duration;
    [SerializeField] bool skill1IsCooldown;

  


    void Start()
    {
        playerInputs = GetComponent<PlayerInput>();
        playerAnimation = GetComponent<PlayerAnimation>();  
        playerStatus = GetComponent<PlayerStatus>();
        playerUIManager = GetComponent<PlayerUIManager>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInputs.actions["BasicAtack"].IsPressed() && !basickAtackisCooldown && playerStatus.canMove && !skill1Atcive)
        {
            StartCoroutine((BasickAtack(arrowPrefab)));
        }

        else if (playerInputs.actions["BasicAtack"].IsPressed() && !basickAtackisCooldown && playerStatus.canMove && skill1Atcive)
        {
            StartCoroutine((BasickAtack(fireArrowPrefab)));
        }

        if(playerInputs.actions["SecondaryAtack"].triggered && !secondaryAtackisCooldown && playerStatus.canMove)
        {
            StartCoroutine(SecondaryAtack());
        }





        if (playerInputs.actions["Skill1"].triggered && !skill1IsCooldown && playerStatus.canMove)
        {
            StartCoroutine(Skill1(skill1Duration));

        }

        IEnumerator BasickAtack(GameObject obj)
        {
            basickAtackisCooldown = true;
            playerUIManager.StartSkillCooldownFade(playerUIManager.cooldownImageBasickAtack, basicAtackCooldown);
            playerAnimation.animator.SetTrigger("shoot");
            GameObject projectile = obj;
           GameObject arrowinstance = Instantiate(projectile, shootPos.position, shootPos.rotation);

            Rigidbody projRb = arrowinstance.GetComponent<Rigidbody>();
         
            projRb.AddForce(arrowinstance.transform.right * 50, ForceMode.Impulse);



            yield return new WaitForSeconds(basicAtackCooldown);
            basickAtackisCooldown = false;
        }


        IEnumerator SecondaryAtack()
        {
            secondaryAtackisCooldown = true;
            playerUIManager.StartSkillCooldownFade(playerUIManager.cooldownImageSecondAtack, secondaryAtackCooldown);
            playerAnimation.animator.SetTrigger("Punch");
            yield return new WaitForSeconds(0.2f);
            secondaryAtackHitbox.SetActive(true);
            yield return new WaitForSeconds(0.2f);

            secondaryAtackHitbox.SetActive(false);
            yield return new WaitForSeconds(secondaryAtackCooldown);
            secondaryAtackisCooldown = false;

        }   









        IEnumerator Skill1(float buffDuration)
        {
            skill1IsCooldown = true;
            playerUIManager.StartSkillCooldownFade(playerUIManager.cooldownImageSkill, skill1Cooldown); 
            playerUIManager.BuffDuration(buffDuration);
            StartCoroutine(Skill1Duration(buffDuration));


            yield return new WaitForSeconds(skill1Cooldown);

            skill1IsCooldown = false;
        }

        IEnumerator Skill1Duration(float buffDuration)
        {
            skill1Atcive = true;
            float originalcd = basicAtackCooldown;  
            basicAtackCooldown = basicAtackCooldown / 2; 


            yield return new WaitForSeconds(buffDuration);
            skill1Atcive = false;
            basicAtackCooldown = originalcd;       
        }


    }
}
