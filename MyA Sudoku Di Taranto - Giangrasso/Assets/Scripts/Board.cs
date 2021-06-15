using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private int[,] solvedGrid = new int[9, 9];
    private string s;

    int[,] riddleGrid = new int[9, 9];
    int piecesToErase = 35;

    public Transform A1, A2, A3, B1, B2, B3, C1, C2, C3;
    public GameObject buttonPrefab;

    private List<NumberField> fieldList = new List<NumberField>();

    private int maxHints;
    public enum Difficulties
    {
        DEBUG,
        EASY,
        MEDIUM,
        HARD,
        INSANE
    }

    public Difficulties difficulty;

    private Queue solvePath = new Queue();

    private void Start()
    {
        FillGridBase(ref solvedGrid);
        SolvedGrid(ref solvedGrid);
        CreateRiddleGrid(ref solvedGrid, ref riddleGrid);

        CreateButtons();
        //InitGrid(ref solvedGrid);
        ////DebugGrid(ref solvedGrid);

        //ShuffleGrid(ref solvedGrid, 100);
        //CreateRiddleGrid();
        //CreateButtons();
    }

    

    private void InitGrid(ref int[,] grid)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                grid[i, j] = (i * 3 + i / 3 + j) % 9 + 1;
            }
        }
    }

    private void DebugGrid(ref int[,] grid)
    {
        s = "";
        int sep = 0;
        for (int i = 0; i < 9; i++)
        {
            s += "|";
            for (int j = 0; j < 9; j++)
            {
                s += grid[i, j].ToString();

                sep = j % 3;
                if (sep == 2)
                {
                    s += "|";
                }
            }
            s += "\n";
        }
        print(s);
    }

    private void ShuffleGrid(ref int[,] grid, int shuffleAmount)
    {
        for (int i = 0; i < shuffleAmount; i++)
        {
            int value1 = Random.Range(1, 10);
            int value2 = Random.Range(1, 10);
            MixTwoGridCells(ref grid, value1, value2);
        }
        DebugGrid(ref grid);
    }

    private void MixTwoGridCells(ref int[,] grid, int value1, int value2)
    {
        int x1 = 0;
        int x2 = 0;

        int y1 = 0;
        int y2 = 0;

        for (int i = 0; i < 9; i+=3)
        {
            for (int k = 0; k < 9; k+=3)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        if(grid[i+j, k+l] == value1)
                        {
                            x1 = i + j;
                            y1 = k + l;
                        }

                        if (grid[i + j, k + l] == value2)
                        {
                            x2 = i + j;
                            y2 = k + l;
                        }
                    }
                }
                grid[x1, y1] = value2;
                grid[x2, y2] = value1;
            }
        }
    }

    private void CreateRiddleGrid()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                riddleGrid[i, j] = solvedGrid[i, j];
            }
        }

        SetDifficulty();
        
        for (int i = 0; i < piecesToErase; i++)
        {
            int x1 = Random.Range(0, 9);
            int y1 = Random.Range(0, 9);

            while(riddleGrid[x1,y1] == 0)
            {
                x1 = Random.Range(0, 9);
                y1 = Random.Range(0, 9);
            }
            riddleGrid[x1, y1] = 0;
        }
        DebugGrid(ref riddleGrid);
    }

    private void CreateButtons()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                GameObject newButton = Instantiate(buttonPrefab);

                NumberField numField = newButton.GetComponent<NumberField>();
                numField.SetValues(i, j, riddleGrid[i, j], i + "," + j, this);
                newButton.name = i + "," + j;

                if(riddleGrid[i,j] == 0)
                {
                    fieldList.Add(numField);
                }

                if(i < 3 && j < 3)
                {
                    newButton.transform.SetParent(A1, false);
                }

                if (i < 3 && j > 2 && j < 6)
                {
                    newButton.transform.SetParent(A2, false);
                }

                if (i < 3 && j > 5)
                {
                    newButton.transform.SetParent(A3, false);
                }

                if (i > 2 && i < 6 && j < 3)
                {
                    newButton.transform.SetParent(B1, false);
                }

                if (i > 2 && i < 6 && j > 2 && j < 6)
                {
                    newButton.transform.SetParent(B2, false);
                }

                if (i > 2 && i < 6 && j > 5)
                {
                    newButton.transform.SetParent(B3, false);
                }

                if (i > 5 && j < 3)
                {
                    newButton.transform.SetParent(C1, false);
                }

                if (i > 5 && j > 2 && j < 6)
                {
                    newButton.transform.SetParent(C2, false);
                }

                if (i > 5 && j > 5)
                {
                    newButton.transform.SetParent(C3, false);
                }
            }
        }
    }

    public void SetInputInRiddleGrid(int x, int y, int value)
    {
        riddleGrid[x, y] = value;
    }

    private void SetDifficulty()
    {
        switch (difficulty)
        {
            case Difficulties.DEBUG:
                piecesToErase = 5;
                maxHints = 2;
                break;

            case Difficulties.EASY:
                piecesToErase = 35;
                maxHints = 5;
                break;

            case Difficulties.MEDIUM:
                piecesToErase = 40;
                maxHints = 7;
                break;

            case Difficulties.HARD:
                piecesToErase = 45;
                maxHints = 10;
                break;

            case Difficulties.INSANE:
                piecesToErase = 55;
                maxHints = 10;
                break;

            default:
                piecesToErase = 35;
                maxHints = 5;
                break;
        }
    }

    public void CheckComplete()
    {
        if (CheckIfWon())
            print("You Won");
        else
            print("Try Again!");
    }

    private bool CheckIfWon()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (riddleGrid[i, j] != solvedGrid[i, j])
                    return false;
            }
        }
        return true;
    }

    public void ShowHint()
    {
        if(fieldList.Count > 0 && maxHints > 0)
        {
            int randIndex = Random.Range(0, fieldList.Count);

            maxHints--;

            riddleGrid[fieldList[randIndex].X, fieldList[randIndex].Y] = solvedGrid[fieldList[randIndex].X, fieldList[randIndex].Y];

            fieldList[randIndex].SetHint(riddleGrid[fieldList[randIndex].X, fieldList[randIndex].Y]);

            fieldList.RemoveAt(randIndex);
        }
        else
        {
            print("No hits left!");
        }
    }




    private bool ColumnContainsNumber(int y, int value, ref int[,] grid)
    {
        for (int x = 0; x < 9; x++)
        {
            if (grid[x, y] == value)
                return true;
        }
        return false;
    }

    private bool RowContainsNumber(int x, int value, ref int[,] grid)
    {
        for (int y = 0; y < 9; y++)
        {
            if (grid[x, y] == value)
                return true;
        }
        return false;
    }

    private bool BlockContainsNumber(int x, int y, int value, ref int[,] grid)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid[x - (x % 3) + i, y - (y % 3) + j] == value)
                    return true;
            }
        }
        return false;
    }

    private bool CheckAll(int x, int y, int value, ref int[,] grid)
    {
        if (ColumnContainsNumber(y, value, ref grid)) return false;

        if (RowContainsNumber(x, value, ref grid)) return false;

        if (BlockContainsNumber(x, y, value, ref grid)) return false;

        return true;
    }

    private bool IsValidGrid(ref int[,] grid)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i, j] == 0) return false;
            }
        }
        return true;
    }

    private void FillGridBase(ref int[,] grid)
    {
        List<int> rowValues = new List<int>() { 1,2,3,4,5,6,7,8,9 };
        List<int> colValues = new List<int>() { 1,2,3,4,5,6,7,8,9 };

        int value = rowValues[Random.Range(0, rowValues.Count)];
        grid[0, 0] = value;
        rowValues.Remove(value);
        colValues.Remove(value);

        for (int i = 1; i < 9; i++)
        {
            value = rowValues[Random.Range(0, rowValues.Count)];
            grid[i, 0] = value;
            rowValues.Remove(value);
        }

        for (int i = 1; i < 9; i++)
        {
            value = colValues[Random.Range(0, colValues.Count)];
            if (i < 3)
            {
                while(BlockContainsNumber(0,0, value, ref grid))
                    value = colValues[Random.Range(0, colValues.Count)];               
            }
            grid[0, i] = value;
            colValues.Remove(value);
        }

        //DebugGrid(ref grid);
    }

    private bool SolvedGrid(ref int[,] grid)
    {
        DebugGrid(ref grid);

        if (IsValidGrid(ref grid)) return true;

        int x = 0;
        int y = 0;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if(grid[i,j] == 0)
                {
                    x = i;
                    y = j;
                    break;
                }
            }
        }

        List<int> possibilities = new List<int>();
        possibilities = GetAllPossiblities(x, y, ref grid);

        for (int p = 0; p < possibilities.Count; p++)
        {
            grid[x, y] = possibilities[p];
            
            if (SolvedGrid(ref grid)) return true;
            
            grid[x, y] = 0;
        }


        return false;
    }

    private List<int> GetAllPossiblities(int x, int y, ref int[,] grid)
    {
        List<int> possibilities = new List<int>();
        for (int val = 1; val <= 9; val++)
        {
            if(CheckAll(x,y,val, ref grid))
            {
                possibilities.Add(val);
            }
        }

        return possibilities;
    }

    private void CreateRiddleGrid(ref int[,] sGrid, ref int[,] rGrid)
    {
        System.Array.Copy(sGrid, rGrid, sGrid.Length);        

        SetDifficulty();

        for (int i = 0; i < piecesToErase; i++)
        {
            int x1 = Random.Range(0, 9);
            int y1 = Random.Range(0, 9);

            while (rGrid[x1, y1] == 0)
            {
                x1 = Random.Range(0, 9);
                y1 = Random.Range(0, 9);
            }
            rGrid[x1, y1] = 0;
        }
        DebugGrid(ref riddleGrid);
    }

    private void AutoSolve(ref int[,] grid)
    {
        if (SolvedGrid(ref grid))
            DebugGrid(ref grid);
        StartCoroutine(PathToSolve());
    }

    private void ClearButtons()
    {
        var buttons = A1.GetComponentsInChildren<NumberField>();
        var buttons2 = A2.GetComponentsInChildren<NumberField>();
        var buttons3 = A3.GetComponentsInChildren<NumberField>();
        var buttons4 = B1.GetComponentsInChildren<NumberField>();
        var buttons5 = B2.GetComponentsInChildren<NumberField>();
        var buttons6 = B3.GetComponentsInChildren<NumberField>();
        var buttons7 = C1.GetComponentsInChildren<NumberField>();
        var buttons8 = C2.GetComponentsInChildren<NumberField>();
        var buttons9 = C3.GetComponentsInChildren<NumberField>();

        foreach (var item in buttons)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in buttons2)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in buttons3)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in buttons4)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in buttons5)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in buttons6)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in buttons7)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in buttons8)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in buttons9)
        {
            Destroy(item.gameObject);
        }
        fieldList.Clear();
    }

    private IEnumerator PathToSolve()
    {
        int watchdog = 100;
        while(solvePath.Count > 0 && watchdog> 0)
        {
            watchdog--;
            riddleGrid = (int[,])solvePath.Dequeue();
            ClearButtons();
            CreateButtons();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
