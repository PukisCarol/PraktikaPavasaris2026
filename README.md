# Praktika2026Pavasaris

Products and order management program built with ASP.NET core, PostgreSQL database and using Onion architecture.

## Prerequisities

For setup you should have:

* Visual Studio 2026
* PostgreSQL
* .NET 10 SDK

## Setup

1. Clone this project
2. In `appsetting.json` replace `YOUR_PASSWORD` with your own PostgreSQL password:

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=OrderManagement;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

3. Build the project
4. Run database migration in bash:

```
cd OrderManagement.Api
dotnet ef migrations add InitialCreate --project ../OrderManagement.Infrastructure --startup-project .
dotnet ef database update --project ../OrderManagement.Infrastructure --startup-project .
```



## Main logic

* Retailers can create products and set optional discounts on them. A discount has a percentage and a minimum quantity — for example, 10% off when ordering 5 or more items. Discount percentage is capped at 100%.
* When creating an order, you provide a list of products with quantities. Each order has an invoice that shows the final price per product with discounts applied where applicable, and the total amount to pay.
* There is also a discount report that shows which discounted products were actually ordered, how many times, and the total amount spent on them.

