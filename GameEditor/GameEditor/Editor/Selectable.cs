namespace GameEditor.Editor
{
    public interface Selectable
    {
        bool isSelected { get; set; }

        void move(float deltaX, float deltaY);

        void resizeCenter(float w, float h);
    }
}
