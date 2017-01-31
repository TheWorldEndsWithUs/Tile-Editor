using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoundariesTileEditor
{
    class TileMap
    {

        public int[] tileMapIndexs;// the list of numbers that create the map

        public Tile[] Tiles;

        public int mapWidth;
        public int mapHeight;

        public int Columns;
        public int Rows;

        public Tile[] MapTiles;

        public TileMap(int columns, int rows, int mWidth, int mHeight)
        {
            Columns = columns;
            Rows = rows;
            mapWidth = mWidth;
            mapHeight = mHeight;

            MapTiles = new Tile[columns * rows];
            int counter = 0;
            for(int y = 0; y < rows;y++)
            {
                for(int x = 0; x < columns;x++)
                {
                    Tile tile = new Tile(y , x , mWidth, mHeight);
                    MapTiles[counter] = tile;
                    counter++;
                }
            }
        }




    }
}
