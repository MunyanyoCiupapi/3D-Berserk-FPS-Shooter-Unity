using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public float viewRange = 10;
    public float speed = 10;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private Transform target;
    private GameObject go;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        go = GameObject.FindGameObjectWithTag("Player");
        target = go.transform;
        agent.speed = speed;
    }
    private void Update()
    {
        var dist = Vector3.Distance(transform.position, target.position);
        if (!(dist < viewRange)) return;

        GetPlayerPosition(out Vector3 playerPosition);
        var dir = (playerPosition - transform.position).normalized;

        var ray = new Ray(transform.position, dir);
        if (Physics.Raycast(ray, out var hit) && hit.collider is { } && hit.collider.CompareTag("Player")) //is ir sablonu atitikimas "{}" 0.5 + 1t
        {
            agent.destination = playerPosition;
        }
    }

    private void GetPlayerPosition(out Vector3 position)
    {
        position = target.position;
    }
}
