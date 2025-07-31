using UnityEngine;

public class BluePowder : PowderModificator
{
    public float colorStrength;
    // Rend l'explosion un peu plus bleue
    override public void ApplyModifier()
    {
        FireworkScript.colorList.Add(0.6f);
    }
}
