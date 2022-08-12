using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int score;
    int level;

    public GameObject asteroidPrefabBig;
    public GameObject asteroidPrefabMedium;
    public GameObject asteroidPrefabSmall;

    public Vector2 mapSize { get; set; }

    List<GameObject> activeAsteroids = new List<GameObject>();
    public MapBorderTeleporter _mapBorderTeleporter;

    [Header("UFO")]
    [SerializeField] Vector2 respawnDelayRange;
    [SerializeField] GameObject ufoPrefab;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject startGamePanel;

    [Header("GameOver")]
    [SerializeField] TextMeshProUGUI scoreTextGameOver;
    [SerializeField] TextMeshProUGUI highscoreTextGameOver;
    [SerializeField] GameObject gameOverPanel;

    void Awake()
    {
        Time.timeScale = 0f;
        instance = this;

        Camera camera = Camera.main;
        mapSize = new Vector2(camera.orthographicSize * camera.aspect, camera.orthographicSize);

        StartNextLevel();
        Time.timeScale = 1f;
        StartCoroutine(SpawnUFOWithDelay());
    }

    public void StartNextLevel()
    {
        for (int i = 0; i < level + 2; i++)
        {
            Vector3 spawnPosition = Random.onUnitSphere;
            spawnPosition.z = 0f;
            spawnPosition *= mapSize.x * 2f;

            float bigAsteroidRadius = asteroidPrefabBig.transform.localScale.x / 2f;
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -mapSize.x + bigAsteroidRadius, mapSize.x - bigAsteroidRadius);
            spawnPosition.y = Mathf.Clamp(spawnPosition.y, -mapSize.y + bigAsteroidRadius, mapSize.y - bigAsteroidRadius);

            GameObject newAsteroidObj = Instantiate(asteroidPrefabBig, spawnPosition, Quaternion.identity);
        }
    }

    public void AsteroidSpawned(GameObject newAsteroidObj)
    {
        activeAsteroids.Add(newAsteroidObj);
    }

    public void AsteroidKilled(GameObject asteroidObj, int scoreReward)
    {
        AddScore(scoreReward);

        activeAsteroids.Remove(asteroidObj);
        _mapBorderTeleporter.teleportableObjects.Remove(asteroidObj.transform);
        
        if (activeAsteroids.Count == 0)
        {
            level++;
            StartNextLevel();
        }
    }

    public void AddScore(int scoreAmount)
    {
        score += scoreAmount;
        scoreText.text = $"Points: {score}";
    }

    public void UFOKilled(int scoreReward)
    {
        AddScore(scoreReward);
        StartCoroutine(SpawnUFOWithDelay());
    }

    IEnumerator SpawnUFOWithDelay()
    {
        yield return new WaitForSeconds(Random.Range(respawnDelayRange.x, respawnDelayRange.y));

        bool atRightSide = false;
        atRightSide.SetRandomBool();
        float yCoord = Random.Range(0.9f * -GameManager.instance.mapSize.y, 0.9f * GameManager.instance.mapSize.y); // 90% of map Y is available for spawn
        Instantiate(ufoPrefab, new Vector2(GameManager.instance.mapSize.x * (atRightSide == true ? 1 : -1), yCoord), Quaternion.identity);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        int highScore = PlayerPrefs.GetInt("highScore");
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
        }
        highscoreTextGameOver.text = $"Highscore: {highScore}";
        scoreTextGameOver.text = $"Your score: {score}";
    }

    public void StartGame()
    {
        startGamePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
