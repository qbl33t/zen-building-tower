using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEngine : MonoBehaviour
{
    public GameObject spawner;

    public float waitTransition = 0.5f;

    public static readonly string TAG_FLOOR = "Floor";
    public static readonly string TAG_TOP_LEVEL = "TopLevel";
    public static readonly string TAG_GAME_ENGINE = "GameEngine";

    private CubeSpawner cubeSpawner;

    private GameState gameStateStartLevel = new()
    {
        massCube = 0.01f,
        speedSpawn = 1.0f,
        towerLevel = 10,
        randomized = true
    };

    public GameState gameState;

    void Awake()
    {
        TriggerLoadScreen("Starting Game");
        gameState = gameStateStartLevel;

        cubeSpawner = spawner.GetComponent<CubeSpawner>();

        StartGame();
    }

    public void RestartGame()
    {
        TriggerLoadScreen("Restarting Game");
        StartGame();
    }

    public void NextLevel()
    {
        TriggerLoadScreen("Next Level");
        gameState.massCube += 0.02f;
        gameState.speedSpawn -= 0.1f;
        StartGame();
    }

    private void StartGame()
    {
        // textLevel.text = "Level: " + gameState.towerLevel;
        // textSpeed.text = "Speed: " + gameState.speedSpawn;
        // textMass.text = "Mass: " + gameState.massCube;
        // textRandom.text = "Random: " + gameState.randomized;

        cubeSpawner.Init(gameState);
    }

    private void TriggerLoadScreen(string text)
    {
        // textLoadScreen.text = text;
        // transition.SetTrigger("Start");
        StartCoroutine(WaitCoroutine(waitTransition));
        // transition.SetTrigger("End");
    }

    IEnumerator WaitCoroutine(float waitTimeInSeconds)
    {
        yield return new WaitForSeconds(waitTimeInSeconds);
    }
}
