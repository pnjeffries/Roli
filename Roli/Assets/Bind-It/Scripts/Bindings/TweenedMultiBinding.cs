using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// A Multi-Binding subtype which allows for tweening
    /// </summary>
    [AddComponentMenu("Binding/Tweened Multi-Binding")]
    public class TweenedMultiBinding : MultiBinding
    {
        [Tooltip("The interpolation curve")]
        public AnimationCurve Curve = AnimationCurve.Linear(0, 0, 1, 1); // new AnimationCurve(new Keyframe(0,0), new Keyframe(1,1));

        [Tooltip("The duration of the tween, in seconds.")]
        public float Duration = 1.0f;

        public override void UpdateTargetValue()
        {
            if (Target == null) return;
            StartCoroutine(Tween());
        }

        IEnumerator Tween()
        {
            object startValue = Binding.GetFromPath(Target, TargetPath);
            if (startValue == null) yield break;

            Type type = startValue.GetType();
            var boundValues = GetBoundValues();
            var converter = GetConverter();
            object endValue = converter.Convert(boundValues, type, this, CultureInfo.CurrentCulture);

            float time = 0f;

            while (time < Duration)
            {
                time += Time.deltaTime;
                float t = time / Duration;
                if (Curve != null) t = Curve.Evaluate(t);
                Binding.SetByPath(Target, TargetPath, TweenedDataBinding.Lerp(startValue, endValue, t));
                yield return null;
            }

            Binding.SetByPath(Target, TargetPath, endValue);
        }
    }

}