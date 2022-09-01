using System.Collections;
using UnityEngine;

public class MenuBall : MonoBehaviour
{
    #region Constants

    private const float SERVE_TIME = 1f;
    private const bool BOUNCE_INACCURACY = true;
    private const float INAC_OFFSET = 15f;

    #endregion

    #region Variables

    public bool rightServing = false;

    private Rigidbody rb;

    private readonly float initialSpeed = 500f;

    [SerializeField]
    private AudioClip[] clips;

    private Vector3 lastVel;
    private bool isServing = true;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(ServeBall());
        StartCoroutine(FailSafe());
    }
    private void Update()
    {
        lastVel = rb.velocity;
    }

    private IEnumerator ServeBall()
    {
        isServing = true;
        transform.position = Vector3.up * 2f;
        yield return new WaitForSeconds(SERVE_TIME);

        float num;

        if (rightServing)
        {
            num = Random.Range(-50f, 50f);
        }
        else
        {
            num = Random.Range(150f, 230f);
        }

        transform.rotation = Quaternion.Euler(num, transform.eulerAngles.y, 0f);
        rb.AddForce(transform.forward * initialSpeed);
        isServing = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 colNormal = collision.GetContact(0).normal;
        float speed = lastVel.magnitude;
        Vector3 direction = Vector3.Reflect(lastVel.normalized, colNormal);
        rb.velocity = direction * speed;
        transform.rotation = Quaternion.LookRotation(rb.velocity);

        if (BOUNCE_INACCURACY)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + Random.Range(-INAC_OFFSET, INAC_OFFSET), transform.rotation.eulerAngles.y, 0f);
            rb.velocity = transform.forward * speed;
        }

        if (collision.collider.CompareTag("Paddle"))
        {
            rb.velocity *= 1.1f;
        }

        AudioSource.PlayClipAtPoint(clips[0], new Vector3(0f, 0f, -10f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Goal"))
        {
            rb.velocity = Vector3.zero;
            rb.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            rb.position = Vector3.zero;
            AudioSource.PlayClipAtPoint(clips[1], new Vector3(0f, 0f, -10f));

            if (other.name == "Goal1")
            {
                rightServing = false;
            }
            else
            {
                rightServing = true;
            }

            StartCoroutine(ServeBall());
        }
    }

    private IEnumerator FailSafe()
    {
        while (true)
        {
            if ((rb.velocity == Vector3.zero && !isServing) || (transform.position.x >= 100f && !isServing) || transform.position.x <= -100f && !isServing)
            {
                Debug.Log(rb.velocity);
                StartCoroutine(ServeBall());
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
