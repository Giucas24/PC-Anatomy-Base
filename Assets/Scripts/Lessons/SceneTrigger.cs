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

    private bool hasTriggered = false;  // Flag per verificare se il trigger è stato attivato

    // Dizionario per salvare chi ha attivato il trigger
    private static Dictionary<string, bool> triggeredInstances = new Dictionary<string, bool>();

    void Start()
    {
        if (PlayerMovement.Instance != null)
        {
            playerMovement = PlayerMovement.Instance;
        }
        /* else
        {
            Debug.LogError("PlayerMovement non trovato. Assicurati che esista un'istanza del Player nella scena.");
        } */
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
            playerMovement.isQuizActive = true; // Per bloccare il movimento del player

            ShowDialog();

            hasTriggered = true;    // Flag per evitare che si attivi di nuovo

            if (!triggeredInstances.ContainsKey(gameObject.name))   // Verifica se il player è già nel dizionario
            {
                triggeredInstances.Add(gameObject.name, true);  // Aggiunge il player nel dizionario
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
