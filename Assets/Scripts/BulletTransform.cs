using System;
using Player;
using UnityEngine;

public class BulletTransform : MonoBehaviour
{
    public Vector3 direction;

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, direction) <= 0.1f)
        {
            transform.position = FindObjectOfType<PlayerController>().transform.position;
            BulletManager.Instance.ResetBullet(this.gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, direction, 5 * Time.deltaTime);
    }
}