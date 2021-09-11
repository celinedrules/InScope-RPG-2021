using Character;

namespace States
{
    public interface IState
    {
        void Enter(Character.Enemy parent);
        void Update();
        void Exit();
    }
}