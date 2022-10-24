using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fallencake.Tools
{
    public class FloatingCloud : FCPoolableObject
    {
        private float _speed = 1;
        private float _endPosX;

        public void StartFloating(float speed, float endPosX)
        {
            _speed = speed;
            _endPosX = endPosX;
        }

        override protected void Update()
        {
            transform.Translate(Vector3.right * (Time.deltaTime * _speed));
            if (_speed > 0 ? (transform.position.x > _endPosX) : (transform.position.x < _endPosX))
            {
                Destroy();
            }
        }
    }
}