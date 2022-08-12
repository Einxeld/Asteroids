using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBorderTeleporter : MonoBehaviour
{
    public List<Transform> teleportableObjects = new List<Transform>();

    void FixedUpdate()
    {
        foreach (Transform tObj in teleportableObjects)
        {
            if (Mathf.Abs(tObj.position.x) > GameManager.instance.mapSize.x) tObj.SetXPosition(-tObj.position.x);
            if (Mathf.Abs(tObj.position.y) > GameManager.instance.mapSize.y) tObj.SetYPosition(-tObj.position.y);
        }
    }
}
