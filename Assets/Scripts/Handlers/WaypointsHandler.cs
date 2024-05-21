using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEditor;
using UnityEngine;

public class WaypointsHandler : MonoBehaviour
{
    public int index;
    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(new Vector3(transform.position.x - 2, transform.position.y, transform.position.z), 2.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == this.gameObject)
                continue;
            
            if (collider.gameObject.CompareTag("Enemy"))
            {
                collider.gameObject.GetComponent<EnemyController>().waypointIndex = index;
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(transform.position.x - 2, transform.position.y, transform.position.z), 2.5f);
    }
}
