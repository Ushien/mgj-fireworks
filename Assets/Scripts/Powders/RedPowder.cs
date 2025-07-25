using UnityEngine;

public class RedPowder : PowderModificator
{
    override public void ApplyModifier()
    {
        ParticleSystem.MainModule mm = attachedFirework.GetComponent<ParticleSystem>().main;
        mm.startColor = new Color((mm.startColor.color.r + 1)/2, mm.startColor.color.g, mm.startColor.color.b); 
    }
}
