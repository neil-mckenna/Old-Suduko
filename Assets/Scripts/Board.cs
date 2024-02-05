using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class Board : MonoBehaviour
{
    #region Variables

    const int magicNine = 9;
    // console string
    string s;

    int[,] solvedGrid = new int[9,9];
    int[,] riddleGrid = new int[9,9];
    int piecesToErase = 35;

    public Transform A1, A2, A3, B1, B2, B3, C1, C2, C3;
    public GameObject buttonPrefab;

    public GameObject winPanel;
    public GameObject losePanel;

    public TMP_Text hintText;

    List<NumberField> fieldList = new List<NumberField>();

    private int maxHints = 0;

    #endregion

    #region Difficulty
    // Difficulty


    public void SetDifficulty()
    {
        switch (Settings.difficulty)
        {
            case Settings.Difficulties.DEBUG:
                piecesToErase = 5;
                maxHints = (magicNine * magicNine) - piecesToErase;
                break;
            case Settings.Difficulties.EASY:
                piecesToErase = 25;
                maxHints = 25;
                break;
            case Settings.Difficulties.MEDIUM:
                piecesToErase = 30;
                maxHints = 20;
                break;
            case Settings.Difficulties.HARD:
                piecesToErase = 35;
                maxHints = 15;
                break;
            case Settings.Difficulties.INSANE:
                piecesToErase = 50;
                maxHints = 10;
                break;

        }

    }

    #endregion

    #region Start
    void Start()
    {
        DeactivatePanels();

        SetDifficulty();
        hintText.text = "Hint! " + maxHints.ToString();

        FillGridBase(ref solvedGrid);
        SolvedGrid(ref solvedGrid);
        CreateRiddleGrid(ref solvedGrid,ref riddleGrid);

        CreateButtons();

        /*SetDifficulty();
        hintText.text = "Hint! " + maxHints.ToString();

        InitGrid(ref solvedGrid);
        //DebugGrid(ref  solvedGrid);

        ShuffleGrid(ref solvedGrid, 10);
        CreateRiddleGrid();

        CreateButtons();*/
    }

    private void DeactivatePanels()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }
    #endregion

    #region Sudoku Algorithm
    void InitGrid(ref int[,] grid)
    {
        // x range
        for (int i = 0; i < magicNine; i++)
        {
            // y range
            for(int j = 0; j < magicNine; j++)
            {
                grid[i, j] = (i * 3 + i / 3 + j) % 9 + 1;

            }
        }

        /*int n1 = 8 * 3; // 24
        int n2 = 8 / 3; // 2
        int n = (n1 + n2 + 0) % 9 + 1;
        print(n1 + "+" + n2 + "+" + 0);
        print(n);*/

    }

    void DebugGrid(ref int[,] grid)
    {
        s = "";
        int sep = 0;

        for (int i = 0; i < magicNine; i++)
        {
            s += "|";
            for (int j = 0; j < magicNine; j++)
            {
                s += grid[i, j].ToString();

                sep = j % 3;
                if(sep == 2)
                {
                    s += "|";
                }
            }

            s += "\n";


        }

        Debug.Log(s);

    }

    void ShuffleGrid(ref int[,] grid, int shuffleAmt)
    {
        for (int i = 0; i < shuffleAmt; i++)
        {
            // 1 - 9
            int value1 = Random.Range(1, 10);
            int value2 = Random.Range(1, 10);

            // MIX 2 CELLS
            MixTwoGridCells(ref grid, value1, value2);

        }
        //DebugGrid(ref grid);

    }

    void MixTwoGridCells(ref int[,] grid, int val1, int val2)
    {
        int x1 = 0;
        int x2 = 0;

        int y1 = 0;
        int y2 = 0;

        for (int i = 0; i < magicNine; i+=3)
        {
            for (int k = 0; k < magicNine; k+=3)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        if (grid[i+j, k+l] == val1)
                        {
                            x1 = i + j;
                            y1 = k + l;
                        }

                        if (grid[i + j, k + l] == val2)
                        {
                            x2 = i + j;
                            y2 = k + l;
                        }


                    }

                }
                grid[x1, y1] = val2;
                grid[x2, y2] = val1;

            }
        }
    }

    // old riddle grid method
    void CreateRiddleGrid()
    {
        // COPY SOLVED GRID
        for (int i = 0; i < magicNine; i++)
        {
            for (int j = 0; j < magicNine; j++)
            {
                // copy solved to riddle
                riddleGrid[i, j] = solvedGrid[i, j];

            }
        }

        // SET DIFFICULTY
        //SetDifficulty();

        // ERASE FROM RIDDLE GRID
        for (int i = 0; i < piecesToErase; i++)
        {
            int x1 = Random.Range(0, 9);
            int y1 = Random.Range(0, 9);

            // REROLL UNTIL WE FIND ONE WITHOUT A ZERO IN BETWWEN
            while (riddleGrid[x1, y1] == 0)
            {
                x1 = Random.Range(0, 9);
                y1 = Random.Range(0, 9);
            }

            // Once we wound one with NO
            riddleGrid[x1, y1] = 0;

        }

        //DebugGrid(ref riddleGrid);
    }

    #endregion

    #region Design & Input

    public void CreateButtons()
    {
        for (int i = 0; i < magicNine; i++)
        {
            for (int j = 0; j < magicNine; j++)
            {
                GameObject newButton = Instantiate(buttonPrefab);

                // SET ALL VALUE
                NumberField numField = newButton.GetComponent<NumberField>();
                numField.SetValues(i, j, riddleGrid[i, j], i + "," + j, this);
                newButton.name = i + "," + j;

                if (riddleGrid[i, j] == 0)
                {
                    fieldList.Add(numField);

                }

                //PARENT THE BUTTON
                //A1
                if(i < 3 && j < 3)
                {
                    newButton.transform.SetParent(A1, false);
                }
                // A2
                else if (i < 3 && j > 2 && j < 6)
                {
                    newButton.transform.SetParent(A2, false);
                }
                // A3
                else if (i < 3 && j > 5 && j < 9)
                {
                    newButton.transform.SetParent(A3, false);
                }
                // B1
                else if (i > 2 && i < 6 && j < 3)
                {
                    newButton.transform.SetParent(B1, false);
                }
                // B2
                else if (i > 2 && i < 6 && j > 2 && j < 6)
                {
                    newButton.transform.SetParent(B2, false);
                }
                // B3
                else if (i > 2 && i < 6 && j > 5 && j < 9)
                {
                    newButton.transform.SetParent(B3, false);
                }
                // C1
                else if (i > 5 && i < 9 && j < 3)
                {
                    newButton.transform.SetParent(C1, false);
                }
                // C2
                else if (i > 5 && i < 9 && j > 2 && j < 6)
                {
                    newButton.transform.SetParent(C2, false);
                }
                // C3
                else if (i > 5 && i < 9 && j > 5 && j < 9)
                {
                    newButton.transform.SetParent(C3, false);
                }


            }

        }
    }

    public void SetInputInRiddle(int x, int y, int value)
    {
        riddleGrid[x, y] = value;

    }

    #endregion

    #region Check

    public void CheckComplete()
    {
        if( CheckIfWon())
        {
            //print("You won!");
            winPanel.SetActive(true);

        }
        else
        {
            //print("Try again!");
            losePanel.SetActive(true);
        }
    }

    bool CheckIfWon()
    {
        for (int i = 0; i < magicNine; i++)
        {
            for (int j = 0; j < magicNine; j++)
            {
                if (riddleGrid[i, j] != solvedGrid[i, j])
                {
                    return false;
                }
            }
        }

        return true;

    }

    #endregion

    #region Hints
    public void ShowHint()
    {
        if(fieldList.Count > 0 && maxHints>0)
        {
            int randIndex = Random.Range(0, fieldList.Count);

            maxHints--;
            hintText.text = "Hint! " + maxHints.ToString();

            riddleGrid[fieldList[randIndex].GetX(), fieldList[randIndex].GetY()]
                = solvedGrid[fieldList[randIndex].GetX(), fieldList[randIndex].GetY()];

            fieldList[randIndex].SetHint(riddleGrid[fieldList[randIndex].GetX(), fieldList[randIndex].GetY()]);
            fieldList.RemoveAt(randIndex);

        }
        else
        {
            print("No Hints left!");
        }

    }

    #endregion

    //--------------------------------------BACK-TRACKING-----------------------------------


    //--------------------------------------ALL-CHECKS--------------------------------------

    //this column contain the number
    bool ColumnContainsNumber(int y, int value, ref int[,] grid)
    {
        for(int x = 0;x < magicNine; x++)
        {
            if (grid[x,y] == value)
            {
                return true;
            }
        }
        return false;
    }

    // this row contains the number
    bool RowContainsNumber(int x, int value, ref int[,] grid)
    {
        for (int y = 0; y < magicNine; y++)
        {
            if (grid[x, y] == value)
            {
                return true;
            }
        }
        return false;
    }

    // this local 3 x 3 block contains the number
    bool BlockContainsNumber(int x, int y, int value, ref int[,] grid)
    {

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if(grid[x-(x%3) + i, y-(y%3) + j] == value)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // all checks
    bool CheckAll(int x, int y, int value, ref int[,] grid)
    {
        if(ColumnContainsNumber(y, value, ref grid)) { return false; }
        if(RowContainsNumber(x, value, ref grid)) { return false; }
        if(BlockContainsNumber(x, y, value, ref grid)) { return false; }

        return true;
    }

    // non 0s
    bool IsValidGrid(ref int[,] grid)
    {
        for (int i = 0; i < magicNine; i++)
        {
            for (int j = 0; j < magicNine; j++)
            {
                if (grid[i, j] == 0)
                {
                    return false;
                }
            }
        }

        return true;
    }



    //--------------------------------------GENERATE-GRID-----------------------------------

    void FillGridBase(ref int[,] grid)
    {
        List<int> rowValues = new List<int>() { 1,2,3,4,5,6,7,8,9 };
        List<int> columnValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        int value = rowValues[Random.Range(0, rowValues.Count)];
        grid[0,0] = value;

        rowValues.Remove(value);
        columnValues.Remove(value);

        // ROW first
        for (int r = 1; r < magicNine; r++)
        {
            value = rowValues[Random.Range(0, rowValues.Count)];
            grid[r, 0] = value;
            rowValues.Remove(value);

        }

        // COLUMNS
        for (int c = 1; c < magicNine; c++)
        {
            value = columnValues[Random.Range(0, columnValues.Count)];
            if(c < 3)
            {
                while (BlockContainsNumber(0,0, value, ref grid))
                {
                    value = columnValues[Random.Range(0, columnValues.Count)];

                }
            }
            grid[0, c] = value;
            columnValues.Remove(value);
        }


    }

    bool SolvedGrid(ref int[,] grid)
    {
        DebugGrid(ref grid);

        if(IsValidGrid(ref grid))
        {
            return true;
        }

        // FIND FIRST FREE CELL
        int x = 0;
        int y = 0;

        for (int i = 0; i < magicNine; i++)
        {
            for (int j = 0; j < magicNine; j++)
            {
                if (grid[i, j] == 0)
                {
                    x = i;
                    y = j;
                    //print(x + " , " + y);
                    break;
                }
            }
        }

        List<int> possibilities = new List<int>();
        possibilities = GetAllPossibilities(x, y, ref grid);

        for (int ps = 0; ps < possibilities.Count; ps++)
        {
            // SET A POSSIBLE VALUE
            grid[x, y] = possibilities[ps];
            // BACKTRACK
            if(SolvedGrid(ref grid))
            {
                return true;
            }

            // reset to 0 as false
            grid[x, y] = 0;


        }



        return false;
    }

    List<int> GetAllPossibilities(int x, int y, ref int[,] grid)
    {
        List<int> possibilities = new List<int>();
        for (int val = 1; val <= magicNine; val++)
        {
            if(CheckAll(x, y, val, ref grid))
            {
                possibilities.Add(val);
            }

        }

        return possibilities;
    }

    //---------------NEW GAME PLAY ------------------------------

    void CreateRiddleGrid(ref int[,] sGrid, ref int[,] rGrid)
    {
        // COPY SOLVED GRID
        System.Array.Copy(sGrid, rGrid, sGrid.Length);

        // SET DIFFICULTY
        //SetDifficulty();

        // ERASE FROM RIDDLE GRID
        for (int i = 0; i < piecesToErase; i++)
        {
            int x1 = Random.Range(0, 9);
            int y1 = Random.Range(0, 9);

            // REROLL UNTIL WE FIND ONE WITHOUT A ZERO IN BETWWEN
            while (rGrid[x1, y1] == 0)
            {
                x1 = Random.Range(0, 9);
                y1 = Random.Range(0, 9);
            }

            // Once we wound one with NO
            rGrid[x1, y1] = 0;

        }

        DebugGrid(ref riddleGrid);
    }



}
