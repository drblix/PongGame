using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    #region Constants

    private const float MOVE_TIME = 1f;
    private const int MIN_TO = 1;
    private const int MAX_TO = 9;

    #endregion

    #region Variables

    private RectTransform main;
    private RectTransform aiPage;

    [SerializeField]
    private TextMeshProUGUI toScore;

    private int chosenScore = 3;

    #endregion

    private void Awake()
    {
        main = transform.Find("Main").GetComponent<RectTransform>();
        aiPage = transform.Find("AIPage").GetComponent<RectTransform>();
        PaddleAI.SetDifficulty("normal");
        toScore.SetText(chosenScore.ToString());
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

    public void ModifyScore(bool raise)
    {
        if (raise)
        {
            chosenScore++;
        }
        else
        {
            chosenScore--;
        }

        chosenScore = Mathf.RoundToInt(Mathf.Clamp(chosenScore, MIN_TO, MAX_TO));
        toScore.SetText(chosenScore.ToString());
        GameManager.winningScore = chosenScore;
    }
}
