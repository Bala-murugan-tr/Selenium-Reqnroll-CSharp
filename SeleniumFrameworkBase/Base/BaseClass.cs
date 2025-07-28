using OpenQA.Selenium;
using SeleniumFrameworkBase.Context;
using SeleniumFrameworkBase.Utils;

namespace SeleniumFrameworkBase.Base;
public class BaseClass {
    public IWebDriver Driver = FrameContext.Current.Driver;
    public TestLogger Logger = FrameContext.Current.Logger;
    public ExcelReader Excel = FrameContext.Current.ExcelData;
}

