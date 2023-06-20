/// <reference path="scripts/analogclock-faces.js" />
class AnalogClock {
	constructor(options) {
		var _this = this;
		const defaultOptions = {
			target: null, // mandatory
			style: 1,
			smooth: true,
			interval: 30,
		};
		this.options = Object.assign(defaultOptions, options);
		this.svg = null;
		this.hour_hand = null;
		this.minute_hand = null;
		this.second_hand = null;
		this.second_hand_shadow = null;
		//----------
		/*var filename = 'clock-' + this.options.style + '.svg';//?a=' + new Date().getMilliseconds();
		$.get('resources/faces/' + filename, function (svg) {
			_this.svg = $(svg);
			_this.init();
		}, 'text');*/
		/*var filename = 'clock-' + this.options.style + '.svg';//?a=' + new Date().getMilliseconds();
		fetch('resources/faces/' + filename)
		.then((res) => res.text())
		.then((svg) => {
			_this.svg = $(atob(svg));
			_this.init();
		});*/
		var base64 = AnalogClock.faces['clock-' + this.options.style];
		this.svg = $(atob(base64));
		this.init();
	}

	init() {
		const target = this.options.target;
		if (target.attr('handled') == 1) return;
		target.html('');
		target.append(this.svg);
		target.attr('handled', 1);
		this.svg.attr('style', target.attr('style'));
		this.svg.css('display', 'inline-block');
		this.hour_hand = this.svg.find('#hour-hand');
		this.minute_hand = this.svg.find('#minute-hand');
		this.second_hand = this.svg.find('#second-hand');
		this.second_hand_shadow = this.svg.find('#second-hand-shadow');

		setInterval(() => {
			const now = new Date();
			this.update(now.getHours(), now.getMinutes(), now.getSeconds(), now.getMilliseconds());
		}, this.options.interval);
	}

	update(hours, minutes, seconds, milliseconds) {
		const options = this.options;

		function rotate(angle, x, y) {
			return `rotate(${angle},${x || 256},${y || 256})`;
		}

		function translate(x, y) {
			return `translate(${x},${y})`;
		}

		const s = seconds + (options.smooth ? milliseconds / 1000 : 0);
		const m = minutes + s / 60;
		const h = (hours + m / 60) % 12;

		const h_a = (360 * h) / 12;
		const m_a = (360 * m) / 60;
		const s_a = (360 * s) / 60;

		this.hour_hand.attr('transform', rotate(h_a));
		this.minute_hand.attr('transform', rotate(m_a));
		this.second_hand.attr('transform', rotate(s_a));
		this.second_hand_shadow.attr('transform', translate(3, 8) + rotate(s_a));
	}
}
AnalogClock.faces = AnalogClock.faces || {};