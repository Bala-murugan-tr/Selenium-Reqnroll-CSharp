using SeleniumFrameworkBase.Enums;

namespace SeleniumFrameworkBase.Config;

public sealed class FrameworkProperties {

    private static readonly Dictionary<string, string> _props = [];

    private static readonly string path = Path.Combine(AppContext.BaseDirectory, "Config", "framework.properties");
    static FrameworkProperties() {
        _props = ConfigLoader.LoadProps(path);
    }

    private static string Get(string key) =>
        Environment.GetEnvironmentVariable(key) ?? (_props.TryGetValue(key, out var val) ? val : throw new ArgumentException($"{key} not set, please make sure it is available in ./Config/framework.properties"));

    public static string Get(string key, string defaultVal) =>
        Environment.GetEnvironmentVariable(key) ?? _props.GetValueOrDefault(key, defaultVal);

    public static readonly string RunDate = DateTime.Now.ToString("dd-MM-yyy");
    public static readonly string RunTime = DateTime.Now.ToString("HH_mm_ss");

    public static string ArtifactsDir => Directory.GetCurrentDirectory().Split("bin")[0];
    public static string ReportDir => Path.Combine(ArtifactsDir, "Reports", RunDate, RunTime);
    public static string DownloadDir => Path.Combine(ArtifactsDir, "Downloads", RunDate, RunTime);

    //  Data Control
    public static string DataDir => Get("data.dir", Path.Combine(Environment.CurrentDirectory, "src", "test", "resources", "testdata"));
    public static string DataStrategy => Get("data.strategy", "INLINE").ToUpperInvariant();
    public static string DataKeyColumn => Get("data.keyColumn", "FIELDS");

    //  Execution Control
    public static ExecutionMode ExecutionMode => Enum.TryParse(Get("execution.mode", "LOCAL").ToUpperInvariant(), out ExecutionMode mode) ? mode : ExecutionMode.LOCAL;
    public static BrowserType ExecutionBrowser => Enum.TryParse(Get("execution.browser", "CHROME").ToUpperInvariant(), out BrowserType browser) ? browser : BrowserType.CHROME;
  
    //  Driver Configuration
    public static bool Headless => bool.Parse(Get("headless", "false"));
    public static bool Incognito => bool.Parse(Get("incognito", "false"));

    //  Retry Strategy
    public static int RetryMaxAttempts => int.Parse(Get("retry.maxAttempts", "2"));
    public static int RetryDelayMs => int.Parse(Get("retry.delayMs", "500"));

    //  Screenshot Options
    public static bool ScreenshotOnNodes => bool.Parse(Get("screenshot.onNodes", "false"));
    public static bool ScreenshotOnSuccess => bool.Parse(Get("screenshot.onSuccess", "true"));
    public static bool ScreenshotOnFailure => bool.Parse(Get("screenshot.onFailure", "true"));

    //  Grid URL
    public static Uri GridUrl {
        get {
            var url = Get("grid.url");
            if (!Uri.TryCreate(url, UriKind.Absolute, out var result))
                throw new UriFormatException("Invalid grid URL: " + url);
            return result;
        }
    }

    public static ParallelScope ParallelScope =>
#if PARALLEL_FEATURES
        ParallelScope.FEATURES;
#else
  ParallelScope.SCENARIOS;
#endif
}
