using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Game.Scripts.UI.Element
{
    public class ProgressUnbox : MonoBehaviour
    {
        Tween sliderTween;

        public void RunProgress(float time)
        {
            var slider = GetComponent<Slider>();
            sliderTween.Stop();
            sliderTween = Tween.Custom(0f, 1f, time, val => slider.value = val, Ease.Linear);
        }

        private void OnDisable()
        {
            sliderTween.Stop();
        }
    }
}