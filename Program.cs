//using inventoryitems.data;
//using microsoft.entityframeworkcore;

//var builder = webapplication.createbuilder(args);

//// add services to the container.
//builder.services.addcontrollers();
//// learn more about configuring swagger/openapi at https://aka.ms/aspnetcore/swashbuckle
//builder.services.addendpointsapiexplorer();
//builder.services.addswaggergen();

//var connectionstring = builder.configuration.getconnectionstring("defaultconnection") ?? throw new invalidoperationexception("connection string 'defaultconnection' not found.");
//builder.services.adddbcontext<applicationdbcontext>(options =>
//    options.usemysql(connectionstring, serverversion.autodetect(connectionstring)));

//var app = builder.build();

//// configure the http request pipeline.
//if (app.environment.isdevelopment())
//{
//    app.useswagger();
//    app.useswaggerui();
//}

//app.usehttpsredirection();

//app.useauthorization();

//app.mapget("/", () => "welcome! see endpoints /swagger/index.html");

//app.mapcontrollers();

//app.run();

namespace InventoryItems
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
