using UnityEngine;

public class RaibowPowder : PowderModificator
{
    // Ajoute un gradiant de couleur arc-en-ciel
    override public void ApplyModifier()
    {
        ParticleSystem.MainModule mm = attachedFirework.GetComponent<ParticleSystem>().main;
        //
    }
}
