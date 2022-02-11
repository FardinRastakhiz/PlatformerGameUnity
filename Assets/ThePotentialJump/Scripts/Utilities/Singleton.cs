using System;

namespace ThePotentialJump.Utilities
{
    public abstract class Singleton<T> where T:class
    {
        protected static bool destroyOnLoad = false;

        private static object lockobj = new object();
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                lock (lockobj)
                {
                    if (instance == null)
                        instance = Activator.CreateInstance<T>();

                    return instance;
                }
            }
        }

        protected Singleton()
        {
        }

    }
}