using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MilkBottleTossGame : MonoBehaviour
{
    [SerializeField] List<PlaceableDetection> _placeables;
    [SerializeField] ItemSpawner _itemSpawner;

    [SerializeField] private int _startingItems = 4;
    [SerializeField] private int _score = 0;
    [SerializeField] private int _itemsRemaining;
    [SerializeField] private ScoreFeedback _scoreFeedback;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _highscoreText;
    [SerializeField] private TextMeshProUGUI _itemsRemainingText;
    [SerializeField] private TextMeshProUGUI _activeScoreText;
    [SerializeField] private TextMeshProUGUI _resultsScoreText;
    [SerializeField] private TextMeshProUGUI _resultsMessageText;
    [SerializeField] private Button _spawnItemButton;

    private bool _gameInProgress = false;
    private int _highScore = 0;

    private void Awake()
    {
        foreach (var item in _placeables)
        {
            item.OnDisplaced += ScorePoint;
        }
    }

    private void OnDestroy()
    {
        foreach (var item in _placeables)
        {
            item.OnDisplaced -= ScorePoint;
        }
    }

    public void NewGame()
    {
        ResetPlaceables();
        _itemSpawner.RemoveAllSpawnedItems();
        _score = 0;
        _itemsRemaining = _startingItems;
        _spawnItemButton.interactable = true;
        _activeScoreText.text = $"Score: {_score}";
        _itemsRemainingText.text = $"Balls Remaining: {_itemsRemaining}";
        Invoke(nameof(EnableScoringAfterReset), _placeables.First().ResetDuration);
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

    private void EnableScoringAfterReset()
    {
        _gameInProgress = true;
    }

    public void ScorePoint()
    {
        if (!_gameInProgress) { return; }
        _score++;
        _activeScoreText.text = $"Score {_score}";
    }

    public void SpawnItem()
    {
        if (_itemsRemaining > 0)
        {
            _itemsRemaining--;
            _itemSpawner.SpawnItem();
            _itemsRemainingText.text = $"Balls Remaining: {_itemsRemaining}";

            if (_itemsRemaining <= 0)
            {
                _spawnItemButton.interactable = false;
            }
        }
    }


    public void ResetPlaceables()
    {
        foreach (var item in _placeables)
        {
            item.ResetPositionAndRotation();
        }
    }
}
