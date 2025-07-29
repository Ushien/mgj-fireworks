/// <summary>
/// Gère les interactions avec les seaux de l'étagère
/// - sélection/déselection avec drag et retour
/// - communique à powderManager et MixingManager quelle poudre est utilisée
/// </summary>

using UnityEngine;

// ------------------------------------------------------------------------------------------------

/// <summary>
/// Gère les interactions avec les seaux de l'étagère
/// </summary>
public class Bucket : MonoBehaviour
{
    // Variables
    // =========
    [Header("Referencing (to change)")]
    [SerializeField]
    private Camera powderCamera;

    [Header("Bucket Parameters")]
    [SerializeField]
    private int index;
    [SerializeField]
    public float rotSpeed = 1f;
    [SerializeField]
    public float returnSpeed = 5f;

    private Vector3 basePosition;
    private const float frontOffset = -40f;
    private bool isGrabbed = false;

    // --------------------------------------------------------------------------------------------
    #region Start/Update

    // Start/Update
    // ============
    void Start()
    {
        basePosition = transform.position;
    }

    void Update()
    {
        if(!MixingManager.Instance.inStudio)
            return;
        HandleMoving();
        HandleInput();
    }

    #endregion
    // --------------------------------------------------------------------------------------------
    #region Methods

    // Gère le déplacement du seau
    // ===========================
    void HandleMoving(){
        // Déplacement si utilisé
        // ----------------------
        if (isGrabbed)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,130), Time.deltaTime * rotSpeed);
            Vector3 mouseWorldPos = powderCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3 (mouseWorldPos.x, mouseWorldPos.y, transform.position.z);
        }

        // Retour si inutilisé
        // -------------------
        if(!isGrabbed)
        {
            transform.position = Vector3.Lerp(transform.position, basePosition, Time.deltaTime * returnSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,0), Time.deltaTime * rotSpeed);
        }
    }

    // Gère la sélection du seau
    // =========================
    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && isClicked())
            SelectBucket();
        if (Input.GetMouseButtonUp(0) && isGrabbed)
            DeselectBucket();
    }

    // Raycast pour check si on clique sur le bucket
    // =============================================
    bool isClicked()
    {
        Ray ray = powderCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit);
        return hit.collider != null && hit.collider.gameObject == gameObject;
    }

    // Sélection du bucket
    // ===================
    void SelectBucket()
    {
        isGrabbed = true;
        FireworkAudioManager.Instance.PlayBipSound();
        transform.position = new Vector3(transform.position.x, transform.position.y, basePosition.z + frontOffset); // Amène devant la poudre
        PowderManager.Instance.SetParticleColor(index);
        PowderManager.Instance.SetParticleAmount(PowderManager.Instance.flux);
        MixingManager.Instance.currentCharge = 0;
        MixingManager.Instance.currentIndex = index;
    }

    // Déselection du bucket
    // =====================
    void DeselectBucket()
    {
        isGrabbed = false;
        transform.position = new Vector3 (transform.position.x, transform.position.y, basePosition.z); // Amène derrière la poudre
        PowderManager.Instance.SetParticleAmount(0);
    }

    #endregion
    // --------------------------------------------------------------------------------------------
}
