using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private Camera powderCamera;
    [SerializeField] private Camera shootCamera;
    [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private Button launchButton;
    private float saveCameraOrtho;
    private Vector3 saveCameraPosition;
    private bool goToPowder = false;
    private bool goToShoot = false;

    [Header("fading du studio")]
    [SerializeField]
    private float fadeSpeed = 3f;
    [SerializeField]
    private float fadeScale = 0.8f;
    [SerializeField]
    private List<GameObject> toFade;
    [SerializeField]
    private GameObject studioBckgrnd;
    private bool fading = true;
    
    // Camera transition setting
    public GameObject powderUI;
    public GameObject powderPrefab;
    public GameObject powderStudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powderCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleFading();
        if (goToPowder)
        {
            // lerp position
            shootCamera.transform.position = Vector3.Lerp(
                shootCamera.transform.position,
                powderCamera.transform.position,
                Time.deltaTime * lerpSpeed
            );

            // lerp fov
            shootCamera.orthographicSize = Mathf.Lerp(
                shootCamera.orthographicSize,
                powderCamera.orthographicSize,
                Time.deltaTime * lerpSpeed
            );

            if (Vector3.Distance(shootCamera.transform.position, powderCamera.transform.position) < 0.8f)
            {
                GoToPowderCamera();
            }
        }
        else if (goToShoot)
        {
            // lerp position
            powderCamera.transform.position = Vector3.Lerp(
                powderCamera.transform.position,
                shootCamera.transform.position,
                Time.deltaTime * lerpSpeed
            );

            // lerp fov
            powderCamera.orthographicSize = Mathf.Lerp(
                powderCamera.orthographicSize,
                shootCamera.orthographicSize,
                Time.deltaTime * lerpSpeed
            );

            if (Vector3.Distance(powderCamera.transform.position, shootCamera.transform.position) < 0.8f)
            {
                GoToShootCamera();
            }
        }
    }

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
            StartCoroutine(inStudioDelay(1.5f));
            TogglePowderUI(true);
        }
    }

    private void GoToShootCamera()
    {
        powderCamera.enabled = false;
        shootCamera.enabled = true;
        goToShoot = false;
        goToPowder = false;
        powderCamera.orthographicSize = saveCameraOrtho;
        powderCamera.transform.position = saveCameraPosition;
    }

    private void GoToPowderCamera()
    {
        powderCamera.enabled = true;
        shootCamera.enabled = false;
        goToShoot = false;
        goToPowder = false;
        shootCamera.orthographicSize = saveCameraOrtho;
        shootCamera.transform.position = saveCameraPosition;
        launchButton.gameObject.SetActive(true);
    }

    private void HandleFading(){
        if (fading) {
            foreach (GameObject objectToFade in toFade)
            {
                SpriteRenderer sr = objectToFade.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Color c = sr.color;
                    float faded = Mathf.Lerp(c.r, 0f, Time.deltaTime * fadeSpeed);
                    sr.color = new Color(faded, faded, faded, 1f);
                }
            }

            // Scale and background fade (keep this if background is still a 3D object)
            powderStudio.transform.localScale = Vector3.Lerp(powderStudio.transform.localScale,
                                                            new Vector3(fadeScale, fadeScale, fadeScale),
                                                            Time.deltaTime * fadeSpeed);
            
            SpriteRenderer bgRenderer = studioBckgrnd.GetComponent<SpriteRenderer>();
            if (bgRenderer != null) {
                Color bgColor = bgRenderer.color;
                float newAlpha = Mathf.Lerp(bgColor.a, 0f, Time.deltaTime * fadeSpeed);
                bgRenderer.color = new Color(bgColor.r, bgColor.g, bgColor.b, newAlpha);
            }
        }

        if (!fading){
            foreach (GameObject objectToFade in toFade)
            {
                SpriteRenderer sr = objectToFade.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Color c = sr.color;
                    float faded = Mathf.Lerp(c.r, 1f, Time.deltaTime * fadeSpeed);
                    sr.color = new Color(faded, faded, faded, 1f);
                }
            }

            // Scale and background fade (keep this if background is still a 3D object)
            powderStudio.transform.localScale = Vector3.Lerp(powderStudio.transform.localScale,
                                                            new Vector3(1f, 1f, 1f),
                                                            Time.deltaTime * fadeSpeed);
            
            SpriteRenderer bgRenderer = studioBckgrnd.GetComponent<SpriteRenderer>();
            if (bgRenderer != null) {
                Color bgColor = bgRenderer.color;
                float newAlpha = Mathf.Lerp(bgColor.a, 1f, Time.deltaTime * fadeSpeed);
                bgRenderer.color = new Color(bgColor.r, bgColor.g, bgColor.b, newAlpha);
            }
        }
    }

    // Toggle powder UI
    // ================
    public void TogglePowderUI(bool activate)
    {
        powderUI.SetActive(activate);
        powderPrefab.SetActive(activate);
    }

    // Activation du studio après un délai de fade
    // ===========================================
    private IEnumerator inStudioDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PowderManager.Instance.inStudio = true;
    }
}
