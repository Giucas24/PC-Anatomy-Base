using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lucchetto : MonoBehaviour
{
    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
    public bool playerInRange;

    // ................. 
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answer1Text;
    public TextMeshProUGUI answer2Text;

    public Button button1;            // Primo bottone
    public Button button2;            // Secondo bottone
    public string question;           // La domanda
    public string[] answers;          // Array delle risposte
    public int correctAnswerIndex;    // Indice della risposta corretta

    private Animator animator;

    // Nuove variabili
    public static int totalLocks = 4;  // Numero totale di lucchetti nel labirinto (modifica questo valore se il numero cambia)
    public static int unlockedLocks = 0;  // Conta i lucchetti sbloccati

    void Start()
    {
        dialogBox.SetActive(false);

        // Aggiungi listener ai bottoni per verificare le risposte
        button1.onClick.AddListener(() => CheckAnswer(0));
        button2.onClick.AddListener(() => CheckAnswer(1));

        animator = GetComponent<Animator>(); // Ottieni il componente Animator
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange)
        {
            if (dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(false);
            }
            else
            {
                // Mostra la domanda
                ShowQuestion();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogBox.SetActive(false);
        }
    }

    void ShowQuestion()
    {
        dialogBox.SetActive(true); // Mostra il dialog box
        questionText.text = question; // Imposta il testo della domanda

        // Imposta il testo dei bottoni
        answer1Text.text = answers[0];
        answer2Text.text = answers[1];
    }

    void CheckAnswer(int index)
    {
        if (index == correctAnswerIndex)
        {
            Debug.Log("Risposta corretta!");
            dialogBox.SetActive(false);
            RemoveLock();

            // Incrementa il numero di lucchetti sbloccati
            unlockedLocks++;

            // Se questo è l'ultimo lucchetto, sblocca la skin
            if (unlockedLocks == totalLocks)
            {
                Debug.Log("Questo è l'ultimo lucchetto!");

                if (SkinManager.Instance != null)
                {
                    Debug.Log("Chiamata a SkinManager.Instance.UnlockSkin()");
                    SkinManager.Instance.UnlockSkin(5); // Indice 7 per sbloccare una skin
                }
                else
                {
                    Debug.LogWarning("SkinManager.Instance è null!");
                }
            }
        }
        else
        {
            Debug.Log("Risposta sbagliata.");
            dialogBox.SetActive(false); // Nasconde il dialog box
        }
    }


    void RemoveLock()
    {
        Debug.Log("Lucchetto rimosso!");
        animator.SetTrigger("Open"); // Attiva il trigger per l'animazione

        // Aspetta la fine dell'animazione prima di distruggere il lucchetto
        StartCoroutine(WaitAndDestroy());
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(3f); // Cambia il tempo con la durata dell'animazione
        Destroy(gameObject); // Rimuove il lucchetto dalla scena
    }
}
