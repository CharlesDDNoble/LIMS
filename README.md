# Library Inventory Management System - CS4430 - Final Group Project

This is the Library Inventory Management System (LIMS) made a part of our 
(*insert names*) WMU Database Management Class Final in 2020. The LIMS 
replicates a standard library inventory tracking system using C# as the 
base language and MySQL as the DBMS. The intended users of the application 
are employees at a library as well as library customers. 

### Dependencies
+ Visual Studio 2019
    - ASP.NET and web development
    - .NET desktop development
    - .NET Core cross-platform development
+ MySQL 8.0.19
    - MySQL Connector/NET 8.0.19
+ Bootstrap 4.4.1

### Installation
*I've (Charles) been using a local MySQL database to test the project. For now I'll assume that's what will be used until we get something else more concrete up and running.*  
1. Install (Visual Studio 2019)[https://visualstudio.microsoft.com/vs/] using the Visual Studio installer  
a. within the Visual Studio installer, add the "ASP.NET and web development" package  
b. within the Visual Studio installer, add the ".NET desktop development" package  
c. within the Visual Studio installer, add the ".NET Core cross-platform development" package  
2. Install (MySQL 8.0.19)[https://dev.mysql.com/downloads/mysql/]  
a. install the (MySQL .NET connector)[https://dev.mysql.com/downloads/connector/net/]  
3. Build the database  
a. Connect to your MySQL Server  
b. Run the database building script */scripts/create_lims_db.sql*  
4. Configure the database connection in DB_Config.json  

### Features Include: ####
*Guest*
+ Search results w/filtering by:
    + Author
    + Date published
    + Genre
    + Type (if differents products)

*User:*
+ Placing reservations
+ Placing book requests
+ Reviewing Books
+ Checking book availability (not reserved/checked out)

*Employee:*
+ Seeing when checked out books are available
+ Seeing what books are in stock
+ Adding new books and new copies of books to db

*Manager:*
+ Placing orders for new books
+ Seeing book orders
+ Checking book requests
