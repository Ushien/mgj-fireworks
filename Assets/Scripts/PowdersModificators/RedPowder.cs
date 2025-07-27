using UnityEngine;

public class RedPowder : PowderModificator
{
    public float colorStrength;
    // Rend l'explosion un peu plus rouge
    override public void ApplyModifier()
    {
        FireworkScript.finalColor = new Vector3 (FireworkScript.finalColor.x + colorStrength,
                                     FireworkScript.finalColor.y,
                                     FireworkScript.finalColor.z );
    }
}
