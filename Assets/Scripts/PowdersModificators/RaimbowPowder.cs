using UnityEngine;
using ParticleSystem = UnityEngine.ParticleSystem;

public class RaimbowPowder : PowderModificator
{
    private int firstIndex = 0;

    
    public override void ApplyModifier()
    {
        
        ParticleSystem.MainModule mainModule = attachedFirework.GetComponent<ParticleSystem>().main;
        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = attachedFirework.GetComponent<ParticleSystem>().colorOverLifetime;
        
        Transform burstTransform = attachedFirework.transform.Find("Burst");
        Transform burstTrailsTransform = attachedFirework.transform.Find("Burst/Trails");
        
        ParticleSystem.SubEmittersModule subEmitters = burstTransform.GetComponent<ParticleSystem>().subEmitters;
        ParticleSystem.MainModule burstMainModule = burstTransform.GetComponent<ParticleSystem>().main;
        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeBurstTrails = burstTrailsTransform.GetComponent<ParticleSystem>().colorOverLifetime;
        
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
        
        mainModule.startColor = new ParticleSystem.MinMaxGradient(rainbowGradient);
        
        subEmitters.SetSubEmitterProperties(firstIndex, ParticleSystemSubEmitterProperties.InheritNothing);
        
        colorOverLifetime.enabled = true;
        colorOverLifetimeBurstTrails.enabled = true;
        
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(rainbowGradient);
        colorOverLifetimeBurstTrails.color = new ParticleSystem.MinMaxGradient(rainbowGradient);
        burstMainModule.startColor = new ParticleSystem.MinMaxGradient(rainbowGradient);
    }
}
