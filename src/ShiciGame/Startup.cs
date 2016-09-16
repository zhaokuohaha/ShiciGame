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
			//loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();
			loggerFactory.AddConsole();

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
					WebSocket websocket = await http.WebSockets.AcceptWebSocketAsync();
					if (!_websocketCollection.Any())
						RelayHelper.CurrentLetter = null;
					_websocketCollection.Add(websocket);



					ArraySegment<Byte> buffer;
					CancellationToken token = CancellationToken.None;
					WebSocketMessageType type = WebSocketMessageType.Text;

					buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes("system-chat###chat-info###欢迎进入诗词接龙, 当前接龙词为: " + RelayHelper.CurrentLetter+"###系统"));
					await websocket.SendAsync(buffer, type, true, token);
					buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes("system-chat###chat-info###" + websocket.SubProtocol + "加入接龙聊天室###系统"));
					_websocketCollection.ForEach(async (socket) =>
					{
						if (socket != null && socket != websocket && socket.State == WebSocketState.Open)
						{
							await socket.SendAsync(buffer, type, true, token);
						}
					});

					while (websocket.State == WebSocketState.Open)
					{
						buffer = new ArraySegment<byte>(new Byte[4096]);
						var received = await websocket.ReceiveAsync(buffer, token);
						switch (received.MessageType)
						{
							case WebSocketMessageType.Text:
								var request = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count).Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries);
								RelayReturn res = new RelayHelper(loggerFactory.CreateLogger("socket")).CheckVarse(request);
								var data = Encoding.UTF8.GetBytes(res.StyleClass + "###" + res.ResponseBody+"###"+websocket.SubProtocol);
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
					areaName: "Custom",
					template: "{area}/{controller}/{action}");
			});
		}
	}
}
