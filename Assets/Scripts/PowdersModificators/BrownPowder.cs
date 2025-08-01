using UnityEngine;

public class BrownPowder : PowderModificator
{
    // Augmente le nombre de particules de l'explosion principale
    override public void ApplyModifier()
    {
        var subEmitters = attachedFirework.subEmitters;

        if (attachedFirework.name == "Fireworks(Clone)")
        {
            ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);
            var trails = burstSystem.trails;
            trails.enabled = true;
            //var ratioConstant = trails.ratio.constant;
            trails.ratio = trails.ratio + 0.15f;

            ParticleSystemRenderer renderer = burstSystem.GetComponent<ParticleSystemRenderer>();
            Material trailMat = new Material(renderer.material);
            renderer.trailMaterial = trailMat;
            
            // // Disable sub emitters from this burst
            // var burstSubEmitters = burstSystem.subEmitters;
            // burstSubEmitters.enabled = false;
            
            // Disable trail
            ParticleSystem trailSubEmitter = burstSystem.subEmitters.GetSubEmitterSystem(0);
            var emission = trailSubEmitter.emission;
            emission.rateOverTime = 0f;
        }

    }
}
