using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using RangeAttribute = UnityEngine.RangeAttribute;

[RequireComponent(typeof(CharacterController))]
public class OverworldPlayerMovement : MonoBehaviour
{
    private InputEventManager IEM;

    [Header("Values")]
    [SerializeField] private float verticalInputRaw;  
    [SerializeField] private float horizontalInputRaw;
    private CharacterController characterController;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;

    private void OnEnable()
    {
        IEM = InputEventManager.Instance;
        IEM.onVerticalInputChanged.AddListener(ReadVerticalInput);
        IEM.onHorizontalInputChanged.AddListener(ReadHorizontalInput);
        characterController = GetComponent<CharacterController>();
    }

    private void OnDisable() // i stg if i forget to remove listeners properly again in this project im gonna explode
    {
        IEM.onVerticalInputChanged.RemoveListener(ReadVerticalInput);
        IEM.onHorizontalInputChanged.RemoveListener(ReadHorizontalInput);
    }

    void ReadVerticalInput(float value) { verticalInputRaw = value; }
    void ReadHorizontalInput(float value) { horizontalInputRaw = value; }

    private void Movement()
    {
        if (characterController != null)
        {
            Vector3 moveDir = new Vector3(horizontalInputRaw, 0f, verticalInputRaw);
            transform.rotation = Quaternion.LookRotation(-moveDir);
            characterController.Move(moveDir * moveSpeed);
        }
    }

    private void Update()
    {
        Movement();
    }
}
