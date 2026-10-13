using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ScanPopupController : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private CanvasGroup popup;
    [SerializeField] private Text messageText;
    [SerializeField] private float fadeDuration = 0.3f;  // тривалість появи/зникнення в сек.

    private bool isShown = true;                         // на старті площин ще немає — попап показаний
    private Coroutine fadeRoutine;

    void OnEnable()
    {
        // подія: площини додано, оновлено або видалено
        planeManager.trackablesChanged.AddListener(OnPlanesChanged);
        // подія: змінився стан AR-сесії (почалось або втрачено відстеження)
        ARSession.stateChanged += OnSessionStateChanged;
    }

    void OnDisable()
    {
        planeManager.trackablesChanged.RemoveListener(OnPlanesChanged);
        ARSession.stateChanged -= OnSessionStateChanged;
    }

    void Start()
    {
        popup.alpha = 1f;
        Refresh();
    }

    void OnPlanesChanged(ARTrackablesChangedEventArgs<ARPlane> args) { Refresh(); }
    void OnSessionStateChanged(ARSessionStateChangedEventArgs args) { Refresh(); }

    // вирішуємо, чи потрібен попап і який текст у ньому показати
    void Refresh()
    {
        bool tracking = ARSession.state == ARSessionState.SessionTracking;
        bool hasPlanes = planeManager.trackables.count > 0;

        if (!tracking)
            messageText.text = TrackingWarning(ARSession.notTrackingReason);
        else if (!hasPlanes)
            messageText.text = "Повільно поводьте телефоном над підлогою або столом, щоб знайти поверхню";

        // площину знайдено і відстеження працює знач попап більше не потрібен
        SetVisible(!tracking || !hasPlanes);
    }

    // причини, чому немає відстеження
    static string TrackingWarning(NotTrackingReason reason)
    {
        switch (reason)
        {
            case NotTrackingReason.InsufficientLight: return "Занадто темно. Увімкніть світло";
            case NotTrackingReason.ExcessiveMotion: return "Рухайте телефон повільніше";
            case NotTrackingReason.InsufficientFeatures: return "Наведіть камеру на поверхню з текстурою, а не однотонну";
            default: return "Повільно поводьте телефоном, щоб розпочати відстеження";
        }
    }

    void SetVisible(bool visible)
    {
        if (visible == isShown) return;      // стан не змінився нічого не робимо
        isShown = visible;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(Fade(visible ? 1f : 0f));
    }

    // плавна зміна прозорості поп-апа
    IEnumerator Fade(float target)
    {
        float start = popup.alpha;
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            popup.alpha = Mathf.Lerp(start, target, time / fadeDuration);
            yield return null;
        }
        popup.alpha = target;
    }
}