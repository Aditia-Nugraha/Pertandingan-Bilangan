using UnityEngine;
using UnityEngine.UI;

public class PagesController : MonoBehaviour
{
    [Header("Pages")]
    [SerializeField] private Image _pageImage;
    [SerializeField] private Sprite[] _pages;

    [Header("Navigation")]
    [SerializeField] private Button _previousButton;
    [SerializeField] private Button _nextButton;

    private int _currentPage;

    private void OnEnable()
    {
        _currentPage = 0;
        RefreshPage();
    }

    public void NextPage()
    {
        if (_currentPage >= _pages.Length - 1)
        {
            return;
        }

        _currentPage++;
        RefreshPage();
    }

    public void PreviousPage()
    {
        if (_currentPage <= 0)
        {
            return;
        }

        _currentPage--;
        RefreshPage();
    }

    private void RefreshPage()
    {
        _pageImage.sprite = _pages[_currentPage];
        _previousButton.interactable = _currentPage > 0;
        _nextButton.interactable = _currentPage < _pages.Length - 1;
    }
}