using System.Diagnostics;

namespace FloridSword.SystemService.Tools
{
    public class ProcessTool : IProcessTool
    {
        public ProcessResult Execute(string command, string arguments)
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            using (process)
            {
                process.Start();

                var stdOut = process.StandardOutput.ReadToEnd();
                var stdErr = process.StandardError.ReadToEnd();

                process.WaitForExit();

                return new ProcessResult
                {
                    ExitCode = process.ExitCode,
                    StdOut = stdOut,
                    StdErr = stdErr
                };
            }
        }
    }
}
