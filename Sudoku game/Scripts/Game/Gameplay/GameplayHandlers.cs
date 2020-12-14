using UnityEngine;

namespace Peak.Speedoku.Scripts.Game.Gameplay
{
    public delegate void GameStartedHandler(MainGameData gameData);
    public delegate void GamePausedHandler(MainGameData gameData);
    public delegate void GameResumedHandler(MainGameData gameData);
    public delegate void GameOverHandler(MainGameData gameData);
    public delegate void GameQuitHandler(MainGameData gameData);
    public delegate void GameExitHandler(MainGameData gameData);

    //public delegate void KeyboardPressedHandler(KeyButtonScript sender);
    public delegate void TimeChangedHandler(int time);
}