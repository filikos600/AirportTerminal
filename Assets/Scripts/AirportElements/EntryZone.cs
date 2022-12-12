using System.Collections.Generic;
using UnityEngine;

public class EntryZone
{
    List<Bench> benches;
    List<Service> services;
    int start_x, start_z, end_x, end_z;
    int min_distance_door_corner = ParametersManager.Instance.min_distance_door_corner;
    int exit_door_step = ParametersManager.Instance.exit_door_step;
    int distance_from_entry = ParametersManager.Instance.distance_from_entry;

    float service_density = ParametersManager.Instance.service_density;
    float bench_density = ParametersManager.Instance.bench_density;

    int distance_from_north_wall, distance_from_south_wall, distance_from_side_walls;

    public List<Service> Services { get => services; }

    public EntryZone(int start_x, int start_z, int end_x, int end_z)
    {
        this.benches = new List<Bench>();
        this.services = new List<Service>();

        this.start_x = start_x;
        this.start_z = start_z;
        this.end_x = end_x;
        this.end_z = end_z;

        this.distance_from_north_wall = (int)((end_z - start_z) * 0.35f);
        this.distance_from_south_wall = (int)((end_z - start_z) * 0.2f);
        this.distance_from_side_walls = (int)((end_z - start_z) * 0.15f);


        fillZone();
    }

    public void fillZone()
    {
        for (int x = start_x; x < end_x; x++)
        {
            for (int z = start_z; z < end_z; z++)
            {
                if (TheGrid.GetGridCell(x, z) == (int)SectorType.Terminal)
                    TheGrid.SetGridCell(x, z, (int)SectorType.Entry);
            }
        }
    }

    public void CreatePath(int entry_x, int entry_z)
    {
        for (int z = entry_z - 1; z >= start_x; z--)
        {
            TheGrid.SetGridCell(entry_x, z, (int)SectorType.EntryPath);
            TheGrid.SetGridCell(entry_x - 1, z, (int)SectorType.EntryPath);
            TheGrid.SetGridCell(entry_x + 1, z, (int)SectorType.EntryPath);
        }
        TheGrid.SetGridCell(entry_x - 1, start_z, (int)SectorType.Entry);
        TheGrid.SetGridCell(entry_x + 1, start_z, (int)SectorType.Entry);


        int x = entry_x;
        while (x + exit_door_step + this.min_distance_door_corner < end_x)
        {
            for (int xx = x; xx <= x + exit_door_step; xx++)
            {
                TheGrid.SetGridCell(xx, distance_from_entry, (int)SectorType.EntryPath);
                TheGrid.SetGridCell(xx, distance_from_entry + 2, (int)SectorType.EntryPath);
                TheGrid.SetGridCell(xx, distance_from_entry + 1, (int)SectorType.EntryPath);
            }
               
            for (int zz = start_z; zz < distance_from_entry; zz++)
            {
                TheGrid.SetGridCell(x + exit_door_step, zz, (int)SectorType.EntryPath);
            }
            if (x - 1 + exit_door_step > 0)
                TheGrid.SetGridCell(x - 1 + exit_door_step, start_z + 1, (int)SectorType.EntryPath);
            if (x + 1 + exit_door_step < end_x)
                TheGrid.SetGridCell(x + 1 + exit_door_step, start_z + 1, (int)SectorType.EntryPath);
            x += exit_door_step;
        }
        x = entry_x;
        while (x - exit_door_step - this.min_distance_door_corner >= start_x)
        {
            
            for (int xx = x; xx >= x - exit_door_step; xx--)
            {
                TheGrid.SetGridCell(xx, distance_from_entry, (int)SectorType.EntryPath);
                TheGrid.SetGridCell(xx, distance_from_entry + 2, (int)SectorType.EntryPath);
                TheGrid.SetGridCell(xx, distance_from_entry + 1, (int)SectorType.EntryPath);
            }
            for (int zz = start_z; zz < distance_from_entry; zz++)
            {
                TheGrid.SetGridCell(x + exit_door_step, zz, (int)SectorType.EntryPath);
            }
            if (x - 1 + exit_door_step > 0)
                TheGrid.SetGridCell(x - 1 + exit_door_step, start_z + 1, (int)SectorType.EntryPath);
            if (x + 1 + exit_door_step < end_x)
                TheGrid.SetGridCell(x + 1 + exit_door_step, start_z + 1, (int)SectorType.EntryPath);
            x -= exit_door_step;
        }

    }

