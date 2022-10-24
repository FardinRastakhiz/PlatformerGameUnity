using UnityEngine;
using System;
using System.Collections.Generic;

namespace Fallencake.Tools
{
	[Serializable]
	/// <summary>
	/// Multiple types pooler object.
	/// </summary>
	public class FCMultiTypeObjectPoolerObject
	{
		public GameObject objectToPool;
		public int PoolSize;
		public int Priority;
		public bool PoolCanIncrease = true;
		public bool Enabled = true;
	}

	/// <summary>
	/// The methods you can pull objects from the pool with
	/// </summary>
	public enum FCPoolingMethods { SequentialOrder, InTurnTypeBased, InTurnPriorityBased, RandomPoolBased, RandomTypeBased, RandomPriorityBased }

	/// <summary>
	/// This class allows you to have a pool of various objects to pool from.
	/// </summary>
	[AddComponentMenu("Fallencake/Tools/Object Pool/FCMultiTypeObjectPooler")]
	public class FCMultiTypeObjectPooler : FCObjectPooler
	{
		/// the list of objects to pool
		public List<FCMultiTypeObjectPoolerObject> Pool;
		[FCInfo("Methods you can pull objects from the pool with:\n- SequentialOrder - the pooler will get all object of current type before moving to the next type object\n- InTurnTypeBased - objects will be spawned in the order has seted in the inspector (from top to bottom)\n- InTurnPriorityBased - tries to get an object from the pool in order, based on the type Priority value, probability to be get picked depends on the Priority value too (the larger the Priority value, the higher the probability it'll be chosen)\n- RandomPoolBased - the pooler will get one object from the whole pool, at random, each object has equal chances to be chosen\n- RandomTypeBased - randomly chooses the type of the object (the larger the amount of object of the specific type, the higher the chances it'll be chosen)\n-RandomPriorityBased - randomly chooses one object from the pool, based on Priority value of its type probability (the larger the Priority, the higher the chances it'll be chosen)", Fallencake.Tools.FCInfoAttribute.InformationType.Info, false)]
		/// the chosen pooling method
		public FCPoolingMethods PoolingMethod = FCPoolingMethods.RandomTypeBased;
		[FCInfo("CanPoolSameTypeNext allows the Pooler to get the next object from pool of the same type as previous.\nIf set to false - the Pooler will get an object of different type to avoid repetition. Only affects random pooling methods.", Fallencake.Tools.FCInfoAttribute.InformationType.Info, false)]
		/// whether or not the same type object can be pooled twice in a row.
		public bool CanPoolSameTypeNext = true;
		/// current object pool variables
		protected List<GameObject> _pooledGameObjects;
		protected List<GameObject> _pooledGameObjectsOriginalOrder;
		protected List<FCMultiTypeObjectPoolerObject> _randomizedPool;
		protected List<FCMultiTypeObjectPoolerObject> _priorityPool;
		protected string _lastPooledObjectName;
		protected int _currentIndex = 0;

		/// <summary>
		/// Set the name of the object pool.
		/// </summary>
		/// <returns>The object pool name.</returns>
		protected override string SetObjectPoolName()
		{
			return ("[MultiTypeObjectPooler] " + this.name);
		}

		/// <summary>
		/// Sorts the list of pooler objects in order, based on its Priority values (from highest to lowest).
		/// </summary>
		/// <returns>The comparison parameter to use when comparing elements.</returns>
		private int SortFunc(FCMultiTypeObjectPoolerObject a, FCMultiTypeObjectPoolerObject b)
        {
			if(a.Priority < b.Priority)
            {
				return 1;
            }
			else if (a.Priority > b.Priority)
            {
				return -1;
			}
			return 0;
        }

