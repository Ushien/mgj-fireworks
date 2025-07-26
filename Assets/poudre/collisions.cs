using UnityEngine;

public class collisions : MonoBehaviour
{

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("detected");
        powder_charge.Instance.currentCharge ++;
    }
}
