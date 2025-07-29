using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // Liste de tous les sons
    // ======================
    public AudioSource sandSound;

    void Awake()
    {
        Instance = this;
    }
}
