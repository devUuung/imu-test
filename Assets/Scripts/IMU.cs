/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class SerialReader : MonoBehaviour
{
    SerialPort serialPort;
    public string portName = "COM"; // Serial port name
    public int baudRate = 9600; // Baud rate
    public Transform targetObject; // Reference to the object whose orientation will be updated

    private float currentRoll = 0;
    private float currentPitch = 0;
    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        try
        {
            serialPort.Open(); // Open serial port
        }
        catch (Exception e)
        {
            Debug.LogError("Serial port open error: " + e.Message);
        }
    }

    void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine();
                Debug.Log("Data received: " + data);

                // Parse the roll, pitch, and yaw from the received data
                string[] values = data.Split(',');
                if (values.Length == 3) // Ensure we received 3 values
                {
                    float roll = float.Parse(values[0]);
                    float pitch = float.Parse(values[1]);

                    currentRoll = Mathf.LerpAngle(currentRoll, roll, 0.1f);
                    currentPitch = Mathf.LerpAngle(currentPitch, pitch, 0.1f);

                    // Convert roll, pitch, and yaw to a Quaternion
                    Quaternion targetRotation = Quaternion.Euler(0, currentRoll, currentPitch - 75);

                    // Apply the rotation to the target object
                    if (targetObject != null)
                    {
                        targetObject.rotation = targetRotation;
                    }
                }
                else
                {
                    Debug.LogWarning("Received unexpected data format.");
                }
            }
            catch (TimeoutException)
            {
                Debug.LogWarning("Serial port read timeout.");
            }
            catch (FormatException e)
            {
                Debug.LogError("Data parsing error: " + e.Message);
            }
        }
    }

    private void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close(); // Close the serial port
        }
    }
}
*/