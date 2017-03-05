using System.IO;

namespace FloridSword.SystemService.Tools
{
    public class ProcFsTool : IProcFsTool
    {
        public string Read(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
