using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightTransition : MonoBehaviour
{
    public DayNightBackground Background;
    public float Duration = 1;
    public AnimationCurve Ease;

    private float targetValue = 0;
    private float currentValue = 0;

    [Button, HideInEditorMode]
    public void TransitionToDay()
    {
        targetValue = 0;
    }

    [Button, HideInEditorMode]
    public void TransitionToNight()
    {
        targetValue = 1;
    }

    private void OnEnable()
    {
        StartCoroutine(LERPToTargetValueCoroutine());
    }

    private IEnumerator LERPToTargetValueCoroutine()
    {
        while (true)
        {
            yield return null;
            float speed = 1 / Duration;
            currentValue = Mathf.MoveTowards(currentValue, targetValue, speed * Time.deltaTime);
            Background.Ratio = Ease.Evaluate(currentValue);
        }
    }
}
