using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlaneVisibilityToggle : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private Button toggleButton;
    [SerializeField] private Text buttonLabel;

    private bool planesVisible = true;

    void Start()
    {
        toggleButton.onClick.AddListener(TogglePlanes);   // обробник натискання
        UpdateLabel();
    }

    public void TogglePlanes()
    {
        planesVisible = !planesVisible;
        SetAllPlanesActive(planesVisible);
        UpdateLabel();
    }

    void Update()
    {
        // Площини, знайдені вже після «Сховати», теж мають бути прихованими
        if (!planesVisible)
            SetAllPlanesActive(false);
    }

    void SetAllPlanesActive(bool active)
    {
        // trackables — усі площини, які зараз відстежує ARPlaneManager
        foreach (ARPlane plane in planeManager.trackables)
        {
            if (plane.gameObject.activeSelf != active)
                plane.gameObject.SetActive(active);
        }
    }

    void UpdateLabel()
    {
        buttonLabel.text = planesVisible ? "Сховати площини" : "Показати площини";
    }
}