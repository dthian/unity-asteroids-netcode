using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    private int size = 3;
    public GameManager gameManager;

    [SerializeField] private ParticleSystem destroyedParticles;

    public void setSize(int newSize)
    {
        size = newSize;
    }

    void Start()
    {
        // Scale based on the size.
        transform.localScale = 0.5f * size * Vector3.one;

        // Now move it, chaotically
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 dir = new Vector2(Random.value, Random.value).normalized;
        float speed = Random.Range(4f - size, 5f - size);   // Larger moves slower.
        rb.AddForce(dir * speed, ForceMode2D.Impulse);

        gameManager.asteroidCount++;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Asteroids can be destroyed by bullets.
        if (col.CompareTag("Bullet"))
        {
            // Destroy the bullet that collided
            Destroy(col.gameObject);

            // Large size asteroids keep breaking up into smaller ones
            // upon destruction.
            if (size > 1)
            {
                for(int i = 0; i < 2; i ++)
                {
                    Asteroid newAsteroid = Instantiate(this, transform.position, Quaternion.identity);
                    newAsteroid.setSize(size -1);
                    newAsteroid.gameManager = gameManager;
                }
            }

            // Explosion
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);

            // Finally destroy Asteroid that got blown up
            Destroy(gameObject);
            gameManager.asteroidCount--;
        }
    }
}
