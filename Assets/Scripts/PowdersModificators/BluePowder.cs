using UnityEngine;

public class BluePowder : PowderModificator
{
    public float colorStrength;
    // Rend l'explosion un peu plus bleue
    override public void ApplyModifier()
    {
        FireworkScript.finalColor = new Vector3 (FireworkScript.finalColor.x,
                                     FireworkScript.finalColor.y,
                                     FireworkScript.finalColor.z + colorStrength);
    }
}
