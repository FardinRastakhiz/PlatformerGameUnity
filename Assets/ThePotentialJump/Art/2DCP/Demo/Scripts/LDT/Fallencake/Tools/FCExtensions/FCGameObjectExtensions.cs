using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Fallencake.Tools
{
    public static class GameObjectExtensions
    {
        static List<Component> _cache = new List<Component>();

        /// <summary>
        /// Returns a component without wasting memory allocation
        /// </summary>
        /// <param name="this"></param>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public static Component FCGetComponentNoAlloc(this GameObject @this, System.Type componentType)
        {
            @this.GetComponents(componentType, _cache);
            var component = _cache.Count > 0 ? _cache[0] : null;
            _cache.Clear();
            return component;
        }

        /// <summary>
        /// Returns a component without wasting memory allocation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static T FCGetComponentNoAlloc<T>(this GameObject @this) where T : Component
        {
            @this.GetComponents(typeof(T), _cache);
            var component = _cache.Count > 0 ? _cache[0] : null;
            _cache.Clear();
            return component as T;
        }
    }
}
