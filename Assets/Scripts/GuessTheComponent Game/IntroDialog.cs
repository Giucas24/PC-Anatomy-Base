using UnityEngine;
using TMPro;

public class IntroDialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public TMP_Text dialogText;
    public string[] dialogLines;
    private int currentLineIndex;
    public GuessTheComponentManager gameManager;
    public static bool isDialogActive;

    void Start()
    {
        InitializeDialog();
    }

    void Update()
    {
        if (isDialogActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    public void InitializeDialog()
    {
        currentLineIndex = 0;
        isDialogActive = true;
        dialogBox.SetActive(true);
        ShowCurrentLine();
    }

    void ShowCurrentLine()
    {
        if (currentLineIndex < dialogLines.Length)
        {
            dialogText.text = dialogLines[currentLineIndex];
        }
        else
        {
            EndDialog();
        }
    }

    void NextLine()
    {
        currentLineIndex++;
        ShowCurrentLine();
    }

    void EndDialog()
    {
        dialogBox.SetActive(false);
        isDialogActive = false;
        gameManager.StartGame();
    }
}