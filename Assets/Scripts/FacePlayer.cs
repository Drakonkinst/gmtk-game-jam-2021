using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private void Update()
    {
        Vector2 playerPos = PlayerStatus.instance.transform.position;
        Vector2 vectorToTarget = playerPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;
        transform.Rotate(new Vector3(0.0f, 0.0f, 30.0f));
    }
}
