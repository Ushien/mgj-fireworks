
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShootingSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// ------------------------------------------------------------------------------------------------

public class PowderManager : MonoBehaviour
{
    // Variables
    // =========
    public static PowderManager Instance;

    [Header("Paramètres de charges")]
    public int powderIndex = 0;             // Indice de la poudre utilisée
    public int maxCharge;                   // nombre maximum de charges de la fusée
    public int charge = 0;                  // nombre actuel de charges de la fusée
    public float powderCap;                   // Temps nécessaire de détection pour une charge
    public float powderTimer = 0f;          // durée cumulée de détection de poudre
    public int powderCount = 0;             // poudre cumulée détectée cette frame
    public int previousPowderCount = 0;    // poudre cumulée à la frame précédente

    [Header("Pink Powder exception")]
    public float bloomIntensity;
    public float scatter;

    [Header("Instantiation des drapeaux")]
    public Transform meche;
    public List<GameObject> flags;
    public Sprite flagMulti;
    public Sprite flagNormal;

    [Header("Poudre PS et couleurs")]
    [SerializeField]
    private float sandVolume = 0.5f;
    public ParticleSystem system;
    public List<Color> colors;
    public int flux = 2000;
    public List<PowderModificator> powderPrefabs;
    public GameObject powderPrefab;
    public Volume volume;
    [SerializeField]
    private float BloomIntensity = 5f;
    [SerializeField]
    private float BloomScatter = 0.3f;
    public bool isPouring = false;
    Bloom bloom;

    [Header("Mèche")]
    public GameObject mecheLit;
    public Animator rocketAnimator;
    [SerializeField] private float closingDelay = 1f;
    public AudioSource sparkSound;
    public GameObject fireCracks;

    [Header("Camera")]
    [SerializeField]
    private Camera powderCamera;
    private const float zDistanceFromCamera = 10f;
    public bool inStudio = false;

    // --------------------------------------------------------------------------------------------
    #region Awake/Update

