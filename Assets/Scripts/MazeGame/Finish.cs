using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject finishBox;
    public bool playerInRange;
    public VectorValue startingPositionDynamic;
    public VectorValue startingPositionPreviousScene;

    private PlayerMovement playerMovementScript;

    void Start()
    {
        finishBox.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && finishBox.activeInHierarchy && Input.GetKeyDown(KeyCode.Return))
        {
            ClosePanelAndLoadScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            finishBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            finishBox.SetActive(false);
        }
    }

    void ClosePanelAndLoadScene()
    {
        finishBox.SetActive(false);

        startingPositionDynamic.initialValue = startingPositionPreviousScene.initialValue;

        SceneManager.LoadScene("LeftClassInterior");
    }
}


