using Jint;

namespace KAG.Runtime
{
    public class GameModuleScriptFile : GameModuleTextFile
    {
        public GameModuleScriptFile(GameModule gameModule, byte[] buffer) : base(gameModule, buffer) { }

        /// <summary>
        /// Execute this script with the specified engine
        /// </summary>
        /// <param name="jint">The JavaScript engine to use for executing the script</param>
        public void Run()
        {
            module.engine.Execute(Text);
        }
    }
}