		/// <summary>
		/// The basic Probability function that will take the item chance, based on its Priority value and then check if you'll get or not.
		/// </summary>
		/// <returns><c>true</c>, if the attempt to get object was successful, <c>false</c> otherwise.</returns>
		protected virtual bool ProbabilityCheck(int itemPriority)
		{
			int prioritySum = 0;
			foreach (FCMultiTypeObjectPoolerObject pooledGameObject in Pool)
			{
				prioritySum += pooledGameObject.Priority;
			}
			float itemProbability = (float)itemPriority / (float)prioritySum *100f;
			float rnd = UnityEngine.Random.Range(1, 101);
			if (rnd <= itemProbability)
            {
				return true;
			}
            else
            {
				return false;
			}
		}

		/// <summary>
		/// Fills the object pool with the amount of objects you specified in the inspector.
		/// </summary>
		public override void FillThePool()
		{
			CreatePool();
			// initializing the pool
			_pooledGameObjects = new List<GameObject>();
			// creating a pool based on the priority value
			_priorityPool = new List<FCMultiTypeObjectPoolerObject>(Pool);
			_priorityPool.Sort(SortFunc);
			// creating a randomized pool for picking purposes
			_randomizedPool = new List<FCMultiTypeObjectPoolerObject>();
			for (int i = 0; i < Pool.Count; i++)
			{
				_randomizedPool.Add(Pool[i]);
			}
			_randomizedPool.FCShuffle();

			// if there's only one item in the Pool, we force CanPoolSameObjectTwice to true
			if (Pool.Count <= 1)
			{
				CanPoolSameTypeNext = true;
			}

			bool stillObjectsToPool;
			int[] poolSizes;

			// if we're gonna pool in the original inspector order
			switch (PoolingMethod)
			{
				case FCPoolingMethods.SequentialOrder:
					_pooledGameObjectsOriginalOrder = new List<GameObject>();
					// we store our poolsizes in a temp array so it doesn't impact the inspector
					foreach (FCMultiTypeObjectPoolerObject pooledGameObject in Pool)
					{
						for (int i = 0; i < pooledGameObject.PoolSize; i++)
						{
							AddTheObjectToThePool(pooledGameObject.objectToPool);
						}
					}
					break;
				case FCPoolingMethods.InTurnTypeBased:
					stillObjectsToPool = true;
					_pooledGameObjectsOriginalOrder = new List<GameObject>();

					// we store our poolsizes in a temp array so it doesn't impact the inspector
					poolSizes = new int[Pool.Count];
					for (int i = 0; i < Pool.Count; i++)
					{
						poolSizes[i] = Pool[i].PoolSize;
					}

					// we go through our objects in the order they were in the inspector, and fill the pool while we find objects to add
					while (stillObjectsToPool)
					{
						stillObjectsToPool = false;
						for (int i = 0; i < Pool.Count; i++)
						{
							if (poolSizes[i] > 0)
							{
								AddTheObjectToThePool(Pool[i].objectToPool);
								poolSizes[i]--;
								stillObjectsToPool = true;
							}
						}
					}
					break;
				case FCPoolingMethods.InTurnPriorityBased:
					stillObjectsToPool = true;
					_pooledGameObjectsOriginalOrder = new List<GameObject>();

					// we store our poolsizes in a temp array so it doesn't impact the inspector
					poolSizes = new int[_priorityPool.Count];
					for (int i = 0; i < _priorityPool.Count; i++)
					{
						poolSizes[i] = _priorityPool[i].PoolSize;
					}

					// we go through our objects in the order they were in the inspector, and fill the pool while we find objects to add
					while (stillObjectsToPool)
					{
						stillObjectsToPool = false;
						for (int i = 0; i < _priorityPool.Count; i++)
						{
							if (poolSizes[i] > 0)
							{
								AddTheObjectToThePool(_priorityPool[i].objectToPool);
								poolSizes[i]--;
								stillObjectsToPool = true;
							}
						}
					}
					break;
				default:
					int k = 0;
					// for each type of object specified in the inspector
					foreach (FCMultiTypeObjectPoolerObject pooledGameObject in Pool)
					{
						// if there's no specified number of objects to pool for that type of object, we do nothing and exit
						if (k > Pool.Count) { return; }

						// we add, one by one, the number of objects of that type, as specified in the inspector
						for (int j = 0; j < Pool[k].PoolSize; j++)
						{
							AddTheObjectToThePool(pooledGameObject.objectToPool);
						}
						k++;
					}
					break;
			}
			if ((PoolingMethod == FCPoolingMethods.InTurnTypeBased) || (PoolingMethod == FCPoolingMethods.SequentialOrder))
			{
				foreach (GameObject pooledObject in _pooledGameObjects)
				{
					_pooledGameObjectsOriginalOrder.Add(pooledObject);
				}
			}


		}

