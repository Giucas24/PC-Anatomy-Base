using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroSequence : MonoBehaviour
{
    public TMP_Text dialogText;
    public GameObject answers;
    public Button[] componentButtons;
    public string[] introductionTexts;
    public string[] componentDescriptions;
    public string finalIntroText;

    private int currentIndex = 0;
    private bool showingComponents = false;
    private bool showingFinalIntro = false;

    public QuestionManager questionManager;

    private bool quizCompleted = false;

    void Start()
    {
        foreach (Button button in componentButtons)
        {
            button.gameObject.SetActive(false);
        }

        ShowNextText();
    }

    void Update()
    {
        if (quizCompleted)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (showingFinalIntro)
            {
                StartQuiz();
            }
            else if (!showingComponents)
            {
                ShowNextText();
            }
            else
            {
                ShowNextComponent();
            }
        }
    }

    // Frasi prima della carrellata delle componenti
    public void ShowNextText()
    {
        if (currentIndex < introductionTexts.Length)
        {
            dialogText.text = introductionTexts[currentIndex];
            currentIndex++;
        }
        else
        {
            showingComponents = true;
            currentIndex = 0;
            ShowNextComponent();
        }
    }

    // Frasi durante la carellata delle componenti
    public void ShowNextComponent()
    {
        if (currentIndex < componentDescriptions.Length)
        {
            dialogText.text = componentDescriptions[currentIndex];
            componentButtons[currentIndex].gameObject.SetActive(true);
            currentIndex++;
        }
        else
        {
            ShowFinalIntro();
        }
    }

    // Ultima frase introduttiva prima dell'inizio del gioco
    void ShowFinalIntro()
    {
        showingComponents = false;
        showingFinalIntro = true;
        dialogText.text = finalIntroText;
    }

    void StartQuiz()
    {
        /* dialogText.text = "Pronto per iniziare il quiz!"; */
        questionManager.StartGame();
    }

    public void CompleteQuiz()
    {
        quizCompleted = true;
    }
}
