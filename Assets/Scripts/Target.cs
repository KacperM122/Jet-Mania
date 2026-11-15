using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;

    public ParticleSystem explosionParticle;
    public AudioSource source;
    public AudioClip explosionAudio;

    public GameObject target;
    public BoxCollider targetCollider;

    public float speed = 30f;

    public void Start()
    {
    }

    private void Update()
    {
        transform.position += transform.rotation * Vector3.forward * Time.deltaTime * speed;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            DestroyTarget();
        }
    }

    public void DestroyTarget()
    {
        Destroy(target);
        Destroy(targetCollider);

        Instantiate(explosionParticle, gameObject.transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
  
}
