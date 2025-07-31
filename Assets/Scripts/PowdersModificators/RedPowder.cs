using UnityEngine;

public class RedPowder : PowderModificator
{
    public float colorStrength;
    // Rend l'explosion un peu plus rouge
    override public void ApplyModifier()
    {
        FireworkScript.colorList.Add(0f);
    }
}
