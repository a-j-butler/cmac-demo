# CMAC Interview Demo

## Basic Overview

The solution is an update to the standard WebAssembly template from Visual Studio and utilises .NET 9 for the framework. As such the project structures may well have a familiar look and feel to them.

The solution utilises the following project:

- cmac.demo (The Host/Server application, this also serves as pseudo client website for hosting the iframe)
- cmac.demo.Client (This is the client WebAssembly project which hosts the manage-donations page and the DonationsForm and DonationsList components)
- cmac.demo.data (This is the EntityFramework data library utilising for simplicity an InMemory database with data seeding at startup)
- cmac.demo.datamodels (This is where the data contract is stored ensuring it is portable and not 3rd party library dependant)
- cmac.demo.viewmodels (This is where the models for use in the Blazor forms are stored again keeping them separate and portable)

## 3rd Party Libraries

For all intents and purposes, there's only one library in play, if you don't consider the EF Core library in the same way. This is the **Radzen.Blazor** nuget package.

## Operation

The host application serves a donations page, containing an iframe, which in turn utilises the manage-donations page within the WebAssembly client application.

Within this simple page, are two fairly basic components; DonationForm and DonationList, which are hopefully pretty self explanatory.

In order to fulfil the database action requirements, inline endpoints specifications have been utilised for brevity in this demo.

#### DonationForm.razor

This component utilises Radzen form elements to allow the user to enter donation information and persist it to the InMemory database. There are also validation and notification pop-ups to support this process.

The component also makes use of an EventCallback to allow for the refreshing of the DonationList (or any other component for that matter) once a donation has been added to the database.

#### DonationList.razor

This component utilises the Radzen Datagrid component and also has a search field for performing filtering of the data based on the requirements (filter on Payment Method and Notes).

This component exposes a refresh method which can then be utilised by other components or hosting pages to force a refresh of the data based on other events.

#### Extensions/AppEndpointExtensions.cs

In this extension class, the **WebApplication** is extended in order to allow for all the inline endpoints to be tidyly specified away from the Program.cs's configuration.

#### Extension/AppDataExtensions.cs

For this extension class, the EF Core code first InMemory database is seeded with Payment Methods and Donations.

#### cmac.demo.data/AppDbContext.cs

In this code, the code first EF core db scaffolding is performed, setting up the DbSets and the keys/foreign keys.

## Conclusions and Remarks

I did run over on time with this, which is why I haven't achieved the in-row Delete and Edit functionality assigned as stretch goals. The overrun was due to two main problems I hit upon;

- Nuance with how InteractiveWebAssembly works causing issues with pages/components preventing HttpClient from being injected properly
- Significant issues with the Radzen data grid which meant I had to abort a methodology where all the filtering, paging and sorting was happening on the DB side, I had to rever this to being a plain search box for the filtering and pulling the whole dataset of result in memory for the paging and sorting. Obviously not a huge issue for an InMemory database in a database, but this is a massive issue when trying work with this grid and a lot of data.
    -  The Grid filter value constructed a string representation of a lambda didn't work ubiquitously and had a significant issue where it would not handle a situation where data is nested, e.g. Donations.PaymentMethods.Name, if you flatten this information on the view model, the lambda is also flattened and therefore doesn't work on the query. This may be navigable by perhaps projecting data to another type before filtering but I didn't experiment with that.
    -  The grid exhibited odd behaviour which attempting to do anything with it, such as filtering, navigating to other pages etc. The grid would correctly for off a LoadData call, which would successfully query with the right information, but would then immediately fire off another LoadData call with the original arguments, thereby materially doing nothing as far as the user's concerned. I could not find a solution to this problem.

#### Unit Testings
Because of the use of inline API endpoints (again for brevity in the demo) and most of the other logic being housed in Blazor code, I didn't see much value in constructing unit tests for the solution. Moving to a more structured format, we would potentially use a DonationDataService, specific Controller classes etc which are much more testable pieces of code, but obviously makes the solution more complete. I also didn't add swagger as I felt it would've been over-kill for the demo, but arguably with inline endpoints implementing swagger allows for easy integration testing.

#### Code Commenting
I have opted to not comment a huge amount, this is in part due to a growing trend in development to code in a way that essentially negates the need for commenting for the most part, however I want to be clear that if that is a direction that CMAC doesn't want to go in I am more than happy to comment to great extent.