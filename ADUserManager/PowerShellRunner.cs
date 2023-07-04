using System.Diagnostics;

namespace ADUserManager
{
    public class PowerShellRunner
    {
        public static string ExecutePowerShellCommand(string command, bool admin)
        {
            int timeoutMilliseconds = 2000;
            // Создаем процесс для выполнения PowerShell
            Process process = new Process();
            process.StartInfo.FileName = "powershell.exe";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Arguments = $"-ExecutionPolicy Bypass -Command \"{command}\"";
            
            // Запускаем процесс и ждем его завершения
            process.Start();
            if (!process.WaitForExit(timeoutMilliseconds))
            {
                // Превышен таймаут, процесс не завершился вовремя

                process.Kill(); // Принудительное завершение процесса
            }

            // Получаем результаты выполнения команды
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            // Возвращаем результат или сообщение об ошибке
            if (!string.IsNullOrEmpty(error))
            {
                return $"Error: {error}";
            }
            else
            {
                return output;
            }
        }
    }
}
