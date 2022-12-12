using System;
using System.Collections.Generic;
using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    public Transform[] Floors;
    public Transform[] Walls;
    public Transform Folder;

    public void RenderGrid()
    {
        for (int x = 0; x < TheGrid.Width; x++)
        {
            for (int z = 0; z < TheGrid.Height; z++)
            {
                RenderCell(x, z, (SectorType)TheGrid.GetGridCell(x, z));
            }
        }
    }

    public void RenderCell(int x, int z, SectorType type)
    {
        Transform cellFolder = new GameObject("cellFolder").transform;
        cellFolder.SetParent(Folder);

        try
        {
            int i = GetFloorType(type);
            float y = 0f;
            if (i == 26)
            {
                y = 4f;
            }
            else if (i%2 == 1) { i--; }
            Transform floor = Instantiate(Floors[i], Folder.TransformPoint(new Vector3(4f * x, y, 4f * z)), Quaternion.identity);
            floor.SetParent(cellFolder);
        }catch(Exception e)
        {
            Debug.LogException(e, this);
            Debug.LogError("x: " + x + " z: " + z + " type: " +  type + " type_int: " + GetFloorType(type));
        }
        
       
        if(z == TheGrid.Width - 1)
        {
            Transform wallNorth = Instantiate(Walls[(int)type % 2],
                Folder.TransformPoint(new Vector3(4f * x, 2f, 4f * z + 2f)), Quaternion.identity);
            wallNorth.SetParent(cellFolder);
        }
        else if (skip_north(x, z))
        {
            ;
        }
        else if ((Mathf.Abs(TheGrid.GetGridCell(x, z + 1) - (int)type)) > 2){
            Transform wallNorth = Instantiate(Walls[Doors(x, z, x, z + 1)],
                Folder.TransformPoint(new Vector3(4f * x, 2f, 4f * z + 2f)), Quaternion.identity);
            wallNorth.SetParent(cellFolder);
        }

        if (x == TheGrid.Height - 1)
        {
            Transform wallEast = Instantiate(Walls[(int)type % 2],
                Folder.TransformPoint(new Vector3(4f * x + 2f, 2f, 4f * z)), Quaternion.Euler(0, 90, 0));
            wallEast.SetParent(cellFolder);
        }
        else if (skip_east(x,z))
        {
            ;
        }
        else if((Mathf.Abs(TheGrid.GetGridCell(x + 1, z) - (int)type)) > 2){
            Transform wallEast = Instantiate(Walls[Doors(x, z, x+1, z)],
                Folder.TransformPoint(new Vector3(4f * x + 2f, 2f, 4f * z)), Quaternion.Euler(0, 90, 0));
            wallEast.SetParent(cellFolder);
        }

        if (z == 0){
            Transform wallSouth = Instantiate(Walls[(int)type % 2],
                Folder.TransformPoint(new Vector3(4f * x, 2f, 4f * z - 2f)), Quaternion.identity);
            wallSouth.SetParent(cellFolder);
        }


        if (x == 0){
            Transform wallWest = Instantiate(Walls[(int)type % 2],
                Folder.TransformPoint(new Vector3(4f * x - 2f, 2f, 4f * z)), Quaternion.Euler(0, 90, 0));
            wallWest.SetParent(cellFolder);
        }
    }

    public int GetFloorType(int type)
    {
        return GetFloorType((SectorType) type);
    }

    public int Doors(int x1, int z1, int x2, int z2)
    {
        if ((((int)TheGrid.GetGridCell(x1, z1)%2 + (int)TheGrid.GetGridCell(x2, z2) % 2) == 2)){
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public bool skip_north(int x, int z)
    {
        if (TheGrid.GetGridCell(x, z) == (int)SectorType.Bench || TheGrid.GetGridCell(x, z + 1) == (int)SectorType.Bench
            || TheGrid.GetGridCell(x, z) == (int)SectorType.BagClaim || TheGrid.GetGridCell(x, z + 1) == (int)SectorType.BagClaim)
            return true;
        else
            return false;
    }

    public bool skip_east(int x, int z)
    {
        if (TheGrid.GetGridCell(x, z) == (int)SectorType.Bench || TheGrid.GetGridCell(x + 1, z) == (int)SectorType.Bench
            || TheGrid.GetGridCell(x, z) == (int)SectorType.BagClaim || TheGrid.GetGridCell(x + 1, z) == (int)SectorType.BagClaim)
            return true;
        else
            return false;
    }

    public int GetFloorType(SectorType type)
    {
        switch (type)
        {
            case SectorType.Terminal:
                return 0;
            case SectorType.TerminalPath:
                return 1;
            case SectorType.Security:
                return 2;
            case SectorType.SecurityPath:
                return 3;
            case SectorType.Entry:
                return 4;
            case SectorType.EntryPath:
                return 5;
            case SectorType.Gates:
                return 6;
            case SectorType.GatesPath:
                return 7;
            case SectorType.Service:
                return 8;
            case SectorType.ServicePath:
                return 9;
            case SectorType.Arrivals:
                return 10;
            case SectorType.ArrivalsPath:
                return 11;
            case SectorType.Schengen:
                return 12;
            case SectorType.SchengenPath:
                return 13;
            case SectorType.NonSchengen:
                return 14;
            case SectorType.NonSchengenPath:
                return 15;
            case SectorType.Bags:
                return 16;
            case SectorType.BagsPath:
                return 17;
            case SectorType.CheckIn:
                return 18;
            case SectorType.CheckInPath:
                return 19;
            case SectorType.BagClaim:
                return 20;
            case SectorType.Bench:
                return 22;

            case SectorType.Empty:
                return 26;
            default:
                return 25;

        }
    }

    public void RenderServicesWalls(List<Service> services)
    {
        foreach (Service service in services)
        {
            int x = 0, z = service.End_z;
            for (x = service.Start_x; x <= service.End_x; x++)
            {
                if (service.Entry_z != z || service.Entry_x != x || service.EntryDirection != Direction.North)
                {
                    Transform wallNorth = Instantiate(Walls[0],
                    Folder.TransformPoint(new Vector3(4f * x, 2f, 4f * z + 2f)), Quaternion.identity);
                }
            }

            x = service.End_x;
            for (z = service.Start_z; z <= service.End_z; z++)
            {
                if (service.Entry_z != z || service.Entry_x != x || service.EntryDirection != Direction.East)
                {
                    Transform wallNorth = Instantiate(Walls[0],
                    Folder.TransformPoint(new Vector3(4f * x + 2f, 2f, 4f * z)), Quaternion.Euler(0, 90, 0));
                }
            }
        }
    }
}
