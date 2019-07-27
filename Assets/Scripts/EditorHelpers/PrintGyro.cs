using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintGyro : MonoBehaviour
{
    [Header("Debug View")]
    public bool Forward;
    public bool Right;
    public bool Up;
    public bool Gravity;

    private bool active;

    private void Start()
    {
        Input.gyro.enabled = true;
    }

    private void Update()
    {
        if (active)
            Debug.Log(Input.gyro.attitude.eulerAngles);
    }
    
    [Button, HideInEditorMode]
    public void Toggle()
    {
        active = !active;
    }

    private void OnDrawGizmos()
    {
        if (active)
        {
            if (Forward)
            {
                Debug.DrawLine(transform.position, transform.position + (Input.gyro.attitude * Vector3.forward), Color.blue);
            }
            if (Right)
            {
                Debug.DrawLine(transform.position, transform.position + (Input.gyro.attitude * Vector3.right), Color.red);
            }
            if (Up)
            {
                Debug.DrawLine(transform.position, transform.position + (Input.gyro.attitude * Vector3.up), Color.green);
            }
            if (Gravity)
            {
                Debug.DrawLine(transform.position, transform.position + (Input.gyro.gravity), Color.magenta);
            }
        }
    }
}
