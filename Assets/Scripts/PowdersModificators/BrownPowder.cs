using UnityEngine;

public class BrownPowder : PowderModificator
{
    // Augmente le nombre de particules de l'explosion principale
    override public void ApplyModifier()
    {
        // Si on manipule l'explosion principale
        var subEmitters = attachedFirework.subEmitters;
        if (attachedFirework.name == "Fireworks(Clone)")
        {
            ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);
            ParticleSystem trailSystem = burstSystem.subEmitters.GetSubEmitterSystem(0);
            ParticleSystem.MainModule mm = trailSystem.main;
            mm.startLifetime = new ParticleSystem.MinMaxCurve(2f,3f); // Augmente la durée de vie des particules du trail
        }
    }
}
