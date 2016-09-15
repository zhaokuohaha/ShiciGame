var websocket;
$().ready(function () {
	websocket = new WebSocket("ws://localhost:12866");
	websocket.onopen = function () {
		$('#spanstatus').text('connected!');
	}
	websocket.onmessage = function (evt) {
		var li = $("<li class='others-chat'>" + evt.data + "</li>");
		$('#spanstatus').append(li);
	}
	websocket.onerror = function (evt) {
		var li = $("<li class='system-chat'>" + evt + "</li>");
		$('#spanstatus').append(li);
	}
	websocket.onclose = function () {
		var li = $("<li class='system-chat'>" + "disconnected ! " + "</li>");
		$('#spanstatus').append(li);
		
	}
	$(".btnSend").click(function () {
		var msgtype = $(this).attr('id') === 'send-varse' ? 'varse' : 'chat';
		console.log(msgtype);
		if (websocket.readyState === websocket.OPEN) {
			var li = $("<li class='my-chat'>" + $("#textInput").val() + "</li>");
			$('#spanstatus').append(li);
			websocket.send(msgtype + "###" + $("#textInput").val());
		}
		else {
			var li = $("<li class='system-chat'>" + "connection is closed ! " + "</li>");
			$('#spanstatus').append(li);
		}

	});
});