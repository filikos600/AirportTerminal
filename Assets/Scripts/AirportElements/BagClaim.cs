public class BagClaim
{
    int start_x, start_z, end_x, end_z;
    public BagClaim(int start_x, int start_z, int end_x, int end_z)
    {
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
                TheGrid.SetGridCell(x, z, (int)SectorType.BagClaim);
            }
        }
    }
}
