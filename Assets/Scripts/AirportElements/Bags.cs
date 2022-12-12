using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bags
{
    List<BagClaim> bagClaims;
    int start_x, start_z, end_x, end_z;
    int path_mid_x, path_mid_z;
    int entry_x, entry_z;

    public Bags(int start_x, int start_z, int end_x, int end_z)
    {
        this.bagClaims = new List<BagClaim>();
        this.start_x = start_x;
        this.start_z = start_z;
        this.end_x = end_x;
        this.end_z = end_z;

        FillZone();

        StartPath();
        ConnectPaths();

        BaggageClaims();
    }

    public void FillZone()
    {
        for (int x = start_x; x < end_x; x++)
        {
            for (int z = start_z; z < end_z; z++)
            {
                if (TheGrid.GetGridCell(x, z) == (int)SectorType.Arrivals)
                    TheGrid.SetGridCell(x, z, (int)SectorType.Bags);
            }
        }
    }

    public void StartPath()
    {
        for(int x = start_x; x < end_x; x++)
        {
            if (TheGrid.GetGridCell(x, end_z)%2 == 1)
            {
                TheGrid.SetGridCell(x, end_z - 1, (int)SectorType.BagsPath);
                TheGrid.SetGridCell(x, end_z - 2, (int)SectorType.BagsPath);
            }
        }
    }

    public void ConnectPaths()
    {
        int first_x = 0, last_x = 0;
        for (int x = start_x; x < end_x; x++)
        {
            if (TheGrid.GetGridCell(x, end_z - 2) == (int)SectorType.BagsPath)
            {
                first_x = x;
                break;
            }
                
        }

        for (int x = end_x-1; x >= start_x; x--)
        {
            if (TheGrid.GetGridCell(x, end_z - 2) == (int)SectorType.BagsPath)
            {
                last_x = x;
                break;
            }

        }

        for (int x = first_x; x < last_x; x++)
        {
            TheGrid.SetGridCell(x, end_z - 2, (int)SectorType.BagsPath);
            TheGrid.SetGridCell(x, end_z - 3, (int)SectorType.BagsPath);
        }

        PathDown(first_x, last_x);
    }
    

    public void PathDown(int first_x, int last_x)
    {
        int x = first_x + (int)(Random.Range(first_x, last_x - first_x));
        this.path_mid_x = x;
        this.path_mid_z = end_z - 2;
        for (int z = start_z; z < path_mid_z; z++)
        {
            TheGrid.SetGridCell(x, z, (int)SectorType.BagsPath);
            TheGrid.SetGridCell(x - 1, z, (int)SectorType.BagsPath);
            TheGrid.SetGridCell(x + 1, z, (int)SectorType.BagsPath);
        }
        TheGrid.SetGridCell(x - 1, start_z, (int)SectorType.Bags);
        TheGrid.SetGridCell(x + 1, start_z, (int)SectorType.Bags);
    }

    public void BaggageClaims()
    {
        int free_height = path_mid_z - start_z;

        int height = free_height - 4;
        int width = Random.Range(3,4);

        int z = end_z - 3 - height;

        for (int x = path_mid_x - 1; x >= 0; x--)
        {
            if (CheckFreeSize(x,z,width,height, SectorType.Bags))
            {
                bagClaims.Add(new BagClaim(x, z, x + width, z + height));
                x -= width + 1;
            }
        }

        for (int x = path_mid_x + 1; x < end_x; x++)
        {
            if (CheckFreeSize(x, z, width, height, SectorType.Bags))
            {
                bagClaims.Add(new BagClaim(x, z, x + width, z + height));
                x += width + 1;
            }
        }

    }

    public bool CheckFreeSize(int start_x, int start_z, int size_x, int size_z, SectorType type)
    {
        for (int x = start_x; x < start_x + size_x; x++)
        {
            for (int z = start_z; z < start_z + size_z; z++)
            {
                if (TheGrid.GetGridCell(x, z) != (int)type)
                    return false;
            }
        }
        return true;
    }

    public void EmptyZones()
    {
        bool end = false;
        for (int x = start_x + 1; x < path_mid_x; x++)
        {
            for (int z = start_z; z < end_z; z++)
            {
                if (TheGrid.GetGridCell(x, z) != (int)SectorType.Bags)
                    end = true;
            }
            if (end)
                break;
            for (int z = start_z; z < end_z; z++)
            {
                TheGrid.SetGridCell(x - 1, z, (int)SectorType.Empty);      
            }
        }

        end = false;

        for (int x = end_x - 2; x > path_mid_x; x--)
        {
            for (int z = start_z; z < end_z; z++)
            {
                if (TheGrid.GetGridCell(x, z) != (int)SectorType.Bags)
                    end = true;
            }
            if (end)
                break;
            for (int z = start_z; z < end_z; z++)
            {
                TheGrid.SetGridCell(x + 1, z, (int)SectorType.Empty);

            }
        }
    }

    public int get_entry_x()
    {
        return path_mid_x;
    }

    public int get_entry_z()
    {
        return start_z;
    }
}
