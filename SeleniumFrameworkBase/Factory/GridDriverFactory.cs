using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using SeleniumFrameworkBase.Config;
using SeleniumFrameworkBase.Enums;

namespace SeleniumFrameworkBase.Factory;
public static class GridDriverFactory {
    public static WebDriver CreateDriver(BrowserType browser, string testcaseName) {
        DriverOptions options;

        switch (browser) {
            case BrowserType.CHROME:
                var chrome = new ChromeOptions();
               
                chrome.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2);
                chrome.AddUserProfilePreference("profile.default_content_setting_values.geolocation", 2);
                chrome.AddUserProfilePreference("download.default_directory", FrameworkProperties.DownloadDir);
                chrome.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                chrome.AddExcludedArgument("enable-automation");
                chrome.AddAdditionalOption("useAutomationExtension", false);
                chrome.AddAdditionalOption("se:name", testcaseName);

                if (FrameworkProperties.Headless)
                    chrome.AddArguments("--headless=new", "--window-size=1920,1080", "--disable-gpu",
                                        "--disable-dev-shm-usage", "--disable-software-rasterizer");
                else
                    chrome.AddArgument("--start-maximized");

                if (FrameworkProperties.Incognito)
                    chrome.AddArgument("--incognito");

                chrome.PageLoadStrategy = PageLoadStrategy.Eager;
                options = chrome;
                break;

            case BrowserType.FIREFOX:
                var firefox = new FirefoxOptions();
                firefox.SetPreference("browser.download.folderList", 2);
                firefox.SetPreference("browser.download.dir", FrameworkProperties.DownloadDir);
                firefox.SetPreference("pdfjs.disabled", true);
                firefox.SetPreference("geo.enabled", false);
                firefox.SetPreference("dom.webnotifications.enabled", false);

                if (FrameworkProperties.Headless)
                    firefox.AddArgument("--headless");
                else
                    firefox.AddArgument("--start-maximized");

                if (FrameworkProperties.Incognito)
                    firefox.AddArgument("-private");

                firefox.PageLoadStrategy = PageLoadStrategy.Eager;
                options = firefox;
                break;

            case BrowserType.EDGE:
                var edge = new EdgeOptions();
                var edgePrefs = new Dictionary<string, object>
                {
                    { "download.default_directory", FrameworkProperties.DownloadDir },
                    { "plugins.always_open_pdf_externally", true },
                    { "profile.default_content_setting_values.notifications", 2 },
                    { "profile.default_content_setting_values.geolocation", 2 }
                };
                edge.AddAdditionalOption("prefs", edgePrefs);
                edge.AddExcludedArgument("enable-automation");
                edge.AddAdditionalOption("useAutomationExtension", false);

                if (FrameworkProperties.Headless)
                    edge.AddArguments("--headless=new", "--window-size=1920,1080", "--disable-gpu",
                                      "--disable-dev-shm-usage", "--disable-software-rasterizer");
                else
                    edge.AddArgument("--start-maximized");

                if (FrameworkProperties.Incognito)
                    edge.AddArgument("--inprivate");

                edge.PageLoadStrategy = PageLoadStrategy.Eager;
                edge.AddArgument("--remote-allow-origins=*");

                options = edge;
                break;

            default:
                throw new ArgumentException($"Unsupported browser type: {browser}");
        }
      
        var gridUri = FrameworkProperties.GridUrl;
        return new RemoteWebDriver(gridUri, options);
    }
}
