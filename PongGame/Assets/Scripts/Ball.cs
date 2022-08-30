using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    #region Constants

    private const bool ANGLE_INACCURACIES = true;
    private const float INAC_RANGE = 0.25f;

    #endregion

    #region Variables

    private Rigidbody rb;

    private float initialSpeed = 500f;

    [SerializeField]
    private AudioClip[] clips;

    private Vector3 lastVel;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * initialSpeed);
    }

    private void Update()
    {
        lastVel = rb.velocity;
    }

    //private void FixedUpdate()
    //{
    //    Vector3 vel = ballSpeed * Time.fixedDeltaTime * Vector3.right;
    //    transform.Translate(vel, Space.Self);
    //    rb.MovePosition(rb.position + vel);
    //    rb.velocity = transform.TransformDirection(vel);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 colNormal = collision.GetContact(0).normal;
        float speed = lastVel.magnitude;
        Vector3 direction = Vector3.Reflect(lastVel.normalized, colNormal) + new Vector3(Random.Range(-INAC_RANGE, INAC_RANGE), Random.Range(-INAC_RANGE, INAC_RANGE), 0f);

        rb.velocity = direction * speed;

        if (collision.collider.CompareTag("Paddle"))
        {
            rb.velocity *= 1.05f;
        }

        AudioSource.PlayClipAtPoint(clips[0], new Vector3(0f, 0f, -10f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Goal1")
        {
            AudioSource.PlayClipAtPoint(clips[1], new Vector3(0f, 0f, -10f));
            Debug.Log("goal for left!");
        }
        else if (other.name == "Goal2")
        {
            AudioSource.PlayClipAtPoint(clips[1], new Vector3(0f, 0f, -10f));
            Debug.Log("goal for right!");
        }
    }
}
