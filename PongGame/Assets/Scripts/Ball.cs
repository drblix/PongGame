using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    #region Constants

    private const float SERVE_TIME = 1f;

    #endregion

    #region Variables

    public bool rightServing = true;

    private Rigidbody rb;
    private GameManager gameManager;

    private float initialSpeed = 500f;

    [SerializeField]
    private AudioClip[] clips;

    private Vector3 lastVel;

    #endregion

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();

        StartCoroutine(ServeBall());
    }
    private void Update()
    {
        lastVel = rb.velocity;
    }

    private IEnumerator ServeBall()
    {
        yield return new WaitForSeconds(SERVE_TIME);

        float num;

        if (rightServing)
        {
            num = Random.Range(-50f, 50f);
        }
        else
        {
            num = Random.Range(180f, 230f);
        }

        transform.rotation = Quaternion.Euler(0f, 0f, num);
        rb.AddForce(transform.right * initialSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 colNormal = collision.GetContact(0).normal;
        float speed = lastVel.magnitude;
        Vector3 direction = Vector3.Reflect(lastVel.normalized, colNormal);
        rb.velocity = direction * speed;

        if (collision.collider.CompareTag("Paddle"))
        {
            rb.velocity *= 1.1f;
        }

        AudioSource.PlayClipAtPoint(clips[0], new Vector3(0f, 0f, -10f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Goal1")
        {
            AudioSource.PlayClipAtPoint(clips[1], new Vector3(0f, 0f, -10f));
            StartCoroutine(gameManager.TeamScore("Blue"));
        }
        else if (other.name == "Goal2")
        {
            AudioSource.PlayClipAtPoint(clips[1], new Vector3(0f, 0f, -10f));
            StartCoroutine(gameManager.TeamScore("Red"));
        }
    }
}
