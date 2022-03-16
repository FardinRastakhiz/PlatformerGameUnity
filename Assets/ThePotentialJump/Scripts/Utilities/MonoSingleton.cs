using UnityEngine;

namespace ThePotentialJump.Utilities
{

    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static bool destroyOnLoad = false;

        private static object lockobj = new object();
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                Debug.LogError($"Game object of type '{typeof(T).Name}' is not present in the scene!");
                return null;
                //lock (lockobj)
                //{
                //    if (instance == null)
                //    {
                //        instance = FindObjectOfType<T>();
                //        if (instance == null)
                //            instance = new GameObject(typeof(T).Name)
                //                .AddComponent<T>();
                //    }

                //    return instance;
                //}
            }
        }

        protected bool destroyed = false;
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                if (!destroyOnLoad) DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                destroyed = true;
                Destroy(this.gameObject);
            }
        }

    }
}