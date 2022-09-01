using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
public class Paddle : MonoBehaviour
{
    #region Constants

    private const float MAX_HEIGHT = 13f;

    #endregion

    #region Variables

    private static readonly float paddleSpeed = 12f;
    public static float PaddleSpeed { get { return paddleSpeed; } }

    private PlayerInput pInput;

    [SerializeField]
    private bool isPlayerTwo = false;

    #endregion

    private void Awake()
    {
        pInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        Movement();

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void Movement()
    {
        float vertical;
        if (!isPlayerTwo)
        {
            vertical = pInput.actions["Vertical"].ReadValue<float>();
        }
        else
        {
            vertical = pInput.actions["Vertical2"].ReadValue<float>();
        }

        Vector3 movement = new(0f, vertical * paddleSpeed * Time.deltaTime, 0f);

        transform.Translate(movement);
        float newY = Mathf.Clamp(transform.position.y, -MAX_HEIGHT, MAX_HEIGHT);
        transform.position = new(transform.position.x, newY, transform.position.z);
    }
}
