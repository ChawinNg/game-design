using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    private List<GameObject> collidingObjects = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        collidingObjects.Add(other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        collidingObjects.Remove(other.gameObject);
    }

    public GameObject[] GetCollidingObjects()
    {
        return collidingObjects.ToArray();
    }
}
