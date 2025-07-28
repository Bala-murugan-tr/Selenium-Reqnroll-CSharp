Feature: ViewHomePage

A short summary of the feature


@Regressionq
Scenario Outline: View  application
	Given User navigated to the application
	When User entered login credentials "<Scenarios>"
	Then User can able to login successfully
Examples:
	| Scenarios           |
	| www.trgan.com       |
	| linkedin.trgan.com  |
	| instagram.trgan.com |

