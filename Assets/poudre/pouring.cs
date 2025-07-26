using UnityEngine;
using System.Collections.Generic;

public class pouring : MonoBehaviour
{
    public ParticleSystem system;
    public List<Color> colors;
    public float zDistanceFromCamera = 10f;

    void Update()
    {
        // Get world position of mouse
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = zDistanceFromCamera;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector3 localPos = system.transform.InverseTransformPoint(worldPos);

        // Move emitter shape
        var shape = system.shape;
        shape.position = localPos;

        // Update emission rate
        var emission = system.emission;
        if (Input.GetMouseButtonDown(0) && !powder_charge.Instance.overShelf)
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
        var colorOverLifetime = system.colorOverLifetime;
        if (index < 0 || index >= colors.Count)
        {
            Debug.LogWarning("Invalid color index.");
            return;
        }

        if(index == 3)
            colorOverLifetime.enabled = true;
        else
            colorOverLifetime.enabled = false;
        Color newColor = colors[index];
        var main = system.main;
        main.startColor = newColor;
        powder_charge.Instance.currentIndex = index;
    }
}
