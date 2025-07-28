using Reqnroll;
using SeleniumFrameworkBase.Base;
using TestProject.Pages;

namespace TestProject.Steps;
[Binding]
public class LoginPageSteps : BaseClass {
    private readonly LoginPage LoginPage;

    public LoginPageSteps() : base() {
        LoginPage = new LoginPage();
    }


    [Given("User navigated to the application")]
    public void GivenUserNavigatedToTheApplication() {
        //LoginPage.Login("https://www.trgan.com");
    }

    [When("User entered login credentials {string}")]
    public void WhenUserEnteredLoginCredentials(string Scenarios) {
        LoginPage.Login("https://" + Scenarios);
        Thread.Sleep(3000);
    }

    [Then("User can able to login successfully")]
    public void ThenUserCanAbleToLoginSuccessfully() {
        Console.WriteLine("viewed homepage");
    }
    [When("User entered login credentials")]
    public void WhenUserEnteredLoginCredentials() {

    }

}
