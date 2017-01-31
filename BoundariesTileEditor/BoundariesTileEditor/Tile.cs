using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BoundariesTileEditor
{
    class Tile
    {

        public int Column, Row,Index;
        public int Width, Height;

        public Bitmap Texture;

        public string imageLocation;
        public Rectangle tileRectangle;

        public Tile(int col , int row , int width , int height)
        {
            Column = col;
            Row = row;
            Width = width;
            Height = height;

           tileRectangle =  new Rectangle(Column * Width, Row * Height, Width, Height);
        }




    }
    public class TileInfo
    {
        public int Index;
        public string FileName;
        public Bitmap Texture;

        public TileInfo(Bitmap img , int index,string name)
        {
            Index = index;
            Texture = img;
            FileName = name;
        }

        public string Extention
        {
            get
            {
                return FileName.Split('.')[1];
            }
        }

        public string Shortname
        {
            get {
                string[] filenames = FileName.Split('\\');
                string shorter = filenames[filenames.Length - 1];
                string final = shorter.Split('.')[0];

                return final;
            }
            
        }
    }

}