    public void CreateSecondPath(int entry_x, int entry_z)
    {
        for (int z = entry_z - 1; z > start_z + distance_from_entry; z--)
        {
            TheGrid.SetGridCell(entry_x, z, (int)SectorType.EntryPath);
            TheGrid.SetGridCell(entry_x + 1, z, (int)SectorType.EntryPath);
            TheGrid.SetGridCell(entry_x - 1, z, (int)SectorType.EntryPath);
        }
        int x = entry_x;
        while(TheGrid.GetGridCell(x, start_z + distance_from_entry) != (int)SectorType.EntryPath)
        {
            TheGrid.SetGridCell(x, start_z + distance_from_entry, (int)SectorType.EntryPath);
            TheGrid.SetGridCell(x, start_z + distance_from_entry - 1, (int)SectorType.EntryPath);
            TheGrid.SetGridCell(x, start_z + distance_from_entry + 1, (int)SectorType.EntryPath);
            x++;
        }
    }

    public void CreateCheckIn()
    {
        int checkins = 0;
        int check_height = (int)(Random.Range((end_z - start_z) * 0.1f, (end_z - start_z) * 0.2f));
        int check_width = (int)(Random.Range((end_x - start_x) * 0.2f, (end_x - start_x) * 0.3f));
        int entry = check_width / 3;

        for (int x = end_x - check_width; x >=0; x--)
        {
            if (checkins < 2 && CheckFreeSize(x, end_z - check_height, check_width, check_height))
            {
                for (int xx = x; xx < x + check_width; xx++)
                {
                    for (int zz = end_z- check_height; zz < end_z; zz++)
                    {
                        TheGrid.SetGridCell(xx, zz, (int)SectorType.CheckIn);
                    }
                    if (xx % entry == 0)
                    {
                        TheGrid.SetGridCell(xx, end_z - check_height, (int)SectorType.CheckInPath);
                        TheGrid.SetGridCell(xx, end_z - check_height - 1, (int)SectorType.EntryPath);
                        TheGrid.SetGridCell(xx, end_z - check_height - 2, (int)SectorType.EntryPath);
                    }
                }
                x -= check_width;
                checkins++;
            }
        }
    }

    public void CreateServices()
    {
        CreateCheckIn();
        CreateBorderServices();
        CreatePathServices();
        FillEmptyBetweenServices();
    }

    public void CreateBorderServices()
    {
        SouthBorder();
        NorthBorder();
        EastBorder();
        WestBorder();
    }

    public bool CheckFreeSize(int start_x, int start_z, int size_x, int size_z)
    {
        for (int x = start_x; x < start_x + size_x; x++)
        {
            for (int z = start_z; z < start_z + size_z; z++)
            {
                if (TheGrid.GetGridCell(x, z) != (int)SectorType.Entry)
                    return false;
            }
        }
        return true;
    }

    public void NorthBorder()
    {
        for (int x = start_x + distance_from_side_walls; x < end_x - distance_from_side_walls - 2; x++)
        {
            int size_x = Random.Range(4, 6);
            while (x + size_x > end_x + 1)
            {
                if (size_x > 2)
                    size_x--;
                else
                    break;
            }
            int size_z = Random.Range(4, 6);
            while (!CheckFreeSize(x, end_z - size_z, size_x, size_z))
            {
                if (size_z > 2)
                    size_z--;
                else
                    break;
            }
            if (CheckFreeSize(x, end_z - size_z, size_x, size_z) && (Random.Range(0, 1f) < service_density))
            {
                services.Add(new Service(x, end_z - size_z, size_x, size_z, Random.Range(x, x + size_x), end_z - size_z, Direction.South, SectorType.EntryPath));
                x += size_x;
                x--;
            }
        }
    }
    public void EastBorder()
    {
        for (int z = start_z + distance_from_south_wall; z < end_z - distance_from_north_wall; z++)
        {
            int size_z = Random.Range(4, 6);
            while (z + size_z > end_z + 1)
            {
                if (size_z > 2)
                    size_z--;
                else
                    break;
            }
            int size_x = Random.Range(4, 6);
            while (!CheckFreeSize(end_x - size_x, z, size_x, size_z))
            {
                if (size_z > 2)
                    size_z--;
                else
                    break;
            }
            if (CheckFreeSize(end_x - size_x, z, size_x, size_z) && (Random.Range(0, 1f) < service_density))
            {
                services.Add(new Service(end_x - size_x, z, size_x, size_z, end_x - size_x, Random.Range(z, z + size_z), Direction.West, SectorType.EntryPath));
                z += size_z;
                z--;
            }
        }
    }

