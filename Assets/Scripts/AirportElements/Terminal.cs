using System.Collections.Generic;
using UnityEngine;

public class Terminal
{
    Security security;
    EntryZone entryZone;
    GatesZone gatesZone;
    ArrivalsZone arrivalsZone;

    float stick_security_to_walls = ParametersManager.Instance.stick_security_to_walls;

    int x_split = TheGrid.Width - (TheGrid.Width * ParametersManager.Instance.gates_split / 100);
    int z_split = TheGrid.Height * ParametersManager.Instance.entry_split / 100;

    const int MIN_SECURITY_WIDTH = 3;
    const int MAX_SECURITY_WIDTH = 6;
    const int MIN_SECURITY_HEIGHT = 8;
    const int MAX_SECURITY_HEIGHT = 14;

    public Terminal()
    {
        int security_height = Random.Range(MIN_SECURITY_WIDTH, MAX_SECURITY_WIDTH);
        int security_width = Random.Range(MIN_SECURITY_HEIGHT, MAX_SECURITY_HEIGHT);

        int security_x = (int)Random.Range(x_split - x_split * stick_security_to_walls/100, TheGrid.Width - security_width - (TheGrid.Width - security_width) * stick_security_to_walls);

        if (security_x < x_split)
            security_x = x_split;
        else if (security_x > TheGrid.Width - security_width)
            security_x = TheGrid.Width - security_width;
        int security_z = Random.Range(z_split - security_height, z_split + 1);

        security = new Security(security_x, security_z, security_width, security_height);

        entryZone = new EntryZone(0, 0, TheGrid.Width, z_split);
        entryZone.CreatePath(security.Entry_x, security.Entry_z);

        gatesZone = new GatesZone(x_split, z_split, TheGrid.Width, TheGrid.Height);
        gatesZone.CreatePath(security.Exit_x, security.Exit_z);
        gatesZone.CreateZones();

        arrivalsZone = new ArrivalsZone(0, z_split, x_split, TheGrid.Height);

        entryZone.CreateSecondPath(arrivalsZone.Entry_x, arrivalsZone.Entry_z);

        entryZone.CreateServices();

        Debug.Log("x_split: " + x_split + " z_split: " + z_split);
    }

    public List<Service> GetServices()
    {
        List<Service> s = new List<Service>();
        s.AddRange(entryZone.Services);
        s.AddRange(gatesZone.Services);
        return s;
    }
}
