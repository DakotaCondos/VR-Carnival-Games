using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonPopGame : MonoBehaviour
{
    [SerializeField] private GameObject _balloonPrefab;
    [SerializeField] private int _startingDarts = 5;
    [SerializeField] private int score = 0;
    [SerializeField] private int dartsRemaining;
    [SerializeField] DartSpawner _dartSpawner;
    [SerializeField] private List<Balloon> _balloonList = new();
    [SerializeField] private VirtualGrid _virtualGrid;
    [SerializeField] Transform _baloonGridTransform;
    [SerializeField] Quaternion _baloonRotation = Quaternion.identity;

    void Start()
    {
        PopulateBalloons();
        ResetGame();
    }

    private void PopulateBalloons()
    {
        if (_virtualGrid == null)
        {
            throw new MissingFieldException("VirtualGrid reference not assigned");
        }

        foreach (var item in _virtualGrid.GetGridPoints())
        {
            var g = Instantiate(_balloonPrefab, _baloonGridTransform);
            g.transform.localPosition = item;
            g.transform.localRotation = _baloonRotation;
            _balloonList.Add(g.GetComponent<Balloon>());
        }
    }

    void ResetGame()
    {
        ClearGrid();
        PopulateGrid();
        score = 0;
        dartsRemaining = _startingDarts;
        UpdateUI();
    }

    void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    ThrowDart();
        //}
    }

    void ThrowDart()
    {
        if (dartsRemaining > 0)
        {
            // Logic for throwing a dart
            dartsRemaining--;
            UpdateUI();
        }
    }

    void ClearGrid()
    {
        // Logic to clear the grid of balloons
    }

    void PopulateGrid()
    {
        // Logic to populate the grid with balloons
    }

    void UpdateUI()
    {
        // Update the UI elements for score and darts remaining
    }

    public void BalloonPopped(bool givesExtraDart)
    {
        score++;
        if (givesExtraDart)
        {
            //dartsRemaining++;
        }
        UpdateUI();
    }
}
