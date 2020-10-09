using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class LocomotionController : MonoBehaviour
{
    public bool teleport = true;
    [Header("Teleport Movement")]
    public XRController leftTeleportRay;
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;

    [Header("Continuous Movement")]
    public XRNode inputSource;
    public float speed = 1f;
    public float gravity = -9.81f;
    public LayerMask groundLayer;

    private float _fallingSpeed;
    private XRRig _xrRig;
    private Vector2 _inputAxis;
    private XRRayInteractor _leftRayInteractor;
    private CharacterController _characterController;

    private void Start()
    {
        _leftRayInteractor = leftTeleportRay.gameObject.GetComponent<XRRayInteractor>();
        _characterController = GetComponent<CharacterController>();
        _xrRig = GetComponent<XRRig>();
    }
    void Update()
    {
        if (teleport)
            TeleportMovement();
        else
            ContinuousMovement();
    }

    private void ContinuousMovement()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out _inputAxis);

    }

    private void TeleportMovement()
    {
        if (!leftTeleportRay) return;
        _leftRayInteractor.allowSelect = CheckIfActivated(leftTeleportRay);
        leftTeleportRay.gameObject.SetActive(CheckIfActivated(leftTeleportRay));
    }

    private void FixedUpdate()
    {
        if (!teleport)
        {
            MoveCharacter();
        }
    }

    private void MoveCharacter()
    {
        CapsuleFollow();
        Quaternion headYaw = Quaternion.Euler(0, _xrRig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(_inputAxis.x, 0, _inputAxis.y);
        _characterController.Move(direction * (speed * Time.deltaTime));


        bool isGrounded = IsGrounded();
        if (isGrounded)
            _fallingSpeed = 0;
        else
            _fallingSpeed += gravity * Time.fixedDeltaTime;

        _characterController.Move(Vector3.up * (_fallingSpeed * Time.fixedDeltaTime));
    }

    public bool CheckIfActivated(XRController controller)
    {
        controller.inputDevice.IsPressed(teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }

    private bool IsGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(_characterController.center);
        float rayLength = _characterController.center.y; //+ 0.01f
        bool hasHit = Physics.SphereCast(rayStart, _characterController.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);


        return hasHit;
    }

    private void CapsuleFollow()
    {
        _characterController.height = _xrRig.cameraInRigSpaceHeight + 0.2f; //addinital height
        Vector3 capsuleCenter = transform.InverseTransformPoint(_xrRig.cameraGameObject.transform.position);
        _characterController.center = new Vector3(capsuleCenter.x, _characterController.height / 2 + _characterController.skinWidth, capsuleCenter.z);
    }
}