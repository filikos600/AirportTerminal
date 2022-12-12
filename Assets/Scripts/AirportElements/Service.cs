using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Service
{
    int start_x, start_z, end_x, end_z, entry_x, entry_z;
    Direction entryDirection;
    public Service(int start_x, int start_z, int size_x, int size_z, int entry_x, int entry_z, Direction entryDirection, SectorType EntryType)
    {
        this.start_x = start_x;
        this.start_z = start_z;
        this.end_x = start_x + size_x - 1;
        this.end_z = start_z + size_z - 1;
        this.entry_x = entry_x;
        this.entry_z = entry_z;
        this.entryDirection = entryDirection;

        for (int x = start_x; x < start_x + size_x; x++)
            for (int z = start_z; z < start_z + size_z; z++)
                TheGrid.SetGridCell(x, z, (int)SectorType.Service);

        TheGrid.SetGridCell(entry_x, entry_z, (int)SectorType.ServicePath);

        switch (entryDirection)
        {
            case Direction.North:
                TheGrid.SetGridCell(entry_x, entry_z + 1, (int)EntryType);
                TheGrid.SetGridCell(entry_x, entry_z + 2, (int)EntryType);
                break;
            case Direction.East:
                TheGrid.SetGridCell(entry_x + 1, entry_z, (int)EntryType);
                TheGrid.SetGridCell(entry_x + 2, entry_z, (int)EntryType);
                break;
            case Direction.South:
                TheGrid.SetGridCell(entry_x, entry_z - 1, (int)EntryType);
                TheGrid.SetGridCell(entry_x, entry_z - 2, (int)EntryType);
                break;
            case Direction.West:
                TheGrid.SetGridCell(entry_x - 1, entry_z, (int)EntryType);
                TheGrid.SetGridCell(entry_x - 2, entry_z, (int)EntryType);
                break;
        }

        Debug.Log("Service created from: (" + this.start_x + "; " + this.start_z + "), to (" + this.end_x + "; " + this.end_z + ") with doors (" + this.entry_x + "; " + this.entry_z + ")\n");
    }

    public int Start_x { get => start_x;}
    public int Start_z { get => start_z;}
    public int End_x { get => end_x;}
    public int End_z { get => end_z;}
    public int Entry_x { get => entry_x;}
    public int Entry_z { get => entry_z;}
    public Direction EntryDirection { get => entryDirection; }
}

