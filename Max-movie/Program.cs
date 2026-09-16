using Max_movie.Data;
using Max_movie.Models;
using Max_movie.Service;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 1. Agregamos los servicios de Swagger (La interfaz de pruebas)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Incluir DbContext
builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MovieDbContext")));

// add identity core con roles y signInManager
builder.Services.AddIdentityCore<Usuario>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireUppercase = false;
}
)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MovieDbContext>()
    .AddSignInManager();

//Manejo de la cookie. Lo ponemos en default, pero hay que ponerlo.
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = IdentityConstants.ApplicationScheme;
})
    .AddIdentityCookies();

builder.Services.ConfigureApplicationCookie(o =>
{
    o.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    o.SlidingExpiration = true;
    o.LoginPath = "/Usuario/Login";
    o.AccessDeniedPath = "/Usuario/AccessDenied";
});

//Servicio de archivos de imagenes
builder.Services.AddScoped<ImagenStorage>();
builder.Services.Configure<FormOptions>(o => {o.MultipartBodyLengthLimit = 2 * 1024 * 1024;});

//Servicio de envio de correos
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<IEmailService, SmtpEmailService>();

var app = builder.Build();

// invocar la ejecucion del dbseeder con un scope
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MovieDbContext>();
        var userManager = services.GetRequiredService<UserManager<Usuario>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Le agregamos el await porque tu método Seed ahora es asíncrono
        await DbSeeder.Seed(context, userManager, roleManager);

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");

    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// 2. Activamos la interfaz visual de Swagger solo cuando estamos desarrollando
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3. Le decimos a .NET que ahora las rutas se definen dentro de cada controlador (Web API)
app.MapControllers();

app.Run();
