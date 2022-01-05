using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "List/Command")]
public class ListCommand : ScriptableObject
{
    private Dictionary<Player, ICommand> dicoCommands = new Dictionary<Player, ICommand>();
    public Dictionary<Player, ICommand> DicoCommands { get { return dicoCommands; } }

    public void AddCommand(Player p, ICommand c)
    {
        if (dicoCommands.ContainsKey(p))
        {
            dicoCommands[p] = c;
        }
        else
        {
            dicoCommands.Add(p, c);
        }
    }

    public void Clear()
    {
        dicoCommands.Clear();
    }
}
