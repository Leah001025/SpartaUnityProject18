using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private GameObject monster;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.5f)
        {
            timer = 0;
            Spawn();
        }
    }

    private void Spawn()
    {
        monster.transform.position = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)].position;
    }
}
