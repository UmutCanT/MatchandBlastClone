public class Tile
{
    private Grid<Tile> grid;
    private int x;
    private int y;

    public Tile(Grid<Tile> grid,  int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
}
