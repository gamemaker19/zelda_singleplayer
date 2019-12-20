namespace GameEditor.Models
{
    public class ShapeInstance
    {
        public string name;
        public string properties;
        
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
