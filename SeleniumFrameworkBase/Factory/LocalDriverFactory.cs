using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using SeleniumFrameworkBase.Config;
using SeleniumFrameworkBase.Enums;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace SeleniumFrameworkBase.Factory;
public static class LocalDriverFactory {
    public static WebDriver CreateDriver(BrowserType browser) {
        WebDriver driver;

        try {
            switch (browser) {
                case BrowserType.CHROME:
                    new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                    var chromeOptions = new ChromeOptions();

                    //  Download preferences
                    var chromePrefs = new Dictionary<string, object>
                    {
                        { "download.default_directory", FrameworkProperties.DownloadDir },
                        { "plugins.always_open_pdf_externally", true },
                        { "profile.default_content_setting_values.notifications", 2 },
                        { "profile.default_content_setting_values.geolocation", 2 }
                    };
                    chromeOptions.AddUserProfilePreference("prefs", chromePrefs);

                    //  Suppress automation UI
                    chromeOptions.AddExcludedArgument("enable-automation");
                    chromeOptions.AddAdditionalOption("useAutomationExtension", false);

                    //  Headless/Incognito
                    if (FrameworkProperties.Headless) {
                        chromeOptions.AddArguments("--headless=new", "--window-size=1920,1080",
                            "--disable-gpu", "--disable-dev-shm-usage", "--disable-software-rasterizer");
                    } else {
                        chromeOptions.AddArgument("--start-maximized");
                    }

                    if (FrameworkProperties.Incognito) {
                        chromeOptions.AddArgument("--incognito");
                    }

                    //  Performance tuning
                    chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
                    chromeOptions.AddArguments("--disable-features=Translate,MediaRouter,VizDisplayCompositor",
                        "--disable-notifications", "--disable-popup-blocking", "--disable-extensions",
                        "--disable-background-networking", "--disable-sync", "--disable-default-apps");

                    driver = new ChromeDriver(chromeOptions);
                    break;

                case BrowserType.FIREFOX:
                    new DriverManager().SetUpDriver(new FirefoxConfig(), VersionResolveStrategy.MatchingBrowser);
                    var firefoxOptions = new FirefoxOptions();

                    // Preferences
                    firefoxOptions.SetPreference("browser.download.folderList", 2);
                    firefoxOptions.SetPreference("browser.download.dir", FrameworkProperties.DownloadDir);
                    firefoxOptions.SetPreference("pdfjs.disabled", true);
                    firefoxOptions.SetPreference("geo.enabled", false);
                    firefoxOptions.SetPreference("dom.webnotifications.enabled", false);

                    if (FrameworkProperties.Headless)
                        firefoxOptions.AddArgument("--headless");
                    else
                        firefoxOptions.AddArgument("--start-maximized");

                    if (FrameworkProperties.Incognito)
                        firefoxOptions.AddArgument("-private");

                    firefoxOptions.PageLoadStrategy = PageLoadStrategy.Eager;

                    driver = new FirefoxDriver(firefoxOptions);
                    break;

                case BrowserType.EDGE:
                    new DriverManager().SetUpDriver(new EdgeConfig(), VersionResolveStrategy.MatchingBrowser);
                    var edgeOptions = new EdgeOptions();

                    var edgePrefs = new Dictionary<string, object>
                    {
                        { "download.default_directory", FrameworkProperties.DownloadDir },
                        { "plugins.always_open_pdf_externally", true },
                        { "profile.default_content_setting_values.notifications", 2 },
                        { "profile.default_content_setting_values.geolocation", 2 }
                    };
                    edgeOptions.AddAdditionalOption("prefs", edgePrefs);
                    edgeOptions.AddExcludedArgument("enable-automation");
                    edgeOptions.AddAdditionalOption("useAutomationExtension", false);

                    if (FrameworkProperties.Headless)
                        edgeOptions.AddArguments("--headless=new", "--window-size=1920,1080",
                            "--disable-gpu", "--disable-dev-shm-usage", "--disable-software-rasterizer");
                    else
                        edgeOptions.AddArgument("--start-maximized");

                    if (FrameworkProperties.Incognito)
                        edgeOptions.AddArgument("--inprivate");

                    edgeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
                    edgeOptions.AddArguments("--disable-features=Translate,MediaRouter,VizDisplayCompositor",
                        "--disable-notifications", "--disable-popup-blocking", "--disable-extensions",
                        "--disable-background-networking", "--disable-default-apps", "--remote-allow-origins=*");

                    driver = new EdgeDriver(edgeOptions);
                    break;

                default:
                    throw new ArgumentException($"Unsupported browser type: {browser}");
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        } catch (Exception ex) {
            throw new InvalidOperationException($"Driver initialization failed for browser: {browser}", ex);
        }

        return driver;
    }
}
