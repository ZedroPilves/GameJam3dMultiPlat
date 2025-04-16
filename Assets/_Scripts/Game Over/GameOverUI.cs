using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void RetryGame()
    {
        SceneManager.LoadScene(2); // <- nome exato da sua cena de gameplay
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(1); // <- nome exato da sua cena de menu
    }
}
