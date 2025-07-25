using System.Collections.Generic;
using UnityEngine;

namespace Utils {
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab; // Le prefab à instancier
        [SerializeField] private int initialPoolSize = 10; // Taille initiale du pool

        private Queue<GameObject> pool = new Queue<GameObject>();

        private void Awake()
        {
            // Pré-instancier les objets dans le pool
            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }

        public GameObject GetObject()
        {
            if (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                // Si le pool est vide, instancier un nouvel objet
                GameObject obj = Instantiate(prefab);
                return obj;
            }
        }

        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}
