using ShootingSystem;
using UnityEngine;

public class Powder : MonoBehaviour
{
    public FireworkProjectile attachedFirework;
    public virtual void ApplyModifier()
    {
        Debug.Log("Must be overriden");
    }
}
