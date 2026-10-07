using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;                    // Touchscreen
using UnityEngine.InputSystem.EnhancedTouch;      // EnhancedTouchSupport, TouchSimulation

/// (дотики, перевірка UI, Raycast по об'єкту)
public static class ARTouchInput
{
    /// поріг скалярного добутку: > 0.9 означає, що пальці рухаються в одному напрямку (кут < ~25)
    public const float SameDirectionThreshold = 0.9f;

    private static int users;                     // скільки скриптів зараз користується дотиками
    private static readonly List<RaycastResult> uiHits = new List<RaycastResult>();
#if UNITY_EDITOR
    private static bool simulating;               // чи ввімкнено симуляцію дотику мишею
#endif

    // якщо в редакторі вимкнено Domain Reload, статичні поля не скидаються самі
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetState()
    {
        users = 0;
#if UNITY_EDITOR
        simulating = false;
#endif
    }

    /// вмикає EnhancedTouch. викликати в OnEnable() (парно до Release())
    public static void Acquire()
    {
        if (users++ > 0) return;
        EnhancedTouchSupport.Enable();
#if UNITY_EDITOR
        // симулюємо дотик мишею (лише один палець)
        if (Touchscreen.current == null)
        {
            TouchSimulation.Enable();
            simulating = true;
        }
#endif
    }

    /// вимикає EnhancedTouch, коли ним більше ніхто не користується. викликати в OnDisable()
    public static void Release()
    {
        if (users == 0 || --users > 0) return;
#if UNITY_EDITOR
        if (simulating)
        {
            TouchSimulation.Disable();
            simulating = false;
        }
#endif
        EnhancedTouchSupport.Disable();
    }

    /// чи знаходиться під пальцем елемент UI
    public static bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null) return false;
        var pointerData = new PointerEventData(EventSystem.current) { position = screenPosition };
        uiHits.Clear();
        EventSystem.current.RaycastAll(pointerData, uiHits);
        return uiHits.Count > 0;
    }

    /// чи рухаються два пальці в одному напрямку
    public static bool IsSameDirection(Vector2 delta1, Vector2 delta2)
    {
        return Vector2.Dot(delta1.normalized, delta2.normalized) > SameDirectionThreshold;
    }

    /// Raycast з камери через точку екрана: чи влучив промінь у target (або його дочірній об'єкт)
    public static bool TryHitObject(Camera cam, Vector2 screenPosition, GameObject target, out RaycastHit result)
    {
        result = default;
        if (cam == null || target == null) return false;

        Ray ray = cam.ScreenPointToRay(screenPosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        bool found = false;
        float best = float.MaxValue;
        foreach (RaycastHit hit in hits)
        {
            if (!hit.transform.IsChildOf(target.transform)) continue;
            if (hit.distance < best)
            {
                best = hit.distance;
                result = hit;
                found = true;
            }
        }
        return found;
    }
}