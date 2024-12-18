using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public Sprite[] skins;
    public RuntimeAnimatorController[] animators;
    public Vector2[] boxColliderOffsets;
    public Vector2[] boxColliderSizes;
    public Vector3[] skinScales;
    public string[] skinNames;
    public bool[] unlockedSkins;

    private const string SelectedSkinKey = "SelectedSkin";  // Key for PlayerPrefs
    private const string SelectedScaleKey = "SelectedScale";    // Key for PlayerPrefs

    private Transform playerTransform;

    public static SkinManager Instance;

    // Once the SkinManager component is loaded in the scene we run this method Awake
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;    // Reference to the current instance
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Load saved skin
        int savedSkinIndex = PlayerPrefs.GetInt(SelectedSkinKey, 0);    // 0 is the default value to pass in case SelectedSkin is not saved yet
        ChangeSkin(savedSkinIndex);

        // Apply correct scale
        Vector3 savedScale = LoadScale(savedSkinIndex);
        playerTransform.localScale = savedScale;
    }


    public void UnlockSkin(int skinIndex)
    {
        if (skinIndex >= 0 && skinIndex < unlockedSkins.Length)
        {
            unlockedSkins[skinIndex] = true;
            Debug.Log("Skin sbloccata: " + skinIndex);
        }
        else
        {
            Debug.LogWarning("Indice skin non valido: " + skinIndex);
        }
    }


    public void ChangeSkin(int skinIndex)
    {
        if (skinIndex < 0 || skinIndex >= skins.Length) return;

        // Change sprite
        SpriteRenderer playerSpriteRenderer = playerTransform.GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sprite = skins[skinIndex];
        }

        // Change animator controller
        Animator playerAnimator = playerTransform.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            playerAnimator.runtimeAnimatorController = animators[skinIndex];
        }

        // Change boxcollider's size and offest
        BoxCollider2D playerBoxCollider = playerTransform.GetComponent<BoxCollider2D>();
        if (playerBoxCollider != null)
        {
            if (skinIndex < boxColliderSizes.Length)
            {
                playerBoxCollider.size = boxColliderSizes[skinIndex];
            }
            if (skinIndex < boxColliderOffsets.Length)
            {
                playerBoxCollider.offset = boxColliderOffsets[skinIndex];
            }
        }

        // Change player's scale
        if (skinIndex < skinScales.Length)
        {
            playerTransform.localScale = skinScales[skinIndex];
        }

        // Update PlayerPrefs
        PlayerPrefs.SetInt(SelectedSkinKey, skinIndex);
        PlayerPrefs.SetFloat($"{SelectedScaleKey}_X_{skinIndex}", playerTransform.localScale.x);
        PlayerPrefs.SetFloat($"{SelectedScaleKey}_Y_{skinIndex}", playerTransform.localScale.y);
        PlayerPrefs.SetFloat($"{SelectedScaleKey}_Z_{skinIndex}", playerTransform.localScale.z);
        PlayerPrefs.Save();

        Debug.Log($"Skin cambiata in: {skinNames[skinIndex]}");
    }

    public string GetSkinName(int index)
    {
        if (index >= 0 && index < skinNames.Length)
        {
            return skinNames[index];
        }
        else return "Unknown skin name";

    }

    public int GetSkinCount()
    {
        return skinNames.Length;
    }

    private Vector3 LoadScale(int skinIndex)
    {
        if (skinIndex >= 0 && skinIndex < skinScales.Length)
        {
            Vector3 scale = new Vector3(
                PlayerPrefs.GetFloat($"{SelectedScaleKey}_X_{skinIndex}", skinScales[skinIndex].x),
                PlayerPrefs.GetFloat($"{SelectedScaleKey}_Y_{skinIndex}", skinScales[skinIndex].y),
                PlayerPrefs.GetFloat($"{SelectedScaleKey}_Z_{skinIndex}", skinScales[skinIndex].z)
            );

            return scale;
        }
        return Vector3.one;
    }
}
