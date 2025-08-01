using UnityEngine;

public class OrangePowder : PowderModificator
{
    [SerializeField] private float positionStrength = 5f;
    [SerializeField] private float sizeStrength = 2f;

    // Augmente le nombre de particules de l'explosion principale
    override public void ApplyModifier()
    {
        var subEmitters = attachedFirework.subEmitters;

        if (attachedFirework.name == "Fireworks(Clone)")
        {
            ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);

            // Enable and configure the Noise module
            var noise = burstSystem.noise;
            noise.enabled = true;
            var positionConstant = noise.positionAmount.constant;
            var sizeConstant = noise.sizeAmount.constant;
            noise.positionAmount = new ParticleSystem.MinMaxCurve(positionConstant + positionStrength);
            noise.sizeAmount = new ParticleSystem.MinMaxCurve(sizeConstant + sizeStrength);
        }
    }
}