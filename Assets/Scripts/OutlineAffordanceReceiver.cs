using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.State;

public class OutlineAffordanceReceiver : MonoBehaviour
{
    [SerializeField] private Outline _outline;
    [SerializeField] private XRBaseInteractable _interactable;

    void Start()
    {
        if (_outline == null)
        {
            throw new MissingReferenceException("Outline refrence is not assigned");
        }

        if (_interactable == null)
        {
            throw new MissingReferenceException("XRBaseInteractable refrence is not assigned");
        }

        // Register event handlers
        AddListeners();
    }

    private void AddListeners()
    {
        _interactable.hoverEntered.AddListener(HandleHoverEntered);
        _interactable.hoverExited.AddListener(HandleHoverExited);
        _interactable.selectEntered.AddListener(HandleSelectEntered);
        _interactable.selectExited.AddListener(HandleSelectExited);
    }

    private void RemoveListeners()
    {
        _interactable.hoverEntered.RemoveListener(HandleHoverEntered);
        _interactable.hoverExited.RemoveListener(HandleHoverExited);
    }

    private void HandleHoverEntered(HoverEnterEventArgs arg0)
    {
        EnableOutline(true);
    }

    private void HandleHoverExited(HoverExitEventArgs arg0)
    {
        EnableOutline(false);
    }

    private void HandleSelectEntered(SelectEnterEventArgs arg0)
    {
        // Maybe change outline color here
        EnableOutline(false);
    }

    private void HandleSelectExited(SelectExitEventArgs arg0)
    {
        EnableOutline(false);
    }

    private void EnableOutline(bool value)
    {
        if (_outline != null)
        {
            _outline.enabled = value;
        }
    }

    void OnDestroy()
    {
        RemoveListeners();
    }
}
