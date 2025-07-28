Feature: OtherApplication

A short summary of the feature


@Regressionq
Scenario Outline: Bingo  application
	Given User navigated to the application
	When User entered login credentials "<Scenarios>"
	Then User can able to login successfully
Examples:
	| Scenarios         |
	| www.youtube.com   |
	| www.gmail.com     |
	| www.google.com    |
	| meet.google.com |

