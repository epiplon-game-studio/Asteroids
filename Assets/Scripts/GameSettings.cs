using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Asteroids/Settings")]
public class GameSettings : ScriptableObject
{
    public int StartingLives;
    public int AsteroidsPoints;
    public int SmallSaucerPoints;
    public int BigSaucerPoints;
}
