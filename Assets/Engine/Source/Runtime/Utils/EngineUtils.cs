namespace KAG.Runtime.Utils
{
    using KAG.Runtime.Modules;

    public class EngineUtils : BaseUtils
    {
        public EngineUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetGlobalObject("Engine", this);
        }

        public void Include(string filePath)
        {
            module.Execute(filePath);
        }
    }
}
