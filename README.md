# Transaction Digest Coding Exercise

## Build and Run Instructions

1. Install Node.js v24.22.1 from https://nodejs.org/en/download
2. Clone repository and open in Visual Studio
3. Go to Tools > Options > Projects and Solutions > Web Package Management and click External Web Tools
4. in 'Locations of external tools', make sure $(PATH) is at the top
5. Set TransactionDigest.Server as the Startup Project
6. Open VS Package Manager Console and make sure the default project is set to TransactionDigest.Data
7. Run Update-Database in PM Console
8. Start the application by running TransactionDigest.Server in Visual Studio

## Design Decision and Assumptions

For this project I wanted to show off some of my software design experience by using a layered architecture to keep the code loosely coupled and easily unit-testable. I decided to break the layers into separate class libraries to enforce strict boundaries and to explicitly define the project dependency heirarchy. Because project dependencies are explicitly defined, you can easily see that the Domain layer has no dependencies, while the Server project in the Client layer depends on every layer below it.

I isolated the business logic in the Application layer, specifically TransactionService.cs. I did this to keep the application loosely coupled, and also to make the business logic easy to unit test. This is also why I chose to use a repository pattern for data access. When unit testing TransactionService, the repository can easily be mocked to provide test data.

