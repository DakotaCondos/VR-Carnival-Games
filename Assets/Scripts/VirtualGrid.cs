using UnityEngine;
using System.Collections.Generic;

public class VirtualGrid : MonoBehaviour
{
    // Enum to represent grid orientation
    public enum GridOrientation
    {
        XY,
        XZ
    }

    [Tooltip("Number of rows in the grid.")]
    [SerializeField] private int rows;

    [Tooltip("Number of columns in the grid.")]
    [SerializeField] private int columns;

    [Tooltip("Distance between each item along the Y/Z axis.")]
    [SerializeField] private float distanceY;

    [Tooltip("Distance between each item along the X axis.")]
    [SerializeField] private float distanceX;

    [Tooltip("Orientation of the grid.")]
    [SerializeField] private GridOrientation orientation = GridOrientation.XY;

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
        float yzOffset = (rows - 1) * distanceY / 2;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                float xPosition = j * distanceX - xOffset;
                float yzPosition = i * distanceY - yzOffset;

                // Create grid point based on the selected orientation
                Vector3 gridPoint;
                if (orientation == GridOrientation.XY)
                {
                    gridPoint = new Vector3(xPosition, yzPosition, 0);
                }
                else // XZ orientation
                {
                    gridPoint = new Vector3(xPosition, 0, yzPosition);
                }

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
