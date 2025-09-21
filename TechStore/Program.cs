using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechStore.Application.Interfaces;
using TechStore.Application.Services;
using TechStore.Domain.Interfaces;
using TechStore.Infrastructure.Data;
using TechStore.Infrastructure.Repositories;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);


//AA Configurar DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//AA Registrar servicios
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();


//AA Agregar servicios
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

//AA MVC
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

//AA Add AutoMapper
//builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//AA
builder.Services.AddAutoMapper(
    typeof(TechStore.Application.Mappings.ProductProfile).Assembly,
    typeof(TechStore.Application.Mappings.ClienteProfile).Assembly
);


// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
