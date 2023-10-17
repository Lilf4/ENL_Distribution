# ENL Distribution
<p align="center">
	<img src="Images/Logo.png" alt="ENL_Logo Image">
</p>

## Purpose
This project was build to showcase and test requirements of a WMS project, created for upcoming H1 students.

## Project Structure
I have split the project into 4 directories

- [**Database**](Database): This is where the database class and any connected classes such as configs lies.
- [**Models**](Models): This is where model classes lie.
- [**Pages**](Pages): This is for designing xaml pages, used inside existing window.
- [**Windows**](Windows): This is for designing xaml windows, seperate window.
<br><br>

## Features
**GUI**
- [**Main Menu**](Pages/MainPage.xaml)
	- [**Products**](Pages/ProductPage.xaml): Displays datagrid of products.
		- [Product editor](Windows/ProductEdit.xaml)
	- [**Employees**](Pages/EmployeePage.xaml): Displays datagrid of employees.
		- [Employee editor](Windows/EmployeeEdit.xaml)
	- [**Orders**](Pages/OrderPage.xaml): Displays datagrid of ongoing and finished orders.
		- [Order editor](Windows/OrderEdit.xaml)
<br><br>
***

[**DATABASE**](Database/Database.cs): CRUD functions for following classes:
- [**Location**](Models/Location.cs)
- [**Product**](Models/Product.cs)
- [**Employee**](Models/Employee.cs)
- [**Order**](Models/Order.cs)
<br><br>
***

**VALIDATION**: Validation classes
- [**Employee Validator**](Validators/EmployeeValidator.cs): Validates FirstName, LastName, PhoneNumber & Email.
- [**Order Validator**](Validators/OrderValidator.cs): Validates Product, Employee & Quantity.
- [**Product Validator**](Validators/ProductValidator.cs): Validates Name, Description, Quantity & Location.
<br><br>

## Dependencies
- [**FluentValidation**](https://docs.fluentvalidation.net/): Library for easily creating validation methods for whole classes. By Jeremy Skinner
- [**Material.Icons.WPF**](https://github.com/SKProCH/Material.Icons/): Library of icons for use in WPF applications. By SKProCH
<br><br>

## Credits

<div style="display: flex; align-items:center;">

[<img src="https://github.com/Dagor94.png" width="80px;" style="border-radius: 40px;"/>](https://github.com/Dagor94)

<div style="display: flex; flex-direction: row;">
<a style="margin-left: 10px" href="https://github.com/Dagor94">Dagor94</a>
<p style="margin-left: 5px; padding-right: 5px">- Contribution:</p>
	
[Design and styling of pages/windows.](https://github.com/Dagor94/ENL_Distribution)

</div>

</div>