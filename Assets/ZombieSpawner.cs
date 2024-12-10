using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;       // 생성할 좀비 프리팹
    public float spawnRadius = 10f;       // 생성 범위 (반지름)
    public float spawnAngleMin = -45f;    // 최소 각도 (카메라 왼쪽)
    public float spawnAngleMax = 45f;     // 최대 각도 (카메라 오른쪽)
    public float spawnHeight = 0f;        // 생성 높이 (지면에서)
    public float spawnInterval = 5f;       // 스폰 간격 (초)
    public float safeRadius = 2f;         // 카메라 주변 안전 영역 반지름

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnZombie();
            timer = 0f;
        }
    }

    public void SpawnZombie()
    {
        float randomAngle = Random.Range(spawnAngleMin, spawnAngleMax);
        Vector3 spawnDirection = Quaternion.Euler(0f, randomAngle, 0f) * GameObject.Find("IMU").transform.forward;

        // 안전 영역 밖에서 생성 위치 찾기 (최대 10번 시도)
        Vector3 spawnPosition;
        int attempts = 0;
        do
        {
            spawnPosition = GameObject.Find("IMU").transform.position + spawnDirection * spawnRadius + Vector3.up * spawnHeight;
            attempts++;
        } while (Vector3.Distance(spawnPosition, GameObject.Find("IMU").transform.position) < safeRadius && attempts < 10);

        // 좀비 생성 (안전 영역 밖에서만)
        if (attempts < 10)
        {
            Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        }
    }
}