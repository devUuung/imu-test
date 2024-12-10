using UnityEngine;
using System;
using System.Text;
using UnityEngine.Android;

public class BLEManager : MonoBehaviour
{
    [SerializeField] OVRSkeleton rightHandSkeleton; // 오른손 관절 정보
    [SerializeField] GameObject bullet;
    [SerializeField] float waitingTime = 3f; // 사운드 발사 대기 시간

    private string deviceName = "Nano34_BLE";
    private string serviceUUID = "180C"; // Arduino의 사용자 정의 서비스 UUID
    private string characteristicUUID = "2A56"; // 트리거 특성 UUID
    public Transform targetObject;

    private string connectedDeviceAddress = null;
    private bool isConnected = false;
    private float currentRoll = 0;
    private float currentPitch = 0;
    private float timer = 0;
    private int trigger = 0;

    void Start()
    {
        RequestPermissions();
        BluetoothLEHardwareInterface.Initialize(true, false, () => {
            Debug.Log("Bluetooth initialized successfully.");
            StartScan();
        }, (error) => {
            Debug.LogError("Error initializing Bluetooth: " + error);
        });
        if (rightHandSkeleton == null)
        {
            rightHandSkeleton = GameObject.FindObjectOfType<OVRSkeleton>();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (rightHandSkeleton != null && rightHandSkeleton.IsDataValid && timer > waitingTime)
        {
            // 데시벨이 임계값을 넘으면 발사
            if (trigger == 1)
            {
                Debug.Log($"Shoot");
                Shoot(); // 총 발사 함수 호출
                timer = 0; // 타이머 리셋
            }
        }
    }

    private void StartScan()
    {
        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) => {
            Debug.Log("Found device: " + name);
            if (name.Contains(deviceName))
            {
                BluetoothLEHardwareInterface.StopScan();
                ConnectToDevice(address);
            }
        });
    }

    private void ConnectToDevice(string address)
    {
        Debug.Log("Connected");
        BluetoothLEHardwareInterface.ConnectToPeripheral(address, null, null, (address, service, characteristic) => {
            connectedDeviceAddress = address;
            isConnected = true;
            Debug.Log("Connected to device: " + address);
            ReadCharacteristic(address);
        }, (address) => {
            // Handle disconnection
            isConnected = false;
            connectedDeviceAddress = null;
            Debug.LogWarning("Disconnected from device: " + address);
            Reconnect();
        });
    }

    private void ReadCharacteristic(string address)
    {
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(address, serviceUUID, characteristicUUID, null, (deviceAddress, characteristic, data) => {
            string receivedString = Encoding.UTF8.GetString(data);
            Debug.Log("Received string: " + receivedString);

            string[] values = receivedString.Split(',');
            if (values.Length == 3)
            {
                if (float.TryParse(values[0], out float roll) &&
                    float.TryParse(values[1], out float pitch) &&
                    float.TryParse(values[2], out float trigger))
                {
                    currentRoll = Mathf.LerpAngle(currentRoll, roll, 0.1f);
                    currentPitch = Mathf.LerpAngle(currentPitch, pitch, 0.1f);

                    Quaternion targetRotation = Quaternion.Euler(180, roll + 90, currentPitch - 100);
                    if (targetObject != null)
                    {
                        targetObject.rotation = targetRotation;
                    }
                }
                else
                {
                    Debug.LogError("Failed to parse data.");
                }
            }
            else
            {
                Debug.LogError("Received data does not contain 3 values.");
            }
        });
    }
    void Shoot()
    {
        // 손의 중심 위치 계산
        Vector3 handPosition = CalculateHandCenter();
        Vector3 gunDirection = targetObject.transform.right;
        // 총구 방향 계산
        
        // 총알이 발사될 위치 계산 (손의 앞쪽)
        float distanceFromHand = 0.1f;
        Vector3 bulletSpawnPosition = handPosition + gunDirection * distanceFromHand;

        // 총알 생성
        GameObject newBullet = Instantiate(bullet, bulletSpawnPosition, Quaternion.LookRotation(gunDirection));

        // 총알 속도 설정
        Vector3 bulletVelocity = gunDirection * 20f; // 속도는 원하는 값으로 조정
        newBullet.GetComponent<Bullet>().SetVelocity(bulletVelocity);
    }

    Vector3 CalculateHandCenter()
    {
        Vector3 handCenter = Vector3.zero;
        int boneCount = 0;

        foreach (var bone in rightHandSkeleton.Bones)
        {
            // 관절이 보이는 경우에만 계산에 포함
            if (bone.Transform != null && bone.Transform.gameObject.activeSelf && IsBoneVisible(bone))
            {
                handCenter += bone.Transform.position;
                boneCount++;
            }
        }

        if (boneCount > 0)
        {
            return handCenter / boneCount; // 유효한 관절들의 평균 위치 반환
        }
        else
        {
            Debug.LogWarning("손의 관절 데이터를 찾을 수 없습니다.");
            return Vector3.zero; // 유효한 관절이 없을 경우 0 반환
        }
    }

    // 관절이 보이는지 여부를 판단하는 함수 (가시성 여부를 체크)
    bool IsBoneVisible(OVRBone bone)
    {
        return bone.Transform.gameObject.activeInHierarchy;
    }
    private void Reconnect()
    {
        Debug.Log("Attempting to reconnect...");
        if (!isConnected)
        {
            StartScan();
        }
    }

    void OnDestroy()
    {
        BluetoothLEHardwareInterface.DeInitialize(() => {
            Debug.Log("Bluetooth deinitialized.");
        });
    }

    void RequestPermissions()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Debug.Log("FineLocation permission not granted.");
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        if (!Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_SCAN"))
        {
            Debug.Log("BLUETOOTH_SCAN permission not granted.");
            Permission.RequestUserPermission("android.permission.BLUETOOTH_SCAN");
        }
        if (!Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_CONNECT"))
        {
            Debug.Log("BLUETOOTH_CONNECT permission not granted.");
            Permission.RequestUserPermission("android.permission.BLUETOOTH_CONNECT");
        }
    }
}