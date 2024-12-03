using UnityEngine;
using UnityEngine.UI;

public class SkinSelector : MonoBehaviour
{
    public Button[] skinButtons; // Bottoni per ciascuna skin
    public Text[] skinNameTexts; // Testi per visualizzare i nomi delle skin
    public Text selectedSkinNameText; // Riferimento al testo del nome della skin selezionata
    public Image selectedSkinImage; // Riferimento all'immagine che mostra la skin selezionata

    private SkinManager skinManager; // Riferimento locale a SkinManager

    private void Start()
    {
        // Ottieni il riferimento a SkinManager tramite il GameManager
        if (GameManager.Instance != null)
        {
            skinManager = GameManager.Instance.skinManager;
        }

        if (skinManager == null)
        {
            Debug.LogError("SkinManager non trovato tramite GameManager!");
            return;
        }

        // Inizializza il sistema dopo aver collegato il SkinManager
        InitializeSelectedSkin();
        AddButtonListeners();
        UpdateSkinButtonNames();
        UpdateSkinButtonInteractableStatus();
    }

    private void AddButtonListeners()
    {
        for (int i = 0; i < skinButtons.Length; i++)
        {
            int index = i;
            skinButtons[i].onClick.AddListener(() => OnSkinSelected(index));
        }
    }

    private void OnSkinSelected(int skinIndex)
    {
        if (skinManager != null)
        {
            skinManager.ChangeSkin(skinIndex);
            UpdateSelectedSkinImageFromButton(skinIndex);
        }
        else
        {
            Debug.LogError("SkinManager non assegnato correttamente!");
        }
    }

    // Inizializza l'immagine e il nome della skin attualmente selezionata
    public void InitializeSelectedSkin()
    {
        // Ottieni l'indice della skin attualmente selezionata
        int selectedSkinIndex = PlayerPrefs.GetInt("SelectedSkin", 0); // Default a 0

        // Assicurati che l'indice sia valido
        if (selectedSkinIndex >= 0 && selectedSkinIndex < skinManager.GetSkinCount())
        {
            // Imposta l'immagine della skin corrente
            selectedSkinImage.sprite = skinManager.skins[selectedSkinIndex];

            // Imposta il nome della skin corrente
            selectedSkinNameText.text = skinManager.skinNames[selectedSkinIndex];

            // Aggiorna anche la posizione, la dimensione e la scala
            UpdateSelectedSkinImageFromButton(selectedSkinIndex);
        }
        else
        {
            Debug.LogError("Indice della skin selezionata non valido!");
        }
    }

    // Aggiorna i nomi sotto i bottoni
    void UpdateSkinButtonNames()
    {
        // Chiedi a SkinManager di restituire i nomi delle skin
        for (int i = 0; i < skinNameTexts.Length; i++)
        {
            // Verifica che l'indice della skin sia valido
            if (skinManager != null && i < skinManager.GetSkinCount())
            {
                skinNameTexts[i].text = skinManager.GetSkinName(i); // Ottieni il nome della skin dal SkinManager
            }
        }
    }

    // Aggiorna lo stato dei bottoni in base alle skin sbloccate
    void UpdateSkinButtonInteractableStatus()
    {
        for (int i = 0; i < skinButtons.Length; i++)
        {
            if (skinManager != null && i < skinManager.unlockedSkins.Length)
            {
                // Abilita o disabilita il pulsante in base allo stato di sblocco
                skinButtons[i].interactable = skinManager.unlockedSkins[i];
            }
            else
            {
                Debug.LogWarning($"Indice {i} fuori dai limiti di unlockedSkins o SkinManager non configurato.");
            }
        }
    }

    // Metodo per aggiornare l'immagine e il nome della skin selezionata (già esistente, rimane invariato)
    void UpdateSelectedSkinImageFromButton(int buttonIndex)
    {
        Debug.Log($"UpdateSelectedSkinImageFromButton chiamato con buttonIndex: {buttonIndex}");

        if (skinButtons == null || skinButtons.Length == 0)
        {
            Debug.LogError("La lista dei bottoni è vuota o non inizializzata!");
            return;
        }

        if (buttonIndex < 0 || buttonIndex >= skinButtons.Length)
        {
            Debug.LogError($"Indice del bottone non valido: {buttonIndex}. Deve essere tra 0 e {skinButtons.Length - 1}.");
            return;
        }

        if (selectedSkinImage == null || selectedSkinNameText == null)
        {
            Debug.LogError("SelectedSkinImage o SelectedSkinName non sono configurati correttamente nell'Inspector!");
            return;
        }

        // Ottieni il RectTransform del bottone selezionato
        RectTransform buttonRect = skinButtons[buttonIndex].GetComponent<RectTransform>();
        if (buttonRect != null)
        {
            RectTransform selectedSkinRect = selectedSkinImage.GetComponent<RectTransform>();
            if (selectedSkinRect != null)
            {
                // Aggiorna scala e dimensioni dell'immagine
                selectedSkinRect.localScale = buttonRect.localScale;
                selectedSkinRect.sizeDelta = buttonRect.sizeDelta;

                // Aggiorna lo sprite dell'immagine
                selectedSkinImage.sprite = skinManager.skins[buttonIndex];

                // Aggiorna il nome della skin
                selectedSkinNameText.text = skinManager.skinNames[buttonIndex];

                // Ottieni il RectTransform del nome della skin del bottone
                RectTransform skinNameRect = skinNameTexts[buttonIndex].GetComponent<RectTransform>();
                if (skinNameRect != null)
                {
                    // Copia la posizione, le dimensioni e la scala del Text del bottone
                    selectedSkinNameText.rectTransform.localPosition = skinNameRect.localPosition;
                    selectedSkinNameText.rectTransform.sizeDelta = skinNameRect.sizeDelta;
                    selectedSkinNameText.rectTransform.localScale = skinNameRect.localScale;

                    // Assicurati che la scala sia corretta (mantieni la scala originale)
                    selectedSkinNameText.rectTransform.localScale = skinNameRect.localScale;
                }

                // Forza il ridisegno della UI
                Canvas.ForceUpdateCanvases();
                selectedSkinImage.SetAllDirty();

                Debug.Log($"SelectedSkin aggiornata: scala {selectedSkinRect.localScale}, dimensioni {selectedSkinRect.sizeDelta}, sprite {selectedSkinImage.sprite.name}, nome {selectedSkinNameText.text}");
            }
            else
            {
                Debug.LogError("SelectedSkinImage non ha un componente RectTransform!");
            }
        }
        else
        {
            Debug.LogError($"Il bottone all'indice {buttonIndex} non ha un RectTransform!");
        }
    }
}
