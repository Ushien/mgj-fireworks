using UnityEngine;

public class GreenPowder : PowderModificator
{
    // Rend l'explosion un peu plus verte
    override public void ApplyModifier()
    {
        ParticleSystem.MainModule mm = attachedFirework.GetComponent<ParticleSystem>().main;
        mm.startColor = new Color(mm.startColor.color.r, (mm.startColor.color.g + 1) / 2, mm.startColor.color.b);
    }
}
