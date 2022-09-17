using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel
{
    private static GameLevel _gameLevel;
    private static object _obj = new object();
    public static GameLevel Instance()
    {
        if (null == _gameLevel)
        {
            lock (_obj)
            {
                if (null == _gameLevel)
                {
                    _gameLevel = new GameLevel();
                }
            }
        }
        return _gameLevel;
    }




}
