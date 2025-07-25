using UnityEngine;
using Utils;

namespace ShootingSystem {
    public class BaseShootingSystem : MonoBehaviour
    {
        [SerializeField] private ObjectPool projectilePool; /// The object pool managing the projectile instances.
        [SerializeField] private Transform projectileSpawnPoint; /// Spawn point for the projectiles.
        [SerializeField] private AudioClip shotSound; /// Sound played when a projectile is shot.
        [SerializeField] private AudioSource audioSource; /// Audio source to play the shot sound.
        [SerializeField] private float projectileSpeed = 20f; /// The speed of the projectile.
        [SerializeField] private float distance = 0f; /// The distance the projectile will travel before returning to the pool.
        [SerializeField] private Vector2 projectileSpreadVariance = new Vector2(0f, 0f); /// Variance in the projectile's direction to create a spread effect.

        /// <summary>
        /// Shoots a projectile in the specified direction with a spread effect.
        /// </summary>
        public void Shoot()
        {
            Vector2 direction = GetDirection();
            Vector2 startPosition = projectileSpawnPoint.position;
            Vector2 targetPosition = startPosition + direction * distance;
            ShotEffects(startPosition, targetPosition);
        }

        /// <summary>
        /// Gets a upward direction, applying a spread effect.
        /// </summary>
        protected Vector2 GetDirection()
        {
            Vector2 direction = transform.up;
            direction += new Vector2(
                Random.Range(-projectileSpreadVariance.x, projectileSpreadVariance.x),
                Random.Range(-projectileSpreadVariance.y, projectileSpreadVariance.y)
            );
            direction.Normalize();
            return direction;
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
        protected void ShotEffects(Vector2 startPosition, Vector2 targetPosition)
        {
            GameObject projectile = projectilePool.GetObject();
            projectile.transform.position = projectileSpawnPoint.position;

            FireworkProjectile projectileBehaviour = projectile.GetComponent<FireworkProjectile>();
            if (projectileBehaviour != null)
            {
                projectileBehaviour.Init(projectileSpeed, distance, projectilePool, startPosition, targetPosition);
                PlayShotSound();
            }
        }    
    }
}
