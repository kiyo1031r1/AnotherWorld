using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Collider))]
public class CollisionDetecter : MonoBehaviour
{
    [SerializeField] private OnTriggerEvent onTriggerStay = new OnTriggerEvent();
    [SerializeField] private OnTriggerEvent onTriggerEnter = new OnTriggerEvent();
    [SerializeField] private OnTriggerEvent onTriggerExit = new OnTriggerEvent();

    private void OnTriggerStay(Collider other)
    {
        onTriggerStay.Invoke(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke(other);
    }

    [Serializable]
    public class OnTriggerEvent : UnityEvent<Collider>
    {

    }
}
