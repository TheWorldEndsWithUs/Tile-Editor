using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace BoundariesTileEditor
{
    class DisplayPanel:Panel
    {
        private Graphics panelGraphics;
        public BufferedGraphics Buffer;
        private BufferedGraphicsContext bufferContext = BufferedGraphicsManager.Current;
        
        public DisplayPanel()
            :base()
        {
            panelGraphics = this.CreateGraphics();
            Buffer = bufferContext.Allocate(panelGraphics,new Rectangle(0,0,this.Width + 1,this.Height + 1));
            
        }

        public void Render()
        {
            Buffer.Render(panelGraphics);
        }
    }
}
