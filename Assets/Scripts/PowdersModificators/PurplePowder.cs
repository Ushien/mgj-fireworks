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

        /*
        ParticleSystem.EmissionModule em = attachedFirework.transform.Find("Burst").GetComponent<ParticleSystem>().emission;
        em.burstCount = 15;
        mm.scalingMode = ParticleSystemScalingMode.Hierarchy;
        */

        ParticleSystem newBurst = Instantiate(subBurst, mainBurst.transform);
        newBurst.name = "Burst";
        ParticleSystem.SubEmittersModule subEmitters = mainBurst.subEmitters;
        subEmitters.AddSubEmitter(newBurst, ParticleSystemSubEmitterType.Death, ParticleSystemSubEmitterProperties.InheritNothing);

        ParticleSystem.MainModule mm = newBurst.transform.Find("Trails").GetComponent<ParticleSystem>().main;
        mm.startSize = newBurst.transform.parent.Find("Trails").GetComponent<ParticleSystem>().main.startSize.constant / 2f;
        mm = newBurst.GetComponent<ParticleSystem>().main;
        mm.startSize = newBurst.transform.parent.GetComponent<ParticleSystem>().main.startSize.constant/2f;
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
