using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// A Data Binding Sub-type that employs tweening
    /// </summary>
    [AddComponentMenu("Binding/Tweened Data Binding")]
    public class TweenedDataBinding : DataBinding
    {
        [Tooltip("The interpolation curve")]
        public AnimationCurve Curve = AnimationCurve.Linear(0,0,1,1); // new AnimationCurve(new Keyframe(0,0), new Keyframe(1,1));

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
            Type type = startValue.GetType();
            object endValue = GetBoundValue(type, GetConverter());

            if (startValue != null)
            {
                float time = 0f;

                while (time < Duration)
                {
                    time += Time.deltaTime;
                    float t = time / Duration;
                    if (Curve != null) t = Curve.Evaluate(t);
                    Binding.SetByPath(Target, TargetPath, Lerp(startValue, endValue, t));
                    yield return null;
                }
            }

            Binding.SetByPath(Target, TargetPath, endValue);
        }

        public static object Lerp(object startValue, object endValue, float t)
        {
            if (startValue is float && endValue is float)
            {
                return Mathf.Lerp((float)startValue, (float)endValue, t);
            }
            else if (startValue is Vector3 && endValue is Vector3)
            {
                return Vector3.Lerp((Vector3)startValue, (Vector3)endValue, t);
            }
            else if (startValue is Vector2 && endValue is Vector2)
            {
                return Vector2.Lerp((Vector2)startValue, (Vector2)endValue, t);
            }
            else if (startValue is Color && endValue is Color)
            {
                return Color.Lerp((Color)startValue, (Color)endValue, t);
            }
            else if (startValue is double d0 && endValue is double d1)
            {
                return d0 + (d1 - d0) * t;
            }
            else if (startValue is int i0 && endValue is int i1)
            {
                return (int)(i0 + (i1 - i0) * t);
            }
            else if (startValue is bool b0 && endValue is bool b1)
            {
                return b0;
            }
            else
            {
                if (Application.isEditor) throw new NotSupportedException(string.Format("Cannot lerp between types '{0}' and '{1}'", startValue?.GetType().Name, endValue?.GetType().Name));
                return startValue;
            }
        }
    }
}