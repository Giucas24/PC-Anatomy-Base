using UnityEngine;

public class QuizUI : MonoBehaviour
{
    public GameObject quizCanvas; // Reference to the Canvas Multiple Choice Quiz
    public QuizManager quizManager; // Reference to the QuizManager
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script

    private void Start()
    {
        quizCanvas.SetActive(false); // Hide the quiz canvas at the start
    }

    public void ShowQuiz()
    {
        quizCanvas.SetActive(true);
        playerMovement.isQuizActive = true; // Blocca il movimento del player
        quizManager.StartQuiz(); // Start the quiz when showing the UI
    }

    public void HideQuiz()
    {
        quizCanvas.SetActive(false);
        playerMovement.isQuizActive = false; // Sblocca il movimento del player
    }

    public void RestartQuiz()
    {
        quizManager.RestartQuiz(); // Restart quiz if needed
        ShowQuiz(); // Show the quiz UI again
    }
}





