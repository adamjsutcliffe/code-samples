using System.Collections.Generic;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Game.Gameplay
{
    public delegate void GameStartedHandler(MainGameData gameData);
    public delegate void GamePausedHandler(MainGameData gameData);
    public delegate void GameResumedHandler(MainGameData gameData);
    public delegate void GameRestartedHandler(MainGameData gameData);
    public delegate void PuzzleSolvedHandler(MainGameData gameData);
    public delegate void GameQuitHandler(MainGameData gameData);

    public delegate void TimeChangedHandler(MainGameData gameData);

    public delegate void SelectFTUECells(Vector2 selectedFtueCell, bool addOrRemove);

    public delegate void MarkFTUECells(Vector2 markedFtueCells, bool addOrRemove);
}