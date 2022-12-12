using System.Collections.Generic;
using UnityEngine;

public class GatesZone
{
    List<Service> services;

    int start_x, start_z, end_x, end_z;
    int distance_from_gates;
    int max_service = ParametersManager.Instance.max_service_size;
    float bench_density = ParametersManager.Instance.bench_density;

    public List<Service> Services { get => services; }

    public GatesZone(int start_x, int start_z, int end_x, int end_z)
    {
        this.services = new List<Service>();

        this.start_x = start_x;
        this.start_z = start_z;
        this.end_x = end_x;
        this.end_z = end_z;

        FillZone();
    }

    public void FillZone()
    {
        for (int x = start_x; x < end_x; x++)
        {
            for (int z = start_z; z < end_z; z++)
            {
                if (TheGrid.GetGridCell(x, z) == (int)SectorType.Terminal)
                    TheGrid.SetGridCell(x, z, (int)SectorType.Gates);
            }
        }
    }

    public bool CheckFreeSize(int start_x, int start_z, int size_x, int size_z)
    {
        for (int x = start_x; x < start_x + size_x; x++)
        {
            for (int z = start_z; z < start_z + size_z; z++)
            {
                if (TheGrid.GetGridCell(x, z) != (int)SectorType.Gates)
                    return false;
            }
        }
        return true;
    }

    public void CreatePath(int entry_x, int entry_z)
    {
        int gates_number = ParametersManager.Instance.exit_gates;
        int exit_gates_step = (end_x - start_x - 2) / gates_number + 1;

        while (exit_gates_step < ParametersManager.Instance.min_gates_distance + 1)
        {
            gates_number--;
            exit_gates_step = (end_x - start_x - 2) / gates_number + 1;
        }

        this.distance_from_gates = Random.Range(2, 4);

        for (int z = entry_z + 1; z < end_z - distance_from_gates; z++)
        {
            TheGrid.SetGridCell(entry_x, z, (int)SectorType.GatesPath);
            TheGrid.SetGridCell(entry_x - 1, z, (int)SectorType.GatesPath);
            TheGrid.SetGridCell(entry_x + 1, z, (int)SectorType.GatesPath);
        }

        for (int x = start_x + 1; x < end_x - 1; x++)
        {
            TheGrid.SetGridCell(x, end_z - distance_from_gates, (int)SectorType.GatesPath);
        }


        int created_gates = 0, x1 = start_x + 1, x2 = end_x - 2;
        while (created_gates < gates_number)
        {
            for (int z = end_z - distance_from_gates; z < end_z; z++)
            {
                TheGrid.SetGridCell(x1, z, (int)SectorType.GatesPath);
            }
            created_gates++;
            x1 += exit_gates_step;
            if (created_gates < gates_number)
            {
                for (int z = end_z - distance_from_gates; z < end_z; z++)
                {
                    TheGrid.SetGridCell(x2, z, (int)SectorType.GatesPath);

                }
                created_gates++;
                x2 -= exit_gates_step;
            }
        }
    }

    public void CreateZones()
    {
        int empty_ratio_x = (int)((end_x - start_x) * 0.3f);    //for one side
        int empty_ratio_z = (int)((end_z - start_z) * 0.4f);
        EmptyZones(empty_ratio_x, empty_ratio_z);
        CreateServices(empty_ratio_x, empty_ratio_z);
        CreateBenches(empty_ratio_x, empty_ratio_z);
    }

    public void EmptyZones(int empty_x, int empty_z)
    {

        for(int x = start_x; x < start_x + empty_x; x++)
        {
            if (TheGrid.GetGridCell(x, start_z) !=  (int)SectorType.Gates)
                    break;
            for (int z = start_z; z < start_z + empty_z; z++)
            {
                TheGrid.SetGridCell(x, z, (int)SectorType.Empty);
            }
        }

        for (int x = end_x - 1; x >= end_x - empty_x; x--)
        {
            if (TheGrid.GetGridCell(x, start_z) != (int)SectorType.Gates)
                break;
            for (int z = start_z; z < start_z + empty_z; z++)
            {
                TheGrid.SetGridCell(x, z, (int)SectorType.Empty);
            }
        }
    }

