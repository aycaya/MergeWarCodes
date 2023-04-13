using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] InputAction firstTouch;
    [SerializeField] InputAction touchPosition;
    public bool isPressed = false;
    public bool isReleased = false;
    public Vector2 touchXYPos;


    void Update()
    {
        isPressed = firstTouch.WasPressedThisFrame();
        isReleased = firstTouch.WasReleasedThisFrame();
        touchXYPos = touchPosition.ReadValue<Vector2>();

    }

    private void OnEnable()
    {
        firstTouch.Enable();
        touchPosition.Enable();
    }


}
