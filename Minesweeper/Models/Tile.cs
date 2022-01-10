using System.Collections.Generic;
using System.Linq;

namespace Minesweeper
{
    public class Tile
    {
        //If True it is a Mine if false it isn't a mine.
        public bool IsMine { get; set; }
        //If the Tile is left clicked it will be pressed
        public bool IsClicked { get; set; }
        //If double tapped this will trigger unvaling of surround Tiles
        public bool IsPressed { get; set; }
        //If the Tile is right clicked it will be flagged(True) or not(False)
        public bool IsFlagged { get; set; }
        //If the Tile is right clicked and flagged it will be questioned(True) or not(False)
        public bool IsAmbiguous { get; set; }
        //The row that the tile lays
        public int Row { get; set; }
        //The column that the tile lays
        public int Column { get; set; }
        //A List of 8 Tiles
        public List<Tile> Surroundings { get; set; }
        //This will check the amound 8 tiles around it to see if they are mines.
        public int MinesAround => Surroundings.Count(x => x.IsMine);

        public Tile(int row, int column)
        {
            Surroundings = new List<Tile>();
            Row = row;
            Column = column;
        }
    }
}
