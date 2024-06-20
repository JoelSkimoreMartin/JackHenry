# Jack Henry Coding Challenge


## Solution

Visual Studio 2022 .NET Core 3.1 Solution

### Projects

1. [Clearent](https://github.com/JoelSkimoreMartin/Clearent/tree/master/Clearent)
    * Library containing the main business logic
1. [Clearent.Models](https://github.com/JoelSkimoreMartin/Clearent/tree/master/Clearent.Models)
    * Library of POCO classes
1. [Clearent.Repo](https://github.com/JoelSkimoreMartin/Clearent/tree/master/Clearent.Repo)
    * Library of data repository
1. [Clearent.Test](https://github.com/JoelSkimoreMartin/Clearent/tree/master/Clearent.Test)
    * Nunit test cases
1. [Clearent.WebApi](https://github.com/JoelSkimoreMartin/Clearent/tree/master/Clearent.WebApi)
   * Web api

## Requirements
> Here is the Clearent code challenge. You don’t need to write a user interface, just create unit tests using your favorite testing framework (xUnit, JUnit, NUnit, etc.). The tests running is all that is needed to “prove” the example. While this solution can be solved very simply, your ability to use and explain your use of SOLID programming principles is the real test.
> 
> There are two requirements to be considered:
> * You MUST use a testing framework (xUnit, JUnit, NUnit, etc.)
> * Please post your answer on http://github.com 
> 
> ### Write a program program that calculates Credit Card interest for a Person.  
>  
> Visa gets 10%
>  
> MC gets 5% interest
>  
> Discover gets 1% interest
>  
>  
> Each Card Type can have multiple cards and there can be multiple Wallets for a Person. 
>  
>  
> NOTE:  SIMPLE INTEREST for this test case (means 1 month of interest, if the interest rate is 10% and the amount is 100.00 – interest in this case would be 10.00) 
> 
>  
> Here are the test Cases - 
>  
> *	1 person has 1 wallet and 3 cards (1 Visa, 1 MC 1 Discover) – Each Card has a balance of $100 – calculate the total interest (simple interest) for this person and per card. 
> *	1 person has 2 wallets  Wallet 1 has a Visa and Discover , wallet 2 a MC -  each card has $100 balance - calculate the total interest(simple interest) for this person and interest per wallet
> *	2 people have **1 wallet each**,  person **1 has 1 wallet** , with 2 cards MC and visa **person 2 has 1 wallet** – 1 visa and 1 MC -  each card has $100 balance - calculate the total interest(simple interest) for each person and interest per wallet
> 


Completed:

- [x] Met requirements
- [x] Prove functionality validity through unit tests
- [x] Illustrated SOLID principles


Additional notes:
   * Added the following in order to better illustrate the SOLID principles
      * Repository layer
      * Web API
      * Reporter classes to format execution results
   * Only created unit tests for the requirement functionality and not for these additional pieces of code
   * The code is light on comments because I'm relying on this document to square the code to the requirements.
      * Want to avoid a game of hide-and-go-seek to find what is where.


## SOLID principles in code


### Single responsibility principle

1. POCO classes
    * Only responsible for storing data in the shape of Person -> Wallet -> CreditCard
        * [CreditCard class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Models/CreditCard.cs)
        * [Person class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Models/Person.cs)
        * [Wallet class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Models/Wallet.cs)
1. [TestDataFactory class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Models/TestData/TestDataFactory.cs)
    * Only responsible for building test data that can be used in both the test cases and web api
1. Group classes / enum
    * Only responsible for grouping results from ICardResolver for use by the reporter
        * [Group class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Groupers/Group.cs)
        * [GroupCalculation class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Groupers/GroupCalculation.cs)
        * [Grouper class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Groupers/Grouper.cs)
        * [Grouping enum](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Models/TestData/Grouping.cs)
1. [JsonRepo class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Repo/JsonRepo.cs)
    * Only responsible for retrieving data from a json data source
1. [CardRepo class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Repo/CardRepo.cs)
    * Only responsible for retrieving credit card info from the datasource
1. DependencyInjectionExtensions classes
    * Only responsible for registering dependency injected classes for a library
        * [DependencyInjectionExtensions class (Clearent library)](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Extensions/DependencyInjectionExtensions.cs)
        * [DependencyInjectionExtensions class (Clearent.Repo library)](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Repo/Extensions/DependencyInjectionExtensions.cs)
1. [SimpleInterestCalculator class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/SimpleInterestCalculator.cs)
    * Only responsible for calculating simple interest for credit cards
1. [JsonReporter class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Reporters/JsonReporter.cs)
    * Only responsible for reporting calculated results in a json format
1. [StringReporter class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Reporters/StringReporter.cs)
    * Only responsible for reporting calculated results in a sentance-based string format
1. [XmlReporter class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Reporters/XmlReporter.cs)
    * Only responsible for reporting calculated results in an xml format


### Open–closed principle


1. Through an abstract base class:
    * [BaseReporter class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Reporters/BaseReporter.cs)
        * Allows new reporters to be added by inheriting from this class
1. Through an interface:
    * [ICardResolver interface](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Models/Interfaces/ICardResolver.cs)
        * Allows new data structures to have the simple interest calculated, by supporting this interface.


### Liskov substitution principle


1. Reporter classes
    * [TestController.cs](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.WebApi/Controllers/TestController.cs)
        * Commented code shows how reporters can be swappped, interchangeably
    * [Clearent/Reporters](https://github.com/JoelSkimoreMartin/Clearent/tree/master/Clearent/Reporters)
        * Implementation of clases that can be interchangeably used
1. ICardResolver interface being supported on POCO objects to retrieve credit cards
    * [ICardResolver interface](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Models/Interfaces/ICardResolver.cs)
        * Defines resolver interface
    * [Person class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Models/Person.cs)
        * Resolves to all credit cards for a person
    * [Wallet class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Models/Wallet.cs)
        * Resolves to all credit cards in a wallet
    * [CreditCard class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Models/CreditCard.cs)
        * Resolves to the current credit card
    * [SimpleInterestCalculator class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/SimpleInterestCalculator.cs)
        * Use of ICardResolver shows how the different POCO classes can be evaluated interchangeably in order to calculate the simple interest rate, depending on the scope supplied


### Interface segregation principle


1. Following interfaces are light weight
    * [ICardResolver](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Models/Interfaces/ICardResolver.cs)
    * [ICardRepo](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Repo/Interfaces/ICardRepo.cs)
    * [IGroupCalculator](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Interfaces/IGroupCalculator.cs)
    * [IJsonRepo](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Repo/Interfaces/IJsonRepo.cs)
    * [IReporter](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Interfaces/IReporter.cs)
    * [ISimpleInterestCalculator](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Interfaces/ISimpleInterestCalculator.cs)


### Dependency inversion principle


1. Initialze dependency injection
    * [Startup class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.WebApi/Startup.cs)
    * [DependencyInjectionExtensions class (Clearent library)](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Extensions/DependencyInjectionExtensions.cs)
    * [DependencyInjectionExtensions class (Clearent.Repo library)](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Repo/Extensions/DependencyInjectionExtensions.cs)
1. Constructor dependency injection in practice
    * [TestController class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.WebApi/Controllers/TestController.cs)
    * [GroupCalculator class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/GroupCalculator.cs)
    * [SimpleInterestCalculator class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/SimpleInterestCalculator.cs)
    * [JsonReporter class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Reporters/JsonReporter.cs)
    * [StringReporter class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Reporters/StringReporter.cs)
    * [XmlReporter class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent/Reporters/XmlReporter.cs)
    * [CardRepo class](https://github.com/JoelSkimoreMartin/Clearent/blob/master/Clearent.Repo/CardRepo.cs)
