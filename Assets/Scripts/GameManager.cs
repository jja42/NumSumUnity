using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    NumberGen.Grid grid;

    int grid_cell_size = 80;

    public GameObject GridLayout;
    public GameObject Row_Sums;
    public GameObject Column_Sums;
    public GameObject GridPrefab;
    public GameObject SumPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetupGrid(int size)
    {
        grid = NumberGen.instance.CreateGrid(size);

        ResizeLayout(size);

        //Generates Grid Cells
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                NumberGen.Num n = grid.grid[i][j];
                GameObject GO = Instantiate(GridPrefab, GridLayout.transform);
                GridEntity g_ent = GO.GetComponent<GridEntity>();
                g_ent.number = n;
                g_ent.position = new Vector2(j, i);
                g_ent.interactible = true;
                g_ent.InitText();
            }
        }


        //Generate Sums
        for (int c = 0; c < grid.column_sums.Count; c++)
        {
            NumberGen.Num n = new();
            n.value = grid.column_sums[c];
            GameObject GO = Instantiate(SumPrefab, Column_Sums.transform);
            GridEntity g_ent = GO.GetComponent<GridEntity>();
            g_ent.number = n;
            g_ent.interactible = false;
            g_ent.InitText();
        }
        for (int r = 0; r < grid.row_sums.Count; r++)
        {
            NumberGen.Num n = new();
            n.value = grid.row_sums[r];
            GameObject GO = Instantiate(SumPrefab, Row_Sums.transform);
            GridEntity g_ent = GO.GetComponent<GridEntity>();
            g_ent.number = n;
            g_ent.interactible = false;
            g_ent.InitText();
        }

    }

    void ResizeLayout(int size)
    {
        RectTransform transform = GridLayout.GetComponent<RectTransform>();

        float dimension = size * grid_cell_size;
        dimension += (size - 1) * 2.5f;

        transform.sizeDelta = new Vector2(dimension, dimension);

        RectTransform rows_sum_transform = Row_Sums.GetComponent<RectTransform>();
        rows_sum_transform.sizeDelta = new Vector2(grid_cell_size, dimension);

        RectTransform column_sum_transform = Column_Sums.GetComponent<RectTransform>();
        column_sum_transform.sizeDelta = new Vector2(dimension, grid_cell_size);

        rows_sum_transform.anchoredPosition = new Vector3(transform.anchoredPosition.x - (dimension/2) - grid_cell_size, rows_sum_transform.anchoredPosition.y);

        column_sum_transform.anchoredPosition = new Vector3(column_sum_transform.anchoredPosition.x, transform.anchoredPosition.y + (dimension / 2) + grid_cell_size);
    }
}
