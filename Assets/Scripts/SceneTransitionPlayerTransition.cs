using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionPlayerInteraction : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public VectorValue startingPositionDynamic;
    private bool playerInTrigger = false;

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            startingPositionDynamic.initialValue = playerPosition;

            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInTrigger = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInTrigger = false;
        }
    }
}
