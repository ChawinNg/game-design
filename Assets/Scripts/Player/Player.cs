using UnityEngine;

public class Player : MonoBehaviour
{
    public Move moveScript;

    // Vector3 forward;
    // Vector3 right;

    void Start()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera is not set in the scene!");
            return;
        }

        // forward = Camera.main.transform.forward;
        // forward.y = 0;
        // forward = Vector3.Normalize(forward);
        // right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }


    void Update()
    {
        moveScript.ResetMove();
        // Example of controlling movement externally
        if (Input.GetKey(KeyCode.W)) 
        {
            moveScript.MoveInDirection("Up");
        }
        else if (Input.GetKey(KeyCode.S)) 
        {
            moveScript.MoveInDirection("Down");
        }
        else if (Input.GetKey(KeyCode.D)) 
        {
            moveScript.MoveInDirection("Right");
        }
        else if (Input.GetKey(KeyCode.A)) 
        {
            moveScript.MoveInDirection("Left");
        }
    }
}