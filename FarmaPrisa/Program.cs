using FarmaPrisa.Data;
using FarmaPrisa.Repository.Interface;
using FarmaPrisa.Repository.Service;
using FarmaPrisa.Services;
using FarmaPrisa.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// 1. Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Registrar el DbContext
builder.Services.AddDbContext<FarmaPrisaContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

//builder.Services.AddScoped<IProductosService, ProductosService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<IPromocionService, PromocionService>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyService>();


//Nuevos Repositorios
builder.Services.AddScoped<IProductRepository, ProductService>();
builder.Services.AddScoped<IBrandRepository, BrandService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Esto ya lo teníamos: para leer los comentarios XML
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    // --- DEFINIMOS LA SEGURIDAD ---
    // Aquí le decimos a Swagger que nuestra API usa autenticación Bearer (JWT).
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, introduce 'Bearer' seguido de un espacio y tu token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // --- APLICAMOS EL REQUISITO DE SEGURIDAD ---
    // Aquí le decimos a Swagger que aplique el requisito de autenticación a los endpoints que lo necesiten.
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Agrega los servicios de autenticación
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Valida que el emisor (quien creó el token) sea el correcto
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            // Valida que el receptor (quien puede usar el token) sea el correcto
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            // Valida que el token no haya expirado
            ValidateLifetime = true,

            // Valida la firma del token para asegurar que no ha sido alterado
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyVueApp",
        builder =>
        {
            // Especifica el origen de la app Vue.js
            builder.WithOrigins("http://127.0.0.1:5173", "https://localhost:7100", "https://nicasystems.com", "http://108.166.183.237:9096", "http://108.166.183.237")
                   .AllowAnyHeader() // Permite cualquier cabecera (Header)
                   .AllowAnyMethod(); // Permite cualquier método (GET, POST, PUT, DELETE)
        });
});

var app = builder.Build();

app.UseStaticFiles(); //exponer wwwroot publicamente
// PARA CUANDO SE TENGA LISTO EL VPS Y LA RUTA DE IMÁGENES ESTÉ CREADA
//app.UseStaticFiles(); // Deja esta línea para servir wwwroot (CSS, JS)

//// Agrega esta configuración para servir el contenido estático de la nueva ruta
//app.UseStaticFiles(new StaticFileOptions
//{
//    // Define la carpeta real en el disco del VPS donde guardarás los archivos
//    FileProvider = new PhysicalFileProvider("/var/lib/farma-data/images"),
//    // Define el prefijo de URL que el cliente usará para acceder
//    RequestPath = "/images"
//});

app.UsePathBase("/api");

// Aplicar Migraciones al Iniciar la Aplicación
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<FarmaPrisaContext>();

        // ¡Comando CRÍTICO! Esto aplica todas las migraciones pendientes
        context.Database.Migrate();

        // Opcional: Aquí podrías añadir lógica para sembrar datos iniciales (Seed Data)
    }
    catch (Exception ex)
    {
        // Manejo de errores: Si la migración falla, registra el error
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al aplicar las migraciones en el startup de la base de datos.");
    }
}

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();




app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowMyVueApp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
