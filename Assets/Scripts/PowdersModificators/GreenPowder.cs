using UnityEngine;

public class GreenPowder : PowderModificator
{
    // Rend l'explosion un peu plus verte
    override public void ApplyModifier()
    {
        // Change la couleur du projectile principal
        ParticleSystem.MainModule mm = attachedFirework.GetComponent<ParticleSystem>().main;
        mm.startColor = new ParticleSystem.MinMaxGradient(new Color(mm.startColor.color.r, (mm.startColor.color.g + 1) / 2, mm.startColor.color.b));

        // Change la couleur du trail
        var subEmitters = attachedFirework.subEmitters;
        ParticleSystem.MainModule mm1 = subEmitters.GetSubEmitterSystem(0).main;
        mm1.startColor = mm.startColor;

        // Si on manipule l'explosion principale
        if (attachedFirework.name == "Fireworks(Clone)")
        {
            // Change la couleur des projectiles de l'explosion
            ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);
            ParticleSystem.MainModule mm2 = burstSystem.main;
            mm2.startColor = mm.startColor;

            // Change la couleur du trail de l'explosion
            ParticleSystem trailSystem = burstSystem.subEmitters.GetSubEmitterSystem(0);
            ParticleSystem.MainModule mm3 = trailSystem.main;
            mm3.startColor = mm.startColor;
        }
    }
}
