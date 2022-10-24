using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Fallencake.Tools
{
	/// <summary>
	/// A basic pooler class, uses to be extended for SingleTypeObjectPooler and MultiTypeObjectPooler classes.
	/// </summary>
	public class FCObjectPooler : MonoBehaviour
	{
		public static FCObjectPooler Instance;		
		/// if true, all pooled objects will be placed into the parent game object. If false, they'll be at top level in the hierarchy.
		public bool inParent = true;
		/// if true, the pool will try to use available pool with the same name instead of creating a new one.
		public bool reusePools = false;
		/// the parent object that the pool will be contained in.
		public Transform parentObject = null;
		/// current object pool variables
		protected GameObject _Pool = null;
        protected FCObjectPool _objectPool;

		/// <summary>
		/// Fill the object pool on Awake
		/// </summary>
		protected virtual void Awake()
	    {
			Instance = this;
			FillThePool();
	    }

		/// <summary>
		/// Creates a new pool or reuses an available one
		/// </summary>
		protected virtual void CreatePool()
		{
			if (!inParent)
			{
				return;
			}
			
			if (!reusePools)
			{
				// creates a container for all the objects we need to pool
				_Pool = new GameObject(SetObjectPoolName());
				_Pool.transform.SetParent(parentObject, false);
				_objectPool = _Pool.AddComponent<FCObjectPool>();
                _objectPool.PooledGameObjects = new List<GameObject>();
                return;
			}
			else
			{
				GameObject waitingPool = GameObject.Find (SetObjectPoolName());
				if (waitingPool != null)
                {
                    _Pool = waitingPool;
                    _objectPool = _Pool.FCGetComponentNoAlloc<FCObjectPool>();
                }
				else
				{
					_Pool = new GameObject(SetObjectPoolName());
					_Pool.transform.SetParent(parentObject, false);
					_objectPool = _Pool.AddComponent<FCObjectPool>();
                    _objectPool.PooledGameObjects = new List<GameObject>();
                }
			}
		}

		/// <summary>
		/// Sets the name of the object pool.
		/// </summary>
		/// <returns>The object pool name.</returns>
		protected virtual string SetObjectPoolName()
		{
			return ("[ObjectPooler] " + this.name);	
		}

		/// <summary>
		/// Use this method to fill the pool with objects
		/// </summary>
	    public virtual void FillThePool()
	    {
	        return ;
	    }

		/// <summary>
		/// Use this method to return a pooled gameobject
		/// </summary>
		/// <returns>The pooled game object.</returns>
		public virtual GameObject GetPooledGameObject()
	    {
	        return null;
	    }

        /// <summary>
        /// Destroys the object pool
        /// </summary>
        public virtual void DestroyObjectPool()
        {
            if (_Pool != null)
            {
                Destroy(_Pool.gameObject);
            }
        }
    }
}