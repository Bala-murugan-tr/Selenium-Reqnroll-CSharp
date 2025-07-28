using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin;
using AventStack.ExtentReports.Reporter;
using Reqnroll;
using SeleniumFrameworkBase.Config;
using SeleniumFrameworkBase.Context;
using SeleniumFrameworkBase.Enums;
using SeleniumFrameworkBase.Factory;
using SeleniumFrameworkBase.Utils;

namespace SeleniumFrameworkBase.Hooks;
[Binding]
public sealed class Hook {

    private static readonly BrowserType Browser = FrameworkProperties.ExecutionBrowser;
    private static readonly ParallelScope Scope = FrameworkProperties.ParallelScope;
    private static readonly string ReportDir = FrameworkProperties.ReportDir;


    [BeforeFeature]
    public static void BeforeFeature(FeatureContext FC) {
        if (Scope != ParallelScope.FEATURES) return;
        string title = FC.FeatureInfo.Title;
        FrameContext fCtx = InitializeFrameContext(FC);
        InitializeExecution(fCtx, title);
        CreateReport(title);
        FC.Set<FrameContext>(fCtx, "FrameContext");
    }

    [AfterFeature]
    public static void AfterFeature(FeatureContext FC) {
        if (Scope != ParallelScope.FEATURES) return;
        FrameContext fCtx = FC.Get<FrameContext>("FrameContext");
        TerminateExecution(fCtx);
        FrameContext.FlushContext();
    }

    [BeforeScenario]
    public void BeforeScenario(FeatureContext FC, ScenarioContext SC) {
        Console.WriteLine(Scope);
        var scenarioName = SC.ScenarioInfo.Title.Replace(" ", "");
        string? arguments = SC.ScenarioInfo.Arguments.Values.OfType<object>().FirstOrDefault()?.ToString();
        string argument = arguments != null ? " - " + arguments : "";
        FrameContext fCtx;
        if (Scope == ParallelScope.SCENARIOS) {
            fCtx = InitializeFrameContext(FC);
            InitializeExecution(fCtx, scenarioName + argument);
            CreateReport(scenarioName + argument);
        } else {
            fCtx = FC.Get<FrameContext>("FrameContext");
        }
        fCtx.ScenarioContext = SC;
        fCtx.Logger.Log("SCENARIO STARTED :" + scenarioName + argument);
        fCtx.ScenarioName = scenarioName + argument;
        fCtx.FeatureName = FC.FeatureInfo.Title;
        var test = fCtx.ExtentReport.CreateTest(scenarioName);
        FrameContext.Current.ExtentTest = test;
        fCtx.ScenarioNode = fCtx.ExtentTest.CreateNode(scenarioName + argument);
    }

    [AfterScenario]
    public void AfterScenario(FeatureContext FC, ScenarioContext SC) {
        var status = SC.ScenarioExecutionStatus;
        var scenarioNode = FrameContext.Current.ScenarioNode;

        switch (status) {
            case ScenarioExecutionStatus.StepDefinitionPending:
                UpdateStatus(scenarioNode, Status.Skip, false, "Step definition missing !");
                break;
            case ScenarioExecutionStatus.UndefinedStep:
                UpdateStatus(scenarioNode, Status.Skip, false, "Undefined Step in Scenario");
                break;
            case ScenarioExecutionStatus.BindingError:
                UpdateStatus(scenarioNode, Status.Fail, false, SC.TestError.Message);
                break;
        }

        FrameContext fCtx;
        if (Scope == ParallelScope.SCENARIOS) {
            fCtx = FrameContext.Current;
            TerminateExecution(fCtx);
            FrameContext.FlushContext();
        } else fCtx = FC.Get<FrameContext>("FrameContext");
        fCtx.Logger.Log($"SCENARIO ENDED   : {fCtx.ScenarioName}");
        fCtx.Logger.Log("=================================================================================================");
    }
    [BeforeStep]
    public void BeforeStep(ScenarioContext SC) {
        GherkinKeyword step = new(SC.StepContext.StepInfo.StepDefinitionType.ToString());
        string title = SC.StepContext.StepInfo.Text;
        FrameContext.Current.StepNode = FrameContext.Current.ScenarioNode.CreateNode(step, title);
    }
    [AfterStep]
    public void AfterStep(ScenarioContext SC) {
        var status = SC.StepContext.Status;
        var node = FrameContext.Current.StepNode;
        bool shouldCapture = FrameworkProperties.ScreenshotOnSuccess;
        switch (status) {
            case ScenarioExecutionStatus.OK:
                UpdateStatus(node, Status.Pass, shouldCapture);
                break;
            case ScenarioExecutionStatus.TestError:
                UpdateStatus(node, Status.Fail, shouldCapture, SC.TestError.Message);
                break;
            case ScenarioExecutionStatus.Skipped:
                UpdateStatus(node, Status.Skip, false, "Skipped due :" + SC.TestError != null ? SC.TestError.Message : " unknown reason");
                break;
        }
    }
    private static FrameContext InitializeFrameContext(FeatureContext FC) {
        FrameContext.InitializeContext();
        FrameContext.Current.FeatureContext = FC;
        FrameContext.Current.ReportDir = Path.Combine(ReportDir, FC.FeatureInfo.Title);
        return FrameContext.Current!;
    }

    private static void InitializeExecution(FrameContext fCtx, string reportfileName) {
        var startTime = DateTime.Now.TimeOfDay;
        var Logger = new TestLogger(FrameContext.Current.ReportDir, reportfileName);
        var Driver = BrowserFactory.GetDriver(Browser);
        fCtx.StartTime = startTime;
        fCtx.Driver = Driver;
        fCtx.Logger = Logger;
        fCtx.Logger.Log($"BROWSER LAUNCHED : [{Browser}]");
    }
    private static void TerminateExecution(FrameContext fCtx) {
        fCtx.Driver.Quit();
        fCtx.Logger.Log($"BROWSER CLOSED : [{Browser}]");
        fCtx.ExtentReport.Flush();
        fCtx.Logger.Log("EXTENT REPORT GENERATED");
        fCtx.Logger.Flush();
    }
    private static void CreateReport(string testName) {
        FrameContext.Current.Logger.Log("EXTENT REPORTING INITIATED");
        var extentReportPath = Path.Combine(FrameContext.Current.ReportDir, testName + ".html");
        FrameContext.Current.Logger.Log($"EXTENT REPORT PATH {extentReportPath}");
        var SparkReporter = new ExtentSparkReporter(extentReportPath);
        SparkReporter.Config.DocumentTitle = testName;
        SparkReporter.Config.ReportName = testName;
        var Report = new ExtentReports();
        Report.AttachReporter(SparkReporter);
        FrameContext.Current.ExtentReport = Report;
    }
    private static void UpdateStatus(ExtentTest test, Status status, bool attachScreenshot = false, string message = "") {
        if (attachScreenshot)
            test.Log(status, message, ScreenshotUtility.Capture(FrameContext.Current.Driver));
        else test.Log(status, message);
    }
}