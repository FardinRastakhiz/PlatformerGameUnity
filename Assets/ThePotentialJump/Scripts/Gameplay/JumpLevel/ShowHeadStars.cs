using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Gameplay
{
    public class ShowHeadStars : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer headStars;
        [SerializeField] private UnityEvent StartShowing;
        [SerializeField] private UnityEvent StopShowing;
        [Space]
        [SerializeField] private bool headStarsOnStart = false;

        private void Start()
        {
            if(headStarsOnStart)
                OnHeadHit();
        }

        private Coroutine showStarsCoroutine;
        public void OnHeadHit()
        {
            if (showStarsCoroutine != null) StopCoroutine(showStarsCoroutine);
            showStarsCoroutine = StartCoroutine(TransientShowing());
        }

        IEnumerator TransientShowing()
        {
            var angles = headStars.transform.eulerAngles;
            var color = headStars.color;
            color.a = 1.0f;
            headStars.color = color;
            StartShowing?.Invoke();
            float timer = 0.0f;
            while (timer < 2.0f)
            {
                angles.z = Mathf.Sin(Time.time * 5) * 15.0f;
                headStars.transform.eulerAngles = angles;
                timer += Time.deltaTime;
                yield return null;
            }

            while (color.a > float.Epsilon)
            {
                angles.z = Mathf.Sin(Time.time * 5) * 15.0f;
                headStars.transform.eulerAngles = angles;
                color.a -= Time.deltaTime;
                headStars.color = color;
                yield return null;
            }

            StopShowing?.Invoke();
        }

    }
}