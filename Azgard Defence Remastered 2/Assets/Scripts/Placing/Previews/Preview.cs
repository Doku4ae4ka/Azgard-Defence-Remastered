using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Preview : MonoBehaviour
{
    [SerializeField] private Placeable Placeable;

    public Vector2Int Size;
    private Vector2Int currentGridPose;

    private bool isPlacingAvailable;
    private bool isMoving;

    protected SpriteRenderer MainRenderer;
    private Color green;
    private Color red;

    private void Awake()
    {
        MainRenderer = GetComponentInChildren<SpriteRenderer>();
        green = new Color(0, 1f, .3f, .8f);
        red = new Color(1, .2f, .2f, .8f);
    }

    public void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        isMoving = true;
    }

    public void OnMouseUp()
    {
        isMoving = false;
    }

    public void SetCurrentMousePosition(Vector2 position, Vector2Int GridPose, Func<Boolean> isBuildAvailable)
    {
        if (isMoving)
        {
            transform.position = position;
            currentGridPose = GridPose;
            SetBuildAvailable(isBuildAvailable());
        }
    }

    public void SetSpawnPosition(Vector2Int GridPose)
    {
        currentGridPose = GridPose;
    }

    public Placeable InstantiateHere()
    {
        if (isPlacingAvailable)
        {
            Vector2Int size = GetSize();

            Cell[] placeInGrid = new Cell[size.x * size.y];
            int index = 0;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    placeInGrid[index++] = new Cell(currentGridPose.x + x, currentGridPose.y + y);
                }
            }

            Placeable placeable = InitPlaceable(placeInGrid);
            Destroy(gameObject);
            return placeable;
        }

        return null;
    }

    private Placeable InitPlaceable(Cell[] placeInGrid)
    {
        Placeable placeable = Instantiate(Placeable, transform.position, Quaternion.identity);
        placeable.GridPlace = new GridPlace(placeInGrid);
        return placeable;
    }

    public void SetBuildAvailable(bool available)
    {
        isPlacingAvailable = available;
        MainRenderer.material.color = available ? green : red;
    }

    public bool IsBuildAvailable()
    {
        return isPlacingAvailable;
    }

    public virtual Vector2Int GetSize()
    {
        return Size;
    }
}