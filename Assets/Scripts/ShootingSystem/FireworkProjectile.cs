using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace ShootingSystem {
    public class FireworkProjectile : MonoBehaviour
    {
        private ObjectPool pool; /// The object pool managing the projectile instances.
        private Vector2 startPosition; /// The starting position of the projectile.
        [SerializeField]
        private List<Powder> powderList;

        /// <summary>
        /// Initializes the projectile with the specified parameters.
        /// </summary>
        /// <param name="pool">The object pool managing the projectile instances.</param>
        /// <param name="startPosition">The starting position of the projectile.</param>
        public void Init(ObjectPool pool, Vector2 startPosition, List<Powder> powderList)
        {
            this.startPosition = startPosition;
            this.pool = pool;
            this.powderList = powderList;
            foreach (Powder powder in powderList)
            {
                powder.attachedFirework = this;
                powder.ApplyModifier();
            }
        }
    }
}
