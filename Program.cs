using Terra.Components;
using Blazored.Toast;
using Terra.Dao.Usuario;
using Terra.Commons;
using Blazored.Toast.Services;
using Terra.Service;
using Terra.Services;
using Terra.Dao.Parametrizacion.Personas;
using Terra.Dao.Parametrizacion.Pais;
using Terra.Dao.Parametrizacion.GrupoSanguineo;
using Terra.Dao.Parametrizacion.NivelAcademico;
using Terra.Dao.Parametrizacion.Cargos;
using Terra.Dao.Herramientas;
using Terra.Dao.Ubicacion;
using Terra.Dao.Operacion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazoredToast();

builder.Services.AddBlazorBootstrap();

builder.Services.AddSingleton<AuthService>();

builder.Services.AddScoped<ITERRADB, TERRADB>();

builder.Services.AddScoped<UsuarioDao>();
builder.Services.AddScoped<PersonaDao>();
builder.Services.AddScoped<PaisDao>();
builder.Services.AddScoped<GrupoSanguineoDao>();
builder.Services.AddScoped<NivelAcademicoDao>();
builder.Services.AddScoped<CargoDao>();
builder.Services.AddScoped<HerramientaDao>();
builder.Services.AddScoped<UbicacionDao>();
builder.Services.AddScoped<OperacionDao>();
builder.Services.AddScoped<IToastService, ToastService>();
builder.Services.AddScoped<ISessionService, SessionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
