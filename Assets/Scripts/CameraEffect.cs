using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    public enum Effect {Shake}

    public void ApplyEffect(float intensity, float duration, Effect effect)
    {
        switch (effect)
        {
            case Effect.Shake:
                StartCoroutine(ShakeEffect(intensity, duration));
                break;
            default:
                break;
        }
        
    }
    IEnumerator ShakeEffect(float intensity, float duration)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1, 1) * intensity;
            float y = Random.Range(-1, 1) * intensity;

            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(x, y, originalPos.z), duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = Vector3.zero;
    }
}
