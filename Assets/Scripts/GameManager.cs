using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;
    public int asteroidCount = 0;
    private int level = 0;

    [SerializeField] private GameObject gameOverScreen;

    public enum GAMESTATE
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (asteroidCount == 0)
        {
            level++;

            // Have a starting level of 4 asteroids
            // And increment by two
            int numAsteroids = 2 + (2*level);
            for (int i = 0; i < numAsteroids; i ++)
            {
                SpawnAsteroid();
            }
        }
    }

    private void SpawnAsteroid()
    {
        // Generate spawning location on screen.
        float offset = Random.Range(0f, 1f);
        Vector2 viewportSpawnPosition = Vector2.zero;

        // Spawn the asteroids based from the sides of the screen.
        // Bottom, top, left, right.
        int edge = Random.Range(0,4);
        if (edge == 0)
        {
            viewportSpawnPosition = new Vector2(offset, 0);
        }
        else if (edge == 1)
        {
            viewportSpawnPosition = new Vector2(offset, 1);
        }
        else if (edge == 2)
        {
            viewportSpawnPosition = new Vector2(0, offset);
        }
        else if (edge == 3)
        {
            viewportSpawnPosition = new Vector2(1, offset);
        }

        // Create the Asteroid
        viewportSpawnPosition = viewportSpawnPosition.normalized;
        Vector3 viewPort3 = new Vector3(viewportSpawnPosition.x, viewportSpawnPosition.y, 0);
        Vector3 worldSpawnPosition = Camera.main.ViewportToWorldPoint(viewPort3);
        worldSpawnPosition.z = 0;
        spawnAsteroid(worldSpawnPosition, Asteroid.DEFAULT_SIZE);
    }

    private void spawnAsteroid(Vector3 pos, int size)
    {
        Asteroid newAsteroid = Instantiate(asteroidPrefab, pos, Quaternion.identity);
        newAsteroid.size = size;
        newAsteroid.notifyDeath = onAsteroidDeath;
        asteroidCount++;
    }

    // When an Asteroid Dies, we want to spawn more if
    // it was a large one.
    public void onAsteroidDeath(Asteroid asteroid)
    {
        // Large size asteroids keep breaking up into smaller ones upon destruction.
        // Only spawn 2 for every breakup now.
        if (asteroid.size > 1)
        {
            for(int i = 0; i < 2; i ++)
            {
                spawnAsteroid(asteroid.transform.position, asteroid.size -1);
            }
        }

        // Finally remove the Asteroid that got blown up
        asteroidCount--;
    }



    public void NotifyGameOver()
    {
        // TODO: display a game over message here.

        // Simply countdown to restart the game.
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        Debug.Log("Restarting Game....");
        gameOverScreen.SetActive(true);

        // Wait countdown to restart and reload scene
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }
}
