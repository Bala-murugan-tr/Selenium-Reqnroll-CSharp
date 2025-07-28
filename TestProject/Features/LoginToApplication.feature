Feature: LoginToApplication


A short summary of the feature

@Regression
Scenario Outline: Login To The Application
	Given User navigated to the application
	When User entered login credentials "<Scenarios>"
	Then User can able to login successfully
Examples:
	| Scenarios         |
	| www.instagram.com |
	| www.facebook.com  |
	| www.threads.com   |

@critical
Scenario Outline: Lgoin To Given Application
	Given User navigated to the applicatio
	When User entered login credentials "<Scenarios>"
	Then User can able to login successfully
Examples:
	| Scenarios         |
	| www.instagram.com |

