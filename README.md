# Jack Henry Coding Challenge


## Solution

Visual Studio 2022 .NET 6.0 Solution

### Projects

| Project | Type | Description |
| ----------- | ----------- | ----------- |
| [JackHenry.WebSite](https://github.com/JoelSkimoreMartin/JackHenry/tree/main/JackHenry.WebSite) | Front-end | Angular UI |
| [JackHenry.WebApi](https://github.com/JoelSkimoreMartin/JackHenry/tree/main/JackHenry.WebApi) | Back-end | Restful web api |
| [JackHenry.Models](https://github.com/JoelSkimoreMartin/JackHenry/tree/main/JackHenry.Models) | Back-end | Library of POCO classes |
| [JackHenry.Business](https://github.com/JoelSkimoreMartin/JackHenry/tree/main/JackHenry.Business) | Back-end | Library containing the main business logic |
| [JackHenry.Repo](https://github.com/JoelSkimoreMartin/JackHenry/tree/main/JackHenry.Repo) | Back-end | Library of data repository |
| [JackHenry.Proxy.Reddit](https://github.com/JoelSkimoreMartin/JackHenry/tree/main/JackHenry.Proxy.Reddit) | Back-end | Library responsible for calls to the Reddit api |
| [JackHenry.UnitTests](https://github.com/JoelSkimoreMartin/JackHenry/tree/main/JackHenry.UnitTests) | Unit Tests | Microsoft unit tests |

## Requirements

> ### Directions:
> Our standard interview process includes a programming exercise for all levels of positions on our team. **There's no time limit on it**, as it's intended to be able to fit around your other responsibilities (home and work), however we would expect this to not take any longer than 4-6 hours. **Simply keep us updated on progress so that we know it’s active.**  Looking forward to it!
> 
> ### Programming Assignment:
> Reddit, much like other social media platforms, provides a way for users to communicate their interests etc. For this exercise, we would like to see you build an application that listens to your choice of subreddits (best to choose one with a good amount of posts). You can use this [link](https://redditcharts.com/) to help identify one that interests you.  We'd like to see this as a .NET 6/7 application, and you are free to use any 3rd party libraries you would like.
> 
> Your app should consume the posts from your chosen subreddit in near real time and keep track of the following statistics between the time your application starts until it ends:
> * Posts with most up votes
> * Users with most posts
>  
> Your app should also provide some way to report these values to a user (periodically log to terminal, return from RESTful web service, etc.). If there are other interesting statistics you’d like to collect, that would be great. There is no need to store this data in a database; keeping everything in-memory is fine. That said, you should think about how you would persist data if that was a requirement.
> 
> To acquire near real time statistics from Reddit, you will need to continuously request data from Reddit's rest APIs.  Reddit implements rate limiting and provides details regarding rate limit used, rate limit remaining, and rate limit reset period via response headers.  Your application should use these values to control throughput in an even and consistent manner while utilizing a high percentage of the available request rate.
> 
> It’s very important that the various application processes do not block each other as Reddit can have a high volume on many of their subreddits.  The app should process posts as concurrently as possible to take advantage of available computing resources. While we are only asking to track a single subreddit, you should be thinking about his you could scale up your app to handle multiple subreddits.
> 
> While designing and developing this application, you should keep SOLID principles in mind. Although this is a code challenge, we are looking for patterns that could scale and are loosely coupled to external systems / dependencies. In that same theme, there should be some level of error handling and unit testing. The submission should contain code that you would consider production ready.
> 
> When you're finished, please put your project in a repository on either GitHub or Bitbucket and send us a link. Please be sure to provide guidance as to where the Reddit API Token values are located so that the team reviewing the code can replace/configure the value. After review, we may follow-up with an interview session with questions for you about your code and the choices made in design/implementation.
> 
> While the coding exercise is intended to be an interesting and fun challenge, we are interested in seeing your best work - aspects that go beyond merely functional code, that demonstrate professionalism and pride in your work.  We look forward to your submission!
> 
> ### Accessing the Reddit API
> 
> To get the API, register [here](https://www.reddit.com/wiki/api/)
> 
> Additional documentation can be found [here](https://www.reddit.com/dev/api/).
> 

Completed:

- [x] Met requirements
- [x] Illustrated basic microservice approach
- [x] Illustrated SOLID principles
- [x] Illustrated Unit test code coverage


Additional notes:
   * notes here

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
