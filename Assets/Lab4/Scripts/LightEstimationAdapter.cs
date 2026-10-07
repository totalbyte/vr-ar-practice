using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

/// Адаптує освітлення віртуального об'єкта до реального освітлення (Light Estimation)
public class LightEstimationAdapter : MonoBehaviour
{
    public ARCameraManager arCameraManager;
    public Light mainLight;
    public Text lightInfoText;

    void OnEnable()
    {
        // підписуємось на подію: метод викликатиметься на кожен новий кадр камери
        arCameraManager.frameReceived += OnCameraFrameReceived;
    }

    void OnDisable()
    {
        arCameraManager.frameReceived -= OnCameraFrameReceived;
    }

    void OnCameraFrameReceived(ARCameraFrameEventArgs args)
    {
        string info = "Оцінка освітлення:\n";

        // Перевіряємо наявність інформації про освітлення і виводимо її

        if (args.lightEstimation.averageBrightness.HasValue)
        {
            float brightness = args.lightEstimation.averageBrightness.Value;
            Debug.Log("Освітленість (Яскравість): " + brightness);
            mainLight.intensity = brightness;                   // темніше в кімнаті — темніше освітлений об'єкт
            info += "Яскравість: " + brightness.ToString("F2") + "\n";
        }
        else
        {
            Debug.Log("Освітленість не доступна.");
            info += "Яскравість: не доступна\n";
        }

        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            float colorTemperature = args.lightEstimation.averageColorTemperature.Value;
            Debug.Log("Колірна температура: " + colorTemperature);
            mainLight.useColorTemperature = true;
            mainLight.colorTemperature = colorTemperature;      // тепле (жовте) або холодне (синє) світло
            info += "Колірна температура: " + colorTemperature.ToString("F0") + " K\n";
        }
        else
        {
            Debug.Log("Колірна температура не доступна.");
            info += "Колірна температура: не доступна\n";
        }

        if (args.lightEstimation.colorCorrection.HasValue)
        {
            Color colorCorrection = args.lightEstimation.colorCorrection.Value;
            Debug.Log("Корекція кольору: " + colorCorrection);
            mainLight.color = colorCorrection;                  // відтінок світла як у реальному оточенні
            info += "Корекція кольору: " + colorCorrection + "\n";
        }
        else
        {
            Debug.Log("Корекція кольору не доступна.");
            info += "Корекція кольору: не доступна\n";
        }

        if (args.lightEstimation.mainLightDirection.HasValue)
        {
            Vector3 lightDirection = args.lightEstimation.mainLightDirection.Value;
            Debug.Log("Напрямок основного світла: " + lightDirection);
            mainLight.transform.rotation = Quaternion.LookRotation(lightDirection);   // світло і тінь падають з того ж боку, що й реальні
            info += "Напрямок світла: " + lightDirection.ToString("F2") + "\n";
        }
        else
        {
            Debug.Log("Напрямок основного світла не доступний.");
            info += "Напрямок світла: не доступний\n";
        }

        if (args.lightEstimation.mainLightIntensityLumens.HasValue)
        {
            float mainLightIntensity = args.lightEstimation.mainLightIntensityLumens.Value;
            Debug.Log("Інтенсивність основного світла (в люменах): " + mainLightIntensity);
            // люмени фізична величина, а intensity у Unity в умовних одиницях,
            // тому для світла беремо нормалізовану яскравість основного джерела
            if (args.lightEstimation.averageMainLightBrightness.HasValue)
                mainLight.intensity = args.lightEstimation.averageMainLightBrightness.Value;
            info += "Інтенсивність світла: " + mainLightIntensity.ToString("F0") + " лм\n";
        }
        else
        {
            Debug.Log("Інтенсивність основного світла не доступна.");
            info += "Інтенсивність світла: не доступна\n";
        }

        if (args.lightEstimation.mainLightColor.HasValue)
        {
            Color mainLightColor = args.lightEstimation.mainLightColor.Value;
            Debug.Log("Колір основного світла: " + mainLightColor);
            mainLight.color = mainLightColor;                   // колір реального джерела світла
            info += "Колір світла: " + mainLightColor + "\n";
        }
        else
        {
            Debug.Log("Колір основного світла не доступний.");
            info += "Колір світла: не доступний\n";
        }

        if (args.lightEstimation.ambientSphericalHarmonics.HasValue)
        {
            var sphericalHarmonics = args.lightEstimation.ambientSphericalHarmonics.Value;
            Debug.Log("Сферичні гармоніки оточуючого світла: " + sphericalHarmonics);
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.ambientProbe = sphericalHarmonics;   // розсіяне світло, що падає на об'єкт з усіх боків
            info += "Сферичні гармоніки: отримано\n";
        }
        else
        {
            Debug.Log("Сферичні гармоніки не доступні.");
            info += "Сферичні гармоніки: не доступні\n";
        }

        if (lightInfoText != null)
            lightInfoText.text = info;
    }
}