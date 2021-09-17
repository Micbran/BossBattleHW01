using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScreenOnDamage : MonoBehaviour
{
    private Camera shakerCamera;
    [SerializeField] private Health stats;

    [SerializeField] private float shakeTimerMax = 0.2f;
    private float shakeTimer = 0f;
    private bool isShaking = false;
    private Vector3 startPosition;


    private void Awake()
    {
        this.startPosition = this.transform.position;
        this.shakerCamera = this.GetComponent<Camera>();
    }

    private void OnEnable()
    {
        stats.OnTakeDamage += this.ShakeScreen;
    }

    private void OnDisable()
    {
        stats.OnTakeDamage -= this.ShakeScreen;
    }

    private void ShakeScreen(int _)
    {
        this.isShaking = true;
        this.shakeTimer = this.shakeTimerMax;
    }

    private void LateUpdate()
    {
        if (isShaking && shakeTimer <= 0)
        {
            this.isShaking = false;
            this.transform.position = this.startPosition;
        }
        else if (isShaking)
        {
            this.shakeTimer -= Time.deltaTime;
            this.transform.position = new Vector3(Random.Range(-1, 1), this.startPosition.y, Random.Range(-1, 1));
        }
    }
}