		/// <summary>
		/// Adds one object of the specified type to the object pool.
		/// </summary>
		/// <returns>The object that just got added.</returns>
		/// <param name="typeOfObject">The type of object to add to the pool.</param>
		protected virtual GameObject AddTheObjectToThePool(GameObject typeOfObject)
		{
			GameObject newGameObject = (GameObject)Instantiate(typeOfObject);
			newGameObject.gameObject.SetActive(false);
			if (inParent)
			{
				newGameObject.transform.SetParent(_Pool.transform, false);
			}
			newGameObject.name = typeOfObject.name;
			_pooledGameObjects.Add(newGameObject);
			return newGameObject;
		}

		/// <summary>
		/// Gets an object from the pool.
		/// </summary>
		/// <returns>The pooled game object.</returns>
		public override GameObject GetPooledGameObject()
		{
			GameObject pooledGameObject;
			switch (PoolingMethod)
			{
				case FCPoolingMethods.SequentialOrder:
					pooledGameObject = GetPooledObjectOriginalOrder();
					break;
				case FCPoolingMethods.InTurnTypeBased:
					pooledGameObject = GetPooledObjectOriginalOrder();
					break;
				case FCPoolingMethods.InTurnPriorityBased:
					pooledGameObject = GetPooledObjectInTurnPriorityBased();
					break;
				case FCPoolingMethods.RandomPoolBased:
					pooledGameObject = GetPooledObjectRandomPoolBased();
					break;
				case FCPoolingMethods.RandomTypeBased:
					pooledGameObject = GetPooledObjectRandomTypeBased();
					break;
				case FCPoolingMethods.RandomPriorityBased:
					pooledGameObject = GetPooledObjectRandomPriorityBased();
					break;
				default:
					pooledGameObject = null;
					break;
			}
			if (pooledGameObject != null)
			{
				_lastPooledObjectName = pooledGameObject.name;
			}
			else
			{
				_lastPooledObjectName = "";
			}
			return pooledGameObject;
		}

		/// <summary>
		/// The Pooler will get an object from the pool according to the order the list has been setup in.
		/// </summary>
		/// <returns>The pooled object original order.</returns>
		protected virtual GameObject GetPooledObjectOriginalOrder()
		{
			int newIndex;
			// if the end of the list has been reached, it start again from the beginning
			if (_currentIndex >= _pooledGameObjectsOriginalOrder.Count)
			{
				ResetCurrentIndex();
			}

			FCMultiTypeObjectPoolerObject searchedObject = GetPoolObject(_pooledGameObjects[_currentIndex].gameObject);

			if (_currentIndex >= _pooledGameObjects.Count)
			{
				return null;
			}
			if (!searchedObject.Enabled)
			{
				_currentIndex++;
				return null;
			}

			// if the object is already active, we need to find another one
			if (_pooledGameObjects[_currentIndex].gameObject.activeInHierarchy)
			{
				GameObject findObject = FindInactiveObject(_pooledGameObjects[_currentIndex].gameObject.name, _pooledGameObjects);
				if (findObject != null)
				{
					_currentIndex++;
					return findObject;
				}

				// if its pool can expand, we create a new one
				if (searchedObject.PoolCanIncrease)
				{
					_currentIndex++;
					return AddTheObjectToThePool(searchedObject.objectToPool);
				}
				else
				{
					// if it can't expand we return nothing
					return null;
				}
			}
			else
			{
				// if the object is inactive, we return it
				newIndex = _currentIndex;
				_currentIndex++;
				return _pooledGameObjects[newIndex];
			}
		}

