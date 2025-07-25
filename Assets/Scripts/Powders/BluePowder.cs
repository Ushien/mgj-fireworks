using UnityEngine;

public class BluePowder : PowderModificator
{
    override public void ApplyModifier()
    {
        ParticleSystem.MainModule mm = attachedFirework.GetComponent<ParticleSystem>().main;
        mm.startColor = new Color(mm.startColor.color.r, mm.startColor.color.g, (mm.startColor.color.b + 1)/2); 
    }
}
