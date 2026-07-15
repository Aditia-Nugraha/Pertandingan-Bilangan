using UnityEngine;

public class UpcomingController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _panel;

    [Header("Animation")]
    [SerializeField] private ScalePanelAnimator _panelAnimator;

    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        _container.SetActive(true);
        _background.SetActive(true);
        _panel.SetActive(true);
        _panelAnimator.PlayShow();
    }

    public void Hide()
    {
        _panel.SetActive(false);
        _background.SetActive(false);
        _container.SetActive(false);
    }

    public void Close()
    {
        _background.SetActive(false);
        _panelAnimator.PlayHide(() =>
        {
            Hide();
        });
    }
}
