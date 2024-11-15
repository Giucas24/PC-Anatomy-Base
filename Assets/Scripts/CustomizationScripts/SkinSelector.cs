using UnityEngine;

public class SkinSelector : MonoBehaviour
{
    public Sprite[] skins; // Array di sprite delle skin
    public RuntimeAnimatorController[] animators; // Array di Animator Controllers per ciascuna skin
    public SpriteRenderer playerSpriteRenderer; // Riferimento al componente SpriteRenderer del giocatore
    public Animator playerAnimator; // Riferimento all'Animator del giocatore
    private BoxCollider2D boxCollider; // Riferimento al BoxCollider2D del giocatore
    public Vector2[] skinOffsets; // Array di offset per il Box Collider
    public Vector2[] skinSizes; // Array di dimensioni per il Box Collider
    public Vector3[] skinScales; // Array di scale personalizzate per ciascuna skin
    private const string SelectedSkinKey = "SelectedSkin"; // Chiave per salvare la skin
    private const string SelectedScaleKey = "SelectedScale"; // Chiave per salvare la scala

    void Start()
    {
        // Ottieni il riferimento al BoxCollider2D del personaggio
        boxCollider = GetComponent<BoxCollider2D>();

        // Carica la skin salvata
        int savedSkinIndex = PlayerPrefs.GetInt(SelectedSkinKey, 0); // Carica l'indice salvato (0 se non esiste)
        ChangeSkin(savedSkinIndex);

        // Carica la scala salvata
        Vector3 savedScale = LoadScale(savedSkinIndex);
        transform.localScale = savedScale;
    }

    public void ChangeSkin(int skinIndex)
    {
        // Verifica che l'indice sia valido
        if (skinIndex >= 0 && skinIndex < skins.Length)
        {
            // Cambia lo sprite della skin
            playerSpriteRenderer.sprite = skins[skinIndex];

            // Cambia l'Animator Controller per la skin selezionata
            playerAnimator.runtimeAnimatorController = animators[skinIndex];

            // Ridimensiona il personaggio per adattarlo alla nuova skin
            if (skinIndex < skinScales.Length)
            {
                Vector3 targetScale = skinScales[skinIndex];
                transform.localScale = targetScale;

                // Salva la scala selezionata
                SaveScale(skinIndex, targetScale);
            }
            else
            {
                Debug.LogWarning("Nessuna scala specificata per questa skin. Scala predefinita utilizzata.");
                transform.localScale = Vector3.one;
            }

            // Aggiorna le dimensioni e la posizione del Box Collider
            if (boxCollider != null)
            {
                // Usa le dimensioni specifiche per la skin selezionata
                if (skinIndex < skinSizes.Length)
                {
                    boxCollider.size = skinSizes[skinIndex];
                }
                else
                {
                    Debug.LogWarning("Nessuna dimensione specificata per questa skin. Dimensioni predefinite utilizzate.");
                    boxCollider.size = new Vector2(1f, 1f); // Dimensione fissa desiderata
                }

                // Usa l'offset specifico per la skin selezionata
                if (skinIndex < skinOffsets.Length)
                {
                    boxCollider.offset = skinOffsets[skinIndex];
                }
                else
                {
                    Debug.LogWarning("Nessun offset specificato per questa skin. Offset predefinito utilizzato.");
                    boxCollider.offset = Vector2.zero; // Usa un offset predefinito
                }
            }

            // Salva la skin selezionata
            PlayerPrefs.SetInt(SelectedSkinKey, skinIndex);
            PlayerPrefs.Save();

            Debug.Log("Skin cambiata in: " + skins[skinIndex].name);
        }
        else
        {
            Debug.LogError("Indice della skin non valido");
        }
    }

    private void SaveScale(int skinIndex, Vector3 scale)
    {
        // Salva la scala per ciascun asse (x, y, z)
        PlayerPrefs.SetFloat($"{SelectedScaleKey}_X_{skinIndex}", scale.x);
        PlayerPrefs.SetFloat($"{SelectedScaleKey}_Y_{skinIndex}", scale.y);
        PlayerPrefs.SetFloat($"{SelectedScaleKey}_Z_{skinIndex}", scale.z);
        PlayerPrefs.Save();
    }

    private Vector3 LoadScale(int skinIndex)
    {
        // Carica la scala per ciascun asse (x, y, z)
        float x = PlayerPrefs.GetFloat($"{SelectedScaleKey}_X_{skinIndex}", 1f); // Default 1
        float y = PlayerPrefs.GetFloat($"{SelectedScaleKey}_Y_{skinIndex}", 1f); // Default 1
        float z = PlayerPrefs.GetFloat($"{SelectedScaleKey}_Z_{skinIndex}", 1f); // Default 1
        return new Vector3(x, y, z);
    }
}
