using System;
using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    [Serializable]
    public class SpringPhysics
    {
        [SerializeField] private Projectile2D projectile;
        [SerializeField] private float springConstant = 0.1f;
        [SerializeField] private float dampingFactor = 0.1f;
        [SerializeField] private float springHeadMass;
        [SerializeField] private float swingThreshold = 0.1f;
        [SerializeField] private Rigidbody2D loadedweight;
        JumpRuler ruler;
        private Rigidbody2D unloadedWeight = null;
        private bool isSwinging = false;


        private WaitForSeconds waitFixedDeltaTime;
        Action<float> setSpringSize;

        private float fakeSpringConstant => 10 * springConstant;
        public float SpringConstant => springConstant;
        public void AddProjectile(Projectile2D projectile, bool resetConstant = false)
        {
            this.projectile = projectile;
            projectile?.SetupParameters(ruler);
            loadedweight = projectile.GetComponent<Rigidbody2D>();
            if (resetConstant)
            {
                springConstant = 30f * loadedweight.mass;
            }
        }
        public void SetParameters(Action<float> setSpringSize, JumpRuler ruler)
        {
            waitFixedDeltaTime = new WaitForSeconds(Time.fixedDeltaTime);
            this.setSpringSize = setSpringSize;
            this.ruler = ruler;
            projectile?.SetupParameters(ruler);
        }

        public void OnWeightCollided()
        {
            Debug.Log("Collided");
            if (loadedweight == null)
            {
                loadedweight = unloadedWeight;
                unloadedWeight = null;
            }
        }

        public void Disable()
        {
            isSwinging = false;
            setSpringSize?.Invoke(0);
        }

        public IEnumerator Swing(float startAmplitude)
        {
            var SE = springConstant * startAmplitude * startAmplitude / 2.0f;
            var v = Mathf.Sqrt((springConstant * startAmplitude * startAmplitude) / loadedweight.mass);
            projectile?.Project(Vector2.up * v);
            //loadedweight.velocity = Vector2.up * v;

            isSwinging = true;
            float addedmass = loadedweight == null ? 0 : loadedweight.mass;
            float overalMass = addedmass + springHeadMass;
            var delta = dampingFactor * dampingFactor - 4 * fakeSpringConstant * overalMass;
            if (delta > float.Epsilon)
                yield return OverDamepedSwing(startAmplitude, 0);
            else if (delta < -float.Epsilon)
                yield return UnderDampedSwing(startAmplitude, 0);
            else
                yield return CriticalSwing(startAmplitude, 0);
        }

        private IEnumerator OverDamepedSwing(float startAmplitude, float velocity)
        {
            float overalMass, t, last_x;
            InitializeSwingParameters(out overalMass, out t, out last_x);

            var delta = dampingFactor * dampingFactor - 4 * fakeSpringConstant * overalMass;
            var s1 = (-dampingFactor + Mathf.Sqrt(delta)) / (2 * overalMass);
            var s2 = (-dampingFactor - Mathf.Sqrt(delta)) / (2 * overalMass);
            var B = (velocity - startAmplitude * s1) / (s2 - s1);
            var A = startAmplitude - B;

            //var t_x_0 = (float)Math.Log((A / B), 2.71828f) / (s2 - s1);
            //var v_x_0 = A * s1 * Mathf.Exp(s1 * t_x_0) + B * s2 * Mathf.Exp(s2 * t_x_0);
            //loadedweight.velocity = Vector2.up * v_x_0;
            do
            {
                var x_t = A * Mathf.Exp(s1 * t) + B * Mathf.Exp(s2 * t);
                SwingIterationControl(ref t, ref last_x, ref x_t);
                yield return waitFixedDeltaTime;
            } while (isSwinging);
        }

        private IEnumerator CriticalSwing(float startAmplitude, float velocity)
        {
            float overalMass, t, last_x;
            InitializeSwingParameters(out overalMass, out t, out last_x);

            var A = startAmplitude;
            var B = velocity + (A * dampingFactor) / (2 * overalMass);
            var expFactor = -dampingFactor / (2 * overalMass);
            do
            {
                var x_t = (A + B * t) * Mathf.Exp(expFactor * t);
                SwingIterationControl(ref t, ref last_x, ref x_t);
                yield return waitFixedDeltaTime;
            } while (isSwinging);
        }


        private IEnumerator UnderDampedSwing(float startAmplitude, float velocity)
        {
            float overalMass, t, last_x;
            InitializeSwingParameters(out overalMass, out t, out last_x);

            var delta = 4 * fakeSpringConstant * overalMass - dampingFactor * dampingFactor;
            delta = Mathf.Sqrt(delta) / (2 * overalMass);
            var A = startAmplitude;
            var B = (velocity + (A * dampingFactor) / (2 * overalMass)) / delta;
            var expFactor = -dampingFactor / (2 * overalMass);


            do
            {
                var x_t = Mathf.Exp(expFactor * t) * (A * Mathf.Cos(delta * t) + B * Mathf.Sin(delta * t));
                SwingIterationControl(ref t, ref last_x, ref x_t);
                yield return waitFixedDeltaTime;
            } while (isSwinging);
        }

        private void InitializeSwingParameters(out float overalMass, out float t, out float last_x)
        {
            float addedmass = loadedweight == null ? 0 : loadedweight.mass;
            overalMass = addedmass + springHeadMass;
            t = 0.0f;
            last_x = 0.0f;
        }

        private void SwingIterationControl(ref float t, ref float last_x, ref float x_t)
        {
            var v = (x_t - last_x) / Time.fixedDeltaTime;

            if (Mathf.Abs(x_t) < swingThreshold)
            {
                //Debug.Log("unloadedWeight: " + unloadedWeight);
                //Debug.Log("v: " + v);
                //if (v > 0 && unloadedWeight == null)
                //{
                //    projectile.Project(Vector3.up * v);
                //    unloadedWeight = loadedweight;
                //    loadedweight = null;
                //}
                if (Mathf.Abs(v) < swingThreshold)
                {
                    x_t = 0;
                    isSwinging = false;
                }
            }
            setSpringSize(x_t);
            last_x = x_t;
            t += Time.fixedDeltaTime;
        }
    }
}