using System.Windows.Media;
using ff14bot.Helpers;

namespace EatMeWpf.Utilities
{
    public static class Logger
    {
        /// <summary>
        ///     Output a string to the RB Log Window
        /// </summary>
        /// <param name="text">Text to output to the RB Window and Log file</param>
        public static void Log(string text)
        {
            Logging.Write(Colors.Cyan, $"[Eat Me] {text}");
        }
    }
}