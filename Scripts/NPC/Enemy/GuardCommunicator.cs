using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCommunicator : MonoBehaviour
{
    public static GuardCommunicator Instance;

    private List<Enemy> _guardList = new List<Enemy>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    public void Register(Enemy g)
    {
        _guardList.Add(g);
    }

    public void Unregister(Enemy g)
    {
        _guardList.Remove(g);
    }

    private void OnDetected(Enemy g, GuardMessageType m)
    {
        for (int i = 0; i < _guardList.Count; i++)
        {
            if (_guardList[i] == g)
                continue;

            float sqrDist = Vector3.SqrMagnitude(_guardList[i].GetPosition() - g.GetPosition());
            if ( sqrDist < 9.0f)
            {
                SendMessage(_guardList[i]);
            }
        }
    }

    private void SendMessage(Enemy g)
    {

    }
}

public enum GuardMessageType
{
    None,
    PlayerDetected,
    PlayerEscaped,
    NeedBackup
}