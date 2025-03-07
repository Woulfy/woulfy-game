using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public InputActionReference moveAction;
    private void LateUpdate()
    {
        var moveValue = moveAction.action.ReadValue<Vector2>();
        Debug.Log(moveValue);
        transform.position += new Vector3(moveValue.x, moveValue.y, 0);
    }
}
