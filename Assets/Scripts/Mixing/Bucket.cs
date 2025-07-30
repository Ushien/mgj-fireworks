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
    private float rotSpeed = 1f;
    [SerializeField]
    private float returnSpeed = 5f;
    [SerializeField]
    private float tolerance = 0.1f;

    private Vector3 basePosition;
    private const float frontOffset = -40f;
    private bool isGrabbed = false;
    private bool firstGrab = true;

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
        if(!PowderManager.Instance.inStudio)
        {
            if(!firstGrab)
                firstGrab = true;
            return;
        }
        
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
        if(!isGrabbed && Vector3.Distance(transform.position, basePosition) >= tolerance && !firstGrab)
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
        AudioManager.Instance.PlayTakeSound();
        transform.position = new Vector3(transform.position.x, transform.position.y, basePosition.z + frontOffset); // Amène devant la poudre
        PowderManager.Instance.ChangePowder(index);
        PowderManager.Instance.isPouring = true;
        firstGrab = false;
    }

    // Déselection du bucket
    // =====================
    void DeselectBucket()
    {
        isGrabbed = false;
        AudioManager.Instance.PlayOffSound();
        transform.position = new Vector3 (transform.position.x, transform.position.y, basePosition.z); // Amène derrière la poudre
        PowderManager.Instance.SetParticleAmount(0);
        PowderManager.Instance.powderCount = 0;
        PowderManager.Instance.isPouring = false;
    }

    #endregion
    // --------------------------------------------------------------------------------------------
}
