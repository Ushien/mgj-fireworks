using UnityEngine;

public class GreenPowder : PowderModificator
{
    public float colorStrength;
    // Rend l'explosion un peu plus verte
    override public void ApplyModifier()
    {
        FireworkScript.finalColor = new Vector3 (FireworkScript.finalColor.x,
                                     FireworkScript.finalColor.y + colorStrength,
                                     FireworkScript.finalColor.z);
    }
}
