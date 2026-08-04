using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPopupView : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private LevelEntryView prefab;
    [SerializeField] private ScrollRect scrollRect;

    private readonly List<LevelEntryView> entries = new();

    public event Action<int> LevelSelected;

    public void Show(IEnumerable<LevelInfo> levels)
    {
        gameObject.SetActive(true);

        foreach (var entry in entries)
            Destroy(entry.gameObject);

        entries.Clear();

        foreach (var level in levels)
        {
            LevelEntryView view = Instantiate(prefab, content);

            view.Show(level);

            view.Clicked += OnLevelClicked;

            entries.Add(view);
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        Canvas.ForceUpdateCanvases();
        
        LevelEntryView currentEntry = entries.FirstOrDefault(e => e.IsCurrent);
        if (currentEntry != null)
        {
            ScrollTo(currentEntry);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnLevelClicked(int level)
    {
        LevelSelected?.Invoke(level);
    }

    private void ScrollTo(LevelEntryView entry)
    {
        Canvas.ForceUpdateCanvases();

        RectTransform contentRect = content;
        RectTransform viewportRect = scrollRect.viewport;
        RectTransform targetRect = entry.GetComponent<RectTransform>();

        float contentHeight = contentRect.rect.height;
        float viewportHeight = viewportRect.rect.height;

        float targetY = Mathf.Abs(targetRect.anchoredPosition.y) - viewportHeight * 0.5f + targetRect.rect.height * 0.5f;

        float normalized =
            1f - Mathf.Clamp01(
                targetY / (contentHeight - viewportHeight));

        scrollRect.verticalNormalizedPosition = normalized;
    }
}