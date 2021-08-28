using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController _instance;

    // How long the object should shake for.
    public float shakeDuration = .1f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.3f;
    public float decreaseFactor = 1.0f;

    private float shakeTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance) Destroy(_instance.gameObject);
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            transform.localPosition = Random.insideUnitSphere * shakeAmount;

            shakeTimer -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeTimer = 0f;
            transform.localPosition = Vector3.zero;
        }
    }

    public static void Shake()
    {
        _instance.shakeTimer = _instance.shakeDuration;
    }   
}
