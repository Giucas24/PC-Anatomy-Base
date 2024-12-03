using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Durata della dissolvenza in secondi
    public float fadeDuration = 1.5f;

    public Canvas gameCanvas; // Riferimento al GameCanvas
    public Text taskText;
    private string initialMessage = "TRASCINA LE COMPONENTI HARDWARE DELLA SCHEDA MADRE NELLE POSIZIONI CORRETTE, SIMULANDO L'ASSEMBLAGGIO DI UN PC.";
    private string victoryMessage = "COMPLIMENTI! HAI SBLOCCATO UN NUOVO ASPETTO! <size=24><color=black>PREMI <b>INVIO</b> PER RITORNARE IN CLASSE!</color></size>";


    // Aggiungiamo una variabile per tracciare lo stato di completamento
    private bool gameCompleted = false;




    public AudioSource source;
    public AudioClip correct;
    public AudioClip incorrect;

    // Start is called before the first frame update
    void Start()
    {
        // Salva le posizioni iniziali degli oggetti
        cpuInitialPos = cpu.transform.position;
        expansionInitialPos = expansion.transform.position;
        videoInitialPos = video.transform.position;
        chipsetInitialPos = chipset.transform.position;
        ramInitialPos = ram.transform.position;
        batteriaCMOSInitialPos = batteriaCMOS.transform.position;
        biosInitialPos = bios.transform.position;
        porteIoInitialPos = porteIo.transform.position;

        // Blocchi iniziali disabilitati
        cpuLocked = false;
        expansionLocked = false;
        videoLocked = false;
        chipsetLocked = false;
        ramLocked = false;
        batteriaCMOSLocked = false;
        biosLocked = false;
        porteIoLocked = false;

        // Nasconde il messaggio di vittoria
        vinto.SetActive(false);

        // Configura il CanvasGroup del finalImage
        finalImageCanvasGroup = finalImage.GetComponent<CanvasGroup>();
        if (finalImageCanvasGroup != null)
        {
            finalImageCanvasGroup.alpha = 0; // Inizialmente invisibile
            finalImage.SetActive(false);    // Nasconde finalImage
        }
        else
        {
            Debug.LogError("CanvasGroup non trovato su finalImage. Aggiungilo nell'Inspector.");
        }

        // Mostra il messaggio iniziale con l'effetto di digitazione
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
            Debug.LogError("GameCanvas non assegnato. Assegna il riferimento al canvas nel pannello Inspector.");
            return;
        }

        RectTransform canvasRectTransform = gameCanvas.GetComponent<RectTransform>();

        if (canvasRectTransform == null)
        {
            Debug.LogError("RectTransform non trovato nel GameCanvas.");
            return;
        }

        // Converte la posizione del mouse nello spazio locale del canvas
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            Input.mousePosition,
            null, // Per Screen Space Overlay, la camera è null
            out localPoint
        );

        // Applica la posizione locale al RectTransform dell'oggetto
        RectTransform elementRectTransform = element.GetComponent<RectTransform>();

        if (elementRectTransform == null)
        {
            Debug.LogError($"RectTransform non trovato nell'elemento: {element.name}");
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
                Debug.LogError($"RectTransform non trovato su {element.name} o {targetElement.name}.");
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


    // Update is called once per frame
    private bool victoryMessageShown = false;

    void Update()
    {
        if (!victoryMessageShown && cpuLocked && expansionLocked && videoLocked && chipsetLocked && ramLocked && batteriaCMOSLocked && biosLocked && porteIoLocked && finalImageCanvasGroup != null && finalImageCanvasGroup.alpha == 0)
        {
            victoryMessageShown = true; // Segna che il messaggio è stato mostrato

            // Avvia la visualizzazione del messaggio di vittoria con effetto digitazione
            StartCoroutine(TypeText(victoryMessage, 0.04f));

            // Cambia il colore del testo in verde
            taskText.color = Color.green;
            taskText.fontSize = 36;

            // Assicurati che il TaskText sia visibile
            taskText.gameObject.SetActive(true);

            // Riproduci il suono di vittoria
            source.clip = correct;
            source.Play();

            // Sblocca una nuova skin tramite SkinManager
            if (!gameCompleted)
            {
                gameCompleted = true; // Imposta il gioco come completato

                if (SkinManager.Instance != null)
                {
                    SkinManager.Instance.UnlockSkin(6);
                    Debug.Log("Nuova skin sbloccata!");
                }

                // Mostra un messaggio di congratulazioni per la skin sbloccata
                StartCoroutine(TypeText(victoryMessage, 0.04f));
            }
        }

        // Gestione del ritorno alla scena precedente con il tasto Invio
        if (gameCompleted && Input.GetKeyDown(KeyCode.Return))
        {
            string previousScene = PlayerPrefs.GetString("PreviousScene", "");
            if (!string.IsNullOrEmpty(previousScene))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(previousScene);
            }
            else
            {
                Debug.LogWarning("Nessuna scena precedente salvata nei PlayerPrefs!");
            }
        }
    }




    // Metodo per chiudere la scena corrente
    private void CloseCurrentScene()
    {
        Debug.Log("Tornando alla scena precedente...");

        // Se stai eseguendo l'app come standalone:
        Application.Quit();

        // Nota: Application.Quit() non funziona nell'editor di Unity.
        // Durante i test, puoi simulare il ritorno alla scena precedente:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Termina il play mode
#endif
    }




    // Coroutine per la dissolvenza
    private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 1; // Assicura che l'alpha sia esattamente 1 alla fine
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

            if (currentChar == '<') // Identifica un tag
            {
                // Aggiunge il tag completo
                int tagEnd = richMessage.IndexOf('>', richIndex);
                result += richMessage.Substring(richIndex, tagEnd - richIndex + 1);
                richIndex = tagEnd + 1;
            }
            else if (plainIndex < partialPlainText.Length && currentChar == partialPlainText[plainIndex])
            {
                // Aggiunge il carattere visibile
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



    // Coroutine per l'effetto di digitazione
    IEnumerator TypeText(string message, float typingSpeed)
    {
        taskText.text = ""; // Pulisce il testo inizialmente
        string richMessage = message; // Copia il messaggio originale con i tag Rich Text
        string plainMessage = RemoveRichTextTags(message); // Rimuove i tag per il conteggio

        for (int i = 0; i < plainMessage.Length; i++)
        {
            // Ricostruisce il testo parziale aggiungendo progressivamente caratteri senza i tag
            string displayedText = InsertRichTags(plainMessage.Substring(0, i + 1), richMessage);
            taskText.text = displayedText;
            yield return new WaitForSeconds(typingSpeed); // Ritardo tra le lettere
        }

        taskText.text = richMessage; // Mostra il messaggio completo con i tag
    }





}










