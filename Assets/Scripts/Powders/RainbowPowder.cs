using UnityEngine;

public class RaibowPowder : PowderModificator
{
    override public void ApplyModifier()
    {
        ParticleSystem.MainModule mm = attachedFirework.GetComponent<ParticleSystem>().main;
        //
    }
}
