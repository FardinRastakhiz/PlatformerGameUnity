using UnityEngine;
using System.Collections.Generic;

namespace Fallencake.Tools
{
    public class FCObjectPool : MonoBehaviour
    {
        [FCReadOnly]
        public List<GameObject> PooledGameObjects;
    }
}
