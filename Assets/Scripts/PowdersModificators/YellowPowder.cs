using UnityEngine;

public class YellowPowder : PowderModificator
{
    override public void ApplyModifier()
    {
        // Change la couleur du projectile principal
        ParticleSystem.MainModule mm = attachedFirework.GetComponent<ParticleSystem>().main;
        mm.startColor = new ParticleSystem.MinMaxGradient(Color.yellow);
        
        // Change la couleur du trail
        ParticleSystem.SubEmittersModule subEmitters = attachedFirework.subEmitters;
        ParticleSystem.MainModule mm1 = subEmitters.GetSubEmitterSystem(0).main;
        mm1.startColor = mm.startColor;
        
        if (attachedFirework.name == "Fireworks(Clone)")
        {
            ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);
            ParticleSystem.MainModule burstMain = burstSystem.main;
            burstMain.startLifetime = new ParticleSystem.MinMaxCurve(burstMain.startLifetime.constantMin+1f, burstMain.startLifetime.constantMax+1f);
                
            // Change la couleur des projectiles de l'explosion
            ParticleSystem.MainModule mm2 = burstSystem.main;
            mm2.startColor = mm.startColor;

            // Change la couleur du trail de l'explosion
            ParticleSystem trailSystem = burstSystem.subEmitters.GetSubEmitterSystem(0);
            ParticleSystem.MainModule mm3 = trailSystem.main;
            mm3.startColor = mm.startColor;
        }
    }
}
