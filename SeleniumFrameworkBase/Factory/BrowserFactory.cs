using OpenQA.Selenium;
using SeleniumFrameworkBase.Config;
using SeleniumFrameworkBase.Enums;

namespace SeleniumFrameworkBase.Factory;

/**
 * A factory class for creating WebDriver instances based on the configured
 * execution mode.
 */

public sealed class BrowserFactory {


    /// <summary>
    ///   Retrieves a WebDriver instance tailored to the selected execution mode Local, Grid, or Cloud) and browser type(e.g., Chrome, Firefox, Edge). </summary>
    /// <param name="browser">The browser type to initialize</param>
    /// <returns>A configured WebDriver instance suitable for the current execution</returns>
    public static WebDriver GetDriver(BrowserType browser) {
        ExecutionMode mode = FrameworkProperties.ExecutionMode;
        switch (mode) {
            case ExecutionMode.LOCAL:
                return LocalDriverFactory.CreateDriver(browser);
            case ExecutionMode.GRID:
                return GridDriverFactory.CreateDriver(browser);
            default:
                return CloudDriverFactory.CreateDriver(browser);
        }
    }

}
