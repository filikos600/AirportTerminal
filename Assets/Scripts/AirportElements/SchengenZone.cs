using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchengenZone
{
    int start_x, start_z, end_x, end_z;
    int gates_number = ParametersManager.Instance.arrival_gates_schen;

    public SchengenZone(int start_x, int start_z, int end_x, int end_z)
    {
        this.start_x = start_x;
        this.start_z = start_z;
        this.end_x = end_x;
        this.end_z = end_z;

        fillZone();
        createArrivalGates();

        createExitPath();
    }

    public void fillZone()
    {
        for (int x = start_x; x < end_x; x++)
        {
            for (int z = start_z; z < end_z; z++)
            {
                if (TheGrid.GetGridCell(x, z) == (int)SectorType.Arrivals)
                    TheGrid.SetGridCell(x, z, (int)SectorType.Schengen);
            }
        }
    }

    public void createArrivalGates()
    {
        int arrival_gates_step = (end_x - start_x - 2) / gates_number + 1;
        
        while (arrival_gates_step < ParametersManager.Instance.min_gates_distance + 1)
        {
            gates_number--;
            arrival_gates_step = (end_x - start_x - 2) / gates_number + 1;
        }

        int created_gates = 0, x1 = start_x + 1, x2 = end_x - 2;
        while (created_gates < gates_number)
        {
            TheGrid.SetGridCell(x1, end_z - 1, (int)SectorType.SchengenPath);

            created_gates++;
            x1 += arrival_gates_step;
            if (created_gates < gates_number)
            {
                TheGrid.SetGridCell(x2, end_z - 1, (int)SectorType.SchengenPath);
                created_gates++;
                x2 -= arrival_gates_step;
            }
        }
    }

    public void createExitPath()
    {
        int z_path_height = 3;
        int entry_x = start_x + (int)Random.Range((end_x-start_x)*0.3f, (end_x - start_x) * 0.5f);

        for (int x=start_x; x<end_x; x++)
        {
            if (x == entry_x || x - entry_x == 1 || x - entry_x == -1)
            {
                for (int z = start_z; z < end_z - z_path_height; z++)
                {
                    TheGrid.SetGridCell(x, z, (int)SectorType.SchengenPath);
                }
                TheGrid.SetGridCell(x, start_z, (int)SectorType.Schengen);
            }
            else
                for (int z = start_z; z < end_z - z_path_height; z++)
                {
                    TheGrid.SetGridCell(x, z, (int)SectorType.Empty);
                }
        }
        TheGrid.SetGridCell(entry_x, start_z, (int)SectorType.SchengenPath);
    }
}
