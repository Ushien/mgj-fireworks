
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
    public int powderIndex = 0;  // Indice de la poudre utilisée
    public int maxCharge;         // nombre maximum de charges de la fusée
    public int charge = 0;        // nombre actuel de charges de la fusée
    public int powderCap;         // quantité de poudre pour une charge
    public int powderCount = 0;   // quantité actuelle de poudre

    [Header("Pink Powder exception")]
    public float bloomIntensity;
    public float scatter;

    [Header("Instantiation des drapeaux")]
    public Transform meche;
    public List<GameObject> flags;

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
        if (powderCount >= powderCap)
        {
            powderCount = 0;
            AddCharge(powderIndex);
        }
    }

    // --------------------------------------------------------------------------------------------
    // Change le type de poudre
    // ========================
    public void ChangePowder(int index)
    {
        SetParticleColor(index);
        SetParticleAmount(flux);
        powderCount = 0;
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
        flags[charge].GetComponent<Renderer>().material.color = PowderManager.Instance.colors[Index];
        charge++;
    }

    // Reset de la charge
    // ==================
    public void ResetCharge(){
        BaseShootingSystem.Instance.powderList.Clear();
        charge = 0;
        powderCount = 0;
        foreach (GameObject flag in flags)
            flag.SetActive(false);
        ResetBloom();
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
    #endregion
    // --------------------------------------------------------------------------------------------
}
