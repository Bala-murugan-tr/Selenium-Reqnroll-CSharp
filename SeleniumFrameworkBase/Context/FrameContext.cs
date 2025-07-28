using AventStack.ExtentReports;
using OpenQA.Selenium;
using Reqnroll;
using SeleniumFrameworkBase.Utils;

namespace SeleniumFrameworkBase.Context;
public class FrameContext {
#nullable disable
    private static readonly ThreadLocal<FrameContext> _context = new();
#nullable enable
    public static FrameContext Current => _context.Value!;
    public static void InitializeContext() => _context.Value = new FrameContext();
    public static void FlushContext() => _context.Value = null;
    public ScenarioContext ScenarioContext { get; set; } = null!;
    public FeatureContext FeatureContext { get; set; } = null!;
    public TestLogger Logger { get; set; } = null!;
    public string ReportDir { get; set; } = null!;
    public IWebDriver Driver { get; set; } = null!;
    //public ExtentReports FeatureReport { get; set; } = null!;
    public ExtentReports ExtentReport { get; set; } = null!;
    public ExtentTest ExtentTest { get; set; } = null!;
    public ExtentTest ScenarioNode { get; set; } = null!;
    public ExtentTest StepNode { get; set; } = null!;
    public ExcelReader ExcelData { get; set; } = null!;
    public string Group { get; set; } = null!;
    public TimeSpan StartTime { get; set; }
    public string FeatureName { get; set; } = null!;
    public string ScenarioName { get; set; } = null!;

}
