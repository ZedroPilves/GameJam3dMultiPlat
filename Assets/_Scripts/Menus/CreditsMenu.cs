using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    public GameObject creditsPanel;

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
}
