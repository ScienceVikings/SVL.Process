using System.Diagnostics;
using System.Text;

namespace SVL.Process;

public static class Process
{
    public static Task<string> Run(string fileName)
    {
        return Run(fileName, string.Empty, new Dictionary<string, string>());
    }
    public static Task<string> Run(string fileName, string arguments)
    {
        return Run(fileName, arguments, new Dictionary<string, string>());
    }
    public static Task<string> Run(string fileName, IDictionary<string, string> environmentVariables)
    {
        return Run(fileName, string.Empty, environmentVariables);
    }
    public static Task<string> Run(string fileName, string arguments, IDictionary<string,string> environmentVariables)
    {
        var startInfo = new ProcessStartInfo()
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true
        };

        if (environmentVariables.Count > 0)
            foreach (var kvp in environmentVariables)
                startInfo.EnvironmentVariables[kvp.Key] = kvp.Value;

        var promise = new TaskCompletionSource<string>();
        var outputData = new StringBuilder();
        var exited = false;
        var emptyOutput = false;

        var proc = new System.Diagnostics.Process();
        proc.StartInfo = startInfo;
        proc.EnableRaisingEvents = true;
        
        //This is a little wild because there is no guaranteed order these get fired in, so you need to check for like, everything everywhere.
        proc.Exited += (_, _) =>
        {
            exited = true;

            if (emptyOutput && exited)
                promise.SetResult(outputData.ToString());
        };
        
        proc.OutputDataReceived += (_, args) =>
        {
            if (!string.IsNullOrWhiteSpace(args.Data))
                outputData.Append(args.Data);
            else
                emptyOutput = true;

            if (emptyOutput && exited)
                promise.SetResult(outputData.ToString());
        };
        
        proc.ErrorDataReceived += (_, args) =>
        {
            if (!string.IsNullOrWhiteSpace(args.Data))
                promise.SetException(new Exception(args.Data));
        };
        
        proc.Start();
        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();

        return promise.Task;
    }
}