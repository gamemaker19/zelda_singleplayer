namespace GameEditor.Models
{
    public class ShapeInstance
    {
        public string name { get; set; }
        public string properties { get; set; }

        public ShapeInstance(string name, string properties)
        {
            this.name = name;
            this.properties = properties;
        }

        /*
        public draw(ctx: CanvasRenderingContext2D)
        {

        }
        */
    }
}
