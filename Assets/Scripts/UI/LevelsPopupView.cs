using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelsPopupView : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private LevelEntryView prefab;

    private readonly List<LevelEntryView> entries = new();

    public event Action<int> LevelSelected;

    public void Show(IEnumerable<LevelProgress> levels)
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
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnLevelClicked(int level)
    {
        LevelSelected?.Invoke(level);
    }
}