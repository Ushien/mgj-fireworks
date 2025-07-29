using UnityEngine;

public class collisions : MonoBehaviour
{

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("detected");
        MixingManager.Instance.currentCharge ++;
    }
}