    // Awake/update
    // ============
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        HandleParticleSound();
        HandleMoving();
        HandleCharge();
    }

    #endregion
    // --------------------------------------------------------------------------------------------
    #region Methods
    
    // Gère le son des particules
    // ==========================
    void HandleParticleSound()
    {
        AudioManager.Instance.sandSound.enabled = system.particleCount > 0;
        if(AudioManager.Instance.sandSound.enabled && system.particleCount != 0)
            AudioManager.Instance.sandSound.volume = (system.particleCount/(float)system.main.maxParticles)*(float)sandVolume;
    }

    // Mouvement de la source des particules
    // ================================================
    void HandleMoving()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = zDistanceFromCamera;
        Vector3 worldPos = powderCamera.ScreenToWorldPoint(mouseScreenPos);
        Vector3 localPos = system.transform.InverseTransformPoint(worldPos);
        var shape = system.shape;
        shape.position = localPos;
    }

    // Gère le remplissage de la fusée
    // ===============================
    void HandleCharge()
    {
        if(powderCount > previousPowderCount){
            previousPowderCount = powderCount;
            powderTimer += Time.deltaTime;
            if (powderTimer >= powderCap)
            {
                powderTimer = 0f;
                powderCount = 0;
                previousPowderCount = 0;
                AddCharge(powderIndex);
            }
        }
        // if (powderCount >= powderCap)
        // {
        //     powderCount = 0;
        //     AddCharge(powderIndex);
        // }
    }

    // --------------------------------------------------------------------------------------------
    // Change le type de poudre
    // ========================
    public void ChangePowder(int index)
    {
        SetParticleColor(index);
        SetParticleAmount(flux);
        powderCount = 0;
        previousPowderCount = 0;
        powderTimer = 0f;
        powderIndex = index;
    }

    // Change la couleur de la poudre
    // ==============================
    public void SetParticleColor(int index)
    {
        // Effet rainbow
        // -------------
        var colorOverLifetime = system.colorOverLifetime;
        colorOverLifetime.enabled = (index == 7);

        // Changement de couleur
        // ---------------------
        Color newColor = colors[index];
        var main = system.main;
        main.startColor = newColor;
    }

    // Change le flux de la poudre
    // ===========================
    public void SetParticleAmount(int amount)
    {
        var emission = system.emission;
        emission.rateOverTime = amount;
    }

    // Ajoute une charge
    // =================
    public void AddCharge(int Index)
    {
        if(charge >= maxCharge)
            return;

        // Pink powder exception
        // ---------------------
        if(Index == 11 && volume.profile.TryGet(out bloom)){
            bloom.intensity.value += bloomIntensity;
            bloom.scatter.value += scatter;
        }

        BaseShootingSystem.Instance.powderList.Add(powderPrefabs[Index]);
        flags[charge].SetActive(true);
        
        // rainbow flag
        // ------------
        if(Index == 7)
        {
            flags[charge].GetComponent<SpriteRenderer>().sprite = flagMulti;
            flags[charge].GetComponent<Renderer>().material.color = PowderManager.Instance.colors[9];
        }
        else{
            flags[charge].GetComponent<SpriteRenderer>().sprite = flagNormal;
            flags[charge].GetComponent<Renderer>().material.color = PowderManager.Instance.colors[Index];
            }
        charge++;
        
        if(charge == maxCharge){
            mecheLit.SetActive(true);
            rocketAnimator.Play("Closing");
            }

    }

    // Reset de la charge
    // ==================
    public void ResetCharge(){
        BaseShootingSystem.Instance.powderList.Clear();
        charge = 0;
        powderCount = 0;
        previousPowderCount = 0;
        powderTimer = 0f;
        foreach (GameObject flag in flags)
            flag.SetActive(false);
        ResetBloom();
        rocketAnimator.Play("Opening");
    }

    // Reset du bloom si besoin
    // ========================
    private void ResetBloom(){
        if(volume.profile.TryGet(out bloom))
        {
            bloom.intensity.value = BloomIntensity;
            bloom.scatter.value = BloomScatter;
        }
    }

    // Ferme la fusée et allume la mèche
    // ---------------------------------
    public void CloseRocket(){
        if (!mecheLit.activeSelf){
            rocketAnimator.Play("Closing");
            mecheLit.SetActive(true);
            StartCoroutine(GoToShootScreen(closingDelay));
        }
        else
        {
            StartCoroutine(FadeSpark(sparkSound, closingDelay));
            CameraManager.Instance.SwitchCamera();
        }

    }

    // Joue le son plusieurs fois
    // ---------------------------
    private IEnumerator GoToShootScreen(float delay)
    {
        yield return new WaitForSeconds(delay);
        CameraManager.Instance.SwitchCamera();
        StartCoroutine(FadeSpark(sparkSound, closingDelay));
    }

    // Fade l'étincelle de la mèche
    // ----------------------------
    private void FadeSpark(){
        mecheLit.SetActive(false);
    }

    // Sert à fade les étincelles de la mèche
    // --------------------------------------
    private IEnumerator FadeSpark (AudioSource audioSource, float duration)
    {
        Vector3 startSize = fireCracks.transform.localScale;
        Vector3 startMecheSize = mecheLit.transform.localScale;
        float startVolume = audioSource.volume;
        float time = 0f;
        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            fireCracks.transform.localScale = Vector3.Lerp(startSize, new Vector3(0f, 0f, 0f), time / duration);
            mecheLit.transform.localScale = Vector3.Lerp(startMecheSize, new Vector3(0f, 0f, 0f), time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0f;
        mecheLit.SetActive(false);
        fireCracks.transform.localScale = startSize;
        mecheLit.transform.localScale = startMecheSize;
        audioSource.volume = startVolume;
    }

    #endregion
    // --------------------------------------------------------------------------------------------
}
