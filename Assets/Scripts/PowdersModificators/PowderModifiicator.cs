using ShootingSystem;
using UnityEngine;
using System.Collections.Generic;

public class PowderModificator : MonoBehaviour
{
    public ParticleSystem attachedFirework;
    public virtual void ApplyModifier()
    {
        Debug.Log("Must be overriden");
    }
    public virtual void ApplyModifier(List<PowderModificator> appliedPowders)
    {
        Debug.Log("Must be overriden");
    }
}
