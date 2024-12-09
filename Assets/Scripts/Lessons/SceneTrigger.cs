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

    // Dizionario statico per tracciare i trigger attivati durante il runtime
    private static Dictionary<string, bool> triggeredInstances = new Dictionary<string, bool>();

    void Start()
    {
        if (PlayerMovement.Instance != null)
        {
            playerMovement = PlayerMovement.Instance;
        }
        else
        {
            Debug.LogError("PlayerMovement non trovato. Assicurati che esista un'istanza del Player nella scena.");
        }

        dialogCanvas.SetActive(false);

        if (triggeredInstances.TryGetValue(gameObject.name, out bool alreadyTriggered) && alreadyTriggered)
        {
            hasTriggered = true;
        }
    }


    // Quando il player entra nel trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se è il player che entra nel trigger
        if (other.CompareTag("Player") && !hasTriggered)
        {
            // Ferma il movimento del giocatore
            playerMovement.isQuizActive = true;

            // Attiva il dialogo
            ShowDialog();

            // Imposta il flag per non attivarlo di nuovo
            hasTriggered = true;

            // Registra che il trigger è stato attivato
            if (!triggeredInstances.ContainsKey(gameObject.name))
            {
                triggeredInstances.Add(gameObject.name, true);
            }
        }
    }

    // Mostra il dialogo
    void ShowDialog()
    {
        dialogCanvas.SetActive(true);  // Attiva il Canvas
        sceneTriggerText.text = dialogMessage;  // Imposta il testo del dialogo
    }

    void Update()
    {
        // Se il dialogo è attivo e il giocatore preme il tasto "spazio"
        if (dialogCanvas.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            CloseDialog();  // Chiudi il dialogo
        }
    }

    // Funzione per chiudere il dialogo
    public void CloseDialog()
    {
        dialogCanvas.SetActive(false);  // Disabilita il Canvas del dialogo
        playerMovement.isQuizActive = false;  // Rende di nuovo possibile il movimento del giocatore
    }
}
