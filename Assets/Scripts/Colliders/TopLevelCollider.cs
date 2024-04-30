using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopLevelCollider : MonoBehaviour
{
    private GameEngine gameEngine;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameEngineObject = GameObject.Find(GameEngine.TAG_GAME_ENGINE);
        gameEngine = gameEngineObject.GetComponent<GameEngine>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(GameEngine.TAG_TOP_LEVEL))
        {
            gameEngine.NextLevel();
            Debug.Log("GameEngine: Finished level!");
        }
    }
}
