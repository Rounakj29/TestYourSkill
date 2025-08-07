## TestYourSkill

TestYourSkill is a full-stack application designed to help users generate and practice interview questions for technical interviews. The application consists of a .NET Core backend and an Angular frontend.

## Project Structure

- **Backend**: ASP.NET Core application serving the API and static content
- **Frontend**: Angular application providing the user interface

## Features

- Generate customized interview questions based on:
  - Technology stack
  - Experience level
  - Role or company targeting
  - Number of questions
  - Additional specific requirements
- View questions in multiple formats:
  - Multiple choice questions
  - Coding challenges with code snippets
  - Theory questions
- Export questions to PDF format
- View detailed explanations and model answers
- Track question difficulty levels and tags

## Getting Started

### Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- [Node.js](https://nodejs.org/) (LTS version recommended)
- [Angular CLI](https://angular.io/cli) (v20.1.4 or later)

### Backend Setup

1. Navigate to the Backend directory:
   ```sh
   cd Backend
   ```

2. Restore NuGet packages:
   ```sh
   dotnet restore
   ```

3. Run the backend application:
   ```sh
   dotnet run
   ```

The API will be available at `https://localhost:7xxx` or `http://localhost:5xxx` (port may vary).

### Frontend Setup

1. Navigate to the Frontend directory:
   ```sh
   cd frontend
   ```

2. Install dependencies:
   ```sh
   npm install
   ```

3. Start the development server:
   ```sh
   ng serve
   ```

The Angular application will be available at `http://localhost:4200`.

## Development

### Backend

- The backend is built with ASP.NET Core
- User requests are processed through the UserAndAiMediatorService
- API endpoints are defined in the Controllers directory
- Static content is served from the wwwroot directory

### Frontend

- Angular application (version 20.1.4)
- Main UI components:
  - SkillVerify Component - Handles the interview question generation interface
  - Uses PrimeNG UI components for enhanced user experience
- Styling with Tailwind CSS and custom CSS
- Run `ng generate component component-name` to create new components
- Run `ng build` to build the project (output to `dist/` directory)

## Building for Production

To build the application for production:

1. Build the frontend:
   ```sh
   cd frontend
   ng build --configuration production
   ```

2. Build the backend:
   ```sh
   cd Backend
   dotnet publish -c Release
   ```
