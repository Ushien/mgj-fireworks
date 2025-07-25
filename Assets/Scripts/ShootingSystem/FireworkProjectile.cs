using System.Collections;
using UnityEngine;
using Utils;

namespace ShootingSystem {
    public class FireworkProjectile : MonoBehaviour
    {
        private ObjectPool pool; /// The object pool managing the projectile instances.
        private float projectileSpeed; /// The speed of the projectile.
        private float distance; /// The distance the projectile will travel before returning to the pool.
        private float remainingDistance; /// The remaining distance the projectile will travel before returning to the pool.
        private Vector2 startPosition; /// The starting position of the projectile.
        private Vector2 targetPosition; /// The target position the projectile will move towards.

        /// <summary>
        /// Initializes the projectile with the specified parameters.
        /// </summary>
        /// <param name="projectileSpeed">The speed of the projectile (unused in this implementation).</param>
        /// <param name="pool">The object pool managing the projectile instances.</param>
        /// <param name="distance">The distance the projectile will travel before returning to the pool.</param>
        /// <param name="startPosition">The starting position of the projectile.</param>
        /// <param name="targetPosition">The target position the projectile will move towards.</param
        public void Init(float projectileSpeed, float distance, ObjectPool pool, Vector2 startPosition, Vector2 targetPosition)
        {
            this.startPosition = startPosition;
            this.targetPosition = targetPosition;
            this.projectileSpeed = projectileSpeed;
            this.pool = pool;
            this.distance = distance;
            this.remainingDistance = distance;
        }

        /// <summary>
        /// Moves the projectile towards the target position and handles the impact effect.
        /// </summary>
        private void Move()
        {
            if(remainingDistance > 0){
                transform.position = Vector2.Lerp(startPosition, targetPosition, 1 - (remainingDistance / distance));
                remainingDistance -=  projectileSpeed * Time.deltaTime;
            }
            else{
                //Impact effect can be added here
                pool.ReturnObject(gameObject);
            }
        }

        /// <summary>
        /// Updates the projectile's position each frame.
        /// </summary>
        private void Update()
        {
            Move();
        }
    }
}
