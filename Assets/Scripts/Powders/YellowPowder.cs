using UnityEngine;

public class YellowPowder : Powder
{
    override public void ApplyModifier()
    {
        ParticleSystem.MainModule mm = attachedFirework.GetComponent<ParticleSystem>().main;
        mm.startColor = Color.yellow;
    }
}
