using UnityEngine;
using System.Collections.Generic;

public class pouring : MonoBehaviour
{
    public static pouring Instance;
    public ParticleSystem system;
    public List<Color> colors;
    public float zDistanceFromCamera = 10f;
    public Camera pouringCamera;
    public bool hasBucket = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(AudioManager.Instance.sandSound.enabled && system.particleCount != 0){
            AudioManager.Instance.sandSound.volume = (system.particleCount/4000f)*0.5f;
            }
        if (system.particleCount > 0 && !AudioManager.Instance.sandSound.enabled)
            AudioManager.Instance.sandSound.enabled = true;
        if (system.particleCount == 0 && AudioManager.Instance.sandSound.enabled)
            AudioManager.Instance.sandSound.enabled = false;
        // Get world position of mouse
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = zDistanceFromCamera;

        Vector3 worldPos = pouringCamera.ScreenToWorldPoint(mouseScreenPos);
        Vector3 localPos = system.transform.InverseTransformPoint(worldPos);

        // Move emitter shape
        var shape = system.shape;
        shape.position = localPos;
    }

    public void SetParticleColor(int index)
    {
        MixingManager.Instance.currentCharge = 0;
        var colorOverLifetime = system.colorOverLifetime;
        if (index < 0 || index >= colors.Count)
        {
            Debug.LogWarning("Invalid color index.");
            return;
        }

        // effet rainbow
        if(index == 7)
            colorOverLifetime.enabled = true;
        else
            colorOverLifetime.enabled = false;
        Color newColor = colors[index];
        var main = system.main;
        main.startColor = newColor;
        MixingManager.Instance.currentIndex = index;
    }

    public void SetParticleAmount(int amount){
        var emission = system.emission;
        emission.rateOverTime = amount;
    }
}
