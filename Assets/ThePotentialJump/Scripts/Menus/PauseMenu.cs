namespace ThePotentialJump.Menus
{
    public class PauseMenu : Utilities.MonoSingleton<PauseMenu>
    {
        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
        }
    }

}