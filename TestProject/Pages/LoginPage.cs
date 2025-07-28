using OpenQA.Selenium;
using SeleniumFrameworkBase.Base;
using SeleniumFrameworkBase.Config;
using SeleniumFrameworkBase.Context;


namespace TestProject.Pages;
internal class LoginPage : BaseClass {
    private readonly By btnLogin = By.Id("");
    private readonly By txtUserName = By.Id("");
    private readonly By txtPassWord = By.Id("");


    internal void EnterUserName() {
        var x = FrameworkProperties.ArtifactsDir;
    }
    internal void EnterPassWord() {
    }
    internal void ClickLoginBtn() {

    }
    internal void Login(string url) {
        FrameContext.Current.Driver.Url = url;
        FrameContext.Current.Logger.Log(url);
    }
}
