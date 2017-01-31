using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoundariesTileEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int newIndex;
        private TileMap Loaded;
        private string LoadedFileName = "";
        private Bitmap buffer;
        private bool saved = false;
        private Graphics mapGraphics;
        private List<TileInfo> tileInfos = new List<TileInfo>();
        private int selectedInfoIndex;
        private bool Pressing = false;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            listView1.View = View.LargeIcon;
            buffer = new Bitmap(panel1.Width, panel1.Height);
            mapGraphics = Graphics.FromImage(buffer);
            panel1.BackgroundImage = buffer;

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            panel1, new object[] { true });
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (Loaded != null)
            e.Effect = DragDropEffects.All;
        }

        private void tileMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox1.Visible = true;
            saveToolStripMenuItem.Enabled = true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int recWidth = panel1.Width / int.Parse(textBox1.Text);
            int recHeight = panel1.Height / int.Parse(textBox2.Text);

            Loaded = new TileMap(int.Parse(textBox1.Text), int.Parse(textBox2.Text), recWidth, recHeight);
            tileToolStripMenuItem.Enabled = true;



            foreach(Tile t in Loaded.MapTiles)
            {
                mapGraphics.DrawRectangle(new Pen(Color.Red), t.tileRectangle);
            }

            panel1.Invalidate();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if(Loaded != null)
            for(int i = 0; i < Loaded.MapTiles.Length;i++)
            {
                var point = panel1.PointToClient(Cursor.Position);
                if(Loaded.MapTiles[i].tileRectangle.Contains(point))
                {
                    if(tileInfos.Count != 0)
                    {
                        Loaded.MapTiles[i].Texture = tileInfos[selectedInfoIndex].Texture;
                        Loaded.MapTiles[i].Index = tileInfos[selectedInfoIndex].Index;
                        mapGraphics.DrawImage(Loaded.MapTiles[i].Texture, Loaded.MapTiles[i].tileRectangle);
                        panel1.Invalidate();
                        saved = false;
                    }
                }
            }
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            if (Loaded == null)
            {
                return;
            }
            else
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                for(int i = 0; i < files.Length;i++)
                {
                    if(files[i].EndsWith(".png")|| files[i].EndsWith(".bmp") || files[i].EndsWith(".jpg") )
                    {
                       
                        Bitmap b = (Bitmap)Image.FromFile(files[i]);
                        TileInfo info = new TileInfo(b, tileInfos.Count(),files[i]);
                        tileInfos.Add(info);
                        listView1.Items.Add(Convert.ToString((tileInfos.Count() - 1)));
                        listView1.LargeImageList.Images.Add(b);
                        listView1.Items[tileInfos.Count() - 1].ImageIndex = tileInfos.Count() - 1;
                        
                    }
                    else
                    {
                       
                        MessageBox.Show("You're trash", "Step your game up", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Pressing = true;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Pressing = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Loaded != null && Pressing)
                for (int i = 0; i < Loaded.MapTiles.Length; i++)
                {
                    var point = panel1.PointToClient(Cursor.Position);
                    if (Loaded.MapTiles[i].tileRectangle.Contains(point))
                    {
                        if (tileInfos.Count != 0)
                        {
                            Loaded.MapTiles[i].Texture = tileInfos[selectedInfoIndex].Texture;
                            Loaded.MapTiles[i].Index = tileInfos[selectedInfoIndex].Index;
                            mapGraphics.DrawImage(Loaded.MapTiles[i].Texture, Loaded.MapTiles[i].tileRectangle);
                            panel1.Invalidate();
                        }
                    }
                }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count != 0 && listView1.SelectedItems[0].Index >= 0)
            selectedInfoIndex = listView1.SelectedItems[0].Index;
        }

        private void changeIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.form1 = this;
            form2.ShowDialog();


            tileInfos[selectedInfoIndex].Index = newIndex;
            imageList1.Images.Clear();
            listView1.LargeImageList.Images.Clear();
            listView1.SmallImageList.Images.Clear();
            listView1.Items.Clear();

            for(int i = 0; i < tileInfos.Count();i++)
            {
                Image bmp = Image.FromFile(Environment.CurrentDirectory + tileInfos[i].FileName);

                listView1.Items.Add(tileInfos[i].Index.ToString());
                listView1.LargeImageList.Images.Add(bmp);
                listView1.Items[i].ImageIndex = i;

            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {


            if (LoadedFileName == "")
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    LoadedFileName = sfd.FileName + ".bdr";
                }

            }

            int amount = LoadedFileName.Split('\\').Count();
            string fname = LoadedFileName.Split('\\')[amount - 1];
            
            if (!Directory.Exists(@"\Content\"))
                Directory.CreateDirectory(@"\Content\" + fname + @"\");
            else
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\Content\");

                Directory.CreateDirectory(Environment.CurrentDirectory + @"\Content\" + fname + @"\");
            }

            foreach (TileInfo tIf in tileInfos)
            {
                string s = Environment.CurrentDirectory + @"\Content\" + fname + "\\" + tIf.Shortname + "." + tIf.Extention;
                tIf.Texture.Save(s, ImageFormat.Png);
            }

            StreamWriter sw = new StreamWriter(LoadedFileName);
            sw.WriteLine("Row:" + Loaded.Rows + "|" + "Columns:" + Loaded.Columns);
            string names = "Names[";
            for (int i = 0; i < tileInfos.Count; i++)
            {
                if (i != tileInfos.Count - 1)
                    names += tileInfos[i].Shortname + "=" + tileInfos[i].Index + ",";
                else
                    names += tileInfos[i].Shortname + "=" + tileInfos[i].Index + "]";
            }
            sw.WriteLine(names);

            string indexs = "{";
            int counter = 0;
            foreach (Tile t in Loaded.MapTiles)
            {
                if (counter != Loaded.MapTiles.Count() - 1)
                    indexs += t.Index + ",";
                else
                    indexs += t.Index + "}";
                counter++;

            }
            sw.WriteLine(indexs);
            sw.Close();
            saved = true;

        }

        private void tileMapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Map Files (.bdr)|*.bdr|All Files (*.*)|*.*";
            ofd.FilterIndex = 1;
            string[] mapinfo = new string[3];
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                LoadedFileName = ofd.FileName;
                string shortname = ofd.FileName.Split('\\')[ofd.FileName.Split('\\').Count() - 1].Split('.')[0];

                mapinfo[0] = sr.ReadLine();
                mapinfo[1] = sr.ReadLine();
                mapinfo[2] = sr.ReadLine();
                sr.Close();

                int rows = int.Parse(mapinfo[0].Split('|')[0].Split(':')[1]);
                int columns = int.Parse(mapinfo[0].Split('|')[1].Split(':')[1]);

                mapinfo[2] = mapinfo[2].TrimStart('{');
                mapinfo[2] = mapinfo[2].TrimEnd('}');
                mapinfo[1] = mapinfo[1].Remove(0, 6);
                mapinfo[1] = mapinfo[1].TrimEnd(']');

                string[] tileInfostuff = mapinfo[1].Split(',');
                int counter = 0;
                foreach (string tle in tileInfostuff)
                {
                    //C:\Users\Jonathan\OneDrive\BoundariesTileEditor\BoundariesTileEditor\bin\Debug\Content\mapfile1
                    string path = Environment.CurrentDirectory + @"\Content\" + shortname + "\\" + tileInfostuff[counter].Split('=')[0] + ".png";
                    TileInfo info = new TileInfo((Bitmap)Image.FromFile(path), int.Parse(tileInfostuff[counter].Split('=')[1]), @"\Content\" + shortname + "\\" + tileInfostuff[counter].Split('=')[0] + ".png");
                    listView1.Items.Add(info.Index.ToString());
                    listView1.LargeImageList.Images.Add(info.Texture);
                    listView1.Items[counter].ImageIndex = counter;
                    tileInfos.Add(info);
                    counter++;
                }

                int[] Indexes = Array.ConvertAll(mapinfo[2].Split(','), int.Parse);

                Loaded = new TileMap(columns, rows, panel1.Width / columns, panel1.Height / rows);
                Loaded.MapTiles = new Tile[columns * rows];


                for (int i = 0; i < Loaded.MapTiles.Length; i++)
                {

                    Loaded.MapTiles[i] = new Tile(i / columns, i % columns, panel1.Width / columns, panel1.Height / rows);
                    Loaded.MapTiles[i].Index = Indexes[i];
                    Loaded.MapTiles[i].Texture = tileInfos[Loaded.MapTiles[i].Index].Texture;
                }

                foreach (Tile maptile in Loaded.MapTiles)
                {
                    mapGraphics.DrawRectangle(new Pen(Color.Red), maptile.tileRectangle);
                    mapGraphics.DrawImage(maptile.Texture, maptile.tileRectangle);
                }
                panel1.Invalidate();
                saveToolStripMenuItem.Enabled = true;
            }
            else
            {

            }

            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if(saved == false)
            {
                var result = MessageBox.Show("Do you really want to close without saving?","Unsaved Work", MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                if(result == DialogResult.Yes)
                {
                    
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void mapAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < Loaded.MapTiles.Count();i++)
            {
                Loaded.MapTiles[i].Index = tileInfos[selectedInfoIndex].Index;
                Loaded.MapTiles[i].Texture = tileInfos[selectedInfoIndex].Texture;
                mapGraphics.DrawImage(Loaded.MapTiles[i].Texture, Loaded.MapTiles[i].tileRectangle);
            }
            panel1.Invalidate();
        }
    }
}
