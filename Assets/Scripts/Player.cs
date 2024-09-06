using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player Visual attributes
    [Header("Ship parameters")]
    [SerializeField] private float shipAccel = 10f;

    [SerializeField] private float shipMaxVelocity = 10f;
    [SerializeField] private float shipRotVelocity = 180f;

    private Rigidbody2D shipRigidBody;
    private bool isAlive = true;
    private bool isAccelerating = false;

    // Weapon Attributes
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private float bulletSpeed = 8f;

    private void Start()
    {
        // Get a reference to the attached RigidBody2D.
        shipRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isAlive)
        {
            HandleMovementInput();
            HandleCombatInput();
        }
    }

    private void FixedUpdate()
    {
        if (!isAlive)
        {
            return;
        }

        if (isAccelerating)
        {
            // Apply acceleration force and clamp it's speed with a limit
            shipRigidBody.AddForce(shipAccel * transform.up);
            shipRigidBody.velocity = Vector2.ClampMagnitude(shipRigidBody.velocity, shipMaxVelocity);
        }
    }

    private void HandleMovementInput()
    {
        // Handle translational movement
        isAccelerating = Input.GetKey(KeyCode.UpArrow);

        // Handle rotational movement.
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(shipRotVelocity *  Time.deltaTime * transform.forward);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-shipRotVelocity *  Time.deltaTime * transform.forward);
        }
    }

    private void HandleCombatInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity);

            // NOTE: we apply bullet velocity with forward movement of ship
            Vector2 shipVel = shipRigidBody.velocity;
            Vector2 shipDir = transform.up;
            float shipForwardSpeed = Vector2.Dot(shipVel, shipDir);
            if (shipForwardSpeed < 0)
            {
                shipForwardSpeed = 0;
            }

            // Finally apply the initial bullet speed and shoot it out with a force
            bullet.velocity = shipDir * shipForwardSpeed;
            bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Asteroid"))
        {
            isAlive = false;

            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.NotifyGameOver();
            Destroy(gameObject);
        }
    }
}