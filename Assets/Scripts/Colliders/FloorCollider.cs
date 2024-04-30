using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollider : MonoBehaviour
{
    private GameEngine gameEngine;

    void Start(){

        GameObject gameEngineObject = GameObject.Find(GameEngine.TAG_GAME_ENGINE);
        gameEngine = gameEngineObject.GetComponent<GameEngine>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(GameEngine.TAG_FLOOR))
        {
            gameEngine.RestartGame();
            Debug.Log("Tower fall detected!");
        }
    }
}
