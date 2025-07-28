using OpenQA.Selenium;
using SeleniumFrameworkBase.Config;
using SeleniumFrameworkBase.Enums;

namespace SeleniumFrameworkBase.Factory;

/**
 * A factory class for creating WebDriver instances based on the configured
 * execution mode.
 */

public sealed class BrowserFactory {


    public static WebDriver GetDriver(BrowserType browser, string testcaseName) {
        ExecutionMode mode = FrameworkProperties.ExecutionMode;
        switch (mode) {
            case ExecutionMode.LOCAL:
                return LocalDriverFactory.CreateDriver(browser);
            case ExecutionMode.GRID:
                return GridDriverFactory.CreateDriver(browser, testcaseName);
            default:
                return CloudDriverFactory.CreateDriver(browser);
        }
    }

}
