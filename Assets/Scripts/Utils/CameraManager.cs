using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ------------------------------------------------------------------------------------------------

public class CameraManager : MonoBehaviour
{
    
    // Variables
    // =========
    [Header("Cameras settings")]
    [SerializeField] private Camera powderCamera;
    [SerializeField] private Camera shootCamera;
    [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private float lerpThreshold = 0.4f;
    [SerializeField] private Button launchButton;
    private float saveCameraOrtho;
    private Vector3 saveCameraPosition;
    private bool goToPowder = false;
    private bool goToShoot = false;

    [Header("Fading du studio")]
    [SerializeField] private float fadeSpeed = 3f;
    [SerializeField] private float fadeScale = 0.8f;
    private bool fading = true;
    
    [Header("GameObjects")]
    [SerializeField] private GameObject powderUI;
    [SerializeField] private GameObject shootUI;
    [SerializeField] private GameObject powderPrefab;
    [SerializeField] private GameObject powderStudio;
    [SerializeField] private List<GameObject> toFade;
    [SerializeField] private GameObject studioBckgrnd;


    // --------------------------------------------------------------------------------------------
    #region Start/Update


    // Start
    // =====
    void Start()
    {
        powderCamera.enabled = false;
        powderStudio.transform.localScale = new Vector3(fadeScale, fadeScale, fadeScale);
    }

    // Update
    // ======
    void Update()
    {
        HandleFading();
        HandleCameraMovement();
    }


    #endregion
    // ----------------------------------------------------------------------------------
    #region Methods


    // Handles camera movement
    // =======================
    private void HandleCameraMovement()
    {
        if (goToPowder)
            LerpCamera(shootCamera, powderCamera);
        else if (goToShoot)
            LerpCamera(powderCamera, shootCamera);
    }


    // Handles the Lerp
    // ================
    private void LerpCamera(Camera fromCam, Camera toCam)
    {
        // position
        fromCam.transform.position = Vector3.Lerp(
            fromCam.transform.position,
            toCam.transform.position,
            Time.deltaTime * lerpSpeed
        );

        // FOV
        fromCam.orthographicSize = Mathf.Lerp(
            fromCam.orthographicSize,
            toCam.orthographicSize,
            Time.deltaTime * lerpSpeed
        );

        // critère d'arrêt
        if (Vector3.Distance(fromCam.transform.position, toCam.transform.position) < lerpThreshold)
        {
            fromCam.enabled = false;
            toCam.enabled = true;
            goToShoot = false;
            goToPowder = false;
            fromCam.orthographicSize = saveCameraOrtho;
            fromCam.transform.position = saveCameraPosition;
            if(toCam == powderCamera){
                launchButton.gameObject.SetActive(true);
                PowderManager.Instance.inStudio = true;
            }
            else
                shootUI.SetActive(true);
        }
    }

    // Switch de caméra
    // ================
    public void SwitchCamera()
    {
        if (powderCamera.enabled)
        {
            goToShoot = true;
            saveCameraOrtho = powderCamera.orthographicSize;
            saveCameraPosition = powderCamera.transform.position;
            fading = true;
            PowderManager.Instance.inStudio = false;
            TogglePowderUI(false);
        }
        else
        {
            goToPowder = true;
            saveCameraOrtho = shootCamera.orthographicSize;
            saveCameraPosition = shootCamera.transform.position;
            fading = false;
            shootUI.SetActive(false);
            TogglePowderUI(true);
        }
    }
    
    // --------------------------------------------------------------------------------------------

    // Gère le fading
    // ==============
    private void HandleFading()
    {
        float targetFade = fading ? 0f : 1f;
        Vector3 targetScale = fading ? Vector3.one * fadeScale : Vector3.one;

        FadeObjects(toFade, targetFade);
        FadeBackground(studioBckgrnd, targetFade);
        powderStudio.transform.localScale = Vector3.Lerp(
            powderStudio.transform.localScale,
            targetScale,
            Time.deltaTime * fadeSpeed
        );
    }

    // Fading des objets
    // =================
    private void FadeObjects(List<GameObject> objects, float targetFade)
    {
        foreach (GameObject obj in objects)
        {
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            float faded = Mathf.Lerp(sr.color.r, targetFade, Time.deltaTime * fadeSpeed);
            sr.color = new Color(faded, faded, faded, 1f);
        }
    }

    // Fading du background
    // ====================
    private void FadeBackground(GameObject bgObject, float targetAlpha)
    {
        SpriteRenderer bgRenderer = bgObject.GetComponent<SpriteRenderer>();
        Color c = bgRenderer.color;
        float newAlpha = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
        bgRenderer.color = new Color(c.r, c.g, c.b, newAlpha);
    }

    // Toggle powder UI
    // ================
    public void TogglePowderUI(bool activate)
    {
        powderUI.SetActive(activate);
        powderPrefab.SetActive(activate);
    }

    #endregion
    // --------------------------------------------------------------------------------------------
}
