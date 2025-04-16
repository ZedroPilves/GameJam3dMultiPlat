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
    public Image cooldownImageBasickAtack;
    public Image cooldownImageSecondAtack;
    public Image cooldownImageSkill;
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


    public void StartSkillCooldownFade(Image cooldownImage, float cooldownDuration)
    {
        StartCoroutine(FadeImageOverCooldown(cooldownImage, cooldownDuration));
    }

    private IEnumerator FadeImageOverCooldown(Image img, float duration)
    {
        Color color = img.color;
        color.a = 215f / 255f; // Start at alpha 215
        img.color = color;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(215f / 255f, 0f, elapsed / duration);
            color.a = newAlpha;
            img.color = color;

            yield return null;
        }

        // Ensure fully transparent at the end
        color.a = 0f;
        img.color = color;
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
