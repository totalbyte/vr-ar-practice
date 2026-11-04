using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

/// перемикає кут snap-повороту по колу 30° → 45° → 90°
/// кнопкою B на правому контролері (або N на клавіатурі для тесту на ПК)
public class SnapTurnAngleSwitcher : MonoBehaviour
{
    [SerializeField] private SnapTurnProvider snapTurnProvider;
    [SerializeField] private float[] angles = { 30f, 45f, 90f };   // доступні кути
    [SerializeField] private int startIndex = 1;                    // стартовий кут — 45°

    private InputAction switchAction;
    private int index;

    private void Awake()
    {
        if (snapTurnProvider == null) snapTurnProvider = GetComponent<SnapTurnProvider>();

        // кнопка B (secondary button) правого контролера
        switchAction = new InputAction("SwitchSnapAngle", InputActionType.Button,
            "<XRController>{RightHand}/secondaryButton");
        switchAction.AddBinding("<Keyboard>/n"); // запасна клавіша для тесту в симуляторі

        index = startIndex;
        Apply();
    }

    private void OnEnable()
    {
        switchAction.performed += OnSwitch;
        switchAction.Enable();
    }

    private void OnDisable()
    {
        switchAction.performed -= OnSwitch;
        switchAction.Disable();
    }

    private void OnDestroy()
    {
        switchAction.Dispose();
    }

    private void OnSwitch(InputAction.CallbackContext context) => NextAngle();

    // наступний кут по колу
    public void NextAngle()
    {
        index = (index + 1) % angles.Length;
        Apply();
    }

    private void Apply()
    {
        snapTurnProvider.turnAmount = angles[index];
        Debug.Log($"Кут snap-повороту: {angles[index]}°");
    }
}