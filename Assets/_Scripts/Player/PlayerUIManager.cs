using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus; 
    [SerializeField] float healthValue;
    [SerializeField] Image healthBar;
    [SerializeField] float depletionSpeed = 0.5f; // Speed of health bar depletion

    [SerializeField] Image buffImage;
    [SerializeField] GameObject buffCounter;


    public void BuffDuration(float duration)
    {
        buffCounter.SetActive(true); // Show the buff counter   
        StartCoroutine(GradualBuffDepletion(duration));
    }

    IEnumerator GradualBuffDepletion(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            buffImage.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        buffImage.fillAmount = 0f; // Ensure the bar is empty at the end
        buffCounter.SetActive(false); // Hide the buff counter
    }
   



    public void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        healthBar.fillAmount = playerStatus.health / 100f; // Initialize health bar
    }   

    public void BuffDuration()
    {
        
    }







    public void DeplenishHealth()
    {
        playerStatus = GetComponent<PlayerStatus>();    
        healthValue = playerStatus.health;
        StartCoroutine(GradualHealthDepletion(healthValue / 100f));
    }

    IEnumerator GradualHealthDepletion(float targetFillAmount)
    {
        while (Mathf.Abs(healthBar.fillAmount - targetFillAmount) > 0.01f)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFillAmount, depletionSpeed * Time.deltaTime);
            yield return null;
        }
        healthBar.fillAmount = targetFillAmount; // Ensure exact value at the end
    }
}
