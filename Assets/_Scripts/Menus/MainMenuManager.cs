using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
   public void StartGame()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
