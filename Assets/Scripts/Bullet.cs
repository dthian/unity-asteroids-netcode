using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLifeTimeSec = 1f;

    void Awake()
    {
        // Mark for deletion after spawn
        Destroy(gameObject, bulletLifeTimeSec);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
