using System.Collections;
using UnityEngine;

public class PaddleAI : MonoBehaviour
{
    #region Constants

    private const float MAX_HEIGHT = 13f;
    private const float PADDLE_SPEED = 12f;
    private const float POS_OFFSET = 0.2f;

    #endregion

    #region Variables

    private static float difficulty = 1.25f; // 0.5f = baby; 0.75f = easy; 1f = normal; 1.25f = hard;
    
    [HideInInspector]
    public bool scored = false;

    private Ball ballScript;
    private Transform ball;

    [SerializeField]
    private bool shouldWait = true;
    private bool pause = false;

    #endregion

    // predict future y-value formula:
    // let x = ball's current angle of approach; let y = ball's distance from paddle (x)
    // sin x * y / sin (180 - (x + 90))

    private void Awake()
    {
        ballScript = FindObjectOfType<Ball>();
        ball = ballScript.transform;
    }

    private void Update()
    {
        Debug.Log(ballScript.approachingY);
        if (!scored && !pause)
        {
            AIMovement();
        }
    }

    private void AIMovement()
    {
        //float futureY = ballScript.approachingY;
        //futureY = Mathf.Clamp(futureY, -MAX_HEIGHT, MAX_HEIGHT);
        float ballY = ball.position.y;
        ballY = Mathf.Clamp(ballY, -MAX_HEIGHT, MAX_HEIGHT);

        if (transform.position.y > (ballY + POS_OFFSET))
        {
            transform.Translate(PADDLE_SPEED * Time.deltaTime * (Vector3.down * difficulty));
        }
        else if (transform.position.y < (ballY - POS_OFFSET))
        {
            transform.Translate(PADDLE_SPEED * Time.deltaTime * (Vector3.up * difficulty));
        }
    }

    public void RoundReset()
    {
        ballScript = FindObjectOfType<Ball>();
        ball = ballScript.transform;
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
    
    private float CalculateFutureY()
    {
        float distX = transform.position.x - ball.position.x;
        float angleApproach = ball.eulerAngles.x * Mathf.Deg2Rad;
        float thirdAngle = 180f - (distX + 90) * Mathf.Deg2Rad;
        float futureY = -(Mathf.Sin(angleApproach) * distX / Mathf.Sin(thirdAngle));
        return futureY;
    }
    
}
