using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace ShootingSystem {
    public class BaseShootingSystem : MonoBehaviour
    {
        [SerializeField] private ObjectPool projectilePool; /// The object pool managing the projectile instances.
        [SerializeField] private Transform projectileSpawnPoint; /// Spawn point for the projectiles.
        [SerializeField] private AudioClip shotSound; /// Sound played when a projectile is shot.
        [SerializeField] private AudioSource audioSource; /// Audio source to play the shot sound.
        [SerializeField] private List<Powder> powderList; /// List of powders you plan to attach to a firework.

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
            if (shotSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shotSound);
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
                powderList = new List<Powder>();
                PlayShotSound();
            }
        }    
    }
}
