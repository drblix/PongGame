using System.Collections;
using UnityEngine;

public class MenuPaddle : MonoBehaviour
{
    #region Constants

    private const float PADDLE_SPEED = 8f;

    #endregion

    #region Variables

    private Transform ball;
    private bool pause = false;

    #endregion

    // predict future y-value formula:
    // let x = ball's current angle of approach; let y = ball's distance from paddle (x)
    // sin x * y / sin (180 - (x + 90))

    private void Awake()
    {
        ball = FindObjectOfType<MenuBall>().transform;
    }

    private void Update()
    {
        if (!pause)
        {
            AIMovement();
        }
    }

    private void AIMovement()
    {
        float ballDistanceX = transform.position.x - ball.position.x;
        float ballAngle = ball.eulerAngles.x;
        float futureY = Mathf.Sin(ballAngle) * ballDistanceX / Mathf.Sin(180f - (ballAngle + 90));
        Debug.Log(futureY);

        float ballY = ball.position.y;
        ballY = Mathf.Clamp(ballY, -3f, 5f);

        if (transform.position.y > ballY)
        {
            transform.Translate(PADDLE_SPEED * Time.deltaTime * Vector3.down);
        }
        else
        {
            transform.Translate(PADDLE_SPEED * Time.deltaTime * Vector3.up);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Ball")
        {
            StartCoroutine(Pause());
        }
    }

    private IEnumerator Pause()
    {
        pause = true;
        yield return new WaitForSeconds(.5f);
        pause = false;
    }
}
