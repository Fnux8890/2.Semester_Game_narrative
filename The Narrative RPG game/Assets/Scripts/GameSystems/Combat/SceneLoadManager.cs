using System;
using GameSystems.CustomEventSystems;
using UnityEditor.SearchService;
using UnityEngine;
using Utilities;

namespace GameSystems.Combat
{
    public enum LeaveDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    public class SceneLoadManager : Singleton<SceneLoadManager>
    {
        private string _lastSceneName;
        private Vector3 _lastPlayerPosition;
        private LeaveDirection _leaveDirection;

        private void Awake()
        {
            SceneLoadHandler.Instance.StoreLastSceneName += SetLastSceneName;
            SceneLoadHandler.Instance.StoreLastPosition += SetLastPosition;
            SceneLoadHandler.Instance.GetLastPosition += GetLastPosition;
            SceneLoadHandler.Instance.GetLastSceneName += GetLastSceneName;
            SceneLoadHandler.Instance.StoreLeaveDirection += SetLeaveDirection;
            SceneLoadHandler.Instance.GetLastLeaveDirection += GetLeaveDirection;
        }

        private void SetLastSceneName(string lastSceneName)
        {
            _lastSceneName = lastSceneName;
        }

        private void SetLastPosition(Vector3 lastPosition)
        {
            _lastPlayerPosition = new Vector3(lastPosition.x, lastPosition.y, 0);
        }

        private Vector3? GetLastPosition()
        {
            return _lastPlayerPosition;
        }

        private string GetLastSceneName()
        {
            return _lastSceneName;
        }

        private void SetLeaveDirection(LeaveDirection direction)
        {
            _leaveDirection = direction;
        }

        private LeaveDirection? GetLeaveDirection()
        {
            return _leaveDirection;
        }
        
    }
}