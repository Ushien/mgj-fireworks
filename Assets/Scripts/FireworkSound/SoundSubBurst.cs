using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSubBurst : MonoBehaviour
{
    private AudioSource audioSource;
    private ParticleSystem particleSystem;
    private int previousParticleCount;
    bool isPlayed = false;
    bool hasStarted = false;
    
    private AudioClip clip;

    [SerializeField] private List<AudioClip> explosionSounds;

    private int limit = 27;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        int randomIndex = Random.Range(0, explosionSounds.Count);
        clip = explosionSounds[randomIndex];
        Debug.Log(gameObject.transform.parent.name);
        particleSystem = transform.parent.Find("Burst/Trails").GetComponent<ParticleSystem>();
        previousParticleCount = particleSystem.particleCount;
    }
    void Update()
    {
        int currentCount = particleSystem.particleCount;
        if (!hasStarted && currentCount > limit)
        {
            hasStarted = true;
        }
        
        if (hasStarted &&particleSystem.particleCount < previousParticleCount+10 && !isPlayed)
        {
            if (clip != null)
            {
                StartCoroutine(PlayClipRepeatedly());
            }
            isPlayed = true;
        }
        
        previousParticleCount = particleSystem.particleCount;
    }
    
    private IEnumerator PlayClipRepeatedly()
    {
        System.Random rand = new System.Random();
        int random = Random.Range(8, 10);
        for (int i = 0; i < random; i++)
        {
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(0.2f);
        }
    }

}
