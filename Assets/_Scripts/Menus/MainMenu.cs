using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(2);
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
