using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Editors
{
    public static class ProcessHelper
    {
        public static Process Run(string exe, string arguments, string workingDirectory = ".", bool waitExit = false)
        {
            try
            {
                var redirectStandardOutput = true;
                var redirectStandardError = true;
                var useShellExecute = false;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    redirectStandardOutput = false;
                    redirectStandardError = false;
                    useShellExecute = true;
                }

                if (waitExit)
                {
                    redirectStandardOutput = true;
                    redirectStandardError = true;
                    useShellExecute = false;
                }

                var info = new ProcessStartInfo
                {
                    FileName = exe,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = useShellExecute,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = redirectStandardOutput,
                    RedirectStandardError = redirectStandardError
                };

                var process = Process.Start(info);

                if (!waitExit)
                {
                    return process;
                }

                if (process == null)
                {
                    throw new Exception($"dir: {Path.GetFullPath(workingDirectory)}, command: {exe} {arguments}");
                }

                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"{process.StandardOutput.ReadToEnd()} {process.StandardError.ReadToEnd()}");
                }

                return process;
            }
            catch (Exception e)
            {
                throw new Exception($"dir: {Path.GetFullPath(workingDirectory)}, command: {exe} {arguments}", e);
            }
        }
    }
}