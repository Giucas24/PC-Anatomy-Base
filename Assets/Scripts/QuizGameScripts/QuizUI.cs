using UnityEngine;

public class QuizUI : MonoBehaviour
{
    public GameObject quizCanvas;
    public QuizManager quizManager;
    public PlayerMovement playerMovement;

    private void Start()
    {
        quizCanvas.SetActive(false);

        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("PlayerMovement not found in the scene");
            }
        }
        else
        {
            Debug.Log("PlayerMovement already assigned");
        }
    }

    public void ShowQuiz()
    {
        if (playerMovement != null)
        {
            quizCanvas.SetActive(true);
            playerMovement.isQuizActive = true;
            quizManager.StartQuiz();
        }
        else
        {
            Debug.LogError("PlayerMovement not found");
        }
    }

    public void HideQuiz()
    {
        if (playerMovement != null)
        {
            playerMovement.isQuizActive = false;
        }
        quizCanvas.SetActive(false);
    }

    public void RestartQuiz()
    {
        quizManager.RestartQuiz();
        ShowQuiz();
    }
}
