using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DragAndDropManager : MonoBehaviour
{
    public GameObject cpu, expansion, video, chipset, ram, batteriaCMOS, bios, cpuBlack, porteIo, expansionBlack, videoBlack, chipsetBlack, ramBlack, batteriaCMOSBlack, biosBlack, porteIoBlack;

    public Text cpuText, expansionText, videoText, chipsetText, ramText, batteriaCMOSText, biosText, porteIoText;


    public GameObject finalImage;

    public GameObject vinto;

    private CanvasGroup finalImageCanvasGroup;

    private Vector2 cpuInitialPos, expansionInitialPos, videoInitialPos, chipsetInitialPos, ramInitialPos, batteriaCMOSInitialPos, biosInitialPos, porteIoInitialPos;

    public bool cpuLocked, expansionLocked, videoLocked, chipsetLocked, ramLocked, batteriaCMOSLocked, biosLocked, porteIoLocked;

    public float fadeDuration = 1.5f;

    public Canvas gameCanvas;
    public Text taskText;
    private string initialMessage = "TRASCINA LE COMPONENTI HARDWARE DELLA SCHEDA MADRE NELLE POSIZIONI CORRETTE, SIMULANDO L'ASSEMBLAGGIO DI UN PC.";
    private string victoryMessage = "COMPLIMENTI! HAI SBLOCCATO UN NUOVO ASPETTO! <size=24><color=black>PREMI <b>INVIO</b> PER RITORNARE IN CLASSE!</color></size>";

    private bool gameCompleted = false;

    public VectorValue startingPositionDynamic;

    public VectorValue startingPositionPreviousScene;
    private PlayerMovement playerMovementScript;
    private bool victoryMessageShown = false;

    public AudioSource source;
    public AudioClip correct;
    public AudioClip incorrect;

    void Start()
    {
        cpuInitialPos = cpu.transform.position;
        expansionInitialPos = expansion.transform.position;
        videoInitialPos = video.transform.position;
        chipsetInitialPos = chipset.transform.position;
        ramInitialPos = ram.transform.position;
        batteriaCMOSInitialPos = batteriaCMOS.transform.position;
        biosInitialPos = bios.transform.position;
        porteIoInitialPos = porteIo.transform.position;

        cpuLocked = false;
        expansionLocked = false;
        videoLocked = false;
        chipsetLocked = false;
        ramLocked = false;
        batteriaCMOSLocked = false;
        biosLocked = false;
        porteIoLocked = false;

        vinto.SetActive(false);

        finalImageCanvasGroup = finalImage.GetComponent<CanvasGroup>();
        if (finalImageCanvasGroup != null)
        {
            finalImageCanvasGroup.alpha = 0;
            finalImage.SetActive(false);
        }
        else
        {
            Debug.LogError("finalImageCanvasGroup is null");
        }

        StartCoroutine(TypeText(initialMessage, 0.04f));
    }


    public void DragCpu()
    {
        if (!cpuLocked)
        {
            DragUIElement(cpu);
        }
    }

    public void DropCpu()
    {
        DropUIElement(cpu, cpuBlack, ref cpuLocked, cpuInitialPos, cpuText);
    }


    //   ------------------------------------------------------------------

    public void DragExpansion()
    {
        if (!expansionLocked)
        {
            DragUIElement(expansion);
        }
    }

    public void DropExpansion()
    {
        DropUIElement(expansion, expansionBlack, ref expansionLocked, expansionInitialPos, expansionText);
    }

    //   ------------------------------------------------------------------

    public void DragVideo()
    {
        if (!videoLocked)
        {
            DragUIElement(video);
        }
    }

    public void DropVideo()
    {
        DropUIElement(video, videoBlack, ref videoLocked, videoInitialPos, videoText);
    }

    //   ------------------------------------------------------------------

    public void DragChipset()
    {
        if (!chipsetLocked)
        {
            DragUIElement(chipset);
        }
    }

    public void DropChipset()
    {
        DropUIElement(chipset, chipsetBlack, ref chipsetLocked, chipsetInitialPos, chipsetText);
    }

    //   ------------------------------------------------------------------

    public void DragRam()
    {
        if (!ramLocked)
        {
            DragUIElement(ram);
        }
    }

    public void DropRam()
    {
        DropUIElement(ram, ramBlack, ref ramLocked, ramInitialPos, ramText);
    }

    //   ------------------------------------------------------------------

    public void DragBatteriaCMOS()
    {
        if (!batteriaCMOSLocked)
        {
            DragUIElement(batteriaCMOS);
        }
    }

    public void DropBatteriaCMOS()
    {
        DropUIElement(batteriaCMOS, batteriaCMOSBlack, ref batteriaCMOSLocked, batteriaCMOSInitialPos, batteriaCMOSText);
    }

    //   ------------------------------------------------------------------

    public void DragBios()
    {
        if (!biosLocked)
        {
            DragUIElement(bios);
        }
    }


    public void DropBios()
    {
        DropUIElement(bios, biosBlack, ref biosLocked, biosInitialPos, biosText);
    }

    //   ------------------------------------------------------------------

    public void DragPorteIo()
    {
        if (!porteIoLocked)
        {
            DragUIElement(porteIo);
        }
    }

    public void DropPorteIo()
    {
        DropUIElement(porteIo, porteIoBlack, ref porteIoLocked, porteIoInitialPos, porteIoText);
    }


    private void DragUIElement(GameObject element)
    {
        if (gameCanvas == null)
        {
            Debug.LogError("gameCanvas not assigned");
            return;
        }

        RectTransform canvasRectTransform = gameCanvas.GetComponent<RectTransform>();

        if (canvasRectTransform == null)
        {
            Debug.LogError("canvasRectTransform not found");
            return;
        }

        // Converts mouse pointer position into the canvasRectTransform space
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            Input.mousePosition,
            null,
            out localPoint
        );

        RectTransform elementRectTransform = element.GetComponent<RectTransform>();

        if (elementRectTransform == null)
        {
            Debug.LogError($"RectTransform not found in: {element.name}");
            return;
        }

        elementRectTransform.localPosition = localPoint;
    }

    private void DropUIElement(GameObject element, GameObject targetElement, ref bool elementLocked, Vector2 initialPosition, Text elementText)
    {
        float Distance = Vector3.Distance(element.transform.position, targetElement.transform.position);
        if (Distance < 50)
        {
            elementLocked = true;

            element.transform.position = targetElement.transform.position;
            element.transform.localScale = targetElement.transform.localScale;

            RectTransform elementRectTransform = element.GetComponent<RectTransform>();
            RectTransform targetElementRectTransform = targetElement.GetComponent<RectTransform>();

            if (elementRectTransform != null && targetElementRectTransform != null)
            {
                elementRectTransform.sizeDelta = targetElementRectTransform.sizeDelta;
            }
            else
            {
                Debug.LogError($"RectTransform not found in {element.name} or {targetElement.name}.");
            }

            elementText.color = Color.green;
            source.clip = correct;
            source.Play();
            if (element == porteIo)
            {
                porteIoBlack.SetActive(false);
            }
        }
        else
        {
            element.transform.position = initialPosition;
            source.clip = incorrect;
            source.Play();
        }
    }

    void Update()
    {
        if (!victoryMessageShown && cpuLocked && expansionLocked && videoLocked && chipsetLocked && ramLocked && batteriaCMOSLocked && biosLocked && porteIoLocked && finalImageCanvasGroup != null && finalImageCanvasGroup.alpha == 0)
        {
            victoryMessageShown = true;

            StartCoroutine(TypeText(victoryMessage, 0.04f));

            taskText.color = Color.green;
            taskText.fontSize = 36;

            taskText.gameObject.SetActive(true);

            source.clip = correct;
            source.Play();

            // Unlock a new skin
            if (!gameCompleted)
            {
                gameCompleted = true;

                if (SkinManager.Instance != null)
                {
                    SkinManager.Instance.UnlockSkin(4);
                    Debug.Log($"New skin unlocked: {SkinManager.Instance.skinNames[4]}");
                }

                StartCoroutine(TypeText(victoryMessage, 0.04f));
            }
        }

        // Return to class
        if (gameCompleted && Input.GetKeyDown(KeyCode.Return))
        {
            startingPositionDynamic.initialValue = startingPositionPreviousScene.initialValue;
            SceneManager.LoadScene("LeftClassInterior");
        }
    }

    string RemoveRichTextTags(string message)
    {
        System.Text.RegularExpressions.Regex richTextRegex = new System.Text.RegularExpressions.Regex(@"<.*?>");
        return richTextRegex.Replace(message, "");
    }

    string InsertRichTags(string partialPlainText, string richMessage)
    {
        int plainIndex = 0;
        int richIndex = 0;
        string result = "";

        while (richIndex < richMessage.Length)
        {
            char currentChar = richMessage[richIndex];

            if (currentChar == '<')
            {
                int tagEnd = richMessage.IndexOf('>', richIndex);
                result += richMessage.Substring(richIndex, tagEnd - richIndex + 1);
                richIndex = tagEnd + 1;
            }
            else if (plainIndex < partialPlainText.Length && currentChar == partialPlainText[plainIndex])
            {
                result += currentChar;
                plainIndex++;
                richIndex++;
            }
            else
            {
                richIndex++;
            }
        }

        return result;
    }

    // Digit text Coroutine
    IEnumerator TypeText(string message, float typingSpeed)
    {
        taskText.text = "";
        string richMessage = message;
        string plainMessage = RemoveRichTextTags(message);

        for (int i = 0; i < plainMessage.Length; i++)
        {
            string displayedText = InsertRichTags(plainMessage.Substring(0, i + 1), richMessage);
            taskText.text = displayedText;
            yield return new WaitForSeconds(typingSpeed);
        }

        taskText.text = richMessage;
    }
}










