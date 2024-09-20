var builder = WebApplication.CreateBuilder(args);
//Mvc Ekleme
builder.Services.AddControllersWithViews();
var app = builder.Build();
//Dosya Kullan�m�
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name:"Default",
    pattern:"{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
