using UnityEngine;

public class Shaker : MonoBehaviour
{        
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;
    private float dampingSpeed = 1.0f;
    private float magnitudeDecrease = 0.9f;
    Vector3 initialPosition;

    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            
            shakeDuration -= Time.deltaTime * dampingSpeed;
            shakeMagnitude *= magnitudeDecrease;
        }
        else if (shakeDuration < 0)
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    public void TriggerShake() {
        initialPosition = transform.localPosition;
        shakeDuration = 1f;
        shakeMagnitude = 1.0f;
    }

}