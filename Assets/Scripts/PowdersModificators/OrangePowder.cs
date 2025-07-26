using UnityEngine;

public class OrangePowder : PowderModificator
{
    // Augmente le nombre de particules de l'explosion principale
    override public void ApplyModifier()
    {
        // Si on manipule l'explosion principale
        var subEmitters = attachedFirework.subEmitters;
        if (attachedFirework.name == "Fireworks(Clone)")
        {
            ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);
            ParticleSystem.EmissionModule emission = burstSystem.emission;
            ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
            emission.GetBursts(bursts);
            float numEmitted = bursts[0].count.constant;
            numEmitted *= 2f;
            bursts[0].count = new ParticleSystem.MinMaxCurve(numEmitted);
            emission.SetBursts(bursts);
        }
    }
}
