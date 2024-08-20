using UnityEngine;
using Io.AppMetrica;

public class AppMetricaTest : MonoBehaviour
{
    void Start()
    {
        // Пример отправки простого события
        AppMetrica.ReportEvent("TestEvent", "{\"parameter1\":\"value1\"}");

        Debug.Log("Test event sent to AppMetrica");
    }
}
