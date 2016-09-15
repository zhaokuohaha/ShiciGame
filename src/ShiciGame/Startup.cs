using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ShiciGame.Entities;
using System.Net.WebSockets;
using System.Threading;
using System.Text;
using ShiciGame.Areas.Commons;

namespace ShiciGame
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
			
		}

        public IConfigurationRoot Configuration { get; }
		public static List<WebSocket> _websocketCollection;
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
        {
			// Add framework services.
			services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("ApplicationDbContext")));
            services.AddMvc();
        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
			app.UseWebSockets();
			app.Use(async (http, next) =>
			{
			if (http.WebSockets.IsWebSocketRequest)
			{
				if (_websocketCollection == null)
				{
					_websocketCollection = new List<WebSocket>();
				}
				var websocket = await http.WebSockets.AcceptWebSocketAsync();
				_websocketCollection.Add(websocket);
				while (websocket.State == WebSocketState.Open)
				{
					var token = CancellationToken.None;
					var buffer = new ArraySegment<Byte>(new Byte[4096]);
					var received = await websocket.ReceiveAsync(buffer, token);
					switch (received.MessageType)
					{
						case WebSocketMessageType.Text:
								var request = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count).Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries);
								RelayHelper res = RelayHelper.CheckVarse(request);
								var type = WebSocketMessageType.Text;
								var data = Encoding.UTF8.GetBytes(res.StyleClass+"###"+res.ResponseBody);
								buffer = new ArraySegment<byte>(data);
								///如果信息合法, 群发
								if (res.IsLicitRequest)
								{
									_websocketCollection.ForEach(async (socket) =>
									{
										if (socket != null && socket != websocket && socket.State == WebSocketState.Open)
										{
											await socket.SendAsync(buffer, type, true, token);
										}
									});
								}
								else//否则只发回原作者
								{
									await websocket.SendAsync(buffer, type, true, token);
								}
								break;
						}
					}
				}
				else
				{
					await next();
				}
			});


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
				routes.MapAreaRoute(
					name: "custom",
					areaName:"Custom",
					template:"{area}/{controller}/{action}");
            });
        }
    }
}
