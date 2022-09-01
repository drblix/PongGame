using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variables

    public static string mode = "AI";
    public static int winningScore = 5;

    private int redScore = 0;
    private int blueScore = 0;

    private PaddleAI paddleAI;

    [SerializeField]
    private GameObject ballFab;

    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private TextMeshProUGUI scoreNoti;

    [SerializeField]
    private AudioSource source;

    #endregion

    private void Awake()
    {
        paddleAI = FindObjectOfType<PaddleAI>();

        if (mode == "AI")
        {
            foreach (Paddle p in FindObjectsOfType<Paddle>())
            {
                if (p.TryGetComponent(out PaddleAI pAi))
                {
                    pAi.enabled = true;
                    p.GetComponent<Paddle>().enabled = false;
                }
            }
        }
        else
        {
            foreach (Paddle p in FindObjectsOfType<Paddle>())
            {
                p.enabled = true;

                if (p.TryGetComponent(out PaddleAI pAi))
                {
                    pAi.enabled = false;
                }
            }
        }
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

        if (redScore >= winningScore || blueScore >= winningScore)
        {
            FindObjectOfType<Ball>().transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            scoreNoti.SetText(name + " WINS!");

            if (name == "Red")
            {
                scoreNoti.color = Color.red;
            }
            else
            {
                scoreNoti.color = Color.blue;
            }

            for (int i = 0; i < 5; i++)
            {
                scoreNoti.gameObject.SetActive(!scoreNoti.gameObject.activeInHierarchy);
                source.Play();
                yield return new WaitForSeconds(1f);
            }

            SceneManager.LoadScene(0);
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
            newBall.GetComponent<Ball>().rightServing = false;
        }
        else
        {
            newBall.GetComponent<Ball>().rightServing = true;
        }

        paddleAI.RoundReset();
    }
}
