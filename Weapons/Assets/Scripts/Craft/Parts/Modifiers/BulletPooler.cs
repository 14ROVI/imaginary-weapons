namespace Assets.Scripts.Craft.Parts.Modifiers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BulletPooler : MonoBehaviour
    {
        public static BulletPooler SharedInstance;
        public List<GameObject> pooledObjects;
        public GameObject objectToPool;
        public int amountToPool;


        void Awake()
        {
            SharedInstance = this;
        }

        // Update is called once per frame
        void Start()
        {
            objectToPool = Mod.Instance.ResourceLoader.LoadAsset<GameObject>("Assets/Content/Craft/Parts/Prefabs/Standard Bullet.prefab");
            amountToPool = 50;

            pooledObjects = new List<GameObject>();
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject obj = Instantiate(objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }


        public GameObject GetPooledObject()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }
            
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }
    }
}