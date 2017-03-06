namespace FloridSword.SystemService.Tools
{
    public interface IProcessTool
    {
        ProcessResult Execute(string command, string arguments);
    }
}
