using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionPlayerTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public VectorValue startingPositionDynamic;
    public VectorValue startingPositionPreviousScene;
    private bool playerInTrigger = false;

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Space))
        {

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                startingPositionPreviousScene.initialValue = player.transform.position;

                startingPositionDynamic.initialValue = playerPosition;

                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("Player not found");
            }
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
