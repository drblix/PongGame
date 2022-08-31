using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBall : MonoBehaviour
{
    #region Constants

    private const float SERVE_TIME = 1f;
    private const bool BOUNCE_INACCURACY = true;
    private const float INAC_OFFSET = 15f;

    #endregion

    #region Variables

    [HideInInspector]
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

        transform.rotation = Quaternion.Euler(num, transform.eulerAngles.y, 0f);
        rb.AddForce(transform.forward * initialSpeed);
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
        rb.velocity *= 1.1f;

        AudioSource.PlayClipAtPoint(clips[0], new Vector3(0f, 0f, -10f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Goal")
        {
            rb.velocity = Vector3.zero;
            rb.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            rb.position = Vector3.zero;
            AudioSource.PlayClipAtPoint(clips[1], new Vector3(0f, 0f, -10f));
            StartCoroutine(ServeBall());
        }
    }
}
