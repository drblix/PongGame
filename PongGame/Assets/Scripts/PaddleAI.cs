using System.Collections;
using UnityEngine;

public class PaddleAI : MonoBehaviour
{
    #region Constants

    private const float MAX_HEIGHT = 13f;
    private const float PADDLE_SPEED = 12f;

    #endregion

    #region Variables

    private static float difficulty = 1.25f; // 0.5f = baby; 0.75f = easy; 1f = normal; 1.25f = hard;
    [HideInInspector]
    public bool scored = false;

    private Transform ball;

    [SerializeField]
    private bool shouldWait = true;
    private bool pause = false;

    #endregion

    private void Awake()
    {
        ball = FindObjectOfType<Ball>().transform;
    }

    private void Update()
    {
        if (!scored && !pause)
        {
            AIMovement();
        }
    }

    private void AIMovement()
    {
        float ballY = ball.position.y;
        ballY = Mathf.Clamp(ballY, -MAX_HEIGHT, MAX_HEIGHT);

        if (transform.position.y > ballY)
        {
            transform.Translate(PADDLE_SPEED * Time.deltaTime * (Vector3.down * difficulty));
        }
        else
        {
            transform.Translate(PADDLE_SPEED * Time.deltaTime * (Vector3.up * difficulty));
        }

        //transform.position = new(transform.position.x, ballY, 0f);
    }

    public void RoundReset()
    {
        ball = FindObjectOfType<Ball>().transform;
        scored = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ball") && shouldWait)
        {
            pause = true;
            StartCoroutine(PauseMovement());
        }
    }

    private IEnumerator PauseMovement()
    {
        yield return new WaitForSeconds(0.75f / difficulty);
        //yield return new WaitUntil(() => ball.position.x < (0f + difficulty * 2f));
        pause = false;
    }

    /// <summary>
    /// Set difficulty of AI
    /// </summary>
    /// <param name="request">Can be: "very easy", "easy", "normal", or "hard"</param>
    public static void SetDifficulty(string request)
    {
        difficulty = request switch
        {
            "very easy" => 0.5f,
            "easy" => 0.75f,
            "normal" => 1f,
            "hard" => 1.3f,
            _ => throw new System.Exception("Invalid difficulty input"),
        };
    }
    
}
