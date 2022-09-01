using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetDifficulty(string dif)
    {
        PaddleAI.SetDifficulty(dif);
    }

    public void StartGame(string mode)
    {
        GameManager.mode = mode;
        SceneManager.LoadScene(1);
    }
}
