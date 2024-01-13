using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrogJumpGame : MonoBehaviour
{
    [SerializeField] ItemSpawner _itemSpawner;

    [SerializeField] private int _startingItems = 5;
    [SerializeField] private int _score = 0;
    [SerializeField] private int _itemsRemaining;
    [SerializeField] private ScoreFeedback _scoreFeedback;
    [SerializeField] private ImpactLauncher impactLauncher;
    [SerializeField] private float distanceScalar = 1;
    [SerializeField] private float jumpHeightScalar = 1;
    [SerializeField] private AudioClip frogClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float frogJumpVolume = 1;


    [SerializeField] List<ColliderScorer> scorerList = new();

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _highscoreText;
    [SerializeField] private TextMeshProUGUI _itemsRemainingText;
    [SerializeField] private TextMeshProUGUI _activeScoreText;
    [SerializeField] private TextMeshProUGUI _resultsScoreText;
    [SerializeField] private TextMeshProUGUI _resultsMessageText;
    [SerializeField] private Button _spawnItemButton;

    private bool _gameInProgress = false;
    private int _highScore = 0;
    private GameObject _activeFrog;

    private void Awake()
    {
        impactLauncher.OnImpact += HandleImpactLauncherTriggered;
    }

    private void OnDestroy()
    {
        impactLauncher.OnImpact -= HandleImpactLauncherTriggered;
    }

    public void NewGame()
    {
        //ResetPlaceables();
        _itemSpawner.RemoveAllSpawnedItems();
        _score = 0;
        _itemsRemaining = _startingItems;
        _spawnItemButton.interactable = true;
        _activeScoreText.text = $"Score: {_score}";
        _itemsRemainingText.text = $"Frogs Remaining: {_itemsRemaining}";
    }

    public void EndGame()
    {
        _gameInProgress = false;
        DetermineScore();
        _resultsScoreText.text = $"You Scored: {_score}";
        _resultsMessageText.text = _scoreFeedback.ProvideFeedback(_score);
        UpdateHighScore(_score);
    }

    private void DetermineScore()
    {
        foreach (ColliderScorer item in scorerList)
        {
            _score += item.GetScore();
        }
    }

    private void UpdateHighScore(int score)
    {
        if (_highScore < score)
        {
            _highscoreText.text = $"Best Score: {score}";
        }
    }


    public void SpawnItem()
    {
        if (_itemsRemaining > 0)
        {
            _itemsRemaining--;
            CreateActiveFrog();
            _itemsRemainingText.text = $"Frogs Remaining: {_itemsRemaining}";

            if (_itemsRemaining <= 0)
            {
                _spawnItemButton.interactable = false;
            }
        }
    }

    public void CreateActiveFrog()
    {
        _activeFrog = _itemSpawner.SpawnAndReturnItem();
    }

    private void HandleImpactLauncherTriggered(Vector3 direction, float distanceFraction)
    {
        // if there is an active frog apply jump effect to it
        if (_activeFrog != null)
        {
            LaunchFrog(_activeFrog, direction, distanceFraction);
            _activeFrog = null;
        }
    }

    private void LaunchFrog(GameObject activeFrog, Vector3 direction, float distanceFraction)
    {
        Rigidbody rigidbody = activeFrog.GetComponent<Rigidbody>();
        if (rigidbody == null) { return; }

        rigidbody.AddForce(direction * distanceFraction * distanceScalar, ForceMode.Impulse);
        rigidbody.AddForce(Vector3.up * jumpHeightScalar, ForceMode.Impulse);

        audioSource.PlayOneShot(frogClip, frogJumpVolume);
    }
}