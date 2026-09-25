using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Path
{
    /// <summary>
    /// Defines a route composed of grid waypoints for enemies to follow.
    /// Can automatically populate waypoints from child transforms or a manual inspector list.
    /// </summary>
    [DisallowMultipleComponent]
    public class PathRoute : MonoBehaviour
    {
        [Header("Waypoint Settings")]
        [Tooltip("Manual list of waypoints. If empty, child transforms will be used automatically.")]
        [SerializeField] private List<Transform> waypoints = new List<Transform>();

        [Header("Editor Visualization (Gizmos)")]
        [SerializeField] private Color pathColor = Color.cyan;
        [SerializeField] private Color nodeColor = Color.yellow;
        [SerializeField] private float nodeRadius = 0.2f;

        private void Awake()
        {
            InitializeWaypoints();
        }

        private void OnValidate()
        {
            if (waypoints == null || waypoints.Count == 0)
            {
                CollectChildWaypoints();
            }
        }

        /// <summary>
        /// Ensures waypoints list is populated.
        /// </summary>
        public void InitializeWaypoints()
        {
            if (waypoints == null || waypoints.Count == 0)
            {
                CollectChildWaypoints();
            }
        }

        private void CollectChildWaypoints()
        {
            if (waypoints == null)
            {
                waypoints = new List<Transform>();
            }
            else
            {
                waypoints.Clear();
            }

            foreach (Transform child in transform)
            {
                waypoints.Add(child);
            }
        }

        /// <summary>
        /// Total number of waypoints in this route.
        /// </summary>
        public int PointCount => waypoints != null ? waypoints.Count : 0;

        /// <summary>
        /// Returns the position of the first waypoint.
        /// </summary>
        public Vector3 GetStartPoint()
        {
            if (PointCount == 0)
            {
                return transform.position;
            }
            return waypoints[0].position;
        }

        /// <summary>
        /// Returns the position of a specific waypoint by index.
        /// </summary>
        public Vector3 GetPoint(int index)
        {
            if (waypoints == null || index < 0 || index >= waypoints.Count)
            {
                return transform.position;
            }

            return waypoints[index].position;
        }

        /// <summary>
        /// Snaps all waypoints to integer coordinates (1x1 Grid).
        /// </summary>
        [ContextMenu("Snap Waypoints to Grid (1x1)")]
        public void SnapWaypointsToGrid()
        {
            CollectChildWaypoints();
            foreach (var point in waypoints)
            {
                if (point != null)
                {
                    Vector3 pos = point.position;
                    pos.x = Mathf.Round(pos.x);
                    pos.y = Mathf.Round(pos.y);
                    pos.z = 0f;
                    point.position = pos;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (waypoints == null || waypoints.Count == 0) return;

            for (int i = 0; i < waypoints.Count; i++)
            {
                if (waypoints[i] == null) continue;

                Gizmos.color = (i == 0) ? Color.green : (i == waypoints.Count - 1) ? Color.red : nodeColor;
                Gizmos.DrawSphere(waypoints[i].position, nodeRadius);

                if (i < waypoints.Count - 1 && waypoints[i + 1] != null)
                {
                    Gizmos.color = pathColor;
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
            }
        }
    }
}
