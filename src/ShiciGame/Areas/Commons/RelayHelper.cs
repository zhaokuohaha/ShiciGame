﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShiciGame.Areas.Commons
{
    public class RelayHelper
    {
		/// <summary>
		/// 是否合法
		/// </summary>
		public bool IsLicitRequest { get;}
		/// <summary>
		/// 返回信息的样式
		/// </summary>
		public string StyleClass { get; }
		/// <summary>
		/// 返回信息正文
		/// </summary>
		public string ResponseBody { get; }

		/// <summary>
		/// 当前接龙字
		/// </summary>
		public static string CurrentLetter;
		public RelayHelper(bool idlicitrequest, string syleclass, string resbody)
		{
			this.IsLicitRequest = IsLicitRequest;
			this.StyleClass = syleclass;
			this.ResponseBody = resbody;
		}
		public static RelayHelper CheckVarse(string[] varse)
		{
			//格式错误
			if (varse.Length != 2)
				return new RelayHelper(false, "system-error", "发送失败, 请求不合法" );
			//聊天信息
			if (varse[0] == "chat")
				return new RelayHelper(true, "others-chat", varse[1]);
			//首字错误
			if (CurrentLetter != null && varse[1].Substring(0, 1) != CurrentLetter)
				return new RelayHelper(false, "system-error", "诗句错误, 首字应为\"" + CurrentLetter);
			switch (VarseExists(varse[1])){
				case 1:
					CurrentLetter = varse[1].Last().ToString();
					return new RelayHelper(true, "others-varse", varse[1]);
				case 0:
					return new RelayHelper(false, "system-error", "诗词库中找不到" + varse[1]);
				case -1:
					return new RelayHelper(false, "system-error", "服务器错误, 请稍后再试或联系管理员解决");
				default:
					return new RelayHelper(false, "system-error", varse[1]);
			}
		}

		/// <summary>
		/// 通过古诗文网验证诗句
		/// </summary>
		/// <param name="varse">诗句</param>
		/// <returns>1表示匹配, 0表示不匹配, -1表示发生异常</returns>
		private static int VarseExists(string varse)
		{
			string url = "http://so.gushiwen.org/search.aspx?value=" + varse;
			try
			{
				WebRequest req = WebRequest.Create(url);
				WebResponse res = req.GetResponseAsync().Result;
				Stream stream = res.GetResponseStream();
				StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
				string str = reader.ReadToEnd();
				//到这里已经读取到响应页面的html文本, 接下来进行解析
				//匹配整句诗是诗句的一部分则不匹配
				string patter = "[^\u4e00-\u9fa5]<span style=\"color:#B00815\">" + varse + "</span>" + "[^\u4e00-\u9fa5]";
				//匹配标题或标题的一部分
				string istitle = "<span style=\"color:#B00815\">" + varse + "</span>[^x00-xff]*</a>";
				Regex regex = new Regex(patter);
				Regex title = new Regex(istitle);
				//是整句诗并且不在标题中
				if (regex.Matches(str).Count > 0 && title.Matches(str).Count<=0)
					return 1;
				else
					return 0;
			}
			catch
			{
				return -1;
			}
		}
	}
}