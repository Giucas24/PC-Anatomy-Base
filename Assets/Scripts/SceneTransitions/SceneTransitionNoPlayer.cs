using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public string sceneToLoad;


    private bool playerInside = false;

    public VectorValue startingPositionPreviousScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.Space))
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                startingPositionPreviousScene.initialValue = player.transform.position;

                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("Player not found");
            }
        }
    }
}
