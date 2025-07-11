namespace EFK2.AI.Interfaces
{
    public interface INavigationAnimatorService
    {
        void SetFloat(float value, int hash);

        void SetBool(bool value, int hash);

        void SetTrigger(int hash);

        void PlayAnimation(int hash);
    }
}
