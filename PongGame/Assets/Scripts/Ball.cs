using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    #region Constants

    private const bool ANGLE_INACCURACIES = true;
    private const float INAC_RANGE = 10f;

    #endregion

    #region Variables

    private Rigidbody rb;

    private float ballSpeed = 400f;

    [SerializeField]
    private AudioClip[] clips;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 vel = ballSpeed * Time.fixedDeltaTime * Vector3.right;
        //transform.Translate(vel, Space.Self);
        //rb.MovePosition(rb.position + vel);
        rb.velocity = transform.TransformDirection(vel);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 colNormal = collision.GetContact(0).normal;

        float offset = 0f;

        if (ANGLE_INACCURACIES)
        {
            offset = Random.Range(-INAC_RANGE, INAC_RANGE);
        }

        if (colNormal == Vector3.down || colNormal == Vector3.up)
        {
            float newAngle = -transform.rotation.eulerAngles.z + offset;
            transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
        }
        else if (colNormal == Vector3.right || colNormal == Vector3.left)
        {
            float newAngle = (180f - transform.rotation.eulerAngles.z) + offset;
            transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
        }

        if (collision.collider.CompareTag("Paddle"))
        {
            ballSpeed += 75f;
        }

        AudioSource.PlayClipAtPoint(clips[0], new Vector3(0f, 0f, -10f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Goal1")
        {
            Debug.Log("goal for left!");
        }
        else if (other.name == "Goal2")
        {
            Debug.Log("goal for right!");
        }
    }
}
