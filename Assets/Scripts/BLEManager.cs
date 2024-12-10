using UnityEngine;
using System;
using System.Text;
using UnityEngine.Android;

public class BLEManager : MonoBehaviour
{
    [SerializeField] OVRSkeleton rightHandSkeleton; // ������ ���� ����
    [SerializeField] GameObject bullet;
    [SerializeField] float waitingTime = 3f; // ���� �߻� ��� �ð�

    private string deviceName = "Nano34_BLE";
    private string serviceUUID = "180C"; // Arduino�� ����� ���� ���� UUID
    private string characteristicUUID = "2A56"; // Ʈ���� Ư�� UUID
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
            // ���ú��� �Ӱ谪�� ������ �߻�
            if (trigger == 1)
            {
                Debug.Log($"Shoot");
                Shoot(); // �� �߻� �Լ� ȣ��
                timer = 0; // Ÿ�̸� ����
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
            if (receivedString == "Shoot")
            {
                Shoot();
            }
        });
    }
    void Shoot()
    {
        // ���� �߽� ��ġ ���
        Vector3 handPosition = CalculateHandCenter();
        Vector3 gunDirection = targetObject.transform.right;
        // �ѱ� ���� ���
        
        // �Ѿ��� �߻�� ��ġ ��� (���� ����)
        float distanceFromHand = 0.1f;
        Vector3 bulletSpawnPosition = handPosition + gunDirection * distanceFromHand;

        // �Ѿ� ����
        GameObject newBullet = Instantiate(bullet, bulletSpawnPosition, Quaternion.LookRotation(gunDirection));

        // �Ѿ� �ӵ� ����
        Vector3 bulletVelocity = gunDirection * 20f; // �ӵ��� ���ϴ� ������ ����
        newBullet.GetComponent<Bullet>().SetVelocity(bulletVelocity);
    }

    Vector3 CalculateHandCenter()
    {
        Vector3 handCenter = Vector3.zero;
        int boneCount = 0;

        foreach (var bone in rightHandSkeleton.Bones)
        {
            // ������ ���̴� ��쿡�� ��꿡 ����
            if (bone.Transform != null && bone.Transform.gameObject.activeSelf && IsBoneVisible(bone))
            {
                handCenter += bone.Transform.position;
                boneCount++;
            }
        }

        if (boneCount > 0)
        {
            return handCenter / boneCount; // ��ȿ�� �������� ��� ��ġ ��ȯ
        }
        else
        {
            Debug.LogWarning("���� ���� �����͸� ã�� �� �����ϴ�.");
            return Vector3.zero; // ��ȿ�� ������ ���� ��� 0 ��ȯ
        }
    }

    // ������ ���̴��� ���θ� �Ǵ��ϴ� �Լ� (���ü� ���θ� üũ)
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