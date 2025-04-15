using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip hoverSound;
    private AudioSource audioSource;

    void Start()
    {
        // Pode ser o AudioSource do próprio objeto, ou um central
        audioSource = GameObject.Find("MenuController").GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource && hoverSound)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}
