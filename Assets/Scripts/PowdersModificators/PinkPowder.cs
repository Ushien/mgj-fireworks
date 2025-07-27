using System.Collections.Generic;
using ShootingSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PinkPowder : PowderModificator
{
    override public void ApplyModifier()
    {
        if (attachedFirework.name == "Fireworks(Clone)")
        {
            ParticleSystem.SubEmittersModule subEmitters = attachedFirework.subEmitters;
            ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);
            ParticleSystem.SubEmittersModule subEmittersBurst = burstSystem.subEmitters;
            ParticleSystem trailsSystem = subEmittersBurst.GetSubEmitterSystem(0);
            
            Material burstMaterial = burstSystem.GetComponent<ParticleSystemRenderer>().material;
            Material trailMaterial = trailsSystem.GetComponent<ParticleSystemRenderer>().material;
            Color currentEmission = burstMaterial.GetColor("_EmissionColor");
            Color currentTrailEmission = trailMaterial.GetColor("_EmissionColor");
            Debug.Log("Current Emission Color: " + currentEmission);
            Debug.Log("Current Trail Emission Color: " + currentTrailEmission);
            
            int intensity = 2; 
            trailMaterial.EnableKeyword("_EMISSION");
            burstMaterial.EnableKeyword("_EMISSION");
            Color color = new Color(currentEmission.r*intensity,currentEmission.g*intensity,currentEmission.b*intensity,currentEmission.a*intensity);
            Debug.Log("color : " + color);
            burstMaterial.SetColor("_EmissionColor", color);
            trailMaterial.SetColor("_EmissionColor", color);

        }
    }
}
