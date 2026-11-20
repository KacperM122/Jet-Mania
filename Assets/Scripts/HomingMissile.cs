using System.Collections;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public Transform targetPosition;
    public float additionalSpeed = 10f;
    public float rotateSpeed = .03f;
    public float damage = 10f;

    public ParticleSystem explosionEffect;

    GameObject plane;

    private Rigidbody rb;

    float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        plane = GameObject.FindGameObjectWithTag("Player");
        speed = plane.GetComponent<Rigidbody>().linearVelocity.magnitude;

        StartCoroutine(AddSpeed());
    }

    //opóŸniony start rakiety
    IEnumerator AddSpeed()
    {
        yield return new WaitForSeconds(0.1f);

        speed += additionalSpeed;
    }

    void FixedUpdate()
    {
        if (targetPosition != null)
        {
            Vector3 direction = targetPosition.position - rb.position;
            direction.Normalize();

            Vector3 amountToRotate = Vector3.Cross(direction, transform.up) * Vector3.Angle(transform.up, direction);

            rb.angularVelocity = -amountToRotate * rotateSpeed;

            rb.linearVelocity = transform.up * speed;
        }
        else
        {
            rb.linearVelocity = transform.up * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Enemy enemy = other.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