		/// <summary>
		/// Randomly choses the type of the object (the larger the amount of object of the specific type, the higher the chances it'll be chosen).
		/// </summary>
		/// <returns>The pooled game object based on the amount of instances of each game object type.</returns>
		protected virtual GameObject GetPooledObjectRandomTypeBased()
		{
			// get a random index 
			int randomIndex = UnityEngine.Random.Range(0, _pooledGameObjects.Count);
			int overflowCounter = 0;
			// we check to see if that object is enabled, if it's not we loop
			while (!PoolObjectEnabled(_pooledGameObjects[randomIndex]) && overflowCounter < _pooledGameObjects.Count)
			{
				randomIndex = UnityEngine.Random.Range(0, _pooledGameObjects.Count);
				overflowCounter++;
			}
			if (!PoolObjectEnabled(_pooledGameObjects[randomIndex]))
			{
				return null;
			}

			// if it's impossible to pool the next object of the same type as previous, the method will get an object of different type
			overflowCounter = 0;
			while (!CanPoolSameTypeNext	&& _pooledGameObjects[randomIndex].name == _lastPooledObjectName && overflowCounter < _pooledGameObjects.Count)
            {
				randomIndex = UnityEngine.Random.Range(0, _pooledGameObjects.Count);
				overflowCounter++;
			}

			// checking if the item is active
			if (_pooledGameObjects[randomIndex].gameObject.activeInHierarchy)
			{
				// trying to find another inactive object of the same type
				GameObject pulledObject = FindInactiveObject(_pooledGameObjects[randomIndex].gameObject.name, _pooledGameObjects);
				if (pulledObject != null)
				{
					return pulledObject;
				}
				else
				{
					// if we couldn't find an inactive object of this type, we see if it can add new one to the pool
					FCMultiTypeObjectPoolerObject searchedObject = GetPoolObject(_pooledGameObjects[randomIndex].gameObject);
					if (searchedObject == null)
					{
						return null;
					}
					if (searchedObject.PoolCanIncrease)
					{
						return AddTheObjectToThePool(searchedObject.objectToPool);
					}
					else
					{
						return null;
					}
				}
			}
			else
			{
				return _pooledGameObjects[randomIndex];
			}
		}

		/// <summary>
		/// The pooler will get one object from the whole pool, at random, each object has equal chances to be chosen.
		/// </summary>
		/// <returns>The pooled object random between all objects.</returns>
		protected virtual GameObject GetPooledObjectRandomPoolBased()
		{
			// we pick one of the objects in the original pool at random
			int randomIndex = UnityEngine.Random.Range(0, Pool.Count);
			int overflowCounter = 0;
			// if it's impossible to pool the next object of the same type as previous, the method will get an object of different type
			while (!CanPoolSameTypeNext && Pool[randomIndex].objectToPool.name == _lastPooledObjectName && overflowCounter < _pooledGameObjects.Count)
			{
				randomIndex = UnityEngine.Random.Range(0, Pool.Count);
				overflowCounter++;
			}
			int originalRandomIndex = randomIndex + 1;
			bool objectFound = false;

			// while we haven't found an object to return, and while we haven't gone through all the different object types, we keep going
			overflowCounter = 0;
			while (!objectFound && randomIndex != originalRandomIndex && overflowCounter < _pooledGameObjects.Count)
			{
				// if our index is at the end, we reset it
				if (randomIndex >= Pool.Count)
				{
					randomIndex = 0;
				}

				if (!Pool[randomIndex].Enabled)
				{
					randomIndex++;
					overflowCounter++;
					continue;
				}

				// we try to find an inactive object of that type in the pool
				GameObject newGameObject = FindInactiveObject(Pool[randomIndex].objectToPool.name, _pooledGameObjects);
				if (newGameObject != null)
				{
					objectFound = true;
					return newGameObject;
				}
				else
				{
					// if there's none and if we can expand, we expand
					if (Pool[randomIndex].PoolCanIncrease)
					{
						return AddTheObjectToThePool(Pool[randomIndex].objectToPool);
					}
				}
				randomIndex++;
				overflowCounter++;
			}
			return null;
		}

