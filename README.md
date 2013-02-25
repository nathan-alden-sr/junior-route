# Updating Project Templates
1. Export the changed ProjectTemplateSourceSolutions to an external project template
	1. Use defaults for _Template name_ and _Template description_; set these later
	2. Use Icon.ico for _Icon Image_ and PreviewImage.png for _Preview Image_
	3. Uncheck _Automatically import the template into Visual Studio_
	4. Click _Finish_
2. Cut JuniorRouteWebApplication.zip and paste it in the appropriate folder alongside T.zip
** _PT1_: C#, no view engine
** _PT2_: VB.NET, no view engine
** _PT3_: C#, Razor view engine
** _PT4_: VB.NET, Razor view engine
3. Extract JuniorRouteWebApplication.zip to its default folder
4. Extract T.zip to its default folder
5. Compare JuniorRouteWebApplication.csproj files in a compare tool and resolve differences
** Be sure $(SolutionDir) is prepended to each packages reference's hint path
6. Compare MyTemplate.vstemplate files in a compare tool and resolve differences
** Be sure NuGet package versions have been updated appropriately
7. Delete the JuniorRouteWebApplication.zip file, the T.zip file and the T folder
8. Compress the contents of the JuniorRouteWebApplication folder (not the folder itself) to T.zip
9. Cut T.zip and paste it in the appropriate folder
10. Delete the JuniorRouteWebApplication folder