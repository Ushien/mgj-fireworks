using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Compte des particules qui tombent dans la fusée
    // ===============================================
    void OnParticleCollision(GameObject other)
    {
        MixingManager.Instance.currentCharge ++;
    }
}
