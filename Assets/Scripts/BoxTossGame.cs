using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxTossGame : MonoBehaviour
{
    [SerializeField] private VirtualGrid _virtualGrid;
    private List<Vector3> _gridpoints;
    [SerializeField] private int _lowPointValue;
    [SerializeField] private int _highPointValue;
    [SerializeField] private ScoreFeedback _scoreFeedback;
    [SerializeField] private List<BoxTossPointInteractor> _pointInteractors = new();
    [SerializeField] private BoxTossPointInteractor _pointInteractorPrefab;
    [SerializeField] private GameObject _gridTransform;
    [SerializeField] private ItemSpawner _itemSpawner;

    [SerializeField] private int _startingBalls = 10;
    [SerializeField] private int _score = 0;
    [SerializeField] private int _ballsRemaining;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _highscoreText;
    [SerializeField] private TextMeshProUGUI _ballsRemainingText;
    [SerializeField] private TextMeshProUGUI _activeScoreText;
    [SerializeField] private TextMeshProUGUI _resultsScoreText;
    [SerializeField] private TextMeshProUGUI _resultsMessageText;
    [SerializeField] private Button _spawnBallButton;

    private bool _gameInProgress = false;
    private int _highScore = 0;


    private void Awake()
    {
        _gridpoints = _virtualGrid.GetGridPoints();
        List<int> intList = IntListGenerator.GenerateIntList(_lowPointValue, _highPointValue, _gridpoints.Count, _gridpoints.Count * (_lowPointValue + _highPointValue) / 2);

        int count = 0;
        foreach (var item in _gridpoints)
        {
            GameObject gameobject = Instantiate(_pointInteractorPrefab.gameObject, _gridTransform.transform);
            gameobject.transform.SetLocalPositionAndRotation(item, Quaternion.identity);
            BoxTossPointInteractor pointInteractor = gameobject.GetComponent<BoxTossPointInteractor>();
            _pointInteractors.Add(pointInteractor);
            pointInteractor.SetPointValue(intList[count]);
            count++;
        }
    }

    public void NewGame()
    {
        _itemSpawner.RemoveAllSpawnedItems();
        _score = 0;
        _ballsRemaining = _startingBalls;
        _gameInProgress = true;
        _spawnBallButton.interactable = true;
        _activeScoreText.text = $"Score: {_score}";
        _ballsRemainingText.text = $"Balls Remaining: {_ballsRemaining}";
        StartCoroutine(CheckForCollisions());
    }

    public void EndGame()
    {
        _gameInProgress = false;
        UpdateScore();
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

    public void UpdateScore()
    {
        _score = 0;

        foreach (BoxTossPointInteractor interactor in _pointInteractors)
        {
            if (interactor.IsColliding())
            {
                _score += interactor.Points;
            }
        }
        _activeScoreText.text = $"Score: {_score}";
    }

    public void SpawnItem()
    {
        if (_ballsRemaining > 0)
        {
            _ballsRemaining--;
            _itemSpawner.SpawnItem();
            _ballsRemainingText.text = $"Balls Remaining: {_ballsRemaining}";

            if (_ballsRemaining <= 0)
            {
                _spawnBallButton.interactable = false;
            }
        }
    }

    private IEnumerator CheckForCollisions()
    {
        while (_gameInProgress)
        {
            UpdateScore();

            yield return new WaitForSeconds(1f); // Wait for 1 second before the next check
        }
    }
}