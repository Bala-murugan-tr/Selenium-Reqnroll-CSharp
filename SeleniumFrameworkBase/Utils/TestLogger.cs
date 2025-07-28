using System.Text;
namespace SeleniumFrameworkBase.Utils;

public class TestLogger {
    private readonly string filePath;
    private readonly StringBuilder buffer = new();

    public TestLogger(string folder, string testName) {
        Directory.CreateDirectory(folder);
        filePath = Path.Combine(folder, $"{testName}.log");
        try {
            File.AppendAllText(filePath, string.Empty);
        } catch (IOException e) {
            Console.Error.WriteLine("Log file creation failed: " + e.Message);
        }
    }

    public void Log(string message) {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        buffer.AppendLine($"[{timestamp}]- {message}");
    }

    public void Flush() {
        try {
            File.AppendAllText(filePath, buffer.ToString());
        } catch (IOException e) {
            Console.Error.WriteLine("Log flush failed: " + e.Message);
        }
    }
}