using UnityEngine;

public class CyanPowder : PowderModificator
{
    // Augmente le nombre de particules de l'explosion principale
    override public void ApplyModifier()
    {
        // Si on manipule l'explosion principale
        if (attachedFirework.name == "Fireworks(Clone)")
        {
            ParticleSystem.EmissionModule emission = attachedFirework.GetComponent<ParticleSystem>().emission;
            ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
            emission.GetBursts(bursts);
            bursts[0].cycleCount = bursts[0].cycleCount * 2; 
            ParticleSystem.MainModule ps =  attachedFirework.GetComponent<ParticleSystem>().main;
            var lifetimeCurve = ps.startLifetime;
            ps.startLifetime = new ParticleSystem.MinMaxCurve(Mathf.Max(1.2f,lifetimeCurve.constantMin/2f), lifetimeCurve.constantMax);
            emission.SetBursts(bursts);

            // Diminue le nombre de particules des subemitters
            var subEmitters = attachedFirework.subEmitters;
            emission = subEmitters.GetSubEmitterSystem(1).emission;
            bursts = new ParticleSystem.Burst[emission.burstCount];
            emission.GetBursts(bursts);
            float numEmitted = bursts[0].count.constant * 0.7f;
            bursts[0].count = new ParticleSystem.MinMaxCurve(numEmitted);
            emission.SetBursts(bursts);

        }
    }
}

