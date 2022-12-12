using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench
{
    int start_x, start_z, end_x, end_z, entry_x, entry_z;
    public Bench(int start_x, int start_z, int size_x, int size_z)
    {
        this.start_x = start_x;
        this.start_z = start_z;
        this.end_x = start_x + size_x - 1;
        this.end_z = start_z + size_z - 1;

        for (int x = start_x; x < end_x; x++)
            for (int z = start_z; z < end_z; z++)
                TheGrid.SetGridCell(x, z, (int)SectorType.Bench);
     }

}

