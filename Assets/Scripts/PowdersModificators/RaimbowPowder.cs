using UnityEngine;
using ParticleSystem = UnityEngine.ParticleSystem;

public class RaimbowPowder : PowderModificator
{
    public override void ApplyModifier()
    {
        Gradient rainbowGradient = new Gradient();
        rainbowGradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(new Color(0.965f, 0.667f, 0.675f), 0f),
                new GradientColorKey(new Color(0.992f, 0.831f, 0.643f), 0.17f),
                new GradientColorKey(new Color(0.976f, 0.961f, 0.722f), 0.34f),
                new GradientColorKey(new Color(0.804f, 0.894f, 0.714f), 0.51f),
                new GradientColorKey(new Color(0.663f, 0.875f, 0.925f), 0.68f),
                new GradientColorKey(new Color(0.647f, 0.757f, 0.902f), 0.85f),
                new GradientColorKey(new Color(0.729f, 0.686f, 0.843f), 1f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );
        
        // Change la couleur du projectile principal
        ParticleSystem.MainModule mm = attachedFirework.main;
        mm.startColor = new ParticleSystem.MinMaxGradient(rainbowGradient);

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = attachedFirework.GetComponent<ParticleSystem>().colorOverLifetime;
        colorOverLifetime.enabled = true;
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(rainbowGradient);

        // Déclaration variables
        ParticleSystem.MainModule mm1;
        ParticleSystem.ColorOverLifetimeModule colorOverLifetime1;

        // Change la couleur du trail
        ParticleSystem subEmitter = attachedFirework.subEmitters.GetSubEmitterSystem(0);

        mm1 = subEmitter.main;
        mm1.startColor = mm.startColor;

        colorOverLifetime1 = subEmitter.colorOverLifetime;
        colorOverLifetime1.enabled = colorOverLifetime.enabled;
        colorOverLifetime1.color = colorOverLifetime.color;

        // Si on manipule l'explosion principale
        if (attachedFirework.name == "Fireworks(Clone)")
        {
            // Change la couleur des projectiles de l'explosion
            ParticleSystem burstSystem = attachedFirework.subEmitters.GetSubEmitterSystem(1);

            mm1 = burstSystem.main;
            mm1.startColor = mm.startColor;

            colorOverLifetime1 = burstSystem.colorOverLifetime;
            colorOverLifetime1.enabled = colorOverLifetime.enabled;
            colorOverLifetime1.color = colorOverLifetime.color;


            // Change la couleur du trail de l'explosion
            ParticleSystem trailSystem = burstSystem.subEmitters.GetSubEmitterSystem(0);

            mm1 = trailSystem.main;
            mm1.startColor = mm.startColor;

            colorOverLifetime1 = trailSystem.colorOverLifetime;
            colorOverLifetime1.enabled = colorOverLifetime.enabled;
            colorOverLifetime1.color = colorOverLifetime.color;
        }
    }
}
