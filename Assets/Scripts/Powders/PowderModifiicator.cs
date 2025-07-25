using ShootingSystem;
using UnityEngine;

public class PowderModificator : MonoBehaviour
{
    public FireworkProjectile attachedFirework;
    public virtual void ApplyModifier()
    {
        Debug.Log("Must be overriden");
    }
}
