using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class GuessTheComponentManager : MonoBehaviour
{
    public TMP_FontAsset orangeKidFont;
    public GameObject[] componentObjects;
    private List<int> usedIndices = new List<int>();
    private int currentComponentIndex;
    private string currentWordToGuess;
    private float timer = 20f;

    public TMP_InputField answerInput;
    public Button submitButton;
    public TMP_Text feedbackText;
    public GameObject wordContainer;
    public TextMeshProUGUI timerText;

    private float revealLetterTimer = 5f;
    private int currentLetterIndex = 0;
    private int maxLettersToReveal = 3;

    private bool isRoundComplete = false;
    private bool isAnswerCorrect = false;

    public GameObject dialogBox;
    public GameObject[] gameUIElements;

    private bool isGameActive = false;

    private List<int> revealedIndices = new List<int>();

    private int correctAnswers = 0;
    private int totalQuestions = 0;
    private const int maxQuestions = 10;

    public TextMeshProUGUI dialogText;
    private bool isGameSummaryActive = false;

    public VectorValue startingPositionDynamic;
    public VectorValue startingPositionPreviousScene;

    private List<int> randomizedIndices;
    private bool isSkinUnlocked = false;

    public void StartGame()
    {
        dialogBox.SetActive(false);

        foreach (var element in gameUIElements)
        {
            element.SetActive(true);
        }

        foreach (var component in componentObjects)
        {
            component.SetActive(false);
        }

        randomizedIndices = new List<int>();
        for (int i = 0; i < componentObjects.Length; i++)
        {
            randomizedIndices.Add(i);
        }
        ShuffleList(randomizedIndices);

        submitButton.onClick.AddListener(SubmitAnswer);

        isGameActive = true;
        StartRound();
    }

    void ShuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void StartRound()
    {
        if (totalQuestions >= maxQuestions || totalQuestions >= randomizedIndices.Count)
        {
            EndGame();
            return;
        }

        totalQuestions++;

        isGameActive = true;
        foreach (var component in componentObjects)
        {
            component.SetActive(false);
        }

        currentComponentIndex = randomizedIndices[totalQuestions - 1];
        componentObjects[currentComponentIndex].SetActive(true);

        currentWordToGuess = componentObjects[currentComponentIndex].name;

        feedbackText.text = "";
        answerInput.text = "";

        DisplayWordWithDashes(currentWordToGuess);

        timer = 20f;
        currentLetterIndex = 0;

        revealedIndices.Clear();

        answerInput.Select();
        answerInput.ActivateInputField();

        isRoundComplete = false;
        isAnswerCorrect = false;
    }

    void Update()
    {
        if (IntroDialogManager.isDialogActive) return;

        if (isGameSummaryActive && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("LeftClassInterior");
        }

        if (!isGameActive || isRoundComplete) return;

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timer).ToString();
        }
        else
        {
            timerText.text = "Tempo scaduto!";
            isRoundComplete = true;
            StartCoroutine(WaitForNextRound());
        }

        if (revealLetterTimer > 0f)
        {
            revealLetterTimer -= Time.deltaTime;
        }
        else if (currentLetterIndex < maxLettersToReveal && currentLetterIndex < currentWordToGuess.Length)
        {
            RevealNextLetter();
            revealLetterTimer = 5f;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SubmitAnswer();
        }
    }

    void SubmitAnswer()
    {
        string trimmedAnswer = answerInput.text.Trim();

        if (trimmedAnswer.ToLower() == currentWordToGuess.ToLower())
        {
            correctAnswers++;
            feedbackText.text = "Corretto!";
            RevealFullWord();
            isAnswerCorrect = true;
            isRoundComplete = true;

            StartCoroutine(WaitForNextRound());
        }
        else
        {
            feedbackText.text = "Sbagliato, riprova!";
        }

        answerInput.text = "";
        answerInput.Select();
        answerInput.ActivateInputField();
    }


    // Closes a round and after timeout starts a new one
    IEnumerator WaitForNextRound()
    {
        isGameActive = false;
        yield return new WaitForSeconds(2f);

        foreach (var component in componentObjects)
        {
            component.SetActive(false);
        }

        StartRound();
    }

    // Shows dash hints
    void DisplayWordWithDashes(string word)
    {
        foreach (Transform child in wordContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (char letter in word)
        {
            CreateCharacter(letter == ' ' ? " " : "-");
        }

        wordContainer.SetActive(true);
    }

    void CreateCharacter(string character)
    {
        GameObject characterObject = new GameObject(character == " " ? "Space" : "Dash");
        characterObject.transform.SetParent(wordContainer.transform);

        TextMeshProUGUI characterText = characterObject.AddComponent<TextMeshProUGUI>();
        characterText.text = character;
        characterText.fontSize = 32;
        characterText.font = orangeKidFont;
        characterText.color = Color.black;
        characterText.characterSpacing = -5;

        RectTransform characterRectTransform = characterObject.GetComponent<RectTransform>();
        characterRectTransform.sizeDelta = new Vector2(40, 40);
    }


    void RevealNextLetter()
    {
        if (revealedIndices.Count < currentWordToGuess.Length)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, currentWordToGuess.Length);
            } while (revealedIndices.Contains(randomIndex));

            revealedIndices.Add(randomIndex);

            Transform dashTransform = wordContainer.transform.GetChild(randomIndex);

            TextMeshProUGUI dashText = dashTransform.GetComponent<TextMeshProUGUI>();
            dashText.text = currentWordToGuess[randomIndex].ToString();
        }
    }

    void RevealFullWord()
    {
        foreach (Transform child in wordContainer.transform)
        {
            TextMeshProUGUI dashText = child.GetComponent<TextMeshProUGUI>();
            int index = child.GetSiblingIndex();
            dashText.text = currentWordToGuess[index].ToString();
        }
    }

    void EndGame()
    {
        isGameActive = false;
        isGameSummaryActive = true;

        // Unlock a new skin
        if (correctAnswers >= 8)
        {
            if (SkinManager.Instance != null)
            {
                SkinManager.Instance.UnlockSkin(5);
                Debug.Log($"New skin unlocked: {SkinManager.Instance.skinNames[5]}");
                isSkinUnlocked = true;
            }
        }

        startingPositionDynamic.initialValue = startingPositionPreviousScene.initialValue;

        dialogBox.SetActive(true);

        if (correctAnswers >= maxQuestions / 2 && isSkinUnlocked)
        {
            dialogText.text = $"Congratulazioni! Hai risposto correttamente a {correctAnswers} su {maxQuestions} domande. Hai sbloccato un nuovo aspetto! Premi <b>Invio</b> per tornare alla classe.";
        }
        else if (correctAnswers >= maxQuestions / 2)
        {
            dialogText.text = $"Congratulazioni! Hai risposto correttamente a {correctAnswers} su {maxQuestions} domande. Premi <b>Invio</b> per tornare alla classe.";
        }
        else
        {
            dialogText.text = $"Peccato! Hai risposto correttamente a {correctAnswers} su {maxQuestions} domande. Premi <b>Invio</b> per tornare alla classe.";
        }


        foreach (var element in gameUIElements)
        {
            element.SetActive(false);
        }

        foreach (var component in componentObjects)
        {
            component.SetActive(false);
        }
    }

}
