using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace PlayerDatabaseValidator;

public partial class MainWindow : Window
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private List<PlayerValidationResult> _allResults = new();
    private string? _currentFilePath;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void LoadButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new()
        {
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Select Players.json"
        };

        if (dialog.ShowDialog() != true)
            return;

        try
        {
            LoadAndValidate(dialog.FileName);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load file:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void LoadAndValidate(string filePath)
    {
        string json = File.ReadAllText(filePath);

        PlayerDatabaseFile? data = JsonSerializer.Deserialize<PlayerDatabaseFile>(json, JsonOptions);
        List<FootballPlayer> players = data?.Players ?? new List<FootballPlayer>();

        _allResults = players
            .Select(p => new PlayerValidationResult(p, players))
            .ToList();

        _currentFilePath = filePath;
        FilePathText.Text = filePath;
        RemoveButton.IsEnabled = true;
        SaveButton.IsEnabled = true;

        RefreshSummary();
        ShowClubHistory(null);

        SearchBox.Text = string.Empty;
        ApplyFilter();
    }

    private void RefreshSummary()
    {
        int validCount = _allResults.Count(r => r.IsValid && r.Warnings.Count == 0);
        int warningCount = _allResults.Count(r => r.IsValid && r.Warnings.Count > 0);
        int errorCount = _allResults.Count(r => !r.IsValid);

        PlayerCountText.Text = $"Players: {_allResults.Count}";
        ValidCountText.Text = $"Valid: {validCount}";
        WarningCountText.Text = $"Warnings: {warningCount}";
        ErrorCountText.Text = $"Errors: {errorCount}";
    }

    private void RemoveButton_Click(object sender, RoutedEventArgs e)
    {
        RemoveSelectedPlayer();
    }

    private void PlayersGrid_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Delete)
        {
            e.Handled = true;
            RemoveSelectedPlayer();
        }
    }

    private void RemoveSelectedPlayer()
    {
        if (PlayersGrid.SelectedItem is not PlayerValidationResult selected)
        {
            MessageBox.Show("Select a player in the list first.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        int selectedIndex = PlayersGrid.Items.IndexOf(selected);

        _allResults.Remove(selected);

        // Re-run validation so duplicate-name warnings reflect the remaining players.
        List<FootballPlayer> remainingPlayers = _allResults.Select(r => r.Player).ToList();
        _allResults = remainingPlayers
            .Select(p => new PlayerValidationResult(p, remainingPlayers))
            .ToList();

        RefreshSummary();
        ApplyFilter();

        int previousIndex = selectedIndex - 1;
        if (previousIndex >= 0 && previousIndex < PlayersGrid.Items.Count)
        {
            PlayersGrid.SelectedIndex = previousIndex;
            PlayersGrid.ScrollIntoView(PlayersGrid.SelectedItem);
        }
        else
        {
            ShowClubHistory(null);
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_currentFilePath))
            return;

        try
        {
            PlayerDatabaseFile data = new()
            {
                Players = _allResults.Select(r => r.Player).ToList()
            };

            JsonSerializerOptions writeOptions = new()
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(data, writeOptions);
            File.WriteAllText(_currentFilePath, json);

            MessageBox.Show("Changes saved successfully.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save file:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter();
    }

    private void PlayersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ShowClubHistory(PlayersGrid.SelectedItem as PlayerValidationResult);
    }

    private void ShowClubHistory(PlayerValidationResult? selected)
    {
        if (selected == null)
        {
            ClubHistoryHeader.Text = "Select a player to see career history";
            ClubHistoryList.ItemsSource = null;
            return;
        }

        string playerName = string.IsNullOrWhiteSpace(selected.Player.Name) ? "(unnamed)" : selected.Player.Name;
        ClubHistoryHeader.Text = $"{playerName} - career history ({selected.Player.Clubs.Count} club(s))";

        // Preserve the original order from the JSON file (do not re-sort).
        List<ClubHistoryEntry> entries = selected.Player.Clubs
            .Select((club, index) => new ClubHistoryEntry(index + 1, club))
            .ToList();

        ClubHistoryList.ItemsSource = entries;
    }

    private void ApplyFilter()
    {
        string query = SearchBox.Text?.Trim() ?? string.Empty;

        List<PlayerValidationResult> filtered = string.IsNullOrEmpty(query)
            ? _allResults
            : _allResults
                .Where(r => r.Player.Name != null &&
                            r.Player.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

        List<SortDescription> sortDescriptions = PlayersGrid.Items.SortDescriptions.ToList();

        PlayersGrid.ItemsSource = filtered;

        foreach (SortDescription sortDescription in sortDescriptions)
            PlayersGrid.Items.SortDescriptions.Add(sortDescription);

        foreach (DataGridColumn column in PlayersGrid.Columns)
        {
            string sortPropertyName = column.SortMemberPath ?? string.Empty;
            SortDescription match = sortDescriptions
                .Cast<SortDescription?>()
                .FirstOrDefault(sd => sd?.PropertyName == sortPropertyName) ?? default;

            column.SortDirection = sortDescriptions.Any(sd => sd.PropertyName == sortPropertyName)
                ? match.Direction
                : (ListSortDirection?)null;
        }

        SearchResultText.Text = string.IsNullOrEmpty(query)
            ? string.Empty
            : $"Showing {filtered.Count} of {_allResults.Count} players";
    }
}
