using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace ShootingSystem {
    public class BaseShootingSystem : MonoBehaviour
    {
        [SerializeField] private ObjectPool projectilePool; /// The object pool managing the projectile instances.
        [SerializeField] private Transform projectileSpawnPoint; /// Spawn point for the projectiles.
        [SerializeField] private AudioSource audioSource; /// Audio source to play the shot sound.
        [SerializeField] private List<PowderModificator> powderList; /// List of powders you plan to attach to a firework.
        [SerializeField] private List<AudioClip> shotSounds;

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
            projectile.transform.position = projectileSpawnPoint.position;

            FireworkProjectile projectileBehaviour = projectile.GetComponent<FireworkProjectile>();
            if (projectileBehaviour != null)
            {
                projectileBehaviour.Init(projectilePool, startPosition, powderList);
                // Vide la poudre pour la prochaine fusée
                powderList = new List<PowderModificator>();
                projectileBehaviour.GetComponent<ParticleSystem>().Play();
                PlayShotSound();
            }
        }    
    }
}
