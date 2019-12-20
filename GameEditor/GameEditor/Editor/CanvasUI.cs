using GameEditor.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace GameEditor.Editor
{
    public class CanvasUI : Canvas
    {
        public ScrollViewer panel;
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

        public int PanelWidth { get { return (int)panel.Width; } }
        public int PanelHeight { get { return (int)panel.Height; } }
        public int CanvasWidth { get { return (int)pictureBox.Width; } }
        public int CanvasHeight { get { return (int)pictureBox.Height; } }

        RenderTargetBitmap bmp;

        public CanvasUI(PictureBox pictureBox, ScrollViewer panel, int canvasWidth, int canvasHeight, Color color)
        {
            this.color = color;
            this.pictureBox = pictureBox;
            pictureBox.Width = canvasWidth;
            pictureBox.Height = canvasHeight;

            baseWidth = canvasWidth;
            baseHeight = canvasHeight;

            this.panel = panel;

            //bmp = new RenderTargetBitmap(baseWidth, baseHeight, 96, 96, PixelFormats.Pbgra32);
            //pictureBox.Source = bmp;

            pictureBox.MouseMove += mouseMoveEvent;
            pictureBox.MouseDown += mouseDownEvent;
            pictureBox.MouseUp += mouseUpEvent;
            pictureBox.MouseWheel += mouseWheelEvent;
            pictureBox.MouseLeave += mouseLeaveEvent;

        }

        public void redraw()
        {
            using (var tempBitmap = new Bitmap(CanvasWidth, CanvasHeight))
            {
                using (var graphics = Graphics.FromImage(tempBitmap))
                {
                    redrawHelper(graphics);
                }
                // Copy GDI bitmap to WPF bitmap.
                var hbmp = tempBitmap.GetHbitmap();
                var options = BitmapSizeOptions.FromEmptyOptions();

                pictureBox.imageSource = Imaging.CreateBitmapSourceFromHBitmap(hbmp, IntPtr.Zero, Int32Rect.Empty, options);
            }

            pictureBox.mouseX = (int)mouseX;
            pictureBox.mouseY = (int)mouseY;
            pictureBox.Render();

            // Redraw the WPF Image control.
            //pictureBox.InvalidateMeasure();
            //pictureBox.InvalidateVisual();
        }

        protected virtual void redrawHelper(Graphics graphics)
        {
            if (this.isNoScrollZoom)
            {
                graphics.ResetTransform();
                graphics.ScaleTransform(zoom, zoom);
                float origHalfCanvasW = (CanvasWidth / zoom) * 0.5f;
                float origHalfCanvasH = (CanvasHeight / zoom) * 0.5f;
                graphics.TranslateTransform(origHalfCanvasW - (CanvasWidth * 0.5f), origHalfCanvasH - (CanvasHeight * 0.5f));
            }
            else
            {
                this.pictureBox.Width = (int)(this.baseWidth * this.zoom);
                this.pictureBox.Height = (int)(this.baseHeight * this.zoom);
                graphics.ResetTransform();
                graphics.ScaleTransform(zoom, zoom);
            }

            graphics.Clear(System.Drawing.Color.Transparent);
            Helpers.drawRect(graphics, new Models.Rect(0, 0, CanvasWidth, CanvasHeight), color);
        }

        public void setSize(int width, int height)
        {
            this.pictureBox.Width = width;
            this.pictureBox.Height = height;
            baseWidth = width;
            baseHeight = height;
        }

        public void mouseMoveEvent(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Update the mouse path that is drawn onto the Panel.
            var oldMouseX = this.mouseX;
            var oldMouseY = this.mouseY;

            var offsetLeft = 0;
            var scrollLeft = this.panel.HorizontalOffset;
            var offsetTop = 0;
            var scrollTop = this.panel.VerticalOffset;

            var rawMouseX = e.GetPosition(pictureBox).X; // - offsetLeft + scrollLeft;
            var rawMouseY = e.GetPosition(pictureBox).Y; // - offsetTop + scrollTop;

            if (!this.isNoScrollZoom)
            {
                this.mouseX = (float)rawMouseX / this.zoom;
                this.mouseY = (float)rawMouseY / this.zoom;
            }
            else
            {
                this.mouseX = (float)rawMouseX;
                this.mouseY = (float)rawMouseY;
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

        public void mouseDownEvent(object sender, MouseButtonEventArgs e)
        {
            //console.log(mouseX + "," + mouseY)
            if (e.ChangedButton == MouseButton.Left)
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
            else if (e.ChangedButton == MouseButton.Middle)
            {
                this.middlemousedown = true;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                this.rightmousedown = true;
            }
        }

        public void mouseUpEvent(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.mousedown = false;
                this.onLeftMouseUp();
            }
            else if (e.ChangedButton == MouseButton.Middle)
            {
                this.middlemousedown = false;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                this.rightmousedown = true;
            }
        }

        public void mouseWheelEvent(object sender, MouseWheelEventArgs e)
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
            var scrollTop = this.panel.VerticalOffset;
            var scrollLeft = this.panel.HorizontalOffset;
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
            //if (!string.IsNullOrEmpty(scrollTop)) this.panel.VerticalOffset = int.Parse(scrollTop);
            //if (!string.IsNullOrEmpty(scrollLeft)) this.panel.HorizontalOffset = int.Parse(scrollLeft);
        }

        public bool isHeld(Keys keyCode)
        {
            return (System.Windows.Forms.Control.ModifierKeys & Keys.Control) == keyCode;
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
