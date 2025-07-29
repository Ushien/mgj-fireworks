using UnityEngine;
using System.Collections.Generic;

// ------------------------------------------------------------------------------------------------

public class PowderManager : MonoBehaviour
{
    // Variables
    // =========
    public static PowderManager Instance;

    [Header("Particles system and colours")]
    public ParticleSystem system;
    public List<Color> colors;

    [SerializeField]
    private float sandVolume = 0.5f;
    [SerializeField]
    private int flux = 2000;

    [Header("Camera")]
    [SerializeField]
    private Camera powderCamera;
    private const float zDistanceFromCamera = 10f;

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
        // toggle et ajustement du bruit de sable
        // --------------------------------------
        AudioManager.Instance.sandSound.enabled = system.particleCount > 0;
        if(AudioManager.Instance.sandSound.enabled && system.particleCount != 0)
            AudioManager.Instance.sandSound.volume = (system.particleCount/(float)system.main.maxParticles)*(float)sandVolume;

        // Ajustement de la source des particules
        // --------------------------------------
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = zDistanceFromCamera;
        Vector3 worldPos = powderCamera.ScreenToWorldPoint(mouseScreenPos);
        Vector3 localPos = system.transform.InverseTransformPoint(worldPos);
        var shape = system.shape;
        shape.position = localPos;
    }

    #endregion
    // --------------------------------------------------------------------------------------------

    #region Methods
    // Methods
    // =======

    // Change la couleur des particules de poudre
    // ==========================================
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

    // Change le flux de particules de poudre
    // ======================================
    public void SetParticleAmount(int amount)
    {
        var emission = system.emission;
        emission.rateOverTime = amount;
    }
    #endregion
}
