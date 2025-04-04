Below is an example README.md file for your project. You can adjust details as needed:

---

# Reverse Proxy Application

## Overview

This project is a reverse proxy application built with ASP.NET Core. It forwards API requests from a client (e.g., Cursor) to a dynamic Ngrok endpoint. The application features:

- **Reverse Proxy:** Forwards GET and POST requests to the current Ngrok URL.
- **Dynamic Ngrok URL:** The Ngrok URL is stored in a dedicated service (`NgrokUrlService`) and can be updated on the fly without republishing the application.
- **Admin Endpoint:** A secure endpoint (`/api/admin/update-ngrok-url`) that allows an administrator to update the Ngrok URL using a secret admin key.
- **Token Authentication:** A custom middleware (`TokenAuthenticationMiddleware`) protects all routes (except the landing page) by validating a token provided in the `Authorization` header.
- **Landing Page:** A default HTML landing page at the base URL that provides a welcome message, description, and author information.
- **Logging:** Exception logging and general logging are integrated to help diagnose issues (with support for Azure Web App Diagnostics).

## Project Structure

- **Controllers**
  - `HomeController.cs`: Returns the landing page.
  - `ForwardingController.cs`: Implements the reverse proxy functionality.
  - `AdminController.cs`: Provides a secure endpoint for updating the Ngrok URL.
- **Middleware**
  - `TokenAuthenticationMiddleware.cs`: Checks for an `Authorization` header on incoming requests (excluding the base route and preflight requests).
- **Services**
  - `NgrokUrlService.cs`: A singleton service that holds and updates the current Ngrok URL.
- **Configuration**
  - `appsettings.json`: Contains configuration for logging, allowed hosts, the default Ngrok URL, the token for authentication, and the secret admin key.

## Prerequisites

- .NET SDK (as specified by the project; for example, .NET 6 or later)
- An active Ngrok URL (for forwarding requests)
- Azure App Service (optional, for deployment)
- A secret token for API authentication and a secret admin key for the admin endpoint. These should be set in `appsettings.json` or provided via environment variables/Azure App Settings.

## Setup and Configuration

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/reverse-proxy-app.git
   cd reverse-proxy-app
   ```

2. **Configure App Settings**

   Open `appsettings.json` and update the following keys:
   - `TargetSettings:NgrokUrl`: Your default Ngrok URL.
   - `Token`: The token required in the `Authorization` header (format: "Bearer {token}").
   - `SecretAdminKey`: A secret value for securing the admin endpoint.

   *Tip:* You can override these settings using environment variables or, if deployed to Azure, through the App Service Application Settings.

3. **Build and Run Locally**

   Restore dependencies and build the project:

   ```bash
   dotnet restore
   dotnet build
   ```

   Run the application:

   ```bash
   dotnet run
   ```

   Access the landing page at `http://localhost:<port>/`.

## How to Use the Application

- **Landing Page:**  
  Visit the base URL (`/`) to view the welcome message, application description, and author information.

- **Forwarding Requests:**  
  Send GET or POST requests to `/api/forwarding/{**catchAll}`. These requests will be forwarded to the Ngrok URL currently stored in `NgrokUrlService`.

- **Token Authentication:**  
  All routes (except the landing page) require an `Authorization` header with the correct token. For example:  
  `Authorization: Bearer <your-token>`

- **Admin Endpoint:**  
  To update the Ngrok URL dynamically, send a POST request to `/api/admin/update-ngrok-url` with the header `X-ADMIN-KEY: <YourSecretAdminKey>` and a JSON body:
  
  ```json
  {
    "NewUrl": "https://new-ngrok-url.ngrok-free.app"
  }
  ```

  This endpoint updates the Ngrok URL without needing to redeploy the application.

## Deployment to Azure

- **Publish:**  
  Use Visual Studio or Azure CLI to publish the application to Azure App Service.

- **Configuration:**  
  Set your `TargetSettings:NgrokUrl`, `Token`, and `SecretAdminKey` via the Azure Portal’s Application Settings to override the values in `appsettings.json`.

- **Remote Debugging:**  
  Optionally, enable remote debugging via the Azure Portal if you need to troubleshoot issues in production.

## Logging and Diagnostics

- The application logs exceptions and other events using the built-in ASP.NET Core logging framework.
- Azure Web App Diagnostics are enabled via `AddAzureWebAppDiagnostics()` in `Program.cs`.
- You can view logs in real time using the **Log Stream** feature in the Azure Portal.
- For advanced diagnostics, consider integrating Application Insights.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author

Mohammed Khalil

---

Feel free to update or extend this README file as your project evolves. If you need further details or adjustments, let me know!
