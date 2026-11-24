using Web_Equipment.Components;
using Web_Equipment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<AuthSession>();

builder.Services.AddHttpClient<AuthApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7016/");
});

builder.Services.AddHttpClient<EquipmentApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7016/");
});

builder.Services.AddHttpClient<PagoApiService>(client =>
{
    client.BaseAddress =
        new Uri("https://localhost:7016/");
});
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
