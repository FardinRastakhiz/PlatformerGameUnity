using UnityEngine;

namespace Fallencake.Tools
{
	/// <summary>
	/// This class should be added to the object that you need to be pooled from Object Pooler.
	/// </summary>
	[AddComponentMenu("Fallencake/Tools/Object Pool/FCPoolableObject")]
    public class FCPoolableObject : MonoBehaviour 
	{

        [Header("Poolable Object")]
		/// The life time, in seconds, of the object. If set to any positive value it'll be set inactive after that time. Value of 0 means the object will live forever.
		public float lifeTime = 0f;

        /// <summary>
        /// Called every frame.
        /// </summary>
        protected virtual void Update() { }

        /// <summary>
        /// Deactivate the instance to reuse it.
        /// </summary>
        public virtual void Destroy()
		{
			gameObject.SetActive(false);
        }

		/// <summary>
		/// Called when the objects get enabled.
		/// </summary>
		protected virtual void OnEnable()
		{
			if (lifeTime > 0f)
			{
                Invoke("Destroy", lifeTime);
            }
		}

		/// <summary>
		/// Called when the object gets disabled.
		/// </summary>
	    protected virtual void OnDisable()
		{
			CancelInvoke();
		}		
	}
}
