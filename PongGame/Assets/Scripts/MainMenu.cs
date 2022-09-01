using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Constants

    private const float MOVE_TIME = 1f;

    #endregion

    #region Variables

    private RectTransform main;
    private RectTransform aiPage;

    #endregion

    private void Awake()
    {
        main = transform.Find("Main").GetComponent<RectTransform>();
        aiPage = transform.Find("AIPage").GetComponent<RectTransform>();
    }

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
   public void ToggleAIMenu(bool state)
    {
        if (LeanTween.isTweening(main) || LeanTween.isTweening(aiPage)) { return; }

        if (state)
        {
            main.LeanMoveX(-800f, MOVE_TIME);
            aiPage.LeanMoveX(0f, MOVE_TIME);
        }
        else
        {
            main.LeanMoveX(0f, MOVE_TIME);
            aiPage.LeanMoveX(800f, MOVE_TIME);
        }
    }
}
