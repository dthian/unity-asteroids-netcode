using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public delegate void OnDeathNotify(Asteroid asteroid);

public class Asteroid : MonoBehaviour
{
    public const int DEFAULT_SIZE = 3;
    public int size = DEFAULT_SIZE;
    public OnDeathNotify notifyDeath;

    [SerializeField] private ParticleSystem destroyedParticles;

    void Start()
    {
        // Scale based on the size.
        transform.localScale = 0.5f * size * Vector3.one;

        // Now move it, chaotically
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 dir = new Vector2(Random.value, Random.value).normalized;
        float speed = Random.Range(4f - size, 5f - size);   // Larger moves slower.
        rb.AddForce(dir * speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Asteroids can be destroyed by bullets.
        if (col.CompareTag("Bullet"))
        {
            // Destroy the bullet.
            Destroy(col.gameObject);

            // Let everyone know astroid has been hit.
            notifyDeath(this);

            // Finally destroy Asteroid that got blown up with Explosion
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
