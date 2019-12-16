using LevelEditor_CS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor_CS.Editor
{
    public class CanvasUI
    {
        public Panel panel;
        public PictureBox pictureBox;

        public bool ctrlHeld = false;
        public float zoom = 1;
        public float noScrollZoom = 1;
        public bool mousedown = false;
        public bool middlemousedown = false;
        public bool rightmousedown = false;
        public HashSet<Keys> keysHeld = new HashSet<Keys>();
        public Color color = Color.White;
        public bool isNoScrollZoom = false;  //Determines whether to use zoom with an outer scroll div, or just hard canvas zoom (without outer scrollbars)
        public float offsetX = 0;
        public float offsetY = 0;

        //All these mouse values below are "normalized" to the current zoom. Only rawMouseX and rawMouseY variables are the real values
        public float mouseX = 0;
        public float mouseY = 0;
        public float deltaX = 0;
        public float deltaY = 0;
        public float lastClickX = 0;
        public float lastClickY = 0;
        public float dragStartX;
        public float dragStartY;
        public float dragEndX;
        public float dragEndY;

        public int baseWidth;
        public int baseHeight;

        public float dragLeftX { get { return Math.Min(this.dragStartX, this.dragEndX); } }
        public float dragRightX { get { return Math.Max(this.dragStartX, this.dragEndX); } }
        public float dragTopY { get { return Math.Min(this.dragStartY, this.dragEndY); } }
        public float dragBotY { get { return Math.Max(this.dragStartY, this.dragEndY); } }

        public int PanelWidth { get { return panel.Width; } }
        public int PanelHeight { get { return panel.Height; } }
        public int CanvasWidth { get { return pictureBox.Width; } }
        public int CanvasHeight { get { return pictureBox.Height; } }

        public CanvasUI(PictureBox pictureBox, Panel panel, int canvasWidth, int canvasHeight, Color color)
        {
            this.color = color;
            this.pictureBox = pictureBox;
            pictureBox.Width = canvasWidth;
            pictureBox.Height = canvasHeight;

            baseWidth = canvasWidth;
            baseHeight = canvasHeight;

            this.panel = panel;
            panel.AutoScroll = true;
            pictureBox.Parent = panel;

            pictureBox.Paint += pictureBox_Paint;
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;

            pictureBox.MouseMove += mouseMoveEvent;
            pictureBox.MouseDown += mouseDownEvent;
            pictureBox.MouseUp += mouseUpEvent;
            pictureBox.MouseWheel += mouseWheelEvent;
            pictureBox.MouseLeave += mouseLeaveEvent;

        }

        public virtual void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (this.isNoScrollZoom)
            {
                e.Graphics.ResetTransform();
                e.Graphics.ScaleTransform(zoom, zoom);
                float origHalfCanvasW = (CanvasWidth / zoom) * 0.5f;
                float origHalfCanvasH = (CanvasHeight / zoom) * 0.5f;
                e.Graphics.TranslateTransform(origHalfCanvasW - (CanvasWidth * 0.5f), origHalfCanvasH - (CanvasHeight * 0.5f));
            }
            else
            {
                this.pictureBox.Width = (int)(this.baseWidth * this.zoom);
                this.pictureBox.Height = (int)(this.baseHeight * this.zoom);
                e.Graphics.ResetTransform();
                e.Graphics.ScaleTransform(zoom, zoom);
            }

            e.Graphics.Clear(Color.Transparent);
            Helpers.drawRect(e.Graphics, new Rect(0, 0, CanvasWidth, CanvasHeight), color);
        }

        public void setSize(int width, int height)
        {
            this.pictureBox.Width = width;
            this.pictureBox.Height = height;
            baseWidth = width;
            baseHeight = height;
        }

        public void mouseMoveEvent(object sender, MouseEventArgs e)
        {
            // Update the mouse path that is drawn onto the Panel.
            var oldMouseX = this.mouseX;
            var oldMouseY = this.mouseY;

            var offsetLeft = 0;
            var scrollLeft = this.panel.HorizontalScroll.Value;
            var offsetTop = 0;
            var scrollTop = this.panel.VerticalScroll.Value;

            var rawMouseX = e.X - offsetLeft + scrollLeft;
            var rawMouseY = e.Y - offsetTop + scrollTop;

            if (!this.isNoScrollZoom)
            {
                this.mouseX = rawMouseX / this.zoom;
                this.mouseY = rawMouseY / this.zoom;
            }
            else
            {
                this.mouseX = rawMouseX;
                this.mouseY = rawMouseY;
            }

            this.mouseX += this.offsetX;
            this.mouseY += this.offsetY;

            this.deltaX = this.mouseX - oldMouseX;
            this.deltaY = this.mouseY - oldMouseY;

            if (this.mousedown)
            {
                this.dragEndX = this.mouseX;
                this.dragEndY = this.mouseY;
            }

            this.onMouseMove(this.deltaX, this.deltaY);
        }

        public void mouseDownEvent(object sender, MouseEventArgs e)
        {
            //console.log(mouseX + "," + mouseY)
            if (e.Button == MouseButtons.Left)
            {
                if (!this.mousedown)
                {
                    this.lastClickX = this.dragStartX;
                    this.lastClickY = this.dragStartY;
                    this.dragStartX = this.mouseX;
                    this.dragStartY = this.mouseY;
                    this.dragEndX = this.mouseX;
                    this.dragEndY = this.mouseY;
                }
                this.mousedown = true;
                this.onLeftMouseDown();
            }
            else if (e.Button == MouseButtons.Middle)
            {
                this.middlemousedown = true;
            }
            else if (e.Button == MouseButtons.Right)
            {
                this.rightmousedown = true;
            }
        }

        public void mouseUpEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mousedown = false;
                this.onLeftMouseUp();
            }
            else if (e.Button == MouseButtons.Middle)
            {
                this.middlemousedown = false;
            }
            else if (e.Button == MouseButtons.Right)
            {
                this.rightmousedown = true;
            }
        }

        public void mouseWheelEvent(object sender, MouseEventArgs e)
        {
            if (this.isHeld(Keys.Control))
            {
                var delta = -(e.Delta / 180);
                this.zoom += delta;
                if (this.zoom < 1) this.zoom = 1;
                if (this.zoom > 5) this.zoom = 5;
                this.redraw();
                this.onMouseWheel(delta);
            }
        }

        public void mouseLeaveEvent(object sender, EventArgs e)
        {
            this.mousedown = false;
            this.onMouseLeave();
        }

        bool once = false;

        public virtual void redraw()
        {
            pictureBox.Invalidate();
        }

        public GridRect getDragGridRect()
        {
            return new GridRect((int)Mathf.Floor(this.dragTopY / Consts.TILE_WIDTH), (int)Mathf.Floor(this.dragLeftX / Consts.TILE_WIDTH),
                (int)Mathf.Floor(this.dragBotY / Consts.TILE_WIDTH), (int)Mathf.Floor(this.dragRightX / Consts.TILE_WIDTH));
        }

        public GridCoords getMouseGridCoords()
        {
            return new GridCoords((int)Mathf.Floor(this.mouseY / Consts.TILE_WIDTH), (int)Mathf.Floor(this.mouseX / Consts.TILE_WIDTH));
        }

        public GridCoords getMouseGridCoordsCustomWidth(float width)
        {
            return new GridCoords((int)Mathf.Floor(this.mouseY / width), (int)Mathf.Floor(this.mouseX / width));
        }

        public void saveScrollPos(string imageKey)
        {
            var scrollTop = this.panel.VerticalScroll.Value;
            var scrollLeft = this.panel.HorizontalScroll.Value;
            var scrollTopKey = "scrollTop";
            var scrollLeftKey = "scrollLeft";
            Helpers.setStorageKey(scrollTopKey, scrollTop.ToString());
            Helpers.setStorageKey(scrollLeftKey, scrollLeft.ToString());
        }

        public void loadScrollPos(string imageKey)
        {
            var scrollTopKey = "scrollTop";
            var scrollLeftKey = "scrollLeft";
            var scrollTop = Helpers.getStorageKey(scrollTopKey);
            var scrollLeft = Helpers.getStorageKey(scrollLeftKey);
            if (!string.IsNullOrEmpty(scrollTop)) this.panel.VerticalScroll.Value = int.Parse(scrollTop);
            if (!string.IsNullOrEmpty(scrollLeft)) this.panel.HorizontalScroll.Value = int.Parse(scrollLeft);
        }

        public bool isHeld(Keys keyCode)
        {
            return (Control.ModifierKeys & Keys.Control) == keyCode;
        }

        public virtual void onMouseLeave() { }

        public virtual void onMouseMove(float deltaX, float deltaY)
        {
        }

        public virtual void onLeftMouseDown()
        {
        }

        public virtual void onLeftMouseUp()
        {
        }

        public virtual void onKeyDown(Keys key, bool firstFrame)
        {
        }

        public virtual void onKeyUp(Keys key)
        {

        }

        public virtual void onMouseWheel(float delta)
        {

        }
    }

}
