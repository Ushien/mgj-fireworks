using UnityEngine;

public class GreenPowder : PowderModificator
{
    public float colorStrength;
    // Rend l'explosion un peu plus verte
    override public void ApplyModifier()
    {
        FireworkScript.colorList.Add(0.3f);
    }
}
