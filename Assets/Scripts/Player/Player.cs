using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    Vector3 forward;
    Vector3 right;

    void Start()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera is not set in the scene!");
            return;
        }

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    void Update()
    {
        if (Input.anyKey)
        {
            Move();
        }
    }

    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 movement = (right * direction.x + forward * direction.z) * speed * Time.deltaTime;
        
        if (movement != Vector3.zero)
            transform.forward = movement.normalized;
        
        transform.position += movement;
    }
}
