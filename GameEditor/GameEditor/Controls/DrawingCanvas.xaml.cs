using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameEditor.Controls
{
    /// <summary>
    /// Interaction logic for DrawingCanvas.xaml
    /// </summary>
    public partial class DrawingCanvas : UserControl
    {
        public ImageSource imageSource;
        DrawingGroup backingStore = new DrawingGroup();

        public int mouseX;
        public int mouseY;

        public DrawingCanvas()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Render(); // put content into our backingStore
            drawingContext.DrawDrawing(backingStore);
        }

        // Call render anytime, to update visual
        // without triggering layout or OnRender()
        public void Render()
        {
            var drawingContext = backingStore.Open();
            Render(drawingContext);
            drawingContext.Close();
        }

        private void Render(DrawingContext drawingContext)
        {
            if (imageSource != null)
            {
                drawingContext.DrawImage(imageSource, new Rect(0, 0, Width, Height));
            }
            //drawingContext.DrawEllipse(new SolidColorBrush(Color.FromArgb(255, 200, 200, 200)), null, new Point(mouseX, mouseY), 12, 12);
        }
    }
}
