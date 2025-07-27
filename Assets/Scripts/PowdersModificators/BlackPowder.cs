using UnityEngine;

public class BlackPowder : PowderModificator
{
    override public void ApplyModifier()
    {
        if (attachedFirework.name == "Fireworks(Clone)")
        {
            // augmente la durée de vie des projectile pour augmenter la taille de l'explosion
            ParticleSystem.SubEmittersModule subEmitters = attachedFirework.subEmitters;
            ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);
            ParticleSystem.MainModule burstMain = burstSystem.main;
            burstMain.startLifetime = new ParticleSystem.MinMaxCurve(burstMain.startLifetime.constantMin+1f, burstMain.startLifetime.constantMax+1f);
        }
    }
}
