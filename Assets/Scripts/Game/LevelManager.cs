using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Serializable]
    public class WaypointsManager
    {
        [Serializable]
        public class Waypoints
        {
            public Transform waypoint;
            public int index;
            public List<GameObject> targets;
        }
        [SerializeField] private GameObject waypointObject;
        [SerializeField] public List<Waypoints> waypoints;
        public void AddWaypoint()
        {
            if (waypoints.Count <= 0)
            {
                GameObject waypointsParent = new GameObject("Waypoints");
                    waypointsParent.tag = "Waypoints";
            }
            
            GameObject obj = Instantiate(waypointObject, Vector3.zero, Quaternion.identity);

            Waypoints wayPoint = new Waypoints();
            wayPoint.waypoint = obj.transform;
            wayPoint.index = waypoints.Count+1;
            obj.transform.parent = GameObject.FindWithTag("Waypoints").transform;
            obj.AddComponent<WaypointsHandler>().index = wayPoint.index;
            
            waypoints.Add(wayPoint);
            Debug.Log("Waypoint added!");
        }
    }
    
    [SerializeField] private WaypointsManager waypointsManager;
    public WaypointsManager GetWaypointsManager()
    {
        return waypointsManager;
    }
    public static LevelManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }
}

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    SerializedProperty _waypointsManagerProperty;
    private void OnEnable()
    {
        _waypointsManagerProperty = serializedObject.FindProperty("waypointsManager");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_waypointsManagerProperty, true);

        SerializedProperty waypointsArray = _waypointsManagerProperty.FindPropertyRelative("waypoints");
        
        if (GUILayout.Button("Add Waypoint"))
        {
            LevelManager levelManager = (LevelManager)target;
            levelManager.GetWaypointsManager().AddWaypoint();
        }

        serializedObject.ApplyModifiedProperties();
    }
    
}


