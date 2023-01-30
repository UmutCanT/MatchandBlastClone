using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelTemplete", menuName = "Levels/Create New Level")]
public class LevelTemplete : ScriptableObject
{
    [SerializeField] TileTemplate[] tileTemplates;

    [SerializeField][Range(2, 10)] int width;
    [SerializeField][Range(2, 10)] int height;
 
    [SerializeField] int stateA;
    [SerializeField] int stateB;
    [SerializeField] int stateC;

    public TileTemplate[] TileTemplates => tileTemplates;
    public int Width => width;
    public int Height => height;
    public int StateA => stateA;
    public int StateB => stateB;
    public int StateC => stateC;
}
