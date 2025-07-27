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
    public List<GameObject> flags;
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
        if (fading == 1) {
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

        if (fading == -1){
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

        if (currentCharge >= chargeNeed)
        {
            AddCharge(currentIndex);
            currentCharge = 0;
        }
    }

    public void AddCharge(int Index)
    {
        if(chargeCount >= maxCharge)
            return;
        BaseShootingSystem.Instance.powderList.Add(powderPrefabs[Index]);
        flags[chargeCount].SetActive(true);

        // Couleur du fanion
        Renderer renderer = flags[chargeCount].GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = pouring.Instance.colors[Index];
        }

        chargeCount++;

    }

    public void ResetCharge(){
        foreach (GameObject flag in flags)
        {
            flag.SetActive(false);
        }

        chargeCount = 0;
        currentCharge = 0;
        BaseShootingSystem.Instance.powderList.Clear();
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

    public void PutCap(){

    }
}
