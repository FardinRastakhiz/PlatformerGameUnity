using UnityEngine;
using System.Collections.Generic;

namespace Fallencake.Tools
{
	/// <summary>
	/// A simple object pooler that can output only a single type of objects
	/// </summary>
	[AddComponentMenu("Fallencake/Tools/Object Pool/FCSingleTypeObjectPooler")]
    public class FCSingleTypeObjectPooler : FCObjectPooler 
	{
	    /// the game object we'll instantiate 
		public GameObject ObjectToPool;
	    /// the number of objects we'll add to the pool
		public int PoolSize = 20;
	    /// if true, the pool will automatically add objects to the itself if needed
		public bool PoolCanIncrease = true;
		/// current object pool variables
		protected List<GameObject> _pooledGameObjects;

		/// <summary>
		/// Fills the object pool with the amount of objects you've specified in the inspector.
		/// </summary>
		public override void FillThePool()
	    {
            if (ObjectToPool == null)
            {
                return;
            }

			CreatePool ();

			// initializing the pool
			_pooledGameObjects = new List<GameObject>();

            int objectsToSpawn = PoolSize;

            if (_objectPool != null)
            {
                objectsToSpawn -= _objectPool.PooledGameObjects.Count;
                _pooledGameObjects = new List<GameObject>(_objectPool.PooledGameObjects);
            }

            // we add to the pool the specified number of objects
            for (int i = 0; i < objectsToSpawn; i++)
	        {
                AddTheObjectToThePool();
	        }
	    }

		/// <summary>
		/// Set the name of the object pool.
		/// </summary>
		/// <returns>The object pool name.</returns>
		protected override string SetObjectPoolName()
		{
			return ("[SingleTypeObjectPooler] " + this.name);	
		}
	    	
	    /// <summary>
	    /// This method returns one inactive object from the pool
	    /// </summary>
	    /// <returns>The pooled game object.</returns>
		public override GameObject GetPooledGameObject()
		{
			// we go through the pool looking for an inactive object
			for (int i=0; i< _pooledGameObjects.Count; i++)
			{
				if (!_pooledGameObjects[i].gameObject.activeInHierarchy)
	            {
	            	// if we find one, we return it
	                return _pooledGameObjects[i];
				}
			}
			// if we haven't found an inactive object (the pool is empty), and if we can extend it, we add one new object to the pool, and return it		
			if (PoolCanIncrease)
			{
				return AddTheObjectToThePool();
			}
			// if the pool is empty and can't grow, we return nothing.
			return null;
		}
		
		/// <summary>
		/// Adds one object of the specified type (in the inspector) to the pool.
		/// </summary>
		/// <returns>The one object to the pool.</returns>
		protected virtual GameObject AddTheObjectToThePool()
		{
			if (ObjectToPool == null)
			{
				Debug.LogWarning("The "+gameObject.name+" ObjectPooler doesn't have any GameObjectToPool defined.", gameObject);
				return null;
			}
			GameObject newGameObject = (GameObject)Instantiate(ObjectToPool);
			newGameObject.gameObject.SetActive(false);
			if (inParent)
			{
				newGameObject.transform.SetParent(_Pool.transform);	
			}
			newGameObject.name = ObjectToPool.name + "-" + _pooledGameObjects.Count;

			_pooledGameObjects.Add(newGameObject);

            _objectPool.PooledGameObjects.Add(newGameObject);

            return newGameObject;
		}
	}
}