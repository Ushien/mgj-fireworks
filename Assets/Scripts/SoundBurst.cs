using UnityEngine;

public class SoundBurst : MonoBehaviour
{
    private AudioSource audioSource;
    private ParticleSystem particleSystem;
    private int previousParticleCount;
    bool isPlayed = false;
    bool hasStarted = false;

    private int limit = 27;
    
    [SerializeField]
    private AudioClip clip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        particleSystem = GetComponent<ParticleSystem>();
    }
    
    void Update()
    {
        int currentCount = particleSystem.particleCount;

        if (!hasStarted && currentCount > limit)
        {
            hasStarted = true;
        }

        if (hasStarted && currentCount <= limit && !isPlayed)
        {
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
            isPlayed = true;
        }
    }

}
