$(function () {
	 $.ajax({
		url: "/Home/GetRandomVerse",
		data: {"count":"50"},
		success: function (data) {
			console.log(data);
			setInterval(startRequest(data), 4800);
		},
		error: function () {
			return "";
		}
	})
	var abg = $(".animate-background");
	var dmTemp = 1;
	
	function random(min, max) {
		return Math.floor(min + Math.random() * (max - min));
	};
	function getReandomColor() {
		return '#' + (function (h) { return new Array(7 - h.length).join("0") + h })((Math.random() * 0x1000000 << 0).toString(16));
	};
	function init() {
		var h = 50;//字幕高度
		var _top = 0;
		var _height = $(".animate-background").height();
		var _width = $(".animate-background").width();
		$(abg).find('div').each(function () {
			if ($(this).is(':animated')) { return; }
			_top += h;
			if (top > _height - h) { _top = 0; }
			var start_left = _width + random(-10, 400);//左边字幕开始区域
			$(this).css({
				left: start_left,
				top: _top,
				color: getReandomColor()
			});
			var time = random(14000, 25000);
			var end_left = _width + 500;//字幕结束消失的位置(相对现实区域的框)
			$(this).animate({
				left: "-" + end_left + "px"
			}, time, function () {
				this.remove();
			});
		});
	};

	function startRequest(data) {
		return function () {
			var base_item = random(0, data.length);
			var count = random(0, 20)
			for (var i = 0; i < count; i++) {
				var pom = $('<div class=pom>' + data[(base_item + i) % data.length].sa_head + "&emsp;&emsp;" + data[(base_item + i) % data.length].sa_tail + '</div>');
				abg.append(pom);
			}
			init();
		}
	};
});