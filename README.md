# ASP.NET Core 2.1 MVC - Template

## To Run
(New project name: NewProject)
1) Download this repository.
2) Change name "Template_2_1.sln" to "NewProject.sln"
3) Change folder name "Template_2_1" to "NewProject"
4) Open Template_2_1.sln and change all "Template_2_1" to "NewProject"
5) Find and rename "Template_2_1.csproj" to "NewProject.csproj"
6) Run NewProject.sln in Visual Studio
7) In Startup.cs rename namespace with (Ctrl + R + R)
8) Right-click on NewProject (Project) -> properties -> section debug -> and check Enable SSL 

## General 
- General fixes in generated files.
- Fixed action for email address change in profile section:
  1. You can not asign multiple accounts to the same email address
  2. Username is changed together with email (Added information, that user should login using new email)
  `(Areas\Identity\Pages\Account\Manage\Index.cshtml.cs)`
- Project uses only .min versions of .css and .js files 
  `(wwwroot/css/)` `(wwwroot/js/)`
  
## Project Include
- Bootstrap 4.1.3
- jQuery 3.3.1
- jQuery Validation 1.17
- jQuery Validation Unobtrusive 3.2.10
- qrcodejs
- cldrjs 1.0 (part)
- globalize 1.4 (part)
- jquery-validation-globalize

## NuGet Packages
- AutoMapper 7.0.1
- NWebsec.AspNetCore.Middleware 2.0.0

## Security
#### Attributes
- AutoValidateAntiforgeryTokenAttribute
- RequireHttpsAttribute

#### Headers:
- Strict-Transport-Security
- X-Content-Type-Options
- Referrer-Policy
- X-XSS-Protection
- X-Frame-Options	
- Content-Security-Policy [.ImageSources(s => s.Self()) - is not compatible with icons from Bootstrap 4 - You can comment this line if You want to use them]
	All csp is set to self, You can use only local css, js, images etc.

## Informations
- There is no partial layouts, You can use only `(Views/Shared/_Layout.cshtml)`
- Validation scripts should be added in `(Areas/Identity/Pages/_ValidationScriptsPartial.cshtml)` and `(Views/Shared/_ValidationScriptsPartial.cshtml)`. Do not forget add scripts in to [include="Development"] and [exclude="Development"] part
- For proper HTTPS redirect You must to install 2 SSL certificates for: [www.mywebsite.com] and [mywebsite.com]

## Added
- Database is automaticly created [include: 2 roles: "Admin" / "User", and 2 default accounts to test [logins and paswords check in DbInitializer.cs]]
	`(Context/Context/DbInitializer.cs)` `(Context/IContext/IDbInitializer.cs)`. Do not forget to add database connection string in `(appsettings.json)`
	
- After registration user is added to role "User"
  `(Areas\Identity\Pages\Account\Register.cshtml.cs)`
  
- Before login user has to confirm address email.
	`(Areas\Identity\Pages\Account\Register.cshtml.cs)`
	
- Added to Identity: UserClaimsPrincipalFactory. Now You can use [Authorize(Roles = "Admin")] attributes to secure controllers
	
- Added "EmailSender" `(Services/Services/EmailSender.cs)`. Emails are correctly sent [but You have to set up account in `(appsettings.json)` as example inside]
	
- Added QR code - You can set up 2fa. Your site name You have to change in `(Areas\Identity\Pages\Account\Manage\EnableAuthenticator.cshtml.cs)` in method `GenerateQrCodeUri(string email, string unformattedKey)`
	
- Added AutoMapper `(Mapper\AutoMapperConfig.cs)`. Added as Singleton and is ready to DI
	
- Added "enum InternalStatus" `(Models\Models\InternalStatus.cs)`, which can be retuned from service to controller - Now you can set up status color and status message for user notification 

- Added information, when user didn't confirm email [Confirm Your email address!], insted of [Invalid login attempt.] `(Services\IServices\IEmailConfirmationCheckerService.cs)` `(Services\Services\EmailConfirmationCheckerService.cs)`

- Added Logger to file "logs.txt" as Singleton, which saves exception. Created for context. You can set up in `(Services\IServices\ILoggerService.cs)` `(Services\Services\LoggerService.cs)`. Example how to use in `(Services\Services\EmailConfirmationCheckerService.cs)`
