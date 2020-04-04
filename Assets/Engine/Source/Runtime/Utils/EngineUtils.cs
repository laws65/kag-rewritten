namespace KAG.Runtime.Utils
{
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
