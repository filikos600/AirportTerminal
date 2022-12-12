using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Security
{
    int entry_x, entry_z, exit_x, exit_z;
    public Security(int start_x, int start_z, int width, int height)
    {
        Debug.Log("Creating security from (" + start_x + "; " + start_z + ") to (" + (int)(start_x + width) + "; " + (start_z + height) + ");\n");

        for (int x = start_x; x < start_x + width; x++)
        {
            for (int z = start_z; z < start_z + height; z++)
            {
                TheGrid.SetGridCell(x, z, (int)SectorType.Security);
            }
        }
        
        this.entry_x = start_x + 1;
        this.entry_z = start_z;
        this.exit_x = (height % 2 == 1) ? start_x + width - 2 : start_x + 1;
        this.exit_z = start_z + height - 1;

        TheGrid.SetGridCell(entry_x, entry_z, (int)SectorType.SecurityPath);
        TheGrid.SetGridCell(exit_x, exit_z, (int)SectorType.SecurityPath);
    }

    public int Entry_x { get => entry_x;}
    public int Entry_z { get => entry_z;}

    public int Exit_x { get => exit_x; }
    public int Exit_z { get => exit_z; }
}
