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
    public AudioSource sandSound;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(sandSound.enabled && system.particleCount != 0){
            sandSound.volume = (system.particleCount/4000f)*0.5f;
            }
        if (system.particleCount > 0 && !sandSound.enabled)
            sandSound.enabled = true;
        if (system.particleCount == 0 && sandSound.enabled)
            sandSound.enabled = false;
        // Get world position of mouse
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = zDistanceFromCamera;

        Vector3 worldPos = pouringCamera.ScreenToWorldPoint(mouseScreenPos);
        Vector3 localPos = system.transform.InverseTransformPoint(worldPos);

        // Move emitter shape
        var shape = system.shape;
        shape.position = localPos;

        // Update emission rate
        var emission = system.emission;
        if (Input.GetMouseButtonDown(0) && hasBucket && powder_charge.Instance.inStudio)
        {
            emission.rateOverTime = 2000f;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            emission.rateOverTime = 0f;
        }
    }

    public void SetParticleColor(int index)
    {
        powder_charge.Instance.currentCharge = 0;
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
        powder_charge.Instance.currentIndex = index;
    }
}
