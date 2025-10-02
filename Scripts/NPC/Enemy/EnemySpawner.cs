using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Enemy _prefab;

    [SerializeField]
    private PathController _pathController = null;
    public PathController PathController => _pathController;

    private Enemy _spawnedGuard;

    private void OnEnable()
    {
        _spawnedGuard = Instantiate(_prefab, transform.position, Quaternion.identity);
        _spawnedGuard.Spawner = this;
    }

    private void OnDisable()
    {
        if(_spawnedGuard != null)
            _spawnedGuard.Despawn();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.25f);
        }

        if (_spawnedGuard == null)
            return;

        string a = "Health : " + _spawnedGuard.Health.GetHealth().ToString();

        if (_spawnedGuard.transform != null)
        {
            a += "\n State : " + _spawnedGuard.StateMachine.CurrState.ToString();

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;
            Handles.Label(_spawnedGuard.GetPosition() + new Vector3(0, 1f, 0), a, style);
        }
    }
}
