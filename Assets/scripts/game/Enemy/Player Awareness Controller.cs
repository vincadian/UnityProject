using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAwarenessController : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set;}

    public Vector2 DirectionToPlayer { get; private set;}

    [SerializeField]
    private float _playerAwarenessDistance;

    private Transform _player;

    private void Awake()
    {
        _player = FindAnyObjectByType<PlayerController>().transform;
    }

    //Update est appelé une fois par frame
    void Update()
    {
        Vector2 enemyToPlayerVector = _player.position - transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;

        if (enemyToPlayerVector.magnitude <= _playerAwarenessDistance)
        {
            AwareOfPlayer = true;
        }

        else
        {
            AwareOfPlayer = false;
        }
    }
    
}
