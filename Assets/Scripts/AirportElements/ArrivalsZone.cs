public class ArrivalsZone
{
    SchengenZone schengenZone;
    NonSchengenZone nonSchengenZone;
    Bags bags;

    int start_x, start_z, end_x, end_z, x_split, z_split;

    int entry_x, entry_z;

    public ArrivalsZone(int start_x, int start_z, int end_x, int end_z)
    {
        this.start_x = start_x;
        this.start_z = start_z;
        this.end_x = end_x;
        this.end_z = end_z;

        x_split = start_x + (int)((end_x - start_x)* 0.4f);
        z_split = start_z + (int)((end_z - start_z) * 0.7f);  

        FillZone();

        schengenZone = new SchengenZone(x_split, z_split, end_x, end_z);
        nonSchengenZone = new NonSchengenZone(start_x, z_split, x_split, end_z);

        bags = new Bags(start_x, start_z, end_x, z_split);
        bags.EmptyZones();
        this.entry_x = bags.get_entry_x();
        this.entry_z = bags.get_entry_z();
    }

    public int Entry_x { get => entry_x; }
    public int Entry_z { get => entry_z; }

    public void FillZone()
    {
        for (int x = start_x; x < end_x; x++)
        {
            for (int z = start_z; z < end_z; z++)
            {
                if (TheGrid.GetGridCell(x, z) == (int)SectorType.Terminal)
                    TheGrid.SetGridCell(x, z, (int)SectorType.Arrivals);
            }
        }
    }

}

