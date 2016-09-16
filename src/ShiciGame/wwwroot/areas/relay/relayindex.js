var websocket;
$().ready(function () {
	$('.container').css("min-height", window.innerHeight);
	websocket = new WebSocket("ws://localhost:12866");
	websocket.onopen = function () {
		ShowChatMessage("system-chat", "chat-left chat-success", "连接成功!", "系统");
		//var li = $("<li class='system-chat chat-success'>" + "connected ! " + "</li>");
		//$('#spanstatus').append(li);
	}
	websocket.onmessage = function (evt) {
		var msg = evt.data.split("###");
		ShowChatMessage(msg[0], msg[1], msg[2],msg[3]);
	}
	websocket.onerror = function (evt) {
		ShowChatMessage("system-chat", "chat-left chat-error", evt.data, "系统");
	}
	websocket.onclose = function () {
		ShowChatMessage("system-chat", "chat-left chat-error", "断开连接 !", "系统");
	}
	$(".btnSend").click(function () {
		var msgstyle = $(this).attr('id') === 'send-varse' ? 'chat-primary' : 'chat-default';
		var msgtype = $(this).attr('id') === 'send-varse' ? 'varse' : 'chat';
		if (websocket.readyState === websocket.OPEN) {
			ShowChatMessage("my-chat", "chat-right " + msgstyle, $("#textInput").val(), "我");
			websocket.send(msgtype + "###" + $("#textInput").val());
		}
		else {
			ShowChatMessage("system-chat", "chat-left chat-error", "连接已断开, 请重新连接!", "系统");
		}
		$("#textInput").val("");
	});
	//回车接龙, ↑键聊天
	$("#textInput").on('keyup', function (e) {
		if (e.keyCode != 13 && e.keyCode != 38) return;
		var msgtype, msgstyle;
		if (e.keyCode == 13) {//发送诗词验证, 如果验证成功, 发送出去
			msgstyle = "chat-primary";
			msgtype = "varse";
		}else{
			msgstyle = "chat-default";
			msgtype = "chat";
		}

		if (websocket.readyState === websocket.OPEN) {
			ShowChatMessage("my-chat", "chat-right " + msgstyle, $("#textInput").val(), "我");
			websocket.send(msgtype + "###" + $("#textInput").val());
		}
		else {
			ShowChatMessage("system-chat", "chat-left chat-error", "连接已断开, 请重新连接!", "系统");
		}
		$("#textInput").val("");
	});

	function ShowChatMessage(role,style, content, username) {
		var li = $("<li class=\"" + role + "\"></li>");
		var user = $("<div class=\"user-info\">\
						<div class=\"user-image\"></div>\
						<div class=\"user-name\">"+ username + "</div>\
					</div>");
		var cont = $("<div class=\"chat-body " + style + "\">" + content + "</div>");
		if (role === 'my-chat') {
			li.addClass('flex-right ');
			cont.addClass('chat-right');
			li.append(cont).append(user);
		} else {
			li.addClass('flex-left ');
			cont.addClass('chat-left');
			li.append(user).append(cont);
		}
		$('#spanstatus').append(li);
		//将窗口滚动到最底部
		var spanstatus = document.getElementById("spanstatus");
		spanstatus.scrollTop = spanstatus.scrollHeight;
	}
});