using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShootingSystem;

public class powder_charge : MonoBehaviour
{
    // Variables
    // =========
    public Transform meche;
    public List<PowderModificator> powderPrefabs;
    public static powder_charge Instance;
    public int maxCharge;
    public int chargeNeed;
    public int currentCharge = 0;
    public List<GameObject> flagGO;
    public int chargeCount = 0;
    private float width;
    public float xOffset = 10f;
    public float yOffset = -10f;
    public float fadeSpeed = 3f;
    public float fadeScale = 0.8f;
    public int currentIndex = 0;
    public bool overShelf = false;
    public GameObject powderUI;
    public GameObject powderPrefab;
    public GameObject powderStudio;
    public List<GameObject> toFade;
    public GameObject studioBckgrnd;
    private int fading = 0;
    public bool inStudio = false;

    // Camera transition setting
    public Camera powderCamera;
    public Camera shootCamera;
    public float lerpSpeed = 5f;
    private bool proceeding = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        width = meche.GetComponent<SpriteRenderer>().bounds.size.x - xOffset;
    }

    void Update()
    {
        if(fading == 1){
            foreach (GameObject objectToFade in toFade)
            {
                Renderer renderer = objectToFade.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Color currentColor = renderer.material.color;
                    float newValue = Mathf.Lerp(currentColor.r, 0f, Time.deltaTime * fadeSpeed); 
                    Color fadedColor = new Color(newValue, newValue, newValue, currentColor.a);
                    renderer.material.color = fadedColor;
                }
            }
            powderStudio.transform.localScale = Vector3.Lerp( powderStudio.transform.localScale,
                                                              new Vector3(fadeScale, fadeScale, fadeScale), 
                                                              Time.deltaTime * fadeSpeed);
            Renderer fadeRenderer = studioBckgrnd.GetComponent<Renderer>();
            if(fadeRenderer != null){
                    Color currentColor = fadeRenderer.material.color;
                    float newAlpha = Mathf.Lerp(currentColor.a, 0f, Time.deltaTime * fadeSpeed); 
                    Color fadedColor = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
                    fadeRenderer.material.color = fadedColor;
            }
        }

        if (fading == -1){
            foreach (GameObject objectToFade in toFade)
            {
                Renderer renderer = objectToFade.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Color currentColor = renderer.material.color;
                    float newValue = Mathf.Lerp(currentColor.r, 1f, Time.deltaTime * fadeSpeed); 
                    Color fadedColor = new Color(newValue, newValue, newValue, currentColor.a);
                    renderer.material.color = fadedColor;
                }
            }
            powderStudio.transform.localScale = Vector3.Lerp( powderStudio.transform.localScale,
                                                              new Vector3(1f, 1f, 1f), 
                                                              Time.deltaTime * fadeSpeed);

            Renderer fadeRenderer = studioBckgrnd.GetComponent<Renderer>();
            if(fadeRenderer != null){
                    Color currentColor = GetComponent<Renderer>().material.color;
                    float newAlpha = Mathf.Lerp(currentColor.a, 0f, Time.deltaTime * fadeSpeed); 
                    Color fadedColor = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
                    fadeRenderer.material.color = fadedColor;
            }
        }

        if (currentCharge >= chargeNeed)
        {
            AddCharge(currentIndex);
            currentCharge = 0;
        }
    }

    public void AddCharge(int Index)
    {
        BaseShootingSystem.Instance.powderList.Add(powderPrefabs[Index]);
        GameObject instance = Instantiate(flagGO[Index], meche);
        instance.transform.position = new Vector3(meche.position.x + width / 2 - (width / maxCharge) * chargeCount, meche.position.y + yOffset, meche.position.z);
        chargeCount++;

    }

    public void DeactivatePowderUI()
    {
        powderUI.SetActive(false);
        powderPrefab.SetActive(false);
    }

    public void ActivatePowderUI()
    {
        powderUI.SetActive(true);
        powderPrefab.SetActive(true);
    }

    public void Fade(bool direction){
        if(direction){
            fading = 1;
            inStudio = false;
        }
        else{
            fading = -1;
            StartCoroutine(InStudioDelay(1.5f));
        }
    }

    private IEnumerator InStudioDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        inStudio = true;
    }
}
