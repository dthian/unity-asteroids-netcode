using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;
    public int asteroidCount = 0;
    private int level = 0;

    [SerializeField] private GameObject gameOverScreen;

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
        Asteroid asteroid = Instantiate(asteroidPrefab, worldSpawnPosition, Quaternion.identity);
        asteroid.gameManager = this;
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
