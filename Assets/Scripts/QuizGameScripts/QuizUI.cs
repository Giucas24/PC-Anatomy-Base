using UnityEngine;

public class QuizUI : MonoBehaviour
{
    public GameObject quizCanvas; // Reference to the Canvas Multiple Choice Quiz
    public QuizManager quizManager; // Reference to the QuizManager
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script

    private void Start()
    {
        quizCanvas.SetActive(false); // Hide the quiz canvas at the start

        // Trova automaticamente PlayerMovement se non è già assegnato
        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("PlayerMovement non trovato nella scena. Verifica che esista un oggetto con PlayerMovement.");
            }
            else
            {
                Debug.Log("PlayerMovement trovato nella scena.");
            }
        }
        else
        {
            Debug.Log("PlayerMovement già assegnato.");
        }
    }

    public void ShowQuiz()
    {
        // Verifica se playerMovement è stato trovato
        if (playerMovement != null)
        {
            quizCanvas.SetActive(true);
            playerMovement.isQuizActive = true; // Blocca il movimento del player
            quizManager.StartQuiz(); // Start the quiz when showing the UI
        }
        else
        {
            Debug.LogError("PlayerMovement non è stato trovato, non posso avviare il quiz.");
        }
    }

    public void HideQuiz()
    {
        if (playerMovement != null)
        {
            playerMovement.isQuizActive = false; // Sblocca il movimento del player
        }
        quizCanvas.SetActive(false);
    }

    public void RestartQuiz()
    {
        quizManager.RestartQuiz(); // Restart quiz if needed
        ShowQuiz(); // Show the quiz UI again
    }
}
