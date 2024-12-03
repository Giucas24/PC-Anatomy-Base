using UnityEngine;
using TMPro;

public class IntroDialogManager : MonoBehaviour
{
    public GameObject dialogBox;  // Il box che contiene il dialogo
    public TMP_Text dialogText;   // Il testo del dialogo
    public string[] dialogLines; // Le linee del dialogo
    private int currentLineIndex = 0;  // Linea corrente del dialogo
    public GuessTheComponentManager gameManager;  // Riferimento al manager del gioco
    public static bool isDialogActive = true; // Il dialogo è attivo di default


    void Start()
    {
        // Mostra il primo dialogo
        ShowCurrentLine();
    }

    void Update()
    {
        // Gestisce il tasto Spazio solo se il dialogo è attivo
        if (isDialogActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine(); // Metodo che gestisce l'avanzamento del dialogo
        }
    }


    void ShowCurrentLine()
    {
        // Mostra la linea corrente del dialogo
        if (currentLineIndex < dialogLines.Length)
        {
            dialogText.text = dialogLines[currentLineIndex];
        }
        else
        {
            EndDialog();  // Se non ci sono più linee, termina il dialogo
        }
    }

    void NextLine()
    {
        currentLineIndex++;
        ShowCurrentLine(); // Mostra la prossima linea
    }

    void EndDialog()
    {
        // Disabilita la dialogBox al termine del dialogo
        dialogBox.SetActive(false);
        isDialogActive = false;

        // Avvia il gioco
        gameManager.StartGame();
    }
}
