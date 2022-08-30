using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Constants

    private const int WINNING_SCORE = 5;

    #endregion

    #region Variables

    private int redScore = 0;
    private int blueScore = 0;

    private PaddleAI paddleAI;

    [SerializeField]
    private GameObject ballFab;

    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private TextMeshProUGUI scoreNoti;

    #endregion

    private void Awake()
    {
        paddleAI = FindObjectOfType<PaddleAI>();
    }

    public IEnumerator TeamScore(string name)
    {
        if (name == "Red")
        {
            redScore++;
        }
        else if (name == "Blue")
        {
            blueScore++;
        }

        scoreNoti.SetText(name + " SCORED!");
        scoreNoti.gameObject.SetActive(true);
        score.SetText(redScore.ToString() + " | " + blueScore.ToString());

        if (blueScore > redScore)
        {
            score.color = Color.blue;
        }
        else if (blueScore < redScore)
        {
            score.color = Color.red;
        }
        else
        {
            score.color = Color.white;
        }

        paddleAI.scored = true;
        if (redScore >= WINNING_SCORE || blueScore >= WINNING_SCORE)
        {
            scoreNoti.SetText(name + " WINS!");
            yield return new WaitForSeconds(3f);
            scoreNoti.gameObject.SetActive(false);
            GameReset(name);
        }
        else
        {
            yield return new WaitForSeconds(2f);
            scoreNoti.gameObject.SetActive(false);
            ResetRound(name);
        }

    }

    private void ResetRound(string winner)
    {
        Destroy(FindObjectOfType<Ball>().gameObject);

        // resets paddle positions
        foreach (Paddle obj in FindObjectsOfType<Paddle>())
        {
            obj.transform.position = new Vector3(obj.transform.position.x, 0f, 0f);
        }

        GameObject newBall = Instantiate(ballFab);
        newBall.transform.position = Vector3.zero;

        if (winner == "Red")
        {
            newBall.GetComponent<Ball>().rightServing = true;
        }
        else
        {
            newBall.GetComponent<Ball>().rightServing = false;
        }

        paddleAI.RoundReset();
    }

    private void GameReset(string name)
    {
        redScore = 0;
        blueScore = 0;
        score.SetText(redScore.ToString() + " | " + blueScore.ToString());
        score.color = Color.white;

        ResetRound(name);
    }
}
