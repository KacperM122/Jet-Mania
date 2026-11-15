using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class Plane : MonoBehaviour
{
    public float throttleIncrement = 0.1f;
    public float maxThrust = 200f;
    public float responsivness = 10f;

    public float lift = 200f;

    public float damage = 10f;
    public float shootingRange = 100f;
    public float fireRate = 15f;

    public Camera mainCamera;

    public AudioClip firing;
    public AudioClip firingEnd;
    public AudioClip engineSpeedup;

    public ParticleSystem engineParticle;

    public AudioSource audioSource;
    public AudioSource audioSourceEngine;

    public GameObject missileSpawner;
    public GameObject missilePrefab;

    public TextMeshProUGUI speedUI;

    TargetingSystem targetingSystem;

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    private float nextTimeToFire = 0f;

    Rigidbody rb;


    private float responseModifier
    {
        get
        {
            return (rb.mass / 10f) * responsivness;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        targetingSystem = GetComponent<TargetingSystem>();
    }

    

    void Update()
    {
        ControlsInputs();

        

        speedUI.text = "Prêdkoœæ: " + rb.linearVelocity.magnitude.ToString("0");

    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(-transform.forward * roll * responseModifier);

        // uniesienie podczas startowanie
        rb.AddForce(Vector3.up * rb.linearVelocity.magnitude * lift);
    }

    private void ControlsInputs()
    {
        // sterowanie
        roll = Input.GetAxis("Horizontal");
        pitch = Input.GetAxis("Vertical");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space)) throttle += throttleIncrement;
        else if (Input.GetKey(KeyCode.LeftControl)) throttle -= throttleIncrement;
        throttle = Mathf.Clamp(throttle, 0f, 100f);

        // pociski rakietowe
        if (Input.GetButtonDown("Fire1"))
        {
            // wystrzelenie pocisku w stronê celu
            GameObject missile = Instantiate(missilePrefab, missileSpawner.transform.position, missileSpawner.transform.rotation);
            if (targetingSystem.currentTarget != null)
            {
                missile.GetComponent<HomingMissile>().targetPosition = targetingSystem.currentTarget.transform;
            }
        }

        // strzelanie
        if (Input.GetButton("Fire2") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            Shoot();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            audioSource.Play();
        }

        if (Input.GetButtonUp("Fire2"))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(firingEnd);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            engineParticle.Play();
            audioSourceEngine.Play();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            engineParticle.Stop();
            audioSourceEngine.Stop();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            targetingSystem.nearestDistance = targetingSystem.defaultNearestDistanceValue;
            targetingSystem.FindClosestTarget();
        }
    }

    // strzelanie
    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, shootingRange))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null) 
            {
                target.TakeDamage(damage);
            }
        }
    }

}
