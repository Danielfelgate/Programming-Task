using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls camera effects within the scene such as shaking.
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>Singleton instance.</summary>
    private static CameraController _instance;

    /// <summary>How long the object should shake for.</summary>
    [Header("Properties")]
    [SerializeField] private float shakeDuration = .1f;
    /// <summary>Amplitude of the shake.</summary>
    [SerializeField] private float shakeAmount = 0.3f;
    /// <summary>The speed at which the shakeTimer decreases.</summary>
    [SerializeField] private float decreaseFactor = 1.0f;


    /// <summary>Internal timer to track shake duration.</summary>
    private float shakeTimer = 0f;

    void Start()
    {
        // Remove existing instance to ensure singleton pattern
        if (_instance) Destroy(_instance.gameObject);
        _instance = this;
    }

    void Update()
    {
        // Create a shaking effect by randomly offsetting the camera while the shakeTimer is counting down
        if (shakeTimer > 0)
        {
            transform.localPosition = Random.insideUnitSphere * shakeAmount;
            shakeTimer -= Time.deltaTime * decreaseFactor;
        }
        // Reset the camera position
        else
        {
            shakeTimer = 0f;
            transform.localPosition = Vector3.zero;
        }
    }

    /// <summary>
    /// Static method to begin the shaking effect on the singleton instance.
    /// </summary>
    public static void Shake()
    {
        _instance.shakeTimer = _instance.shakeDuration;
    }
}
