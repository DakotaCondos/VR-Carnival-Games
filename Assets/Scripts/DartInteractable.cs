using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DartInteractable : XRGrabInteractable
{
    [SerializeField] private TriggerNotifier _triggerNotifier;
    private bool _isGrabbed = false;
    private Rigidbody _grabbedRigidbody;


    protected override void Awake()
    {
        base.Awake();
        _grabbedRigidbody = GetComponent<Rigidbody>();
        if (_grabbedRigidbody == null)
        {
            Debug.LogWarning("Rigidbody not found");
        }
    }

    private void Update()
    {
        if (_isGrabbed) { return; }
        if (!_grabbedRigidbody.useGravity && _grabbedRigidbody.velocity.magnitude > Mathf.Epsilon)
        {
            _grabbedRigidbody.useGravity = true;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_triggerNotifier != null)
        {
            _triggerNotifier.OnTriggerEvent += HandleTriggerEvent;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_triggerNotifier != null)
        {
            _triggerNotifier.OnTriggerEvent -= HandleTriggerEvent;
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _isGrabbed = true;
        _grabbedRigidbody.useGravity = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        _isGrabbed = false;
        _grabbedRigidbody.useGravity = true;
    }
    private void HandleTriggerEvent(GameObject gameObject)
    {
        if (_isGrabbed) { return; }
        Debug.Log(gameObject.name);
        _grabbedRigidbody.velocity = Vector3.zero;
        _grabbedRigidbody.angularVelocity = Vector3.zero;
        _grabbedRigidbody.useGravity = false;
    }
}
