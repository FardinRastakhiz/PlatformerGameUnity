using UnityEngine;

namespace ThePotentialJump.Utilities
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static bool destroyOnLoad = false;

        private static object lockobj = new object();
        private static T instance;

        public static T Instance
        {
            get
            {
                lock (lockobj)
                {

                    if (instance == null)
                    {
                        instance = FindObjectOfType<T>();
                        if (instance == null)
                            instance = new GameObject(typeof(T).Name)
                                .AddComponent<T>();
                    }

                    return instance;
                }
            }
        }


        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                if (!destroyOnLoad) DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

    }
}