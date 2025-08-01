using System.Collections.Generic;
using ShootingSystem;
using UnityEngine;

public class PurplePowder : PowderModificator
{
    [SerializeField]
    private ParticleSystem subBurst;
    // Rajoute une explosion supplémentaire
    override public void ApplyModifier(List<PowderModificator> appliedPowders)
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
        // Set a new start color

        ParticleSystem.SubEmittersModule subEmitters = mainBurst.subEmitters;
        subEmitters.AddSubEmitter(newBurst, ParticleSystemSubEmitterType.Death, ParticleSystemSubEmitterProperties.InheritNothing);

        ParticleSystem.MainModule mm = newBurst.transform.Find("Trails").GetComponent<ParticleSystem>().main;
        mm.startSize = newBurst.transform.parent.Find("Trails").GetComponent<ParticleSystem>().main.startSize.constant / 2f;
        mm = newBurst.GetComponent<ParticleSystem>().main;
        
        mm.startSize = newBurst.transform.parent.GetComponent<ParticleSystem>().main.startSize.constant / 2f;
        newBurst.GetComponent<ParticleSystemRenderer>().material = mainBurst.GetComponent<ParticleSystemRenderer>().material;
        newBurst.transform.Find("Trails").GetComponent<ParticleSystemRenderer>().material = newBurst.GetComponent<ParticleSystemRenderer>().material;
        //Vector3 finalSubColor = FireworkScript.finalColor.normalized;
        //mm.startColor = new ParticleSystem.MinMaxGradient(new Color(finalSubColor.x, finalSubColor.y, finalSubColor.z));

        foreach (PowderModificator appliedPowder in appliedPowders)
        {
            appliedPowder.attachedFirework = newBurst.GetComponent<ParticleSystem>();
            appliedPowder.ApplyModifier();
        }

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
