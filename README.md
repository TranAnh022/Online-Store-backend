# FullStack Project - Online-Store Backend

![.NET Core](https://img.shields.io/badge/.NET%20Core-v.8-purple)
![EF Core](https://img.shields.io/badge/EF%20Core-v.8-cyan)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-v.16-drakblue)

This project involves the development of a backend system for an Ecommerce platform. It encompasses the design and implementation of a database schema, REST API endpoints, custom PostgreSQL functions, and a backend server using ASP.NET Core and Entity Framework.

## Table of Contents

- [FullStack Project - Online-Store Backend](#fullstack-project---online-store-backend)
  - [Table of Contents](#table-of-contents)
  - [Technologies and Libraries](#technologies-and-libraries)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Database Setup using PgAdmin4](#database-setup-using-pgadmin4)
    - [Running project locally](#running-project-locally)
      - [Prerequisites](#prerequisites-1)
      - [Setup Instructions](#setup-instructions)
  - [Database Schema Design and ERD Creation](#database-schema-design-and-erd-creation)
  - [REST API Design](#rest-api-design)
  - [PostgreSQL Queries, Functions and Procedures](#postgresql-queries-functions-and-procedures)
  - [Backend Server with ASP.NET Core](#backend-server-with-aspnet-core)
    - [Architecture Diagram](#architecture-diagram)
  - [Unit Testing](#unit-testing)
  - [Repository Structure](#repository-structure)
  - [Team Collaboration](#team-collaboration)
    - [Team Members](#team-members)
    - [How We Work](#how-we-work)
  - [Acknowledgements](#acknowledgements)

## Technologies and Libraries

| Technology                                                                                                                                 | Purpose                                                                                        |
| ------------------------------------------------------------------------------------------------------------------------------------------ | ---------------------------------------------------------------------------------------------- |
| **[ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet)**                                                                         | Core framework for building server-side logic, routing, middleware, and dependency management. |
| **[Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli#create-the-database)** | ORM for database operations, abstracts SQL queries, simplifying data manipulation.             |
| **[PostgreSQL](https://www.postgresql.org/)**                                                                                              | Relational database management system for storing all application data.                        |

| Library                                                                                                                                | Purpose                                                                                               |
| -------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
| **[AutoMapper](https://www.nuget.org/packages/automapper/)**                                                                           | Automates the mapping of data entities to DTOs, reducing manual mapping code.                         |
| **[Ardalis.GuardClauses](https://www.nuget.org/packages/Ardalis.GuardClauses)**                                                        | Provides guard clauses to enforce pre-conditions in methods, enhancing robustness and error handling. |
| **[Microsoft.AspNetCore.Identity](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity)**                                      | Manages user authentication, security, password hashing, and role management within the application.  |
| **[JWT Bearer Authentication](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/9.0.0-preview.3.24172.13)** | Implements token-based authentication for securing API endpoints, requiring valid JWTs for access.    |
| **[xUnit](https://www.nuget.org/packages/xunit)**                                                                                      | Framework for unit testing, ensuring components function correctly in isolation.                      |
| **[Moq](https://www.nuget.org/packages/Moq)**                                                                                          | Mocking library used with xUnit to simulate behavior of dependencies during testing.                  |

## Getting Started

This section provides instructions on how to set up your environment and get the project up and running on your local machine for development and testing purposes.

### Prerequisites

Before you begin, ensure you have the following installed:

- **PostgreSQL** (Ensure it's running on the default port or update the connection string in the project configuration accordingly.)
- Any SQL client that supports PostgreSQL (e.g., pgAdmin, DBeaver, or the command-line tool psql)

### Database Setup using PgAdmin4

To set up the initial database, schema, and necessary tables, you will use the SQL scripts provided in the `data/sql` directory. Follow these steps to create your database:

1. Create Database and Schema:
   Navigate to the directory containing your SQL scripts

```sh
cd path/to/your/project/data/sql
```

Log in to your PostgreSQL database using psql or your preferred SQL client. Run the script to create the database and schema:

```sh
psql -U username -d postgres -f createDatabase.sql
```

> Replace username with your PostgreSQL database username. This command assumes you're using the default postgres database to run the script that creates your new database.

2. Verify Database Creation:
   After running the script, connect to the PostgreSQL server and check if the database along with its tables has been created successfully:

```sql
\c your_database_name
\dt
```

> This will switch to your newly created database and list all the tables, ensuring everything is set up correctly.

### Running project locally

Follow these steps to get your project up and running on your local machine for development and testing purposes.

#### Prerequisites

- .NET 8.0 SDK or later
- PostgreSQL server
- Git

#### Setup Instructions

1. **Clone the repository**:
   Open your terminal and execute the following command to clone the repository:
   ```bash
   git clone https://github.com/VictoriiaShtyreva/Team-3_BackendTeamwork.git
   ```
2. **Configure the application**:
   Navigate to the Web API layer in the back-end directory. Open the appsettings.json file and add the following configurations:
   ```bash
   "ConnectionStrings": {
    "Localhost": "<YOUR_LOCAL_DB_CONNECTION_STRING>"
   },
   "Secrets": {
    "JwtKey": "YourSecretKey",
    "Issuer": "YourIssuer"
   }
   ```
   > Replace <YOUR_LOCAL_DB_CONNECTION_STRING> with your actual PostgreSQL connection strings.
3. **Create the database**: Run the following commands
   ```bash
    dotnet tool install --global dotnet-ef
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet ef migrations add CreateDb
    dotnet ef database update
   ```
   > If a Migrations folder already exists, delete it.
4. **Start the Backend**: Navigate to the WebAPI layer directory and run the following command to start the backend server on your local machine
   ```bash
   dotnet watch run
   ```
5. **Access Swagger UI**: Open a web browser of your choice and enter the following URL in your browser's address bar:
   ```bash
   http://localhost:<YOUR_LOCALHOST>/swagger/index.html
   ```
   > This URL directs you to the Swagger UI page that is automatically generated by the Swagger middleware in your ASP.NET Core application.

![Swagger UI page](/readmeImg/Swager.png)

## Database Schema Design and ERD Creation

Detailed database schema definitions including data types, constraints, and relationships. All the data is stored and retrieved using [PostgreSQL](https://www.postgresql.org/), a free and open-source relational database management system. ERD diagram was created using [Lucidchart](https://www.lucidchart.com/pages/landing?utm_source=google&utm_medium=cpc&utm_campaign=_chart_en_tier2_mixed_search_brand_exact_&km_CPC_CampaignId=1520850463&km_CPC_AdGroupID=57697288545&km_CPC_Keyword=lucidchart&km_CPC_MatchType=e&km_CPC_ExtensionID=&km_CPC_Network=g&km_CPC_AdPosition=&km_CPC_Creative=442433237648&km_CPC_TargetID=kwd-33511936169&km_CPC_Country=9072483&km_CPC_Device=c&km_CPC_placement=&km_CPC_target=&gad_source=1&gclid=CjwKCAjwrIixBhBbEiwACEqDJdqG-nDfXAHumE5poslMyIZM3meH7qJUs0CXQsOxBaL5fCNZwOUMURoC7MwQAvD_BwE).

![Database schema](/readmeImg/Database_Schema.png)

> Below is a table describing the relationships between the various entities in the ERD. Each table's primary key (PK) and foreign key (FK) relationships are denoted in the ERD, which establish the links between different entities.

| Entity 1     | Relationship | Entity 2         | Description                                                                                                                           |
| ------------ | ------------ | ---------------- | ------------------------------------------------------------------------------------------------------------------------------------- |
| `users`      | One to Many  | `reviews`        | Each user can write multiple reviews. A review must be associated with a user.                                                        |
| `products`   | One to Many  | `reviews`        | Each product can have multiple reviews. A review must be related to a product.                                                        |
| `products`   | One to Many  | `product_images` | Each product can have multiple product images. A product images must be related to a product.                                         |
| `categories` | One to Many  | `products`       | Each category can encompass multiple products. A product must belong to a category.                                                   |
| `users`      | One to Many  | `orders`         | Each user can place multiple orders. An order is linked to the user who placed it.                                                    |
| `orders`     | One to Many  | `order_items`    | Each order can contain multiple items. Order items are linked back to their respective orders.                                        |
| `products`   | One to Many  | `cart_items`     | Products can be part of multiple order items, showing the quantity and price per order. Each order item is associated with a product. |
| `users`      | One to One   | `carts`          | Each user has one cart associated with them. The cart holds the products that the user is currently interested in purchasing.         |
| `carts`      | One to Many  | `cart_items`     | Each cart can hold multiple items. Cart items link the product with its quantity in the user's cart.                                  |

## REST API Design

The table below outlines the organizational structure of our repository regarding the API design documents. Each entity within our REST API is associated with a specific folder path that contains the design files, blueprints, and detailed documentation. This structure ensures that all information related to a particular entity is centralized, allowing for easy navigation and maintenance by developers and collaborators.

| Entity      | File name                                 | Description                                      |
| ----------- | ----------------------------------------- | ------------------------------------------------ |
| Users       | [users.md](docs-api/users.md)             | Contains endpoints for managing user data.       |
| Products    | [products.md](docs-api/products.md)       | Contains endpoints for managing product data.    |
| Categories  | [categories.md](docs-api/categories.md)   | Contains endpoints for managing category data.   |
| Reviews     | [reviews.md](docs-api/reviews.md)         | Contains endpoints for managing review data.     |
| Orders      | [orders.md](docs-api/orders.md)           | Contains endpoints for managing order data.      |
| Carts       | [carts.md](docs-api/carts.md)             | Contains endpoints for managing cart data.       |
| Cart Items  | [cart_items.md](docs-api/cart_items.md)   | Contains endpoints for managing cart item data.  |
| Order Items | [order_items.md](docs-api/order_items.md) | Contains endpoints for managing order item data. |

## PostgreSQL Queries, Functions and Procedures

This section details the SQL queries, functions, and procedures used in our application to interact with the PostgreSQL database, ensuring data integrity and consistency across all database operations.

| Entity     | File Name                                           | Description                                    |
| ---------- | --------------------------------------------------- | ---------------------------------------------- |
| Users      | [functions.sql](queries/user/functions.sql)         | Contains SQL functions related to users.       |
| Categories | [functions.sql](queries/categories/functions.sql)   | Contains SQL functions related to categories.  |
|            | [procedures.sql](queries/categories/procedures.sql) | Contains SQL procedures related to categories. |
| Reviews    | [functions.sql](queries/reviews/functions.sql)      | Contains SQL functions related to reviews.     |
|            | [procedures.sql](queries/reviews/procedures.sql)    | Contains SQL procedures related to reviews.    |
| Products   | [functions.sql](queries/product/functions.sql)      | Contains SQL functions related to products.    |
| Orders     | [functions.sql](queries/orders/functions.sql)       | Contains SQL functions related to orders.      |
|            | [procedures.sql](queries/orders/procedures.sql)     | Contains SQL procedures related to orders.     |
| Carts      | [functions.sql](queries/carts/functions.sql)        | Contains SQL functions related to carts.       |
|            | [procedures.sql](queries/carts/procedures.sql)      | Contains SQL procedures related to carts.      |

## Backend Server with ASP.NET Core

### Architecture Diagram

![Architecture Diagram](/readmeImg/Ecommerce.png)

> The diagram represents the architecture of an Ecommerce platform built with ASP.NET Core and complied with the Clean Architecture. It is structured into several layers, each responsible for different aspects of the application:

- **Ecommerce.Core**: This layer includes classes and interfaces that define the basic entities like products, categories, users, and more. It also contains repository interfaces which abstract the data access logic.
- **Ecommerce.Service**: This layer contains services that handle business logic and operations, interacting with the Core layer to manipulate and retrieve data.
- **Ecommerce.Controller**: This layer is responsible for handling incoming HTTP requests and returning responses. It interacts with the Service layer to fetch and send data back to the client.
- **Ecommerce.WebAPI**: This houses the controllers and is the entry point for client interactions through HTTP requests. It includes middleware for error handling.

## Unit Testing

Unit tests in this project are crucial for ensuring code quality and functionality. The **Ecommerce.Test** namespace contains tests that cover various components:

- **Testing Strategy**: Utilizes xUnit. Mocking is facilitated by libraries like Moq to simulate repository interactions.

![UnitTestsm](/readmeImg/UnitTests.png)

## Repository Structure

Our project is organized as follows to maintain a clean and navigable codebase:

```plaintext
/Team-3_BackendTeamwork
|-- /data
|   |-- /mock
|   |   |-- cart_items.csv
|   |   |-- carts.csv
|   |   |-- categories.csv
|   |   |-- order_items.csv
|   |   |-- orders.csv
|   |   |-- products.csv
|   |   |-- reviews.csv
|   |   |-- users.csv
|   |   |-- product_images.csv
|   |-- /sql
|       |-- createdatabase.sql
|
|-- /docs-api
|   |-- cart_items.md
|   |-- carts.md
|   |-- categories.md
|   |-- order_items.md
|   |-- orders.md
|   |-- products.md
|   |-- reviews.md
|   |-- users.md
|
|-- /queries
|   |-- /users
|   |   |-- functions.sql
|   |-- /categories
|   |   |-- functions.sql
|   |   |-- procedures.sql
|   |-- /carts
|   |   |-- functions.sql
|   |   |-- procedures.sql
|   |-- /reviews
|   |   |-- functions.sql
|   |   |-- procedures.sql
|   |-- /products
|   |   |-- functions.sql
|   |-- /orders
|       |-- functions.sql
|       |-- procedures.sql
|
|-- /Ecommerce
|   |-- /Ecommerce.Core
|   |   |-- /Common
|   |   |   |-- ProductQueryOptions.cs        // Defines options for querying products, such as pagination and filtering criteria.
|   |   |   |-- QueryOptions.cs               // Base class for query options, providing common properties like Sort.
|   |   |   |-- UserQueryOptions.cs           // Specialized query options for user-related data retrieval.
|   |   |   |-- UserCredential.cs             // Represents credentials for a user, used in authentication processes.
|   |   |-- /Entities
|   |   |   |-- BaseEntity.cs                 // Base class for all entities, containing common properties like Id.
|   |   |   |-- Category.cs                   // Represents a product category with properties like Name and Description.
|   |   |   |-- Product.cs                    // Defines a product with properties such as Name, Price, and CategoryId.
|   |   |   |-- ProductImage.cs               // Associated images for products, storing paths or image data.
|   |   |   |-- Review.cs                     // Customer reviews for products, includes Rating and Comment.
|   |   |   |-- TimeStamp.cs                  // Adds timestamps to entities, handled by TimeStampInterceptor for automatic updates.
|   |   |   |-- User.cs                       // User profile information, including credentials and roles.
|   |   |   |-- CartAggregate.cs              // Represents a shopping cart, including a collection of CartItems.
|   |   |   |-- OrderAggregate.cs             // Details an order, encapsulating order items and transaction data.
|   |-- /Interfaces
|   |   |-- /Interfaces
|   |   |   |-- IBaseRepository.cs            // Generic interface for CRUD operations applicable to all entities.
|   |   |   |-- ICartRepository.cs            // Specific operations for cart management not covered by the generic repository.
|   |   |   |-- ICategoryRepository.cs        // Custom repository actions for categories, like bulk update or specialized queries.
|   |   |   |-- IOrderRepository.cs           // Additional methods for managing orders, including status updates and history.
|   |   |   |-- IProductImageRepository.cs    // Handles operations specific to product image storage and retrieval.
|   |   |   |-- IProductRepository.cs         // Product-specific repository methods, such as searching by category.
|   |   |   |-- IReviewRepository.cs          // Methods for managing product reviews, including approval processes.
|   |   |   |-- IUserRepository.cs            // User-related data access functionalities, including search by role.
|   |   |-- /ValueObjects
|   |   |   |-- ProductSnapshot.cs            // A snapshot of product data at the time of transaction, used in order details.
|   |   |   |-- UserRole.cs                   // Defines roles within the system to manage access control levels.
|   |   |   |-- OrderStatus.cs                // Defines status of orders within the system to manage access control levels.
|   |   |-- /Exceptions
|   |       |-- ErrorDetails.cs               // Format for API error responses, includes status code and message.
|   |       |-- AppException.cs               // Custom exception type for application-specific errors, used for unified handling.
|   |
|   |-- /Ecommerce.Service
|   |   |-- /DTO
|   |   |   |-- CartDto.cs                    // Data transfer object for cart contents, includes list of CartItemDto.
|   |   |   |-- CartItemDto.cs                // Represents a single cart item in a transferable format, includes ProductId and Quantity.
|   |   |   |-- CategoryDto.cs                // Simplified category data for transfer, primarily used in listing endpoints.
|   |   |   |-- OrderDto.cs                   // Summary of an order for client applications, includes OrderItems and total cost.
|   |   |   |-- OrderItemDto.cs               // Details of an individual order item within an order.
|   |   |   |-- ProductDto.cs                 // Product data formatted for client delivery, includes descriptions and pricing.
|   |   |   |-- ProductImageDto.cs            // Data transfer format for images, may include a URL or binary data.
|   |   |   |-- ReviewDto.cs                  // Format for delivering review data to clients, includes user context and content.
|   |   |   |-- UserDto.cs                    // User profile information suitable for client-side use, includes user roles.
|   |-- /Services
|   |   |   |-- /Services
|   |   |   |-- AuthService.cs                // Manages authentication processes, including token generation and validation.
|   |   |   |-- BaseService.cs                // Base service providing common functionalities to all services, such as logging.
|   |   |   |-- CartItemService.cs            // Business logic related to individual cart items, such as additions and removals.
|   |   |   |-- CartService.cs                // Overall management of shopping carts, including session handling and persistence.
|   |   |   |-- CategoryService.cs            // Operations related to category management, from creation to modification.
|   |   |   |-- OrderItemService.cs           // Detailed logic for handling order items during the purchase process.
|   |   |   |-- OrderService.cs               // Coordinates all aspects of order processing, from placement to delivery.
|   |   |   |-- ProductImageService.cs        // Handles the storage and retrieval of product images.
|   |   |   |-- ProductService.cs             // Core service for product management, including inventory and updates.
|   |   |   |-- ReviewService.cs              // Manages the lifecycle of product reviews, including moderation.
|   |   |   |-- UserService.cs                // User profile and credential management, including registration and updates.
|   |   |   |-- TokenService.cs               // Generates and validates security tokens for user authentication.
|   |   |-- /Shared
|   |       |-- AutoMapperProfile.cs          // Configurations for AutoMapper to handle entity to DTO mappings efficiently.
|   |       |-- PasswordService.cs            // Provides password hashing and verification services.
|   |
|   |-- /Ecommerce.Controller
|   |   |-- /Controller
|   |   |   |-- AuthController.cs             // Handles authentication requests, like login and token refresh.
|   |   |   |-- CartController.cs             // API endpoints for cart interactions, such as adding or removing items.
|   |   |   |-- CategoryController.cs         // Provides API access to category data, including CRUD operations.
|   |   |   |-- OrderController.cs            // Manages order-related endpoints, from creation to status updates.
|   |   |   |-- ProductController.cs          // Controls product data exposure through the API, including search and details.
|   |   |   |-- ProductImageController.cs     // Endpoints for uploading and retrieving product images.
|   |   |   |-- ReviewController.cs           // API operations for managing reviews, including posting and editing.
|   |   |   |-- UserController.cs             // Handles user profile requests and user-specific data interactions.
|   |   |
|   |-- /Ecommerce.WebAPI
|   |   |-- /Repo
|   |   |   |-- CartItemRepository.cs         // Repository for handling operations specific to cart items.
|   |   |   |-- CartRepository.cs             // Manages data access related to the shopping cart.
|   |   |   |-- CategoryRepository.cs         // Accesses and manipulates category data in the database.
|   |   |   |-- ProductImageRepo.cs           // Handles the persistence of product images.
|   |   |   |-- ProductRepo.cs                // Facilitates data access for product management.
|   |   |   |-- ReviewRepository.cs           // Manages review data interactions.
|   |   |   |-- UserRepo.cs                   // Repository for user data access and manipulation.
|   |   |-- /Data
|   |   |   |-- AppDbContext.cs               // Entity Framework context, configures model relationships and database mappings.
|   |   |   |-- TimeStampInterceptor.cs       // EF Core interceptor to automatically update timestamp fields on save.
|   |   |-- /Middleware
|   |       |-- ExceptionMiddleware.cs        // Centralizes exception handling for the API, standardizing error responses.
|   |
|   |-- /Ecommerce.Test
|   |   |-- /UnitTests
|   |   |   |-- CartItemTests.cs              // Tests for cart item functionalities.
|   |   |   |-- CartTests.cs                  // Tests for cart functionalities.
|   |   |   |-- CategoryTests.cs              // Tests for category functionalities.
|   |   |   |-- OrderTests.cs                 // Tests for order functionalities.
|   |   |   |-- ProductImageTests.cs          // Tests for product image functionalities.
|   |   |   |-- ProductTests.cs               // Tests for product functionalities.
|   |   |   |-- ReviewTests.cs                // Tests for review functionalities.
|   |   |   |-- UserTests.cs                  // Tests for user functionalities.
|   |
|   |   |-- /Service
|   |   |   |-- CartItemServiceTests.cs       // Tests for cart item service operations.
|   |   |   |-- CartServiceTests.cs           // Tests for cart service operations.
|   |   |   |-- CategoryServiceTests.cs       // Tests for category service operations.
|   |   |   |-- OrderItemServiceTests.cs      // Tests for order item service operations.
|   |   |   |-- OrderServiceTests.cs          // Tests for order service operations.
|   |   |   |-- ProductImageServiceTests.cs   // Tests for product image service operations.
|   |   |   |-- ProductServiceTests.cs        // Tests for product service operations.
|   |   |   |-- ReviewServiceTests.cs         // Tests for review service operations.
|   |   |   |-- UserServiceTests.cs           // Tests for user service operations.
|
|-- README.md

```

## Team Collaboration

### Team Members

- **Viktoriia Shtyreva**

  - Responsibilities:
    - Database schema, ERD diagram, SQL scripts for initial database, schema and tables, prepare mock data for testing database.
    - Designing REST API endpoints for Categories and Reviews.
    - Develop PostgreSQL functions for CRUD operations and querying Reviews and Categories.
    - Start to set up clean architecture for our project.
    - Set up Core Layer.
    - Implement Ardalis.GuardClauses for robust input validation across our project.
    - Enhance entity models with methods such as Update, Delete, etc., to encapsulate business logic effectively.
    - Align existing methods with our endpoint requirements for consistency.
    - Develop Exception Middleware utilizing built-in exceptions for streamlined error handling.
    - Start working on implementation logic into Service Layer + UnitTests for Service Layer + Core Layer.
    - AutoMapper.
    - Controllers: Category, Review, cart.
    - Repository: Category, Review, cart, cart items.
    - Create `.http` files for sending requests.
    - Write comprehensive README.
  - Contact: [GitHub profile](https://github.com/VictoriiaShtyreva)

- **Tran Anh**

  - Responsibilities:
    - Database schema, REST API Endpoints Design for Users and Products with Authentication.
    - Develop PostgreSQL functions for CRUD operations and querying for Users and Products, Carts.
    - Initiate the development of the Service Layer, including defining DTOs, abstractions, and essential services.
    - Database context.
    - Set up Service Layer
    - Data Validation into Database + Seed data.
    - TimestampInterceptor.
    - Set up authentication.
    - Set up Controller Layer: user, authentication, product, productImage, order.
    - Repository: user, product, productImage, order, order items.
    - Create `.http` files for sending requests.
  - Contact: [GitHub profile](https://github.com/TranAnh022)

- **Bekir Kasan**
  - Responsibilities: Database schema, REST API Endpoints Design for Order, OrderItem, Cart, CartItem. Develop PostgreSQL functions for CRUD operations and querying Cart, Order.
  - Contact: [GitHub profile](https://github.com/Bskasan)

### How We Work

Our team follows agile methodologies, with regular stand-ups. As task management tool we used [Trello](https://trello.com/home). We use Git for version control with feature branching and pull requests to manage code integration.

## Acknowledgements

- Integrify-Finland for providing the assignment.
- All contributors who help in enhancing and maintaining the project.

[**Return**](#technologies-and-libraries)
