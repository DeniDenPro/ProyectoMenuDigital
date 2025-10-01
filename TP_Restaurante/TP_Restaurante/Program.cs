using Application.Interfaces;
using Application.Interfaces.ICategory;
using Application.Interfaces.ICategory.ICategoryServices;
using Application.Interfaces.IDeliveryType;
using Application.Interfaces.IDeliveryType.IDeliveryTypeService;
using Application.Interfaces.IDish;
using Application.Interfaces.IDish.IDishService;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrder.IOrderService;
using Application.Interfaces.IOrderItem;
using Application.Interfaces.IStatus;
using Application.Interfaces.IStatus.IStatusService;
using Application.Services;
using Application.Services.CategoryService;
using Application.Services.CategoryServices;
using Application.Services.DeliveryTypeService;
using Application.Services.DishServices;
using Application.Services.OrderService;
using Application.Services.OrderServices;
using Application.Services.StatusService;
using Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Command;
using Infrastructure.Data;
using Infrastructure.Querys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using TP_Restaurante.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configurar EF Core con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//INJECTIONS
//builder Dish
builder.Services.AddScoped<IDishCommand, DishCommand>();
builder.Services.AddScoped<IDishQuery, DishQuery>();
builder.Services.AddScoped<ICreateDishUseCase, CreateDishUseCase>();
builder.Services.AddScoped<IUpdateDishUseCase, UpdateDishUseCase>();
builder.Services.AddScoped<ISearchAsyncUseCase, SearchAsyncUseCase>();
builder.Services.AddScoped<IGetDishByIdUseCase, GetDishByIdUseCase>();
builder.Services.AddScoped<IDeleteDishUseCase, DeleteDishUseCase>();


//builder Category
builder.Services.AddScoped<ICategoryQuery, CategoryQuery>();
builder.Services.AddScoped<ICategoryCommand, CategoryCommand>();
builder.Services.AddScoped<IGetAllCategoriesService, GetAllCategoriesService>();
builder.Services.AddScoped<IGetCategoryByIdService, GetCategoryByIdService>();
builder.Services.AddScoped<ICategoryExistUseCase, CategoryExistUseCase>();

//builder DeliveryType
builder.Services.AddScoped<IDeliveryTypeCommand, DeliveryTypeCommand>();
builder.Services.AddScoped<IDeliveryTypeQuery, DeliveryTypeQuery>();
builder.Services.AddScoped<IGetAllDeliveryTypesService, GetAllDeliveryTypesService>();
builder.Services.AddScoped<IGetDeliveryTypeByIdService, GetDeliveryTypesByIdService>();


//builder Order
builder.Services.AddScoped<IOrderCommand, OrderCommand>();
builder.Services.AddScoped<IOrderQuery, OrderQuery>();
builder.Services.AddScoped<ICreateOrderService, CreateOrderService>();
builder.Services.AddScoped<IGetOrderFechaStatusService, GetOrderFechaStatusService>();
builder.Services.AddScoped<IUpdateOrderService, UpdateOrderService>();
builder.Services.AddScoped<IUpdateOrderItemStatusService, UpdateOrderItemStatusService>();
builder.Services.AddScoped<IGetOrdersByIdService, GetOrderByIdService>();

//builder OrderItem
builder.Services.AddScoped<IOrderItemCommand, OrderItemCommand>();
builder.Services.AddScoped<IOrderItemQuery, OrderItemQuery>();


//builder Status
builder.Services.AddScoped<IStatusQuery, StatusQuery>();
builder.Services.AddScoped<IGetAllStatusesService, GetAllStatusesService>();


//
builder.Services.AddControllers();
//Validation with FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<DishRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DishUpdateRequestValidator>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MenuDigital",
        Version = "v1",
        Description = "API del proyecto MenuDigital"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply pending migrations at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Middleware custom for exception handling
app.UseMiddleware<ErrorHandlingMiddleware>();

//app.UseHttpsRedirection(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
