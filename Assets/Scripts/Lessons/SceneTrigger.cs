using UnityEngine;
using UnityEngine.UI; // Per usare il componente Text
using System.Collections.Generic; // Per usare i dizionari

public class SceneTrigger : MonoBehaviour
{
    public GameObject player;  // Riferimento al player
    private PlayerMovement playerMovement; // Riferimento al movimento del giocatore

    public GameObject dialogCanvas;  // Riferimento al Canvas che contiene il dialogo
    public Text sceneTriggerText;    // Riferimento al Text che contiene il dialogo

    public string dialogMessage;     // Messaggio che apparirà nel dialogo

    private bool hasTriggered = false;  // Flag per verificare se il trigger è stato attivato

    // Dizionario statico per tracciare i trigger attivati durante il runtime
    private static Dictionary<string, bool> triggeredInstances = new Dictionary<string, bool>();

    void Start()
    {
        // Assicurati che il Canvas sia disabilitato all'inizio
        dialogCanvas.SetActive(false);

        // Assicurati che il player sia correttamente referenziato
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogError("Player non assegnato nel componente SceneTrigger.");
        }

        // Controlla se il trigger è già stato attivato in questa sessione
        if (triggeredInstances.TryGetValue(gameObject.name, out bool alreadyTriggered) && alreadyTriggered)
        {
            hasTriggered = true; // Imposta il flag per evitare di mostrare il dialogo di nuovo
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
