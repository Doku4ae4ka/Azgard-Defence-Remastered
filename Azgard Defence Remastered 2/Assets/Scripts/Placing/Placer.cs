using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placer : MonoBehaviour
{
    public List<Placeable> placedThings;

    private TileMapHolder grid;
    private Preview placeablePreview;

    private void Awake()
    {
        placedThings = new List<Placeable>();
    }

    private TileMapHolder GetGrid()
    {
        if (grid == null)
        {
            grid = GetComponent<TileMapHolder>();
        }

        return grid;
    }

    private void Update()
    {
        if (placeablePreview == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(placeablePreview.gameObject);
            placeablePreview = null;
            return;
        }
        else if (Input.GetKeyUp("space"))
        {
            InstantiatePlaceable();
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPos = GetGrid().GetGridPosHere(mouse);

            Vector2 cellCenter;
            if (GetGrid().IsAreaBounded(gridPos.x, gridPos.y, Vector2Int.one))
            {
                cellCenter = GetGrid().GetGridCellPosition(gridPos);
            }
            else
            {
                cellCenter = mouse;
            }

            placeablePreview.SetCurrentMousePosition(cellCenter, gridPos, () => GetGrid().IsBuildAvailable(gridPos, placeablePreview));
        }
    }

    public void ShowPlaceablePreview(Preview preview)
    {
        if (placeablePreview != null)
        {
            Destroy(placeablePreview.gameObject);
        }

        var cameraOffset = new Vector3(Camera.main.pixelWidth * 0.5f, Screen.height * 0.5f, 0);

        var cameraPos = Camera.main.transform.position;
        var instPreviewPos = new Vector2(cameraPos.x, cameraPos.y);

        placeablePreview = Instantiate(preview, instPreviewPos, Quaternion.identity);

        Vector2Int gridPos = GetGrid().GetGridPosHere(placeablePreview.transform.position);

        if (GetGrid().IsAreaBounded(gridPos.x, gridPos.y, Vector2Int.one))
        {
            placeablePreview.SetSpawnPosition(gridPos);
            placeablePreview.SetBuildAvailable(GetGrid().IsBuildAvailable(gridPos, placeablePreview));
        }
        else
        {
            placeablePreview.SetBuildAvailable(false);
        }
    }

    private void InstantiatePlaceable()
    {
        if (placeablePreview != null && placeablePreview.IsBuildAvailable())
        {
            Placeable placeableInstance = placeablePreview.InstantiateHere();

            placedThings.Add(placeableInstance);
            OccupyCells(placeableInstance.GridPlace);

            Destroy(placeablePreview.gameObject);

            if (placeablePreview != null)
            {
                placeablePreview = null;
            }
        }
    }

    private void OccupyCells(GridPlace place)
    {
        GetGrid().SetGridPlaceStatus(place, true);
    }

}