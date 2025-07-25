using UnityEngine;

public class BluePowder : PowderModificator
{
    // Rend l'explosion un peu plus bleue
    override public void ApplyModifier()
    {
        ParticleSystem.MainModule mm = attachedFirework.GetComponent<ParticleSystem>().main;
        mm.startColor = new Color(mm.startColor.color.r, mm.startColor.color.g, (mm.startColor.color.b + 1) / 2);
    }
}