    public void SouthBorder()
    {
        for (int x = start_x + distance_from_side_walls; x < end_x - distance_from_side_walls - 1; x++)
        {
            int size_x = Random.Range(4, 6);
            while (x + size_x > end_x + 1)
            {
                if (size_x > 2)
                    size_x--;
                else
                    break;
            }
            int size_z = Random.Range(4, 6);
            while (!CheckFreeSize(x, start_z, size_x, size_z))
            {
                if (size_z > 2)
                    size_z--;
                else
                    break;
            }
            if (CheckFreeSize(x, start_z, size_x, size_z) && (Random.Range(0, 1f) < service_density))
            {
                services.Add(new Service(x, start_z, size_x, size_z, Random.Range(x, x + size_x), start_z + size_z - 1, Direction.North, SectorType.EntryPath));
                x += size_x;
                x--;
            }
        }
    }

    public void WestBorder()
    {
        for (int z = start_z  + distance_from_south_wall; z < end_z - distance_from_north_wall; z++)
        {
            int size_z = Random.Range(4, 6);
            while (z + size_z > end_z + 1)
            {
                if (size_z > 2)
                    size_z--;
                else
                    break;
            }
            int size_x = Random.Range(4, 6);
            while (!CheckFreeSize(start_x, z, size_x, size_z))
            {
                if (size_z > 2)
                    size_z--;
                else
                    break;
            }
            if (CheckFreeSize(start_x, z, size_x, size_z) && (Random.Range(0, 1f) < service_density))
            {
                services.Add(new Service(start_x, z, size_x, size_z, start_x + size_x - 1, Random.Range(z, z + size_z), Direction.East, SectorType.EntryPath));
                z += size_z;
                z--;
            }
        }
    }


    public void CreatePathServices()
    {
        int distance_from_north_wall = (int)((end_x - start_x) * 0.15f);
        int distance_from_wall = (int)((end_x - start_x) * 0.05f);
        int additional_services_distance = 2;
        int entry_x = 0, entry_z = 0;
        
        for (int x = end_x - 1; x > entry_z; x--)
        {
                if (TheGrid.GetGridCell(x, distance_from_entry + 1) == (int)SectorType.EntryPath && TheGrid.GetGridCell(x, distance_from_entry + 2) == (int)SectorType.EntryPath && TheGrid.GetGridCell(x, distance_from_entry + 3) == (int)SectorType.EntryPath)
                {
                    entry_x = x;
                    entry_z = distance_from_entry + 1;
                    break;
                }
        }

        int size_z = 7;
        int size_x = 6;

        bool created;
        for (int z = entry_z; z < end_z - distance_from_north_wall; z++)
        {
            created = false;
            for (int x = entry_x - 1; x > distance_from_wall + size_x; x--)
                if (CheckFreeSize(x, z + 1, size_x, size_z) && (Random.Range(0f, 1f) < bench_density))
                {
                    benches.Add(new Bench(x, z + 1, size_x, size_z));
                    x -= size_x;
                    x -= additional_services_distance;
                    created = true;
                }
            for (int x = entry_x + 2; x < TheGrid.Width - 1 - size_x - distance_from_wall; x++)
                if (CheckFreeSize(x, z + 1, size_x, size_z) && (Random.Range(0f,1f) < bench_density))
                {
                    benches.Add(new Bench(x, z + 1, size_x, size_z));
                    x += size_x;
                    x += additional_services_distance;
                    created = true;
                }
            if (created)
                {
                z += size_z;
                z += additional_services_distance;
                }
        }
    }

    public void FillEmptyBetweenServices()
    {
        Debug.Log("side " + distance_from_side_walls + " north: " + distance_from_north_wall + " south: " + distance_from_south_wall);
        //west
        for (int z = start_z; z < end_z; z++)
        {
            for (int x = start_x; x < distance_from_side_walls; x++)
            {
                if (TheGrid.GetGridCell(x, z) == (int)SectorType.Entry)
                    TheGrid.SetGridCell(x, z, (int)SectorType.Empty);
            }
           
        }

        //east
        for (int z = start_z; z < end_z; z++)
        {
            for (int x = end_x - 1; x > end_x - 1 - distance_from_side_walls; x--)
            {
                if (TheGrid.GetGridCell(x, z) == (int)SectorType.Entry)
                    TheGrid.SetGridCell(x, z, (int)SectorType.Empty);
            }

        }

        //north
        for (int x = start_x; x < end_x; x++)
        {
            for (int z = end_z - 1; z > end_z - 1 - distance_from_side_walls; z--)
            {
                if (TheGrid.GetGridCell(x, z) == (int)SectorType.Entry)
                    TheGrid.SetGridCell(x, z, (int)SectorType.Empty);
            }

        }
    }
}
