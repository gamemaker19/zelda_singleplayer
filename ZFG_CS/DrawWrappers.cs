using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class BatchDrawable : Transformable, Drawable
    {
        public VertexArray vertices;
        public Texture texture;
        public View view;

        public BatchDrawable(Texture texture, View view)
        {
            vertices = new VertexArray();
            vertices.PrimitiveType = PrimitiveType.Quads;
            this.texture = texture;
            this.view = view;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Global.window.SetView(view);

            // apply the transform
            states.Transform *= this.Transform;

            // apply the tileset texture
            states.Texture = texture;

            // draw the vertex array
            target.Draw(vertices, states);
        }
    }

    public class DrawableWrapper
    {
        public View view;
        public Shader shader;
        public Drawable drawable;

        public DrawableWrapper(View view, Shader shader, Drawable drawable)
        {
            this.view = view;
            this.shader = shader;
            this.drawable = drawable;
        }
    }

    public class DrawLayer : Transformable, Drawable
    {
        public List<Texture> textures = new List<Texture>();
        public Dictionary<Texture, BatchDrawable> batchDrawables = new Dictionary<Texture, BatchDrawable>();
        public List<DrawableWrapper> oneOffs = new List<DrawableWrapper>();

        public DrawLayer()
        {

        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var texture in textures)
            {
                target.Draw(batchDrawables[texture]);
            }
            for (int i = 0; i < oneOffs.Count; i++)
            {
                var oneOff = oneOffs[i];
                Global.window.SetView(oneOff.view);
                if (oneOff.shader != null)
                {
                    RenderStates renderStates = new RenderStates(states);
                    renderStates.Shader = oneOff.shader;
                    target.Draw(oneOff.drawable, renderStates);
                }
                else
                {
                    target.Draw(oneOff.drawable);
                }
            }
        }
    }

    public class DrawWrappers
    {
        public static Dictionary<float, DrawLayer> walDrawObjects = new Dictionary<float, DrawLayer>();

        public static void DrawLine(float x1, float y1, float x2, float y2, Color color, float thickness, float depth, bool isWorldPos = true)
        {
            View view = isWorldPos ? Global.view : Global.hudView;

            DrawLayer drawLayer;
            if (!walDrawObjects.ContainsKey(depth))
            {
                walDrawObjects[depth] = new DrawLayer();
            }
            drawLayer = walDrawObjects[depth];

            LineShape line = new LineShape(new Vector2f(x1, y1), new Vector2f(x2, y2), color, thickness);
            
            drawLayer.oneOffs.Add(new DrawableWrapper(view, null, line));
        }

        public static void DrawRect(float x1, float y1, float x2, float y2, bool filled, Color color, int thickness, float depth, bool isWorldPos = true)
        {
            View view = isWorldPos ? Global.view : Global.hudView;

            DrawLayer drawLayer;
            if (!walDrawObjects.ContainsKey(depth))
            {
                walDrawObjects[depth] = new DrawLayer();
            }
            drawLayer = walDrawObjects[depth];

            RectangleShape rect = new RectangleShape(new Vector2f(x2 - x1, y2 - y1));
            rect.Position = new Vector2f(x1, y1);
            if(filled)
            {
                rect.FillColor = color;
            }
            else
            {
                rect.FillColor = Color.Transparent;
                rect.OutlineColor = color;
                rect.OutlineThickness = thickness;
            }
            
            drawLayer.oneOffs.Add(new DrawableWrapper(view, null, rect));
        }

        public static void DrawRectWH(float x1, float y1, float w, float h, bool filled, Color color, int thickness, float depth, bool isWorldPos = true)
        {
            View view = isWorldPos ? Global.view : Global.hudView;

            DrawLayer drawLayer;
            if (!walDrawObjects.ContainsKey(depth))
            {
                walDrawObjects[depth] = new DrawLayer();
            }
            drawLayer = walDrawObjects[depth];

            RectangleShape rect = new RectangleShape(new Vector2f(w, h));
            rect.Position = new Vector2f(x1, y1);
            if (filled)
            {
                rect.FillColor = color;
            }
            else
            {
                rect.FillColor = Color.Transparent;
                rect.OutlineColor = color;
                rect.OutlineThickness = thickness;
            }

            drawLayer.oneOffs.Add(new DrawableWrapper(view, null, rect));
        }

        public static void DrawPolygon(List<Point> points, Color color, bool fill, float depth, bool camOffset, bool isWorldPos = true)
        {
            View view = isWorldPos ? Global.view : Global.hudView;

            DrawLayer drawLayer;
            if (!walDrawObjects.ContainsKey(depth))
            {
                walDrawObjects[depth] = new DrawLayer();
            }
            drawLayer = walDrawObjects[depth];

            ConvexShape shape = new ConvexShape((uint)points.Count);
            for(int i = 0; i < points.Count; i++)
            {
                shape.SetPoint((uint)i, new Vector2f(points[i].x, points[i].y));
            }
            if (fill)
            {
                shape.FillColor = color;
            }
            else
            {
                shape.OutlineColor = color;
                shape.OutlineThickness = 1;
            }

            drawLayer.oneOffs.Add(new DrawableWrapper(view, null, shape));
        }

        public static void DrawTextureImmediate(Texture texture, float sx, float sy, float sw, float sh, float dx, float dy)
        {
            Sprite sprite = new Sprite(texture, new IntRect((int)sx, (int)sy, (int)sw, (int)sh));
            sprite.Position = new Vector2f(dx, dy);
            Global.window.Draw(sprite);
        }

        public static void DrawTexture(Texture texture, float sx, float sy, float sw, float sh, float dx, float dy, float depth, 
            float cx = 0, float cy = 0, float xScale = 1, float yScale = 1, float angle = 0, float alpha = 1, Shader shader = null, bool isWorldPos = true)
        {
            View view = isWorldPos ? Global.view : Global.hudView;

            DrawLayer drawLayer;
            if (!walDrawObjects.ContainsKey(depth))
            {
                walDrawObjects[depth] = new DrawLayer();
            }
            drawLayer = walDrawObjects[depth];

            if (shader != null || cx != 0 || cy != 0 || xScale != 1 || yScale != 1 || angle != 0)
            {
                Sprite sprite = new Sprite(texture, new IntRect((int)sx, (int)sy, (int)sw, (int)sh));
                sprite.Position = new Vector2f(dx, dy);
                sprite.Origin = new Vector2f(cx, cy);
                sprite.Scale = new Vector2f(xScale, yScale);
                sprite.Color = new Color(sprite.Color.R, sprite.Color.G, sprite.Color.B, (byte)(int)(alpha * 255));
                sprite.Rotation = angle;
                drawLayer.oneOffs.Add(new DrawableWrapper(view, shader, sprite));
                return;
            }

            BatchDrawable batchDrawable = null;
            if (!drawLayer.batchDrawables.ContainsKey(texture))
            {
                drawLayer.batchDrawables[texture] = new BatchDrawable(texture, view);
                drawLayer.textures.Add(texture);
            }
            batchDrawable = drawLayer.batchDrawables[texture];

            Vertex vertex1 = new Vertex(new Vector2f(dx, dy));
            Vertex vertex2 = new Vertex(new Vector2f(dx, dy + sh));
            Vertex vertex3 = new Vertex(new Vector2f(dx + sw, dy + sh));
            Vertex vertex4 = new Vertex(new Vector2f(dx + sw, dy));

            vertex1.TexCoords = new Vector2f(sx, sy);
            vertex2.TexCoords = new Vector2f(sx, sy + sh);
            vertex3.TexCoords = new Vector2f(sx + sw, sy + sh);
            vertex4.TexCoords = new Vector2f(sx + sw, sy);

            batchDrawable.vertices.Append(vertex1);
            batchDrawable.vertices.Append(vertex2);
            batchDrawable.vertices.Append(vertex3);
            batchDrawable.vertices.Append(vertex4);
        }
    }
}