    public void CreateServices(int empty_x, int empty_z)
    {
        int size_x = 0, size_z = 0;
        //souths
        for (int x = start_x + empty_x; x < end_x - empty_x; x++)
        {
            size_x = Random.Range(4, 6);
            while (x + size_x > end_x + 1)
            {
                if (size_x > 2)
                    size_x--;
                else
                    return;
            }
            size_z = empty_z;
            
            if (CheckFreeSize(x, start_z, size_x, size_z))
            {
                services.Add(new Service(x, start_z, size_x, size_z, Random.Range(x, x + size_x), start_z + size_z - 1, Direction.North, SectorType.GatesPath));
                x += size_x;
                x--;
            }
            else
            {
                while (TheGrid.GetGridCell(x, start_z) == (int)SectorType.Gates)
                {
                    for (int zz = start_z; zz < start_z + size_z; zz++)
                        TheGrid.SetGridCell(x, zz, (int)SectorType.Empty);
                    x++;
                }
            }

        }

        //east
        int services_number = 1;
        int size_z_remaining = end_z - start_z - distance_from_gates - empty_z - 1;
        int z = start_z + empty_z;

        while (services_number > 0)
        {
            size_z = size_z_remaining;
            if (size_z >= max_service)
            {
                size_z = (size_z_remaining >= max_service ? max_service : size_z_remaining / 2);
                size_z_remaining -= size_z;
                services_number++;
            }
            size_x = Random.Range(3, 4);

            while (!CheckFreeSize(end_x - size_x, z, size_x, size_z))
            {
                if (size_z > 2)
                    size_z--;
                else
                    return;
            }
            if (CheckFreeSize(end_x - size_x, z, size_x, size_z))
            {
                services.Add(new Service(end_x - size_x, z, size_x, size_z, end_x - size_x, Random.Range(z, z + size_z), Direction.West, SectorType.GatesPath));
                services_number--;
                z += size_z;
            }
        }

        //west
        services_number = 1;
        size_z_remaining = end_z - start_z - distance_from_gates - empty_z - 1;
        z = start_z + empty_z;

        while (services_number > 0)
        {
            size_z = size_z_remaining;
            if (size_z >= max_service)
            {
                size_z = (size_z_remaining >= max_service ? max_service : size_z_remaining / 2);
                size_z_remaining -= size_z;
                services_number++;
            }
            size_x = Random.Range(3, 4);

            while (!CheckFreeSize(start_x, z, size_x, size_z))
            {
                if (size_z > 2)
                    size_z--;
                else
                    return;
            }
            if (CheckFreeSize(start_x, z, size_x, size_z))
            {
                services.Add(new Service(start_x, z, size_x, size_z, start_x + size_x - 1, Random.Range(z, z + size_z), Direction.East, SectorType.GatesPath));
                services_number--;
                z += size_z;
            }
        }

    }

    public void CreateBenches(int empty_x, int empty_z)
    {
        int additional_services_distance = 1;
        int entry_x = 0, entry_z = 0;

        for (int x = end_x - 1; x > entry_z; x--)
        {
            if (TheGrid.GetGridCell(x, end_z - distance_from_gates - 1) == (int)SectorType.GatesPath && TheGrid.GetGridCell(x, end_z - distance_from_gates - 2) == (int)SectorType.GatesPath && TheGrid.GetGridCell(x, end_z - distance_from_gates - 3) == (int)SectorType.GatesPath)
            {
                entry_x = x;
                entry_z = end_z - distance_from_gates + 1;
                break;
            }
        }

        int size_z = 5;
        int size_x = 5;

        bool created;
       for (int z = entry_z; z > start_z + empty_z; z --)
        {
            created = false;
            for (int x = entry_x - 1; x > start_x + empty_x/2; x--)
                if (CheckFreeSize(x, z - size_z - 1, size_x, size_z + 1) && (Random.Range(0f, 1f) < bench_density))
                {
                    new Bench(x, z - size_z, size_x, size_z);
                    created = true;
                    x -= size_x;
                    x -= additional_services_distance;
                }
            for (int x = entry_x + 2; x < end_x - 1 - size_x - empty_x/2; x++)
                if (CheckFreeSize(x, z - size_z - 1, size_x, size_z + 1) && (Random.Range(0f, 1f) < bench_density))
                {
                    new Bench(x, z - size_z, size_x, size_z);
                    created = true;
                    x += size_x;
                    x += additional_services_distance;
                }
            if (created)
            {
                z -= additional_services_distance;
                z -= size_z;
            }
        }
    }
}
