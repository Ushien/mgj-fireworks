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
        private List<PowderModificator> powderList;

        /// <summary>
        /// Initializes the projectile with the specified parameters.
        /// </summary>
        /// <param name="pool">The object pool managing the projectile instances.</param>
        /// <param name="startPosition">The starting position of the projectile.</param>
        public void Init(ObjectPool pool, Vector2 startPosition, List<PowderModificator> powderList)
        {
            this.startPosition = startPosition;
            this.pool = pool;
            this.powderList = powderList;

            //Place les purplePowders dans une liste à part, afin de gérer l'héritage des caractéristiques
            List<PowderModificator> purplePowders = new List<PowderModificator>();
            List<PowderModificator> otherPowders = new List<PowderModificator>();

            foreach (PowderModificator powder in powderList)
            {
                if (powder is PurplePowder)
                {
                    purplePowders.Add(powder);
                }
                else
                {
                    otherPowders.Add(powder);
                }
            }

            foreach (PowderModificator powder in otherPowders)
            {
                powder.attachedFirework = GetComponent<ParticleSystem>();
                powder.ApplyModifier();
            }

            foreach (PowderModificator purplePowder in purplePowders)
            {
                purplePowder.attachedFirework = GetComponent<ParticleSystem>();
                purplePowder.ApplyModifier(otherPowders);
            }
        }

        public void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive(withChildren: true))
            {
                pool.ReturnObject(gameObject);
            }
        }
    }
}
