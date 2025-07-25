using System.Collections;
using UnityEngine;
using Utils;

namespace ShootingSystem {
    public class FireworkProjectile : MonoBehaviour
    {
        private ObjectPool pool; /// The object pool managing the projectile instances.
        private Vector2 startPosition; /// The starting position of the projectile.

        /// <summary>
        /// Initializes the projectile with the specified parameters.
        /// </summary>
        /// <param name="pool">The object pool managing the projectile instances.</param>
        /// <param name="startPosition">The starting position of the projectile.</param>
        public void Init(ObjectPool pool, Vector2 startPosition)
        {
            this.startPosition = startPosition;
            this.pool = pool;
        }
    }
}
