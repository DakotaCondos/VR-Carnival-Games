using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalloonPopGame : MonoBehaviour
{
    [SerializeField] private GameObject _balloonPrefab;
    [SerializeField] private int _startingDarts = 10;
    [SerializeField] private int _score = 0;
    [SerializeField] private int _dartsRemaining;
    [SerializeField] private DartSpawner _dartSpawner;
    [SerializeField] private List<Balloon> _balloonList = new();
    [SerializeField] private VirtualGrid _virtualGrid;
    [SerializeField] private Transform _baloonGridTransform;
    [SerializeField] private Quaternion _baloonRotation = Quaternion.identity;
    [SerializeField] private ScoreFeedback _scoreFeedback;

    [Header("UI")]
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _freeplayPanel;
    [SerializeField] private GameObject _activeGamePanel;
    [SerializeField] private GameObject _resultsPanel;
    [SerializeField] private TextMeshProUGUI _highscoreText;
    [SerializeField] private TextMeshProUGUI _dartsRemainingText;
    [SerializeField] private TextMeshProUGUI _activeScoreText;
    [SerializeField] private TextMeshProUGUI _resultsScoreText;
    [SerializeField] private TextMeshProUGUI _resultsMessageText;
    [SerializeField] private Button _spawnDartButton;

    private bool _gameInProgress = false;
    private int _highScore = 0;

    private void Start()
    {
        PopulateBalloons();
    }

    private void PopulateBalloons()
    {
        if (_virtualGrid == null)
        {
            throw new MissingFieldException("VirtualGrid reference not assigned");
        }

        foreach (var item in _virtualGrid.GetGridPoints())
        {
            GameObject gameObject = Instantiate(_balloonPrefab, _baloonGridTransform);
            gameObject.transform.localPosition = item;
            gameObject.transform.localRotation = _baloonRotation;
            Balloon balloon = gameObject.GetComponent<Balloon>();
            _balloonList.Add(balloon);
            balloon.OnBalloonPop += HandleBalloonPopped;
        }
    }

    private void OnDestroy()
    {
        foreach (var item in _balloonList)
        {
            item.OnBalloonPop -= HandleBalloonPopped;
        }
    }

    public void NewGame()
    {
        _dartSpawner.RemoveAllDarts();
        ResetBalloons();
        _score = 0;
        _dartsRemaining = _startingDarts;
        _gameInProgress = true;
        _spawnDartButton.interactable = true;
        _activeScoreText.text = $"Score: {_score}";
        _dartsRemainingText.text = $"Darts Remaining: {_dartsRemaining}";
    }

    public void EndGame()
    {
        _gameInProgress = false;
        _resultsScoreText.text = $"You Scored: {_score}";
        _resultsMessageText.text = _scoreFeedback.ProvideFeedback(_score);
        UpdateHighScore(_score);
    }

    private void UpdateHighScore(int score)
    {
        if (_highScore < score)
        {
            _highscoreText.text = $"Best Score: {score}";
        }
    }

    public void SpawnDart()
    {
        if (_dartsRemaining > 0)
        {
            _dartsRemaining--;
            _dartSpawner.SpawnDart();
            _dartsRemainingText.text = $"Darts Remaining: {_dartsRemaining}";

            if (_dartsRemaining <= 0)
            {
                _spawnDartButton.interactable = false;
            }
        }
    }

    public void ResetBalloons()
    {
        foreach (var item in _balloonList)
        {
            item.ResetBalloon();
        }
    }

    public void HandleBalloonPopped()
    {
        if (!_gameInProgress) { return; }
        _score++;
        _activeScoreText.text = $"Score: {_score}";
    }
}
