using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace ShootingSystem {
    public class BaseShootingSystem : MonoBehaviour
    {
        [SerializeField] private ObjectPool projectilePool; /// The object pool managing the projectile instances.
        [SerializeField] private Transform projectileSpawnPoint; /// Spawn point for the projectiles.
        [SerializeField] private AudioSource audioSource; /// Audio source to play the shot sound.
        [SerializeField] public List<PowderModificator> powderList; /// List of powders you plan to attach to a firework.
        [SerializeField] private List<AudioClip> shotSounds;
        [SerializeField] private float xOffset = 0.3f;

        public static BaseShootingSystem Instance;

        // Ajout système couleur Alexandre

        void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Shoots a projectile in the specified direction with a spread effect.
        /// </summary>
        public void Shoot()
        {
            Vector2 startPosition = projectileSpawnPoint.position;
            ShotEffects(startPosition);
        }

        /// <summary>
        /// Plays the shot sound if available.
        /// </summary>
        protected void PlayShotSound()
        {
            if (shotSounds != null && audioSource != null)
            {
                int randomIndex = UnityEngine.Random.Range(0, shotSounds.Count);
                AudioClip randomClip = shotSounds[randomIndex];
                audioSource.PlayOneShot(randomClip);
            }
        }

        /// <summary>
        /// Handles the effects of shooting a projectile, including instantiation and initialization.
        /// </summary>
        protected void ShotEffects(Vector2 startPosition)
        {
            GameObject projectile = projectilePool.GetObject();
            projectile.transform.position = new Vector3 (projectileSpawnPoint.position.x + xOffset, projectileSpawnPoint.position.y, 280f);

            FireworkProjectile projectileBehaviour = projectile.GetComponent<FireworkProjectile>();
            if (projectileBehaviour != null)
            {
                projectileBehaviour.Init(projectilePool, startPosition, powderList);
                projectileBehaviour.GetComponent<ParticleSystem>().Play();
                PlayShotSound();
            }
        }    
        
        public void flushPowderList()
        {
            powderList = new List<PowderModificator>();
        }
    }
}
