﻿@Inherits Junior.Route.ViewEngines.Razor.Template
<!DOCTYPE html>
<html>
	<head>
		<title>Hello, world.</title>
	</head>
	<body>
		@RenderBody()
		@RenderSection("Time")
	</body>
</html>