using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SceneTrigger : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement playerMovement;

    public GameObject dialogCanvas;
    public Text sceneTriggerText;

    public string dialogMessage;

    private bool hasTriggered = false;

    // Dictionary for saving who activated the trigger
    private static Dictionary<string, bool> triggeredInstances = new Dictionary<string, bool>();

    void Start()
    {
        if (PlayerMovement.Instance != null)
        {
            playerMovement = PlayerMovement.Instance;
        }
        else
        {
            Debug.LogError("PlayerMovement not found");
        }
        dialogCanvas.SetActive(false);

        if (triggeredInstances.TryGetValue(gameObject.name, out bool alreadyTriggered) && alreadyTriggered)
        {
            hasTriggered = true;
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            playerMovement.isQuizActive = true; // To block player's movement

            ShowDialog();

            hasTriggered = true;

            if (!triggeredInstances.ContainsKey(gameObject.name))   // Check if player is already in the dictionary
            {
                triggeredInstances.Add(gameObject.name, true);  // Add player to the dictionary
            }
        }
    }

    void ShowDialog()
    {
        dialogCanvas.SetActive(true);
        sceneTriggerText.text = dialogMessage;
    }

    void Update()
    {
        if (dialogCanvas.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            CloseDialog();
        }
    }

    public void CloseDialog()
    {
        dialogCanvas.SetActive(false);
        playerMovement.isQuizActive = false;
    }
}
