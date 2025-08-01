using UnityEngine;

public class BrownPowder : PowderModificator
{
    [SerializeField] float maxTimeStrength = 1f;
    [SerializeField] float minTimeStrength = 0.8f;
    // Augmente le nombre de particules de l'explosion principale
    override public void ApplyModifier()
    {
        var subEmitters = attachedFirework.subEmitters;

        if (attachedFirework.name == "Fireworks(Clone)")
        {
            ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);
            var trails = burstSystem.trails;
            trails.enabled = true;
            float newMin = trails.lifetime.constantMin + minTimeStrength;
            float newMax = trails.lifetime.constantMax + maxTimeStrength;
            trails.lifetime = new ParticleSystem.MinMaxCurve(newMin, newMax);
            ParticleSystemRenderer renderer = burstSystem.GetComponent<ParticleSystemRenderer>();
            Material trailMat = new Material(renderer.material);
            renderer.trailMaterial = trailMat;
            
            // Disable sub emitters from this burst
            var burstSubEmitters = burstSystem.subEmitters;
            burstSubEmitters.enabled = false;
        }

    }
}
