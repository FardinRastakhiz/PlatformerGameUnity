using System;
using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Inputs
{
    public class InputController : Utilities.MonoSingleton<InputController>
    {
        void Start()
        {
            StartCoroutine(UpdateInputs());
        }

        private int leftKeyCounter = 0;
        public event EventHandler PressLeft;
        public event EventHandler ReleaseLeft;

        private int rightKeyCounter = 0;
        public event EventHandler PressRight;
        public event EventHandler ReleaseRight;
        public event EventHandler PressSpace;
        public event EventHandler ReleaseSpace;

        private IEnumerator UpdateInputs()
        {
            while (true)
            {
                MultipleKeyPress(PressLeft, ref leftKeyCounter, KeyCode.LeftArrow, KeyCode.A);
                MultipleKeyPress(PressRight, ref rightKeyCounter, KeyCode.RightArrow, KeyCode.D);
                MultipleKeyRelease(ReleaseLeft, ref leftKeyCounter, KeyCode.LeftArrow, KeyCode.A);
                MultipleKeyRelease(ReleaseRight, ref rightKeyCounter, KeyCode.RightArrow, KeyCode.D);

                //if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                //    PressLeft?.Invoke(this, null);
                //if (Input.GetKeyUp(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.A)) // !?
                //    ReleaseLeft?.Invoke(this, null);
                //if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                //    PressRight?.Invoke(this, null);
                //if (Input.GetKeyUp(KeyCode.RightArrow))
                //    ReleaseRight?.Invoke(this, null);
                if (Input.GetKeyDown(KeyCode.Space))
                    PressSpace?.Invoke(this, null);
                if (Input.GetKeyUp(KeyCode.Space))
                    ReleaseSpace?.Invoke(this, null);
                yield return null;
            }
        }

        private void MultipleKeyPress(EventHandler targetEvent, ref int counter, params KeyCode[] keys)
        {
            int newInputs = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                if (Input.GetKeyDown(keys[i]))
                {
                    newInputs++;

                }
            }
            counter += newInputs;
            if (newInputs > 0)
                targetEvent?.Invoke(this, null);
        }

        private void MultipleKeyRelease(EventHandler targetEvent, ref int counter, params KeyCode[] keys)
        {
            int newRelease = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                if (Input.GetKeyUp(keys[i]))
                {
                    newRelease++;

                }
            }
            counter -= newRelease;
            if (newRelease > 0 && counter <= 0)
                targetEvent?.Invoke(this, null);
        }
    }
}
