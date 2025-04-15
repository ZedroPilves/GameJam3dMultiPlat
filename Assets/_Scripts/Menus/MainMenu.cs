using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        Debug.Log("Abrindo Configurações");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Saindo...");
    }
}
