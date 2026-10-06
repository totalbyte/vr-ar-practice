using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARSessionStateUI : MonoBehaviour
{
    [SerializeField] private Text statusText;
    [SerializeField] private ARPlaneManager planeManager;

    // Підписуємось на подію зміни стану сесії
    void OnEnable() { ARSession.stateChanged += OnStateChanged; }
    void OnDisable() { ARSession.stateChanged -= OnStateChanged; }

    void OnStateChanged(ARSessionStateChangedEventArgs args)
    {
        Debug.Log("AR session state: " + args.state);
    }

    void Update()
    {
        // Кількість площин і причина втрати трекінгу змінюються без події, тому текст оновлюємо щокадру
        int planes = planeManager != null ? planeManager.trackables.count : 0;

        statusText.text =
            "Стан AR-сесії: " + ARSession.state + "\n" +
            Describe(ARSession.state) + "\n" +
            "Трекінг: " + ARSession.notTrackingReason + "\n" +
            "Площин знайдено: " + planes;
    }

    static string Describe(ARSessionState state)
    {
        switch (state)
        {
            case ARSessionState.None: return "сесію не запущено";
            case ARSessionState.Unsupported: return "пристрій не підтримує AR";
            case ARSessionState.CheckingAvailability: return "перевірка підтримки AR";
            case ARSessionState.NeedsInstall: return "потрібно встановити сервіси AR";
            case ARSessionState.Installing: return "встановлення сервісів AR";
            case ARSessionState.Ready: return "AR готовий";
            case ARSessionState.SessionInitializing: return "ініціалізація, повільно рухайте телефон";
            case ARSessionState.SessionTracking: return "відстеження працює";
            default: return state.ToString();
        }
    }
}