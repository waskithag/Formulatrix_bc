using Entity_Framework.Data;
using Entity_Framework.Models;
using Entity_Framework.Services;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("EF Core Demo");

using var context = new SalesDbContext();
await SeedDatabaseAsync(context);

//add simple CRUD with console input for Employee/Product/Sales
var employeeService = new EmployeeService(context);
var productService = new ProductService(context);
var salesService = new SalesService(context);

await RunMainMenuAsync(employeeService, productService, salesService);


static async Task SeedDatabaseAsync(SalesDbContext context)
{
    if (await context.Employees.AnyAsync() || await context.Products.AnyAsync() || await context.Sales.AnyAsync())
    {
        Console.WriteLine("Database already contain data"); 
        return;
    }

    Console.WriteLine("Seeding database with initial data");
    SalesDbContext.SeedData(context);
}

static async Task RunMainMenuAsync(EmployeeService employeeService, ProductService productService, SalesService salesService)
{
    while (true)
    {
        Console.WriteLine("\n=== Main Menu ===");
        Console.WriteLine("1. Manage Employees");
        Console.WriteLine("2. Manage Products");
        Console.WriteLine("3. Manage Sales");
        Console.WriteLine("0. Exit");
        Console.Write("Select an option: ");

        var choice = Console.ReadLine()?.Trim();
        if (choice == null || choice == "0")
        {
            Console.WriteLine("Exiting application. Goodbye!");
            break;
        }

        switch (choice)
        {
            case "1":
                await ManageEmployeesAsync(employeeService);
                break;
            case "2":
                await ManageProductsAsync(productService);
                break;
            case "3":
                await ManageSalesAsync(salesService);
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
}

static async Task ManageEmployeesAsync(EmployeeService employeeService)
{
    while (true)
    {
        Console.WriteLine("\n--- Employee Management ---");
        Console.WriteLine("1. List all employees");
        Console.WriteLine("2. View employee by ID");
        Console.WriteLine("3. Add employee");
        Console.WriteLine("4. Update employee");
        Console.WriteLine("5. Delete employee");
        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Select an option: ");

        var choice = Console.ReadLine()?.Trim();
        if (choice == null || choice == "0") break;

        try
        {
            switch (choice)
            {
                case "1":
                    var employees = await employeeService.GetAllEmployeeAsync();
                    Console.WriteLine("\n--- Employees ---");
                    if (employees.Count == 0)
                    {
                        Console.WriteLine("No employees found.");
                    }
                    else
                    {
                        foreach (var emp in employees)
                        {
                            Console.WriteLine($"ID: {emp.Id,-4} | Name: {emp.Name}");
                        }
                    }
                    break;

                case "2":
                    var idToView = PromptInt("Enter Employee ID: ");
                    if (!idToView.HasValue) break;
                    var empFound = await employeeService.GetEmployeeByIdAsync(idToView.Value);
                    if (empFound == null)
                    {
                        Console.WriteLine($"Employee with ID {idToView.Value} not found.");
                    }
                    else
                    {
                        Console.WriteLine($"ID: {empFound.Id} | Name: {empFound.Name}");
                    }
                    break;

                case "3":
                    Console.Write("Enter Employee Name: ");
                    var name = Console.ReadLine()?.Trim();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("Employee name cannot be empty.");
                        break;
                    }
                    var created = await employeeService.CreateEmployeeAsync(new Employee { Name = name });
                    Console.WriteLine($"Employee created successfully with ID: {created.Id}");
                    break;

                case "4":
                    var idToUpdate = PromptInt("Enter Employee ID to update: ");
                    if (!idToUpdate.HasValue) break;
                    var existing = await employeeService.GetEmployeeByIdAsync(idToUpdate.Value);
                    if (existing == null)
                    {
                        Console.WriteLine($"Employee with ID {idToUpdate.Value} not found.");
                        break;
                    }
                    Console.Write($"Enter new name (current: '{existing.Name}', press Enter to keep): ");
                    var newName = Console.ReadLine()?.Trim();
                    if (string.IsNullOrWhiteSpace(newName))
                    {
                        newName = existing.Name;
                    }
                    var updated = await employeeService.UpdateEmployeeAsync(idToUpdate.Value, new Employee { Name = newName });
                    if (updated != null)
                    {
                        Console.WriteLine($"Employee ID {updated.Id} updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update employee.");
                    }
                    break;

                case "5":
                    var idToDelete = PromptInt("Enter Employee ID to delete: ");
                    if (!idToDelete.HasValue) break;
                    var deleted = await employeeService.DeleteEmployeeAsync(idToDelete.Value);
                    if (deleted)
                    {
                        Console.WriteLine($"Employee ID {idToDelete.Value} deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Employee with ID {idToDelete.Value} not found or could not be deleted.");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

static async Task ManageProductsAsync(ProductService productService)
{
    while (true)
    {
        Console.WriteLine("\n--- Product Management ---");
        Console.WriteLine("1. List all products");
        Console.WriteLine("2. View product by ID");
        Console.WriteLine("3. Add product");
        Console.WriteLine("4. Update product");
        Console.WriteLine("5. Delete product");
        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Select an option: ");

        var choice = Console.ReadLine()?.Trim();
        if (choice == null || choice == "0") break;

        try
        {
            switch (choice)
            {
                case "1":
                    var products = await productService.GetAllProductsAsync();
                    Console.WriteLine("\n--- Products ---");
                    if (products.Count == 0)
                    {
                        Console.WriteLine("No products found.");
                    }
                    else
                    {
                        foreach (var prod in products)
                        {
                            Console.WriteLine($"ID: {prod.Id,-4} | Name: {prod.Name,-20} | Price: {prod.Price,10:N2} | Stock: {prod.Stock,-5}");
                        }
                    }
                    break;

                case "2":
                    var idToView = PromptInt("Enter Product ID: ");
                    if (!idToView.HasValue) break;
                    var prodFound = await productService.GetProductByIdAsync(idToView.Value);
                    if (prodFound == null)
                    {
                        Console.WriteLine($"Product with ID {idToView.Value} not found.");
                    }
                    else
                    {
                        Console.WriteLine($"ID: {prodFound.Id} | Name: {prodFound.Name} | Price: {prodFound.Price:N2} | Stock: {prodFound.Stock}");
                    }
                    break;

                case "3":
                    Console.Write("Enter Product Name: ");
                    var name = Console.ReadLine()?.Trim();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("Product name cannot be empty.");
                        break;
                    }
                    var price = PromptDecimal("Enter Price: ");
                    if (!price.HasValue) break;
                    var stock = PromptInt("Enter Stock: ");
                    if (!stock.HasValue) break;

                    var created = await productService.CreateProductAsync(new Product
                    {
                        Name = name,
                        Price = price.Value,
                        Stock = stock.Value
                    });
                    Console.WriteLine($"Product created successfully with ID: {created.Id}");
                    break;

                case "4":
                    var idToUpdate = PromptInt("Enter Product ID to update: ");
                    if (!idToUpdate.HasValue) break;
                    var existing = await productService.GetProductByIdAsync(idToUpdate.Value);
                    if (existing == null)
                    {
                        Console.WriteLine($"Product with ID {idToUpdate.Value} not found.");
                        break;
                    }
                    Console.Write($"Enter new name (current: '{existing.Name}', press Enter to keep): ");
                    var newName = Console.ReadLine()?.Trim();
                    if (string.IsNullOrWhiteSpace(newName))
                    {
                        newName = existing.Name;
                    }
                    var newPrice = PromptDecimal($"Enter new price (current: {existing.Price:N2}, press Enter to keep): ", existing.Price);
                    if (!newPrice.HasValue) break;
                    var newStock = PromptInt($"Enter new stock (current: {existing.Stock}, press Enter to keep): ", existing.Stock);
                    if (!newStock.HasValue) break;

                    var updated = await productService.UpdateProductAsync(idToUpdate.Value, new Product
                    {
                        Name = newName,
                        Price = newPrice.Value,
                        Stock = newStock.Value
                    });
                    if (updated != null)
                    {
                        Console.WriteLine($"Product ID {updated.Id} updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update product.");
                    }
                    break;

                case "5":
                    var idToDelete = PromptInt("Enter Product ID to delete: ");
                    if (!idToDelete.HasValue) break;
                    var deleted = await productService.DeleteProductAsync(idToDelete.Value);
                    if (deleted)
                    {
                        Console.WriteLine($"Product ID {idToDelete.Value} deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Product with ID {idToDelete.Value} not found or could not be deleted.");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

static async Task ManageSalesAsync(SalesService salesService)
{
    while (true)
    {
        Console.WriteLine("\n--- Sales Management ---");
        Console.WriteLine("1. List all sales records");
        Console.WriteLine("2. View sales record by ID");
        Console.WriteLine("3. Create new sales record");
        Console.WriteLine("4. Update sales record");
        Console.WriteLine("5. Delete sales record");
        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Select an option: ");

        var choice = Console.ReadLine()?.Trim();
        if (choice == null || choice == "0") break;

        try
        {
            switch (choice)
            {
                case "1":
                    var salesList = await salesService.GetAllSalesAsync();
                    Console.WriteLine("\n--- Sales Records ---");
                    if (salesList.Count == 0)
                    {
                        Console.WriteLine("No sales records found.");
                    }
                    else
                    {
                        foreach (var s in salesList)
                        {
                            var productInfo = s.Products.Count > 0
                                ? string.Join(", ", s.Products.Select(p => $"{p.Name} ({p.Price:N2})"))
                                : "No products";
                            var totalAmount = s.Products.Sum(p => p.Price);
                            Console.WriteLine($"Sales ID: {s.Id,-4} | Employee ID: {s.EmployeeId,-4} | Products: [{productInfo}] | Total: {totalAmount:N2}");
                        }
                    }
                    break;

                case "2":
                    var idToView = PromptInt("Enter Sales ID: ");
                    if (!idToView.HasValue) break;
                    var sale = await salesService.GetSalesByIdAsync(idToView.Value);
                    if (sale == null)
                    {
                        Console.WriteLine($"Sales record with ID {idToView.Value} not found.");
                    }
                    else
                    {
                        Console.WriteLine($"Sales ID: {sale.Id}");
                        Console.WriteLine($"Employee ID: {sale.EmployeeId}");
                        Console.WriteLine("Products:");
                        if (sale.Products.Count == 0)
                        {
                            Console.WriteLine("  (No products)");
                        }
                        else
                        {
                            foreach (var p in sale.Products)
                            {
                                Console.WriteLine($"  - [ID: {p.Id}] {p.Name} - Price: {p.Price:N2}");
                            }
                            Console.WriteLine($"Total Amount: {sale.Products.Sum(p => p.Price):N2}");
                        }
                    }
                    break;

                case "3":
                    var empId = PromptInt("Enter Employee ID: ");
                    if (!empId.HasValue) break;

                    Console.Write("Enter Product IDs separated by commas (e.g. 1, 2) or press Enter for none: ");
                    var productIds = ParseIntList(Console.ReadLine());

                    var createdSale = await salesService.CreateSalesAsync(empId.Value, productIds);
                    Console.WriteLine($"Sales record created successfully with ID: {createdSale.Id} ({createdSale.Products.Count} product(s))");
                    break;

                case "4":
                    var idToUpdate = PromptInt("Enter Sales ID to update: ");
                    if (!idToUpdate.HasValue) break;
                    var existingSale = await salesService.GetSalesByIdAsync(idToUpdate.Value);
                    if (existingSale == null)
                    {
                        Console.WriteLine($"Sales record with ID {idToUpdate.Value} not found.");
                        break;
                    }

                    var newEmpId = PromptInt($"Enter new Employee ID (current: {existingSale.EmployeeId}, press Enter to keep): ", existingSale.EmployeeId);
                    if (!newEmpId.HasValue) break;

                    var currentProdIds = string.Join(", ", existingSale.Products.Select(p => p.Id));
                    Console.Write($"Enter new Product IDs separated by commas (current: [{currentProdIds}], press Enter to keep): ");
                    var prodInput = Console.ReadLine()?.Trim();
                    List<int> updatedProdIds;
                    if (string.IsNullOrWhiteSpace(prodInput))
                    {
                        updatedProdIds = existingSale.Products.Select(p => p.Id).ToList();
                    }
                    else
                    {
                        updatedProdIds = ParseIntList(prodInput);
                    }

                    var updatedSale = new Sales
                    {
                        EmployeeId = newEmpId.Value,
                        Products = updatedProdIds.Select(id => new Product { Id = id }).ToList()
                    };

                    var resultSale = await salesService.UpdateSalesAsync(idToUpdate.Value, updatedSale);
                    if (resultSale != null)
                    {
                        Console.WriteLine($"Sales record ID {resultSale.Id} updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update sales record.");
                    }
                    break;

                case "5":
                    var idToDelete = PromptInt("Enter Sales ID to delete: ");
                    if (!idToDelete.HasValue) break;
                    var deleted = await salesService.DeleteSalesAsync(idToDelete.Value);
                    if (deleted)
                    {
                        Console.WriteLine($"Sales record ID {idToDelete.Value} deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Sales record with ID {idToDelete.Value} not found or could not be deleted.");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

static int? PromptInt(string prompt, int? defaultValue = null)
{
    while (true)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();
        if (input == null) return null;
        input = input.Trim();
        if (string.IsNullOrWhiteSpace(input) && defaultValue.HasValue)
        {
            return defaultValue.Value;
        }
        if (int.TryParse(input, out var val))
        {
            return val;
        }
        Console.WriteLine("Invalid number. Please enter a valid integer.");
    }
}

static decimal? PromptDecimal(string prompt, decimal? defaultValue = null)
{
    while (true)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();
        if (input == null) return null;
        input = input.Trim();
        if (string.IsNullOrWhiteSpace(input) && defaultValue.HasValue)
        {
            return defaultValue.Value;
        }
        if (decimal.TryParse(input, out var val))
        {
            return val;
        }
        Console.WriteLine("Invalid number. Please enter a valid decimal value.");
    }
}

static List<int> ParseIntList(string? input)
{
    if (string.IsNullOrWhiteSpace(input)) return [];
    var list = new List<int>();
    var parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    foreach (var part in parts)
    {
        if (int.TryParse(part, out var val))
        {
            list.Add(val);
        }
        else
        {
            Console.WriteLine($"Warning: '{part}' is not a valid integer and was skipped.");
        }
    }
    return list;
}