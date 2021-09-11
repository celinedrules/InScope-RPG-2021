using Character;

namespace Control
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(Player callingController);
    }
}