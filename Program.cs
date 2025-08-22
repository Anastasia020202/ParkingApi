using ParkingApi.Repositories;
using ParkingApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IPlazaRepository, PlazaRepository>();
builder.Services.AddSingleton<IPlazaService, PlazaService>();
builder.Services.AddSingleton<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddSingleton<IUsuarioService, UsuarioService>();
builder.Services.AddSingleton<IReservaRepository, ReservaRepository>();
builder.Services.AddSingleton<IReservaService, ReservaService>();
builder.Services.AddSingleton<IVehiculoRepository, VehiculoRepository>();
builder.Services.AddSingleton<IVehiculoService, VehiculoService>();
builder.Services.AddSingleton<ITicketRepository, TicketRepository>();
builder.Services.AddSingleton<ITicketService, TicketService>();
builder.Services.AddSingleton<IAuthService, AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
