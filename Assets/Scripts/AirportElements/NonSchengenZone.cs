using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonSchengenZone
{
    List<Security> securities;
    int start_x, start_z, end_x, end_z;
    int arrival_gates_number = ParametersManager.Instance.arrival_gates_non_schen;

    public NonSchengenZone(int start_x, int start_z, int end_x, int end_z)
    {
        this.securities = new List<Security>();
        this.start_x = start_x;
        this.start_z = start_z;
        this.end_x = end_x;
        this.end_z = end_z;

        fillZone();

        createArrivalGates();

        CreateSecurities();

    }

    public void fillZone()
    {
        for (int x = start_x; x < end_x; x++)
        {
            for (int z = start_z; z < end_z; z++)
            {
                if (TheGrid.GetGridCell(x, z) == (int)SectorType.Arrivals)
                    TheGrid.SetGridCell(x, z, (int)SectorType.NonSchengen);
            }
        }
    }

    public void createArrivalGates()
    {
        int arrival_gates_step = (end_x - start_x - 2) / arrival_gates_number + 1;

        while (arrival_gates_step < ParametersManager.Instance.min_gates_distance + 1)
        {
            arrival_gates_number--;
            arrival_gates_step = (end_x - start_x - 2) / arrival_gates_number + 1;
        }

        int created_gates = 0, x1 = start_x + 1, x2 = end_x - 2;
        while (created_gates < arrival_gates_number)
        {
            TheGrid.SetGridCell(x1, end_z - 1, (int)SectorType.NonSchengenPath);

            created_gates++;
            x1 += arrival_gates_step;
            if (created_gates < arrival_gates_number)
            {
                TheGrid.SetGridCell(x2, end_z - 1, (int)SectorType.NonSchengenPath);
                created_gates++;
                x2 -= arrival_gates_step;
            }
        }
    }

    public void CreateSecurities()
    {
        int security_height = 3;
        int security_width = 3;

        int x = start_x;
        while ((end_x - x) % 3 != 0)
        {
            TheGrid.SetGridCell(x, start_z, (int)SectorType.Empty);
            TheGrid.SetGridCell(x, start_z + 1, (int)SectorType.Empty);
            TheGrid.SetGridCell(x, start_z + 2, (int)SectorType.Empty);
            x++;
        }
        while (x < end_x)
        {
            securities.Add(new Security(x, start_z, security_width, security_height));
            x += 3;
        }
    }
}
