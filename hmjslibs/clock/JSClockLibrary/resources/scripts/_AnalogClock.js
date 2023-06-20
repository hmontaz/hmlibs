var AnalogClock = function (o) {
	var _this = this;
	var defaultOptions = {
		target: null,//mandatory
		style: 1,
		smooth: true,
		interval: 30,
	}
	this.options = $.extend(defaultOptions, o);
	//----------
	var filename = 'clock-' + this.options.style + '.svg?a=' + new Date().getMilliseconds();
	$.get('resources/faces/' + filename, function (svg) {
		_this.svg = $(svg);
		_this.init();
	}, 'text');
};

AnalogClock.prototype.init = function () {
	var _this = this;
	var target = this.options.target;
	if (target.attr('handled') == 1) return;
	target.html('');
	target.append(this.svg);
	target.attr('handled', 1);
	//this.svg.attr({ height: target.height() });
	this.svg.attr('style', target.attr('style'));
	this.svg.css('display', 'inline-block');
	this.hour_hand = this.svg.find('#hour-hand');
	this.minute_hand = this.svg.find('#minute-hand')
	this.second_hand = this.svg.find('#second-hand')
	this.second_hand_shadow = this.svg.find('#second-hand-shadow')

	setInterval(function () {
		var now = new Date();
		_this.update(now.getHours(), now.getMinutes(), now.getSeconds(), now.getMilliseconds());
	}, this.options.interval);

};

AnalogClock.prototype.update = function (hours, minutes, seconds, milliseconds) {
	var options = this.options;
	function rotate(angle, x, y) { return 'rotate(' + angle + ',' + (x || 256) + ',' + (y || 256) + ')'; }
	function translate(x, y) { return 'translate(' + x + ',' + y + ')'; }
	var s = seconds + (options.smooth ? (milliseconds / 1000) : 0);
	var m = minutes + s / 60;
	var h = (hours + m / 60) % 12;

	var h_a = 360 * h / 12;
	var m_a = 360 * m / 60;
	var s_a = 360 * s / 60;
	this.hour_hand.attr('transform', rotate(h_a));
	this.minute_hand.attr('transform', rotate(m_a));
	this.second_hand.attr('transform', rotate(s_a));
	this.second_hand_shadow.attr('transform', translate(3, 8) + rotate(s_a));

};