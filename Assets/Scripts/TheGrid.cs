public static class TheGrid
{
    static int[,] grid;
    static int width;
    static int height;
    static int size;

    public static int[,] Grid { get => grid; }
    public static int Width { get => width; }
    public static int Height { get => height; }
    public static int Size { get => size; }

    public static int GetGridCell(int x, int z){
        return grid[x, z];
    }

    public static void SetGridCell(int x, int z, int cell)
    {
        grid[x, z] = cell;
    }

    public static Terminal CreateGrid(int x, int z)
    {
        grid = new int[x,z];
        width = x;
        height = z;
        size = x * z;
        return new Terminal();
    }


}
