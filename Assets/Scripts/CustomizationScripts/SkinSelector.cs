using UnityEngine;
using UnityEngine.UI;

public class SkinSelector : MonoBehaviour
{
    public Button[] skinButtons;
    public Text[] skinNameTexts;
    public Text selectedSkinNameText;
    public Image selectedSkinImage;

    private SkinManager skinManager;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            skinManager = GameManager.Instance.skinManager;
        }

        if (skinManager == null)
        {
            Debug.LogError("SkinManager is null from Start");
            return;
        }

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
            Debug.LogError("SkinManager is null from OnSkinSelected");
        }
    }

    // Show current selected skin
    public void InitializeSelectedSkin()
    {
        int selectedSkinIndex = PlayerPrefs.GetInt("SelectedSkin", 0);  // 0 is the default value to pass in case SelectedSkin is not saved yet

        if (selectedSkinIndex >= 0 && selectedSkinIndex < skinManager.GetSkinCount())
        {
            selectedSkinImage.sprite = skinManager.skins[selectedSkinIndex];

            selectedSkinNameText.text = skinManager.skinNames[selectedSkinIndex];

            UpdateSelectedSkinImageFromButton(selectedSkinIndex);
        }
        else
        {
            Debug.LogError("selectedSkinIndex not valid");
        }
    }

    void UpdateSkinButtonNames()
    {
        for (int i = 0; i < skinNameTexts.Length; i++)
        {
            if (skinManager != null && i < skinManager.GetSkinCount())
            {
                skinNameTexts[i].text = skinManager.GetSkinName(i);
            }
        }
    }

    // Update unlocked / locked buttons
    void UpdateSkinButtonInteractableStatus()
    {
        for (int i = 0; i < skinButtons.Length; i++)
        {
            if (skinManager != null && i < skinManager.unlockedSkins.Length)
            {
                skinButtons[i].interactable = skinManager.unlockedSkins[i];
            }
            else
            {
                Debug.Log($"SkinManager is null from UpdateSkinButtonInteractableStatus or Index {i} out of range of unlockedSkins");
            }
        }
    }

    // Update selected skin's sprite and name in the Skin Panel
    void UpdateSelectedSkinImageFromButton(int buttonIndex)
    {
        if (skinButtons == null || skinButtons.Length == 0)
        {
            Debug.LogError("skinButtons is null or empty");
            return;
        }

        if (buttonIndex < 0 || buttonIndex >= skinButtons.Length)
        {
            Debug.LogError($"buttonIndex is not valid: {buttonIndex}");
            return;
        }

        if (selectedSkinImage == null || selectedSkinNameText == null)
        {
            Debug.LogError("selectedSkinImage or selectedSkinNameText is null from UpdateSelectedSkinImageFromButton");
            return;
        }

        RectTransform buttonRect = skinButtons[buttonIndex].GetComponent<RectTransform>();
        if (buttonRect != null)
        {
            RectTransform selectedSkinRect = selectedSkinImage.GetComponent<RectTransform>();
            if (selectedSkinRect != null)
            {
                selectedSkinRect.localScale = buttonRect.localScale;
                selectedSkinRect.sizeDelta = buttonRect.sizeDelta;

                selectedSkinImage.sprite = skinManager.skins[buttonIndex];

                selectedSkinNameText.text = skinManager.skinNames[buttonIndex];

                RectTransform skinNameRect = skinNameTexts[buttonIndex].GetComponent<RectTransform>();
                if (skinNameRect != null)
                {
                    selectedSkinNameText.rectTransform.localPosition = skinNameRect.localPosition;
                    selectedSkinNameText.rectTransform.sizeDelta = skinNameRect.sizeDelta;
                    selectedSkinNameText.rectTransform.localScale = skinNameRect.localScale;
                }

                selectedSkinImage.SetAllDirty();    // Set the selectedSkinImage to dirty so in the next canvas update it will be updated
                Canvas.ForceUpdateCanvases();   // Force a Canvas update for all the canvases in the scene
            }
            else
            {
                Debug.LogError("selectedSkinImage doesn't have a RectTransform");
            }
        }
        else
        {
            Debug.LogError($"skinButtons.[{buttonIndex}] doesn't have a RectTransform");
        }
    }
}
