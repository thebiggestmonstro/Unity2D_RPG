using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    InputAction moveAction;

    [SerializeField]
    InputAction jumpAction;

    bool bIsSpacePressed = false;

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Enable();
    }

    private void Update()
    {
        UpdateMoving();    
    }

    void UpdateMoving()
    {
        bIsSpacePressed = jumpAction.IsPressed();
        if (bIsSpacePressed)
            gameObject.transform.position += new Vector3(0, 1, 0) * 10 * Time.deltaTime;

        float VerticalValue = moveAction.ReadValue<Vector2>().x;
        gameObject.transform.position += new Vector3(VerticalValue, 0, 0) * 10 * Time.deltaTime;
    }
}
