<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication2.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js"></script>
	<script src="resources/AnalogClock.js"></script>
	<script src="resources/json5.js"></script>
</head>
<body style="background-color: #505050;">
	<script>
		function initAnalogClock() {
			//console.log(svg);
			$('.analog-clock').each(function () {
				var target = $(this);
				var json = target.attr('options') || '{}';
				//console.log(json);
				var o = JSON5.parse(json);

				o.target = target;
				var clock = new AnalogClock(o);

			});
		}


	</script>
	<style>
		.analog-clock {
			width: 310px;
			height: 310px;
			max-height: 300px;
		}
	</style>
	<form id="form1" runat="server">


		<table style="zoom: .5;">
			<tr>
				<td>
					<span class="analog-clock" options="{smooth:false}"></span>
				</td>
				<td><span class="analog-clock" options="{style:2}"></span></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td></td>
			</tr>
			<tr>
				<td>
					<span class="analog-clock" options="{style:3}"></span>
				</td>
				<td>
					<span class="analog-clock" options="{style:4}"></span>
				</td>
			</tr>
			<tr>
				<td>
					<span class="analog-clock" options="{style:5}"></span>
				</td>
				<td>
					<span class="analog-clock" options="{style:6}"></span>
				</td>
			</tr>
		</table>

	</form>
	<script>
		initAnalogClock();
	</script>
</body>
</html>
