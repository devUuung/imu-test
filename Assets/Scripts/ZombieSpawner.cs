using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Meta.XR.MultiplayerBlocks.Shared;

namespace Meta.XR.MultiplayerBlocks.Fusion
{
    public class ZombieSpawner : MonoBehaviour
    {
        public GameObject zombiePrefab;       // ������ ���� ������
        public float spawnRadius = 10f;       // ���� ���� (������)
        public float spawnAngleMin = -45f;    // �ּ� ���� (ī�޶� ����)
        public float spawnAngleMax = 45f;     // �ִ� ���� (ī�޶� ������)
        public float spawnHeight = 0f;        // ���� ���� (���鿡��)
        public float spawnInterval = 5f;       // ���� ���� (��)
        public float safeRadius = 2f;         // ī�޶� �ֺ� ���� ���� ������

        private float timer = 0f;
        public NetworkRunner _networkRunner;
        private void OnEnable()
        {
            FusionBBEvents.OnSceneLoadDone += OnLoaded;
        }

        private void OnDisable()
        {
            FusionBBEvents.OnSceneLoadDone -= OnLoaded;
        }

        private void OnLoaded(NetworkRunner networkRunner)
        {
            _networkRunner = networkRunner;
        }

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

            // ���� ���� �ۿ��� ���� ��ġ ã�� (�ִ� 10�� �õ�)
            Vector3 spawnPosition;
            int attempts = 0;
            do
            {
                spawnPosition = GameObject.Find("IMU").transform.position + spawnDirection * spawnRadius + Vector3.up * spawnHeight;
                attempts++;
            } while (Vector3.Distance(spawnPosition, GameObject.Find("IMU").transform.position) < safeRadius && attempts < 10);

            // ���� ���� (���� ���� �ۿ�����)
            if (attempts < 10)
            {
                // Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
                _networkRunner.Spawn(zombiePrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}