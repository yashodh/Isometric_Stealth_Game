using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathController : MonoBehaviour
{
    private Enemy _guard;
    private NavMeshAgent _agent;
    public LoopType _type = LoopType.PingPong;
    public List<PathPoint> _pathPoints = new List<PathPoint>();

    private int _currIndex;
    private int _direction = 1;
    private bool _isRegistered = false;
    private bool _pointReached = false;

    private float _currTime = 0.0f;

    public void Register(Enemy guard)
    {
        _isRegistered = true;
        _guard = guard;
        _agent = guard.Agent;
        GoToNearest();
    }

    public void Unregister()
    {
        _agent.ResetPath();
        _agent = null;
        _guard = null;
        _isRegistered = false;
        _pointReached = false;
    }

    private void GoToNearest()
    {
        _currIndex = GetClosestPointIndex();
        GoToPoint(_currIndex);
    }

    private int GetClosestPointIndex()
    {
        PathPoint near = _pathPoints[0];
        int index = 0;
        for (int i = 1; i < _pathPoints.Count; i++)
        {
            float v1 = Vector3.SqrMagnitude(_guard.GetPosition() - near.transform.position);
            float v2 = Vector3.SqrMagnitude(_guard.GetPosition() - _pathPoints[i].transform.position);

            if (v2 < v1)
            {
                near = _pathPoints[i];
                index = i;
            }
        }
        
        return index;
    }

    private void Update()
    {
        if (_direction == 0 || !_isRegistered)
            return;

        if (_agent.remainingDistance < 0.25f)
        {
            _agent.ResetPath();
            _pointReached = true;
        }

        if (_pointReached)
        {
            _currTime += Time.deltaTime;

            if (_currTime >= _pathPoints[_currIndex].WaitTime)
            {
                _currTime = 0;
                _pointReached = false;

                SetNextMove();

                _currIndex += _direction;
                GoToPoint(_currIndex);
            }
        }
    }

    private void SetNextMove()
    {
        switch (_type)
        {
            case LoopType.PingPong:
                {
                    if (_currIndex == 0)
                        _direction = 1;
                    else if (_currIndex == _pathPoints.Count - 1)
                        _direction = -1;
                }
                break;
            case LoopType.Loop:
                {
                    _direction = 1;
                    if (_currIndex == _pathPoints.Count - 1)
                        _currIndex = -1;
                }
                break;
            case LoopType.Once:
                {
                    if (_currIndex == _pathPoints.Count - 1)
                        _direction = 0;
                }
                break;
        }
    }

    private void GoToPoint(int index)
    {
        if (index < _pathPoints.Count && index >= 0)
            _agent.SetDestination(_pathPoints[index].transform.position);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _pathPoints.Count - 1; i++)
        {
            if (_pathPoints[i] == null)
                continue;

            Debug.DrawLine(_pathPoints[i].gameObject.transform.position, _pathPoints[i + 1].gameObject.transform.position, Color.cyan);
        }

        if (_type == LoopType.Loop)
            Debug.DrawLine(_pathPoints[_pathPoints.Count - 1].gameObject.transform.position, _pathPoints[0].gameObject.transform.position, Color.cyan);
    }
}

public enum LoopType
{
    PingPong,
    Once,
    Loop,
}
