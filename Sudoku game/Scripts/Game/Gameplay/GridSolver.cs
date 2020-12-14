using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Game.Gameplay
{

    public class GridSolver : MonoBehaviour
    {
        //enum GridSolutionType
        //{
        //    None = 0,
        //    SingleRow = 1,
        //    SingleColumn = 2,
        //    SingleSquare = 3,
        //    CompleteRow = 4,
        //    CompleteColumn = 5,
        //    CompleteSquare = 6,
        //    OnlyAvailable = 7,
        //    NakedSingle = 8,
        //    SeveralMissingRow = 9,
        //    SeveralMissingColumn = 10,
        //    DoubleDeduction = 11
        //}

        #region - Public API
        public static GridSolutionType SolveGridAtIndex(GridSquareScript[] gridSquares, GridSquareScript selection)
        {
            if (SingleRuleCheck(selection, gridSquares))
            {
                return GridSolutionType.None;
            }
            GridSolutionType check = SimpleCompletionCheck(selection.Index, gridSquares);
            if (check != GridSolutionType.None)
            {
                return check;
            }
            check = OnlyAvailableNumberCheck(selection, gridSquares);
            if(check != GridSolutionType.None)
            {
                return check;
            }
            check = OnlyAvailableSquareHashCheck(selection, gridSquares);
            if (check != GridSolutionType.None)
            {
                return check;
            }
            check = CheckForNakedSingle(selection.Index, gridSquares);
            if (check != GridSolutionType.None)
            {
                return check;
            }
            check = CheckSeveralMissing(selection, gridSquares);
            if (check != GridSolutionType.None)
            {
                return check;
            }
            check = CheckDoubleDeduction(gridSquares, selection);
            if (check != GridSolutionType.None)
            {
                return check;
            }
            return GridSolutionType.None;
        }

        #endregion

        #region - Checks

        /// <summary>
        /// Simple overlap check to duplicate numbers in the same row, column or big square
        /// </summary>
        /// <param name="number"></param>
        /// <param name="index"></param>
        /// <param name="gridSquares"></param>
        /// <returns>True if no duplicate numbers for the selected index</returns>
        private static bool SingleRuleCheck(GridSquareScript selection, GridSquareScript[] gridSquares)
        {
            int row = GridMaths.RowForSquare(selection.Index);
            int col = GridMaths.ColumnForSquare(selection.Index);
            int bigSquare = GridMaths.BigSquareForSquare(selection.Index);
            //print($"Grid - keyboard number: {number} on square: {index}");
            bool existsInRow = DoesRowContainNumber(row, selection, gridSquares);
            bool existsInCol = DoesColumnContainNumber(col, selection, gridSquares);
            bool existsInBig = DoesBigSquareContainNumber(bigSquare, selection, gridSquares);
            bool failed = existsInRow || existsInCol || existsInBig;
            if (!existsInRow
                && !existsInCol
                && !existsInBig)
            {
                //TODO add check for only number 
            }
            print($"[CHECK] Simple overlap test: row: {existsInRow} col: {existsInCol} big: {existsInBig} => {failed}");
            return failed;
        }

        private static GridSolutionType OnlyAvailableNumberCheck(GridSquareScript selection, GridSquareScript[] gridSquares)
        {
            int row = GridMaths.RowForSquare(selection.Index);
            int col = GridMaths.ColumnForSquare(selection.Index);
            int bigSquare = GridMaths.BigSquareForSquare(selection.Index);

            int[] rowMissing = MissingNumbersInRow(gridSquares, row);
            int[] colMissing = MissingNumbersInColumn(gridSquares, col);
            int[] bigMissing = MissingNumbersInBigSquare(gridSquares, bigSquare);

            bool overlapCheck = rowMissing.Intersect(colMissing).Intersect(bigMissing).ToArray().Length == 0;

            print($"Only available row: {string.Join(",", rowMissing)} col; {string.Join(",", colMissing)} big; {string.Join(",", bigMissing)} ");

            return overlapCheck ? GridSolutionType.SingleRule : GridSolutionType.None;
        }

        /// <summary>
        /// Simple completion check to see if hte row, column or square has all nine numbers in them.
        /// WARNING Doesn't check for duplicates!!
        /// </summary>
        /// <param name="index"></param>
        /// <param name="gridSquares"></param>
        /// <returns>True if any of the checks are true</returns>
        private static GridSolutionType SimpleCompletionCheck(int index, GridSquareScript[] gridSquares) //TODO can be simplified
        {
            if (RowCompleteCheck(gridSquares, GridMaths.RowForSquare(index)))
            {
                print("[CHECK] Simple completion test - Row complete");
                return GridSolutionType.CompleteRule;
            }
            else if (ColumnCompleteCheck(gridSquares, GridMaths.ColumnForSquare(index)))
            {
                print("[CHECK] Simple completion test - Column complete");
                return GridSolutionType.CompleteRule;
            }
            else if (BigSquareCompleteCheck(gridSquares, GridMaths.BigSquareForSquare(index)))
            {
                print("[CHECK] Simple completion test - Big Square complete");
                return GridSolutionType.CompleteRule;
            }
            return GridSolutionType.None;
        }

        /// <summary>
        /// Check for if number is present in other columns or rows to rule out other positions so only one available position
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="gridSquares"></param>
        /// <returns>True if this index is only available square for number in relation to number in cols/rows outside of big square </returns>
        private static GridSolutionType OnlyAvailableSquareHashCheck(GridSquareScript selection, GridSquareScript[] gridSquares)
        {
            return GridEmptyIntersectIndices(gridSquares, selection).Length == 0 ? GridSolutionType.OnlyAvailable : GridSolutionType.None;
        }

        /// <summary>
        /// Naked single check, if all numbers except one are accounted for in the square for index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="gridSquares"></param>
        /// <returns>True if all numbers are accounted for with selection entry</returns>
        private static GridSolutionType CheckForNakedSingle(int index, GridSquareScript[] gridSquares)
        {
            int[] rowExisting = ExistingNumbersInRow(gridSquares, GridMaths.RowForSquare(index)).ToArray();
            int[] colExisting = ExistingNumbersInColumn(gridSquares, GridMaths.ColumnForSquare(index)).ToArray();
            int[] bigExisting = ExistingNumbersInBigSquare(gridSquares, GridMaths.BigSquareForSquare(index)).ToArray();
            int[] allExisting = rowExisting.Concat(colExisting).Concat(bigExisting).Distinct().ToArray(); //.Where(x => x != number)

            bool countCheck = allExisting.Length == GridMaths.gridSize;
            print($"[CHECK] Naked single check - count: {countCheck}");
            return countCheck ? GridSolutionType.NakedSingle : GridSolutionType.None; //allExisting.Length == GridMaths.gridSize;
        }

        /// <summary>
        /// Check, like the hash, but for numbers in boxes and cols/rows
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="gridSquares"></param>
        /// <returns>True if this index is only available square for number in relation to number in big squares/cols/rows</returns>
        private static GridSolutionType CheckSeveralMissing(GridSquareScript selection, GridSquareScript[] gridSquares)
        {
            bool missingRow = CheckSeveralMissingRow(selection, gridSquares);
            bool missingCol = CheckSeveralMissingColumn(selection, gridSquares);
            print($"[CHECK] missing passed row: {missingRow} col: {missingCol}");
            return (missingRow || missingCol) ? GridSolutionType.SeveralMissing : GridSolutionType.None;
        }

        private static bool CheckSeveralMissingRow(GridSquareScript selection, GridSquareScript[] gridSquares)
        {
            int row = GridMaths.RowForSquare(selection.Index);
            GridSquareScript[] emptyRowSquares = EmptyGridsAtIndexes(GridMaths.GridRowIndices(row), gridSquares, selection);
            int[] emptyIndices = emptyRowSquares.Select(x => x.Index).Distinct().ToArray();
            int[] bigSquareIndices = emptyIndices.Select(x => GridMaths.BigSquareForSquare(x)).Distinct().ToArray();
            List<int> usableSquares = new List<int>();
            for (int i = 0; i < bigSquareIndices.Length; i++)
            {
                int box = bigSquareIndices[i];
                if (!DoesBigSquareContainNumber(box, selection, gridSquares))
                {
                    int[] checks = emptyIndices.Intersect(GridMaths.GridRowInSquare(row, box)).ToArray();
                    if (checks.Length > 0)
                    {
                        usableSquares.AddRange(checks);
                    }
                }
            }
            if (usableSquares.Count > 0)
            {
                return usableSquares.All(x => DoesColumnContainNumber(GridMaths.ColumnForSquare(x), selection, gridSquares));
            }
            return true;
        }

        private static bool CheckSeveralMissingColumn(GridSquareScript selection, GridSquareScript[] gridSquares)
        {
            int col = GridMaths.ColumnForSquare(selection.Index);
            GridSquareScript[] emptyColSquares = EmptyGridsAtIndexes(GridMaths.GridColumnIndices(col), gridSquares, selection);
            int[] emptyIndices = emptyColSquares.Select(x => x.Index).Distinct().ToArray();
            int[] bigSquareIndices = emptyColSquares.Select(x => GridMaths.BigSquareForSquare(x.Index)).Distinct().ToArray();

            List<int> usableSquares = new List<int>();
            for (int i = 0; i < bigSquareIndices.Length; i++)
            {
                int box = bigSquareIndices[i];
                if (!DoesBigSquareContainNumber(box, selection, gridSquares))
                {
                    int[] checks = emptyIndices.Intersect(GridMaths.GridColumnInSquare(col, box)).ToArray();
                    if (checks.Length > 0)
                    {
                        usableSquares.AddRange(checks);
                    }
                }
            }
            if (usableSquares.Count > 0)
            {
                return usableSquares.All(x => DoesRowContainNumber(GridMaths.RowForSquare(x), selection, gridSquares));
            }
            return true;
        }

        private static GridSolutionType CheckDoubleDeduction(GridSquareScript[] gridSquares, GridSquareScript selection)
        {
            int[] intersectIndices = GridEmptyIntersectIndices(gridSquares, selection);
            int[] missingRows = intersectIndices.Select(GridMaths.RowForSquare).Where(x => x != GridMaths.RowForSquare(selection.Index)).Distinct().ToArray();
            int[] missingCols = intersectIndices.Select(GridMaths.ColumnForSquare).Where(x => x != GridMaths.ColumnForSquare(selection.Index)).Distinct().ToArray();
            print($"Intersect indices: {string.Join(", ", intersectIndices)} Missing rows: {string.Join(", ", missingRows)} cols: {string.Join(", ", missingCols)}");

            int forceRowCount = 0;
            int big = GridMaths.BigSquareForSquare(selection.Index);

            for (int i = 0; i < missingRows.Length; i++)
            {   
                int[] otherBigRows = GridMaths.OtherBigSquaresForRow(missingRows[i], big);
                for (int j = 0; j < otherBigRows.Length; j++)
                {
                    if (DoesSquareForceRowNumber(otherBigRows[j], selection, missingRows[i], gridSquares))
                    {
                        forceRowCount += 1;
                    }
                }
            }

            int forceColCount = 0;
            for (int i = 0; i < missingCols.Length; i++)
            {
                int[] otherBigCols = GridMaths.OtherBigSquaresForColumn(missingCols[i], big);
                for (int j = 0; j < otherBigCols.Length; j++)
                {
                    if (DoesSquareForceColumnNumber(otherBigCols[j], selection, missingCols[i], gridSquares))
                    {
                        forceColCount += 1;
                    }
                }
            }
            print($"[CHECK] Double deduction: Forced rows: {missingRows.Length} = {forceRowCount} && cols: {missingCols.Length} = {forceColCount} check: {forceColCount == missingCols.Length && forceRowCount == missingRows.Length}");
            return (forceColCount == missingCols.Length && forceRowCount == missingRows.Length) ? GridSolutionType.DoubleDeduction : GridSolutionType.None;
        }

        #endregion

        #region - Helper methods

        private static int[] GridEmptyIntersectIndices(GridSquareScript[] gridSquares, GridSquareScript selection)
        {
            int square = GridMaths.BigSquareForSquare(selection.Index);
            GridSquareScript[] emptySquares = EmptyGridsAtIndexes(GridMaths.GridBigSquareIndices(square), gridSquares, selection);
            int[] emptyIndices = EmptyGridsAtIndexes(GridMaths.GridBigSquareIndices(square), gridSquares, selection).Select(x => x.Index).Distinct().ToArray();

            //check how many row are in the empty indicies
            int[] emptyCols = emptyIndices.Where(x => !DoesColumnContainNumber(GridMaths.ColumnForSquare(x), selection, gridSquares)).ToArray();
            int[] emptyRows = emptyIndices.Where(x => !DoesRowContainNumber(GridMaths.RowForSquare(x), selection, gridSquares)).ToArray();
            int[] intersectIndices = emptyCols.Intersect(emptyRows).ToArray();
            print($"GridEmptyIntersectIndices: ({string.Join(",", emptyIndices)}) empty rows: {string.Join(",", emptyRows)} cols: {string.Join(",", emptyCols)} intersect: {string.Join(",", intersectIndices)}");
            return emptyCols.Intersect(emptyRows).ToArray();
        }

        private static bool DoesRowContainNumber(int row, GridSquareScript selection, GridSquareScript[] gridSquares)
        {
            GridSquareScript[] rowSquares = GridsAtIndexes(GridMaths.GridRowIndices(row), gridSquares);
            return rowSquares.Any(x => x.Number == selection.Number && !x.IsTarget);//&& x.Index != selection.Index);
        }

        private static bool DoesColumnContainNumber(int col, GridSquareScript selection, GridSquareScript[] gridSquares)
        {
            GridSquareScript[] colSquares = GridsAtIndexes(GridMaths.GridColumnIndices(col), gridSquares);
            return colSquares.Any(x => x.Number == selection.Number && !x.IsTarget);//x.Index != selection.Index);
        }

        private static bool DoesBigSquareContainNumber(int big, GridSquareScript selection, GridSquareScript[] gridSquares)
        {
            GridSquareScript[] bigSquareSquares = GridsAtIndexes(GridMaths.GridBigSquareIndices(big), gridSquares);
            return bigSquareSquares.Any(x => x.Number == selection.Number && !x.IsTarget); //x.Index != selection.Index);
        }

        private static bool DoesRowInSquareHaveSpace(int row, int square, int index, GridSquareScript[] gridSquares)
        {
            GridSquareScript[] rowInSquareSquares = GridsAtIndexes(GridMaths.GridRowInSquare(row, square), gridSquares);
            return rowInSquareSquares.Any(x => x.Number == 0 && !x.IsTarget); //x.Index != index);
        }

        private static bool DoesColumnInSquareHaveSpace(int col, int square, int index, GridSquareScript[] gridSquares)
        {
            GridSquareScript[] colInSquareSquares = GridsAtIndexes(GridMaths.GridColumnInSquare(col, square), gridSquares);
            return colInSquareSquares.Any(x => x.Number == 0 && !x.IsTarget); //x.Index != index);
        }

        private static bool RowCompleteCheck(GridSquareScript[] gridSquares, int row)
        {
            int[] rowSquares = GridMaths.GridRowIndices(row);
            int[] numbers = GridsAtIndexes(rowSquares, gridSquares).Where(x => x.Number > 0).Select(x => x.Number).Distinct().ToArray();
            print($"Row check - Numbers count: {numbers.Length} => {string.Join(",", numbers)}");
            return numbers.Length == 9;
        }

        private static bool ColumnCompleteCheck(GridSquareScript[] gridSquares, int col)
        {
            int[] colSquares = GridMaths.GridColumnIndices(col);
            int[] numbers = GridsAtIndexes(colSquares, gridSquares).Where(x => x.Number > 0).Select(x => x.Number).Distinct().ToArray();
            print($"Column check - Numbers count: {numbers.Length} => {string.Join(",", numbers)}");
            return numbers.Length == 9;
        }

        private static bool BigSquareCompleteCheck(GridSquareScript[] gridSquares, int square)
        {
            int[] bigSquares = GridMaths.GridBigSquareIndices(square);
            int[] numbers = GridsAtIndexes(bigSquares, gridSquares).Where(x => x.Number > 0).Select(x => x.Number).Distinct().ToArray();
            print($"Big square check - Numbers count: {numbers.Length} => {string.Join(",", numbers)}");
            return numbers.Length == 9;
        }

        ////TODO - NEEDED?
        private static int[] MissingNumbersInRow(GridSquareScript[] gridSquares, int row)
        {
            int[] allNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            GridSquareScript[] rowData = GridsAtIndexes(GridMaths.GridRowIndices(row), gridSquares);
            int[] remaining = allNumbers.Except(rowData.Select(s => s.Number).Where(x => x != 0)).ToArray();
            print($"ROW remaining numbers: {string.Join(",", remaining)}");
            return remaining;
        }

        //TODO - NEEDED?
        private static int[] MissingNumbersInColumn(GridSquareScript[] gridSquares, int col)
        {
            int[] allNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            GridSquareScript[] colData = GridsAtIndexes(GridMaths.GridColumnIndices(col), gridSquares);
            int[] remaining = allNumbers.Except(colData.Select(s => s.Number).Where(x => x != 0)).ToArray();
            print($"COL remaining numbers: {string.Join(",", remaining)}");
            return remaining;
        }

        //TODO - NEEDED?
        private static int[] MissingNumbersInBigSquare(GridSquareScript[] gridSquares, int big)
        {
            int[] allNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            GridSquareScript[] bigSquareData = GridsAtIndexes(GridMaths.GridBigSquareIndices(big), gridSquares);
            int[] remaining = allNumbers.Except(bigSquareData.Select(s => s.Number).Where(x => x != 0)).ToArray();
            print($"BIG SQUARE remaining numbers: {string.Join(",", remaining)}");
            return remaining;
        }

        private static bool DoesSquareForceRowNumber(int big, GridSquareScript selection, int row, GridSquareScript[] gridSquares)
        {
            print($"DoesSquare {big} ForceRow {row} Number: {selection.Number}");
            bool numberCheck = DoesBigSquareContainNumber(big, selection, gridSquares);
            if (DoesBigSquareContainNumber(big, selection, gridSquares))
            {
                return false;
            }
            int[] emptyIndices = EmptyGridsAtIndexes(GridMaths.GridBigSquareIndices(big), gridSquares, selection).Select(x => x.Index).Distinct().ToArray();
            int[] noColIndices = emptyIndices.Where(x => !DoesColumnContainNumber(GridMaths.ColumnForSquare(x), selection, gridSquares)).ToArray();
            int[] noRowIndices = emptyIndices.Where(x => !DoesRowContainNumber(GridMaths.RowForSquare(x), selection, gridSquares)).ToArray();
            int[] forcedRows = noColIndices.Intersect(noRowIndices).Select(GridMaths.RowForSquare).Distinct().ToArray();

            print($"Empty indices in square {big} = {string.Join(",", emptyIndices)}- no col indices ({string.Join(",", noColIndices)})  no row indices ({string.Join(",", noRowIndices)}) =  {string.Join(",", forcedRows)} = {row} ");

            return forcedRows.Length == 1 && forcedRows[0] == row;

        }

        private static bool DoesSquareForceColumnNumber(int big, GridSquareScript selection, int col, GridSquareScript[] gridSquares)
        {
            print($"DoesSquare {big} ForceColumn {col} Number: {selection.Number}");
            bool numberCheck = DoesBigSquareContainNumber(big, selection, gridSquares);
            if (DoesBigSquareContainNumber(big, selection, gridSquares))
            {
                return false;
            }
            int[] emptyIndices = EmptyGridsAtIndexes(GridMaths.GridBigSquareIndices(big), gridSquares, selection).Select(x => x.Index).Distinct().ToArray();
            int[] noColIndices = emptyIndices.Where(x => !DoesColumnContainNumber(GridMaths.ColumnForSquare(x),selection, gridSquares)).ToArray();
            int[] noRowIndices = emptyIndices.Where(x => !DoesRowContainNumber(GridMaths.RowForSquare(x), selection, gridSquares)).ToArray();
            int[] forcedColumns = noColIndices.Intersect(noRowIndices).Select(GridMaths.ColumnForSquare).Distinct().ToArray();

            print($"Empty indices in square {big} = {string.Join(",", emptyIndices)} - no col indices ({string.Join(",", noColIndices)})  no row indices ({string.Join(",", noRowIndices)}) = {string.Join(",", forcedColumns)} =  {col}");
            return forcedColumns.Length == 1 && forcedColumns[0] == col;
        }

        private static int[] ExistingNumbersInRow(GridSquareScript[] gridSquares, int row)
        {
            int[] allNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            GridSquareScript[] rowData = GridsAtIndexes(GridMaths.GridRowIndices(row), gridSquares);
            int[] remaining = allNumbers.Intersect(rowData.Where(x => x.Number != 0 && !x.IsTarget).Select(s => s.Number)).ToArray();
            print($"ROW {row} existing numbers: {string.Join(",", remaining)}");
            return remaining;
        }

        private static int[] ExistingNumbersInColumn(GridSquareScript[] gridSquares, int col)
        {
            int[] allNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            GridSquareScript[] colData = GridsAtIndexes(GridMaths.GridColumnIndices(col), gridSquares);
            int[] remaining = allNumbers.Intersect(colData.Where(x => x.Number != 0 && !x.IsTarget).Select(s => s.Number)).ToArray();
            print($"COL {col} existing numbers: {string.Join(",", remaining)}");
            return remaining;
        }

        private static int[] ExistingNumbersInBigSquare(GridSquareScript[] gridSquares, int big)
        {
            int[] allNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            GridSquareScript[] bigSquareData = GridsAtIndexes(GridMaths.GridBigSquareIndices(big), gridSquares);
            int[] remaining = allNumbers.Intersect(bigSquareData.Where(x => x.Number != 0 && !x.IsTarget).Select(s => s.Number)).ToArray();
            print($"BIG SQUARE {big} existing numbers: {string.Join(",", remaining)}");
            return remaining;
        }

        private static GridSquareScript[] GridsAtIndexes(int[] indexes, GridSquareScript[] gridSquares)
        {
            GridSquareScript[] grids = new GridSquareScript[indexes.Length];
            for (int i = 0; i < indexes.Length; i++)
            {
                if (gridSquares.Length > i && gridSquares[i] != null)
                {
                    grids[i] = gridSquares[indexes[i]];
                }
            }
            return grids;
        }

        private static GridSquareScript[] EmptyGridsAtIndexes(int[] indexes, GridSquareScript[] gridSquares, GridSquareScript selection)
        {
            GridSquareScript[] grids = new GridSquareScript[indexes.Length];
            for (int i = 0; i < indexes.Length; i++)
            {
                if (gridSquares.Length > i && gridSquares[i] != null)
                {
                    grids[i] = gridSquares[indexes[i]];
                }
            }
            return grids.Where(x => x.Number == 0 || !selection.IsTarget).ToArray();
        }

        /// <summary>
        /// Pass in the index for a row or column
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The other row or columns in the square</returns>
        //private static int[] OtherIndicesInSquare(int index)
        //{
        //    int[] indices = GridMaths.GridRowInSquare(0, index / 3);
        //    return indices.Where(x => x != index).ToArray();
        //}

        #endregion
    }
}
