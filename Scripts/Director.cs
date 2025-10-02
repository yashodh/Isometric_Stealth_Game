using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    Player _player;
    List<Enemy> _enemyList = new List<Enemy>();

    public static Director Instance;

    public void Init()
    {
        Instance = this;
        _player = Player.Instance;
    }

    public void RegisterEnemy(Enemy e)
    {
        if (!_enemyList.Contains(e))
            _enemyList.Add(e);
    }

    public void UnregisterEnemy(Enemy e)
    {
        if (_enemyList.Contains(e))
            _enemyList.Remove(e);
    }

    private List<Enemy> FindEnemiesWithinRadius(Vector3 center, float radius)
    {
        List<Enemy> inRange = new List<Enemy>();

        for (int i = 0; i < _enemyList.Count; i++)
        {
            float distSqr = Vector3.SqrMagnitude(center - _enemyList[i].GetPosition());

            if (distSqr <= radius * radius)
            {
                inRange.Add(_enemyList[i]);
            }
        }

        return inRange;
    }

    public void ProcessMessage(Message m, Vector3 location)
    {
        List<Enemy> e = new List<Enemy>();
        float radius = 0f;

        switch (m)
        {
            case Message.Distraction:
                {
                    radius = 3.5f;
                }
                break;
            case Message.Explosion:
                {
                    radius = 10.0f;
                }
                break;
            case Message.PlayerDetected:
                {
                    radius = 5.0f;
                }
                break;
            case Message.NormalEnemyDeath:
                {
                    radius = 2.0f;
                }
                break;
            case Message.Whistle:
                { 
                    radius = 4.0f;
                }
                break;
        }

        e = FindEnemiesWithinRadius(location, radius);

        for (int i = 0; i < e.Count; i++)
        {
            if (e[i].IsCurrentState(EnemyState.DEAD) ||
                e[i].IsCurrentState(EnemyState.ATTACK))
                continue;

            Debug.DrawLine(location, e[i].GetPosition(), Color.magenta, radius);
            e[i].InvestigateLocation(location);
        }
    }
}

public enum Message
{
    Distraction,
    Explosion,
    NormalEnemyDeath,
    PlayerDetected,
    Whistle,
}