		/// <summary>
		/// Tries to get an object from the pool in order based on the type Priority value, probability to be get picked depends on the Priorit value too (the larger the Priority value, the higher the probability it'll be chosen)
		/// </summary>
		/// <returns>The pooled game object random between objects.</returns>
		protected virtual GameObject GetPooledObjectInTurnPriorityBased()
		{
			// we pick one of the objects in the original pool at random
			int typeIndex = 0;
			int overflowCounter = 0;

			// if it's impossible to pool the next object of the same type as previous, the method will get an object of different type
			while (!CanPoolSameTypeNext && _priorityPool[typeIndex].objectToPool.name == _lastPooledObjectName && overflowCounter < _pooledGameObjects.Count)
			{
				typeIndex++;
				if (typeIndex >= _priorityPool.Count)
				{
					typeIndex = 0;
				}
				overflowCounter++;
			}

			int originalTypeIndex = typeIndex + 1;
			bool objectFound = false;

			// checking the probability of an object appearing depending on the priority of its type.
			overflowCounter = 0;
			while (!ProbabilityCheck(_priorityPool[typeIndex].Priority) && overflowCounter < _pooledGameObjects.Count)
			{
				typeIndex++;
				if (typeIndex >= Pool.Count)
				{
					typeIndex = 0;
				}
				overflowCounter++;
			}

			// while we haven't found an object to return, and while we haven't gone through all the different object types, we keep going
			overflowCounter = 0;
			while (!objectFound && overflowCounter < _pooledGameObjects.Count)
			{
				// if our index is at the end, we reset it
				if (typeIndex >= _priorityPool.Count)
				{
					typeIndex = 0;
				}

				if (!_priorityPool[typeIndex].Enabled)
				{
					typeIndex++;
					overflowCounter++;
					continue;
				}

				// we try to find an inactive object of that type in the pool
				GameObject newGameObject = FindInactiveObject(_priorityPool[typeIndex].objectToPool.name, _pooledGameObjects);
				if (newGameObject != null)
				{
					objectFound = true;
					return newGameObject;
				}
				else
				{
					// if there's none and if we can expand, we expand
					if (_priorityPool[typeIndex].PoolCanIncrease)
					{
						return AddTheObjectToThePool(_priorityPool[typeIndex].objectToPool);
					}
				}
				typeIndex++;
				overflowCounter++;
			}
			return null;
		}

		/// <summary>
		/// Gets one object from the pool, at random, but ignoring its pool size, each object has equal chances to get picked
		/// </summary>
		/// <returns>The pooled game object random between objects.</returns>
		protected virtual GameObject GetPooledObjectRandomPriorityBased()
		{
			// we pick one of the objects in the original pool at random
			int typeIndex = 0;
			int overflowCounter = 0;

			// if it's impossible to pool the next object of the same type as previous, the method will get an object of different type
			while (!CanPoolSameTypeNext && _priorityPool[typeIndex].objectToPool.name == _lastPooledObjectName && overflowCounter < _pooledGameObjects.Count)
			{
				typeIndex++;
				if (typeIndex >= _priorityPool.Count)
				{
					typeIndex = 0;
				}
				overflowCounter++;
			}

			int originalTypeIndex = typeIndex + 1;
			bool objectFound = false;

			// checking the probability of an object appearing depending on the priority of its type.
			overflowCounter = 0;
			while (!ProbabilityCheck(_priorityPool[typeIndex].Priority) && overflowCounter < _pooledGameObjects.Count)
			{
				typeIndex++;
				if (typeIndex >= Pool.Count)
				{
					typeIndex = 0;
				}
				overflowCounter++;
			}

			// while we haven't found an object to return, and while we haven't gone through all the different object types, we keep going
			overflowCounter = 0;
			while (!objectFound && overflowCounter < _pooledGameObjects.Count)
			{
				// if our index is at the end, we reset it
				if (typeIndex >= _priorityPool.Count)
				{
					typeIndex = 0;
				}

				if (!_priorityPool[typeIndex].Enabled)
				{
					typeIndex++;
					overflowCounter++;
					continue;
				}

				// we try to find an inactive object of that type in the pool
				GameObject newGameObject = FindInactiveObject(_priorityPool[typeIndex].objectToPool.name, _pooledGameObjects);
				if (newGameObject != null)
				{
					objectFound = true;
					return newGameObject;
				}
				else
				{
					// if there's none and if we can expand, we expand
					if (_priorityPool[typeIndex].PoolCanIncrease)
					{
						return AddTheObjectToThePool(_priorityPool[typeIndex].objectToPool);
					}
				}
				typeIndex++;
				overflowCounter++;
			}
			return null;
		}

