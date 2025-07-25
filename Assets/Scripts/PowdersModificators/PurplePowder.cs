using UnityEngine;

public class PurplePowder : PowderModificator
{
    [SerializeField]
    private ParticleSystem subBurst;
    // Rajoute une explosion supplémentaire
    override public void ApplyModifier()
    {
        ParticleSystem mainBurst = FindLastBurst(attachedFirework.transform).GetComponent<ParticleSystem>();
        if (mainBurst == null || subBurst == null)
        {
            Debug.LogWarning("Quelque chose ne se passe pas comme prévu");
            return;
        }
        ParticleSystem newBurst = Instantiate(subBurst, mainBurst.transform);
        newBurst.name = "Burst";
        ParticleSystem.SubEmittersModule subEmitters = mainBurst.subEmitters;
        subEmitters.AddSubEmitter(newBurst, ParticleSystemSubEmitterType.Death, ParticleSystemSubEmitterProperties.InheritNothing);
    }

    private Transform FindLastBurst(Transform originExplosion)
    {
        if (originExplosion.transform.Find("Burst") == null) {
            return originExplosion.transform;
        }
        else {
            return FindLastBurst(originExplosion.transform.Find("Burst").transform);
        }
    }
}
