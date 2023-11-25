using UnityEngine;
using System.Collections.Generic;

public class VirtualGrid : MonoBehaviour
{
    [Tooltip("Number of rows in the grid.")]
    [SerializeField] private int rows;

    [Tooltip("Number of columns in the grid.")]
    [SerializeField] private int columns;

    [Tooltip("Distance between each item along the Y axis.")]
    [SerializeField] private float distanceY;

    [Tooltip("Distance between each item along the X axis.")]
    [SerializeField] private float distanceX;

    private List<Vector3> gridPoints;


    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        gridPoints = new List<Vector3>();

        // Calculate the offsets to center the grid at (0, 0, 0)
        float xOffset = (columns - 1) * distanceX / 2;
        float yOffset = (rows - 1) * distanceY / 2;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                float xPosition = j * distanceX - xOffset;
                float yPosition = i * distanceY - yOffset;
                Vector3 gridPoint = new Vector3(xPosition, yPosition, 0);
                gridPoints.Add(gridPoint);
            }
        }
    }

    public List<Vector3> GetGridPoints()
    {
        if (gridPoints == null || gridPoints.Count == 0)
        {
            GenerateGrid();
        }

        return gridPoints;
    }
}