		/// <summary>
		/// Gets an object of the specified name from the pool
		/// </summary>
		/// <returns>The pooled game object of type.</returns>
		/// <param name="type">Type.</param>
		protected virtual GameObject GetPooledGameObjectOfType(string searchedName)
		{
			GameObject newObject = FindInactiveObject(searchedName, _pooledGameObjects);

			if (newObject != null)
			{
				return newObject;
			}
			else
			{
				// if we've not returned the object, that means the pool is empty (at least it means it doesn't contain any object of that specific type)
				// so if the pool is allowed to expand
				GameObject searchedObject = FindObject(searchedName, _pooledGameObjects);
				if (searchedObject == null)
				{
					return null;
				}

				if (GetPoolObject(FindObject(searchedName, _pooledGameObjects)).PoolCanIncrease)
				{
					// we create a new game object of that type, we add it to the pool for further use, and return it.
					GameObject newGameObject = (GameObject)Instantiate(searchedObject);
					_pooledGameObjects.Add(newGameObject);
					return newGameObject;
				}
			}

			// if the pool was empty for that object and not allowed to expand, we return nothing.
			return null;
		}

		/// <summary>
		/// Finds an inactive object in the pool based on its name.
		/// Returns null if no inactive object by that name were found in the pool
		/// </summary>
		/// <returns>The inactive object.</returns>
		/// <param name="searchedName">Searched name.</param>
		protected virtual GameObject FindInactiveObject(string searchedName, List<GameObject> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				// if we find an object inside the pool that matches the asked type
				if (list[i].name.Equals(searchedName))
				{
					// and if that object is inactive right now
					if (!list[i].gameObject.activeInHierarchy)
					{
						// we return it
						return list[i];
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Finds an object in the pool based on its name, active or inactive
		/// Returns null if there's no object by that name in the pool
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="searchedName">Searched name.</param>
		protected virtual GameObject FindObject(string searchedName, List<GameObject> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				// if we find an object inside the pool that matches the asked type
				if (list[i].name.Equals(searchedName))
				{
					// and if that object is inactive right now
					return list[i];
				}
			}
			return null;
		}

		/// <summary>
		/// Try to return the MultiTypeObjectPoolerObject from the original Pool based on a GameObject.
		/// </summary>
		/// <returns>The pool object.</returns>
		/// <param name="testedObject">Tested object.</param>
		protected virtual FCMultiTypeObjectPoolerObject GetPoolObject(GameObject testedObject)
		{
			if (testedObject == null)
			{
				return null;
			}
			int i = 0;
			foreach (FCMultiTypeObjectPoolerObject poolerObject in Pool)
			{
				if (testedObject.name.Equals(poolerObject.objectToPool.name))
				{
					return poolerObject;
				}
				i++;
			}
			return null;
		}

		protected virtual bool PoolObjectEnabled(GameObject testedObject)
		{
			FCMultiTypeObjectPoolerObject searchedObject = GetPoolObject(testedObject);
			if (searchedObject != null)
			{
				return searchedObject.Enabled;
			}
			else
			{
				return false;
			}
		}

		public virtual void EnableObjects(string name, bool newStatus)
		{
			foreach (FCMultiTypeObjectPoolerObject poolerObject in Pool)
			{
				if (name.Equals(poolerObject.objectToPool.name))
				{
					poolerObject.Enabled = newStatus;
				}
			}
		}

		public virtual void ResetCurrentIndex()
		{
			_currentIndex = 0;
		}
	}
}