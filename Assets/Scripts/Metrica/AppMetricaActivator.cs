using Io.AppMetrica;
using UnityEngine;

public static class AppMetricaActivator {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Activate() {
        AppMetrica.Activate(new AppMetricaConfig("APIKey") {
            FirstActivationAsUpdate = !IsFirstLaunch(),
        });
        Debug.Log("AppMetrica activated");
    }

    private static bool IsFirstLaunch() {
        // Пример простой логики проверки первого запуска
        if (!PlayerPrefs.HasKey("FirstLaunch"))
        {
            PlayerPrefs.SetInt("FirstLaunch", 1);
            return true;
        }
        return false;
    }
}
