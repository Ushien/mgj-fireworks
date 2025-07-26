using UnityEngine;

public class WhitePowder : PowderModificator
{
    // Augmente la gravité sur l'explosion principale
    override public void ApplyModifier()
    {
        // Si on manipule l'explosion principale
        var subEmitters = attachedFirework.subEmitters;
        if (attachedFirework.name == "Fireworks(Clone)")
        {
            ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);
            ParticleSystem.MainModule mm = burstSystem.main;
            float gravity= mm.gravityModifier.constant;
            if (gravity < 1f)
            {
                mm.gravityModifier = 1.8f;
                mm.gravityModifier = new ParticleSystem.MinMaxCurve(1.8f);
            }
            else
            {
                mm.gravityModifier = new ParticleSystem.MinMaxCurve(gravity + 1f);
            }
        }
    }
}
