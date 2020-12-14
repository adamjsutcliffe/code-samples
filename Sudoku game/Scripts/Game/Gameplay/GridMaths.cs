using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Game.Gameplay
{
    public static class GridMaths
    {
        public static int gridSize = 9;
        public static int smallGrid = 3;
        public static int otherBigSquares = 2;
        public static int bigGridTotalRow = 27;

        private static int[] topLineIndices = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 27, 28, 29, 30, 31, 32, 33, 34, 35, 54, 55, 56, 57, 58, 59, 60, 61, 62 };
        private static int[] bottomLineIndices = { 18, 19, 20, 21, 22, 23, 24, 25, 26, 45, 46, 47, 48, 49, 50, 51, 52, 53, 72, 73, 74, 75, 76, 77, 78, 79, 80 };
        private static int[] leftLineIndices = { 0, 9, 18, 27, 36, 45, 54, 63, 72, 3, 12, 21, 30, 39, 48, 57, 66, 75, 6, 15, 24, 33, 42, 51, 60, 69, 78 };
        private static int[] rightLineIndices = { 2, 11, 20, 29, 38, 47, 56, 65, 74, 5, 14, 23, 32, 41, 50, 59, 68, 77, 8, 17, 26, 35, 44, 53, 62, 71, 80 };

        public static int RowForSquare(int index)
        {
            return index / gridSize;
        }

        public static int ColumnForSquare(int index)
        {
            return index % gridSize;
        }

        public static int BigSquareForSquare(int index)
        {
            int rowDiv = RowForSquare(index) / smallGrid;
            int colDiv = ColumnForSquare(index) / smallGrid;

            return (rowDiv * smallGrid) + colDiv;
        }

        public static int[] GridRowIndices(int row)
        {
            int multiplier = row * gridSize;
            int[] rowIndices = new int[gridSize];
            for (int i = 0; i < rowIndices.Length; i++)
            {
                rowIndices[i] = i + multiplier;
            }
            return rowIndices;
        }

        public static int[] GridColumnIndices(int column)
        {
            int[] columnIndices = new int[gridSize];
            for (int i = 0; i < columnIndices.Length; i++)
            {
                columnIndices[i] = column + (i * gridSize);
            }
            return columnIndices;
        }

        public static int[] GridBigSquareIndices(int square)
        {
            int divisor = square / smallGrid;
            int remainder = square % smallGrid;

            int startIndex = (bigGridTotalRow * divisor) + (remainder * smallGrid);
            int[] squareIndices = new int[gridSize];

            for (int i = 0; i < squareIndices.Length; i += smallGrid)
            {
                squareIndices[i] = startIndex + (smallGrid * i);
                squareIndices[i + 1] = startIndex + (smallGrid * i) + 1;
                squareIndices[i + 2] = startIndex + (smallGrid * i) + 2;
            }
            return squareIndices;
        }

        //private int[] QuickRowIndices(int row)
        //{
        //    int addition = row * 9;
        //    return new int[] { 0 + addition, 1 + addition, 2 + addition, 3 + addition, 4 + addition, 5 + addition, 6 + addition, 7 + addition, 8 + addition };
        //}

        public static int[] GridRowInSquare(int row, int square)
        {
            //Validity check, is row in square???
            int multiplier = square % smallGrid;
            int rowMultiplier = square / smallGrid;
            int rowCheck = row / smallGrid;
            bool valid = rowMultiplier == rowCheck;
            //Debug.Log($"row in square check: {row} in {square} multiplier: {multiplier} rowMultiplier: {rowMultiplier} row check: {rowCheck} valid: {valid}");
            if (!valid)
            {
                Debug.LogError($"GridRowInSquare - Row: {row} in Square: {square} NOT VALID");
            }
            int[] indices = new int[smallGrid];
            int startPoint = row * gridSize;

            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = (i + startPoint) + (multiplier * smallGrid);
            }
            //Debug.Log($"Grid row ({row}) in square ({square}) = {string.Join(",", indices)}");
            return indices;
        }

        public static int[] GridColumnInSquare(int col, int square)
        {
            //Validity check, is row in square???
            int multiplier = square % smallGrid;
            int divisor = square / smallGrid;
            int colCheck = col / smallGrid;
            int colStart = col % smallGrid;
            bool valid = multiplier == colCheck;
            //Debug.Log($"Column in square check: {col} in {square} multiplier: {multiplier} divoisor; {divisor} col check: {colCheck} col start: {colStart} valid: {valid}");
            if (!valid)
            {
                Debug.LogError($"GridColumnInSquare - Column: {col} in Square: {square} NOT VALID");
            }
            int[] indices = new int[smallGrid];
            int startPoint = (bigGridTotalRow * divisor) + (multiplier * smallGrid);

            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = startPoint + (i * gridSize) + colStart;
            }
            //Debug.Log($"Grid col ({col}) in square ({square}) = {string.Join(",", indices)}");
            return indices;
        }

        public static int[] OtherBigSquaresForRow(int row, int big)
        {
            int[] indices = GridRowIndices(row).Select(x => BigSquareForSquare(x)).Where(x => x != big).Distinct().ToArray();
            return indices;
        }

        public static int[] OtherBigSquaresForColumn(int col, int big)
        {
            int[] indices = GridColumnIndices(col).Select(x => BigSquareForSquare(x)).Where(x => x != big).Distinct().ToArray();
            return indices;
        }

        public static bool ShouldHighlightTopLine(int index)
        {
            //rows 0,3,6
            return topLineIndices.Contains(index);
        }

        public static bool ShouldHighlightBottomLine(int index)
        {
            //rows 2,5,8
            return bottomLineIndices.Contains(index);
        }

        public static bool ShouldHighlightLeftLine(int index)
        {
            //cols 0,3,6
            return leftLineIndices.Contains(index);
        }

        public static bool ShouldHighlightRightLine(int index)
        {
            //cols 2,5,8
            return rightLineIndices.Contains(index);
        }

        public static string RotateGridNinety(string original)
        {
            char[] originalArray = original.ToCharArray();
            char[] newProblem = new char[originalArray.Length];
            for (int i = 0; i < originalArray.Length; i++)
            {
                int newIndex = RotateIndexByNinety(i);
                newProblem[newIndex] = originalArray[i];
            }
            return string.Join("",newProblem);
        }

        public static int[] RotateTargetsNinety(int[] originals)
        {
            int[] newTargets = new int[originals.Length];
            for (int i = 0; i < originals.Length; i++)
            {
                newTargets[i] = RotateIndexByNinety(originals[i]);
            }
            return newTargets;
        }

        private static int RotateIndexByNinety(int original)
        {
            int x = (original % 9) - 4;
            int y = (original / 9) + 4;
            float xCos = x * 0;
            float yCos = y * 0;
            float xSin = x * 1;
            float ySin = y * 1;
            int newX = (int)(xCos - ySin) + 4;
            int newY = (int)(xSin + yCos) - 4;
            int newIndex = Mathf.Abs((newY * 9) + newX);
            Debug.Log($"OG: {original} old: {x},{y} => new: {newX},{newY} newIndex: {newIndex}");
            return newIndex;
        }

        public static string MirrorGrid(string original)
        {
            char[] originalArray = original.ToCharArray();
            char[] newProblem = new char[originalArray.Length];
            for (int i = 0; i < originalArray.Length; i++)
            {
                int newIndex = MirrorIndex(i);
                newProblem[newIndex] = originalArray[i];
            }
            return string.Join("", newProblem);
        }

        public static int[] MirrorTargets(int[] originals)
        {
            int[] newTargets = new int[originals.Length];
            for (int i = 0; i < originals.Length; i++)
            {
                newTargets[i] = MirrorIndex(originals[i]);
            }
            return newTargets;
        }

        private static int MirrorIndex(int original)
        {
            int x = (original % 9) - 4;
            int y = (original / 9) + 4;
            int newX = (x * -1) + 4;
            int newY = y - 4;
            int newIndex = Mathf.Abs((newY * 9) + newX);
            return newIndex;
        }
    }
}
