using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using DwfTools.W2d.Opcodes;

namespace DwfTools.Controls
{
    public class DrawingDisplayControl : Control
    {
        public DrawingDisplayControl()
        {
            Name = "DrawingDisplay";
            Size = new Size(320, 240);

            //turn on double buffering
            /*SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);*/
        }

        List<IOpcode> opcodes;

        protected List<IOpcode> GetData()
        {
            if (opcodes == null)
            {
                
                opcodes = W2dParser.GetParsedData(@"C:\Users\Liquidsnk\Documents\Visual Studio 2010\Projects\DwfTools\DwfTools\Sample\FF6A06F4-B452-48B0-A328-74702E883FEA.w2d");
            }
            
            return opcodes;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var opcodes = GetData();
            var rendering = new GdiRendering(e.Graphics, e.ClipRectangle);
            rendering.zoom = zoom;
            rendering.Render(opcodes);
        }

        int zoom = 1;

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            zoom += e.Delta / 300;
            if (zoom == 0) zoom = 1;

            Invalidate();
        }
    }
}