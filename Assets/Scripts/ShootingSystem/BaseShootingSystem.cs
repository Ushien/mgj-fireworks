using UnityEngine;
using Utils;

namespace ShootingSystem {
    public class BaseShootingSystem : MonoBehaviour
    {
        [SerializeField] private ObjectPool projectilePool;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private AudioClip shotSound;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float projectileSpeed = 20f;
        [SerializeField] private float distance = 0f;
        [SerializeField] private Vector2 projectileSpreadVariance = new Vector2(0f, 0f);

        public void Shoot()
        {
            Vector2 direction = GetDirection();
            Vector2 startPosition = projectileSpawnPoint.position;
            Vector2 targetPosition = startPosition + direction * distance;
            ShotEffects(startPosition, targetPosition);
        }
        
        public virtual void Canceled()
        {
            throw new System.NotImplementedException();
        }

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

        protected void PlayShotSound(){
            if (shotSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shotSound);
            }
        }

        protected void ShotEffects(Vector2 startPosition, Vector2 targetPosition) 
        {
            GameObject projectile = projectilePool.GetObject();
            projectile.transform.position = projectileSpawnPoint.position;

            FireworkProjectile projectileBehaviour = projectile.GetComponent<FireworkProjectile>();
            if (projectileBehaviour != null)
            {
                projectileBehaviour.Init(projectileSpeed,distance,projectilePool,startPosition,targetPosition);
                PlayShotSound();
            }
        }    
    }
}
