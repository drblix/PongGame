using UnityEngine;

public class PaddleAI : MonoBehaviour
{
    #region Constants

    private const float MAX_HEIGHT = 13f;
    private const float PADDLE_SPEED = 12f;


    #endregion

    #region Variables

    public static float difficulty = 0.75f; // 0.5f = baby; 0.75f = easy; 1f = normal; 1.5f = hard;
    public bool scored = false;

    private Transform ball;


    #endregion

    private void Awake()
    {
        ball = FindObjectOfType<Ball>().transform;
    }

    private void Update()
    {
        if (!scored)
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
}
