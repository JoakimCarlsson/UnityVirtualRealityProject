using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    public XRController leftTeleportRay;
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;
    private XRRayInteractor _leftRayInteractor;

    private void Start()
    {
        _leftRayInteractor = leftTeleportRay.gameObject.GetComponent<XRRayInteractor>();
    }
    void Update()
    {
        if (leftTeleportRay)
        {
            _leftRayInteractor.allowSelect = CheckIfActivated(leftTeleportRay);
            leftTeleportRay.gameObject.SetActive(CheckIfActivated(leftTeleportRay));
        }
    }
    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}