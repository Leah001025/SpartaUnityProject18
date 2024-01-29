using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telepoter : MonoBehaviour
{
    public Vector2Int TelepotePosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            collision.gameObject.transform.position = new Vector3(TelepotePosition.x, TelepotePosition.y, 0);
        }
    }
}
