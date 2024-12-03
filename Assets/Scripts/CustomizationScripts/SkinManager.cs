using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public Sprite[] skins; // Array di sprite delle skin
    public RuntimeAnimatorController[] animators; // Array di Animator Controllers per ciascuna skin
    public Vector2[] skinOffsets; // Array di offset per il Box Collider
    public Vector2[] skinSizes; // Array di dimensioni per il Box Collider
    public Vector3[] skinScales; // Array di scale personalizzate per ciascuna skin
    public string[] skinNames; // Array di nomi delle skin
    public bool[] unlockedSkins; // Array che tiene traccia delle skin sbloccate (true se sbloccata)

    private const string SelectedSkinKey = "SelectedSkin"; // Chiave per salvare la skin
    private const string SelectedScaleKey = "SelectedScale"; // Chiave per salvare la scala

    private Transform playerTransform; // Riferimento al Transform del player

    // Variabile per garantire che ci sia solo un'istanza del SkinManager
    public static SkinManager Instance;

    private void Awake()
    {
        // Se esiste già un'istanza di SkinManager, distruggiamo questa nuova istanza
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Altrimenti, impostiamo l'istanza e non distruggiamo questo oggetto al cambio di scena
        Instance = this;
        DontDestroyOnLoad(gameObject);  // Mantieni il SkinManager tra le scene
    }

    private void Start()
    {
        // Trova il player alla partenza
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Carica la skin salvata
        int savedSkinIndex = PlayerPrefs.GetInt(SelectedSkinKey, 0); // Carica l'indice salvato (0 se non esiste)
        ChangeSkin(savedSkinIndex);

        // Carica e applica la scala salvata
        Vector3 savedScale = LoadScale(savedSkinIndex);
        playerTransform.localScale = savedScale;

        Debug.Log($"Scala caricata e applicata per la skin {savedSkinIndex}: {savedScale}");
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

        // Cambia lo sprite del player
        SpriteRenderer playerSpriteRenderer = playerTransform.GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sprite = skins[skinIndex];
        }

        // Cambia l'Animator Controller del player
        Animator playerAnimator = playerTransform.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            playerAnimator.runtimeAnimatorController = animators[skinIndex];
        }

        // Gestisci BoxCollider, offset e dimensioni
        BoxCollider2D playerBoxCollider = playerTransform.GetComponent<BoxCollider2D>();
        if (playerBoxCollider != null)
        {
            if (skinIndex < skinSizes.Length)
            {
                playerBoxCollider.size = skinSizes[skinIndex];
            }
            if (skinIndex < skinOffsets.Length)
            {
                playerBoxCollider.offset = skinOffsets[skinIndex];
            }
        }

        // Cambia la scala del player
        if (skinIndex < skinScales.Length)
        {
            playerTransform.localScale = skinScales[skinIndex];
        }

        // Salva la skin selezionata
        PlayerPrefs.SetInt(SelectedSkinKey, skinIndex);
        PlayerPrefs.SetFloat($"{SelectedScaleKey}_X_{skinIndex}", playerTransform.localScale.x);
        PlayerPrefs.SetFloat($"{SelectedScaleKey}_Y_{skinIndex}", playerTransform.localScale.y);
        PlayerPrefs.SetFloat($"{SelectedScaleKey}_Z_{skinIndex}", playerTransform.localScale.z);
        PlayerPrefs.Save();

        Debug.Log($"Skin cambiata in: {skinNames[skinIndex]}");
    }


    // Restituisce il nome della skin dato l'indice
    public string GetSkinName(int index)
    {
        if (index >= 0 && index < skinNames.Length)
        {
            return skinNames[index];
        }
        return "Unknown Skin"; // Default se l'indice è fuori dal range
    }

    // Restituisce il numero di skin disponibili
    public int GetSkinCount()
    {
        return skinNames.Length;
    }

    private Vector3 LoadScale(int skinIndex)
    {
        // Verifica se i valori di scala salvati nei PlayerPrefs per questa skin esistono
        // Se non esistono, carica la scala predefinita dalla lista 'skinScales'
        if (skinIndex >= 0 && skinIndex < skinScales.Length)
        {
            // Carica i valori di scala dai PlayerPrefs, se non esistono applica i valori da skinScales
            Vector3 scale = new Vector3(
                PlayerPrefs.GetFloat($"{SelectedScaleKey}_X_{skinIndex}", skinScales[skinIndex].x),
                PlayerPrefs.GetFloat($"{SelectedScaleKey}_Y_{skinIndex}", skinScales[skinIndex].y),
                PlayerPrefs.GetFloat($"{SelectedScaleKey}_Z_{skinIndex}", skinScales[skinIndex].z)
            );

            return scale;
        }

        // Se l'indice della skin è fuori dai limiti, restituisci una scala predefinita
        return Vector3.one; // Default scale
    }
}
