using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace minutnik;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer _timer;
    private int _workMinutes = 25;
    private int _breakMinutes = 5;
    private int _remainingSeconds = 25 * 60;
    private int _cycles = 0;
    private readonly List<string> _history = new();
    private bool _isBreak = false;
    private bool _isRunning = false;

    public MainWindow()
    {
        InitializeComponent();

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += OnTimerTick;

        UpdateTimeDisplay();
        UpdateCyclesDisplay();
        UpdatePhaseDisplay();
    }

    private void OnStartPauseClick(object? sender, RoutedEventArgs e)
    {
        if (_isRunning)
        {
            _timer.Stop();
            _isRunning = false;
            StartPauseButton.Content = "Start";
        }
        else
        {
            _timer.Start();
            _isRunning = true;
            StartPauseButton.Content = "Pauza";
        }
    }

    private void OnResetClick(object? sender, RoutedEventArgs e)
    {
        _timer.Stop();
        _isRunning = false;
        _isBreak = false;
        _cycles = 0;
        _remainingSeconds = _workMinutes * 60;

        StartPauseButton.Content = "Start";
        UpdateTimeDisplay();
        UpdateCyclesDisplay();
        UpdatePhaseDisplay();
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        if (_remainingSeconds > 0)
        {
            _remainingSeconds--;
            UpdateTimeDisplay();
        }

        if (_remainingSeconds <= 0)
        {
            if (!_isBreak)
            {
                // Work session ended -> break starts automatically
                _cycles++;
                _history.Add($"{DateTime.Now:HH:mm} — cykl {_cycles}");
                HistoryList.ItemsSource = _history.ToArray();
                UpdateCyclesDisplay();

                _isBreak = true;
                _remainingSeconds = _breakMinutes * 60;
                UpdatePhaseDisplay();
                UpdateTimeDisplay();
            }
            else
            {
                // Break session ended -> work starts automatically
                _isBreak = false;
                _remainingSeconds = _workMinutes * 60;
                UpdatePhaseDisplay();
                UpdateTimeDisplay();
            }
        }
    }

    private void OnWorkDurationChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        if (e.NewValue.HasValue)
        {
            _workMinutes = Math.Max(1, (int)e.NewValue.Value);
            if (!_isRunning && !_isBreak)
            {
                _remainingSeconds = _workMinutes * 60;
                UpdateTimeDisplay();
            }
        }
    }

    private void OnBreakDurationChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        if (e.NewValue.HasValue)
        {
            _breakMinutes = Math.Max(1, (int)e.NewValue.Value);
            if (!_isRunning && _isBreak)
            {
                _remainingSeconds = _breakMinutes * 60;
                UpdateTimeDisplay();
            }
        }
    }

    private void UpdateTimeDisplay()
    {
        int minutes = _remainingSeconds / 60;
        int seconds = _remainingSeconds % 60;
        TimeTextBlock.Text = $"{minutes:D2}:{seconds:D2}";
    }

    private void UpdateCyclesDisplay()
    {
        CyclesTextBlock.Text = $"Skończone cykle: {_cycles}";
    }

    private void UpdatePhaseDisplay()
    {
        PhaseTextBlock.Text = _isBreak ? "Przerwa" : "Praca";
        PhaseTextBlock.Foreground = _isBreak
            ? Avalonia.Media.Brushes.Green
            : Avalonia.Media.Brushes.DodgerBlue;
    }
}
