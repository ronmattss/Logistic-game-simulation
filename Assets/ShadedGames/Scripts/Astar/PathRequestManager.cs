using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAssets.Scripts.Util;

namespace ShadedGames.Scripts.Astar
{
    public class PathRequestManager : Singleton<PathRequestManager>
    {
        Queue<PathResult> results = new Queue<PathResult>();
        PathFinder pathFinder;

        void Awake()
        {
            pathFinder = GetComponent<PathFinder>();
        }
        void Update()
        {
            if (results.Count > 0)
            {
                int itemsInQuueue = results.Count;
                lock (results)
                {
                    for (int i = 0; i < itemsInQuueue; i++)
                    {
                        PathResult result = results.Dequeue();
                        result.callBack(result.path, result.success);
                    }
                }
            }
        }

        public void RequestPath(PathRequest request) // this should be static
        {
            ThreadStart threadStart = delegate
            {
                pathFinder.FindPath(request, FinishedProcessingPath);
            };
            threadStart.Invoke();
        }
        public void FinishedProcessingPath(PathResult result)
        {
            lock (results)
            {
                results.Enqueue(result);
            }
        }
    }
    public struct PathResult
    {
        public Vector3[] path;
        public bool success;
        public Action<Vector3[], bool> callBack;

        /// TODO: CALLBACK PATHRESULT AND WAYPOINTS AND WHAT WILL BE RETURND
        // TODO: CURRENT 0305
        public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callBack)
        {
            this.path = path;
            this.success = success;
            this.callBack = callBack;
        }
    }
    public struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callBack;
        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callBack)
        {
            pathStart = _start;
            pathEnd = _end;
            callBack = _callBack;
        }
    }
}