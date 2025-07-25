using UnityEngine;

public class YellowPowder : PowderModificator
{
    override public void ApplyModifier()
    {
        ParticleSystem.MainModule mm = attachedFirework.GetComponent<ParticleSystem>().main;
        //
    }
}
