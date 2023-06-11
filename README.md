# URL Shortening Service

## UrlShortener API is a service that generates default shorter URLs or allows users to create their own. The API also enables users to redirect original URLs from their shorter counterparts.

### UrlShortener API uses MongoDB as its database. To run the API on your local machine, you need to make the following configurations and installations.
- Install MongoDB from https://www.mongodb.com/try/download/community
- Install MongoDB Shell from https://www.mongodb.com/try/download/shell
- Use the following command on MongoDB Shell to access UrlShortenerDb: `use UrlShortener`
- Use the following command on MongoDB Shell to use UrlCollection: `db.createCollection(‘Url’)`
- It’s also necessary to have the correct .Net version installed on your machine. Your machine should have .Net 6.0 installed, which you can download from https://dotnet.microsoft.com/en-us/download/dotnet/6.0
