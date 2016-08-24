$(function () {
	var abg = $(".animate-background");
	var bgColor = GetRandomColor(190,240);
	abg.css("background", bgColor);
	var data = [
		"浮想绕曲岸",
		"圆影覆华池",
		"常恐秋风早",
		"飘零君不知",
		"一骑红尘妃子笑，无人知是荔枝来。",
		"日啖荔枝三百颗，不辞长作岭南人。",
		"世间珍果更无加，玉雪肌肤罩绛纱。",
		"唤起封姨清晚景，更将荔子荐新圆。",
		"寒夜客来茶当酒，竹炉汤沸火初红。",
		"无由持一碗，寄与爱茶人。",
		"琴里知闻唯渌水，茶中故旧是蒙山。",
		"呵笔难临帖，敲床且煮茶。",
		"矮纸斜行闲作草，晴窗细乳戏分茶。",
		"豆蔻连梢煎熟水，莫分茶。",
		"老去逢春如病酒，唯有，茶瓯香篆小帘栊。",
		"当年曾胜赏，生香熏袖，活火分茶。",
		"商人重利轻别离，前月浮梁买茶去。",
		"酿泉为酒，泉香而酒洌；",
		"得欢当作乐，斗酒聚比邻。",
		"中军置酒饮归客，胡琴琵琶与羌笛。",
		"欲持一瓢酒，远慰风雨夕。",
		"把酒对斜日，无语问西风。",
		"山远近，路横斜，青旗沽酒有人家。",
		"木兰舟上珠帘卷，歌声远，椰子酒倾鹦鹉盏。",
		"何意更觞昌歜酒，为君击节一长歌。",
		"称是秦时避世人，劝酒相欢不知老。",
		"春酒香熟鲈鱼美，谁同醉？缆却扁舟篷底睡。",
		"摘青梅荐酒，甚残寒，犹怯苎萝衣。",
		"酒伴来相命，开尊共解酲。",
		"江南三月听莺天，买酒莫论钱。",
		"何时更杯酒，再得论心胸。",
		"鹔鹴换美酒，舞衣罢雕龙。",
	];
	showPoem(0);
	function GetRandomNum(Min, Max) {
		var Range = Max - Min;
		var Rand = Math.random();
		return (Min + Math.round(Rand * Range));
	};
	function GetRandomColor(min, max) {
		var r = GetRandomNum(min, max);
		var g = GetRandomNum(min, max);
		var b = GetRandomNum(min+10, max);
		return 'rgb(' + r + ',' + g + ',' + b + ')';
	};
	function showPoem(i) {
		setInterval(function () {
			i++;
			var poem = $('<div class=poem id ='+i+'></div>');
			poem.text(data[i%data.length]);
			poem.css("top", GetRandomNum(150, abg.height()) + 'px');
			poem.css("left", GetRandomNum(0, abg.width() - 180) + 'px');
			poem.css("background-color", bgColor);
			abg.append(poem);
			poem.show(500);
			setTimeout(function () {
				poem.hide(1000);
				setTimeout(function () {
					poem.remove();
				}, 1000);
			}, 5000)
		}, 800);
	};
	function animate(object) {

	}
	
	
});