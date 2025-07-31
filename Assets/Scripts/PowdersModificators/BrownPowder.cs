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
            ParticleSystem trailSystem = burstSystem.subEmitters.GetSubEmitterSystem(0);

            // Modify main module
            var main = trailSystem.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.1f, 10f);
            main.maxParticles = 10000;

            // Modify emission module
            var emission = trailSystem.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(0f);     // example value
            emission.rateOverDistance = new ParticleSystem.MinMaxCurve(5f);  // example value
        }
    }
}
