# PixelNest Documentation

## Short Description

PixelNest is a web application that is still under development. Purpose of this application is because I wanted to expand my knowledge of Angular and ASP.NET.
For storing data I used MSSQL.

## Frontend

For frontend part as primary and only framework I used Angular 15.
### Angular Project Folder Structure
The project is organized following a feature-modular approach, separating core functionalities, shared components, and feature-specific modules.

Key Directories:

- `core/:` Contains essential services, guards, interceptors, and data transfer objects (DTOs) used across the application.
  - `guards/:` Houses route guards for authentication and role-based access control.
  - `interceptors/:` Contains HTTP interceptors for token handling, logging, etc.
  - `services/:` Global services that provide application-wide functionalities and state mengagment.
- `features/:` Encapsulates specific application features, each with its components and feature modules.
  - `components/:` Contains UI components specific to features.
  - `features.module.ts:` Defines feature-specific modules.
- `shared/:` Contains reusable components, pipes, and modules used across multiple features.
  - `components/:` Reusable UI components shared across different features.
  - `pipes/:` Custom pipes shared across the application.
  - `shared.module.ts:` Imports and exports reusable modules and assets.
- `utility/:` Utility or helper functions and services that are independent of specific application logic.

---

App-level files:

- `app-routing.module.ts:` Defines the root routes of the application.
- `app.component.ts:` Contains the root component (AppComponent) of the Angular application, including its template, styles, and logic.
- `app.module.ts:` The root module of the Angular application, where core services, modules, and components are initialized.

### App overview
Upon entering the web app, users are greeted with a "Get Started" page, which is a feature component that provides basic navigation to the Authentication component. On the "Get Started" page, users also have the option to continue to the app as guest.



#### Authentication component
- The Authentication component consists of two child components:
  - Login Form
  - Register Form
- Upon every successful registration or login operation, intuitive UI feedback is provided using Lottie animations.


#### Dashboard component
The Dashboard is composed of multiple feature child components, including:
- Feed: Displays user posts (the main section).
- Messages (in development).
- Notifications (in development).

These child components are defined in `dashboard-module.ts`, which is imported into `feature-module.ts.`


#### Feed and Post Components  
- The main focus of the dashboard is the Feed, where posts are displayed.
- Each post is a shared component within `shared-module.ts.`

---

#### Modular Architecture
Each component has its own responsibility, allowing for clear separation between:

`Feature Module (feature-module.ts):` Houses feature-specific components like the `Dashboard` and `Get Started` page.
`Shared Module (shared-module.ts):` Contains reusable components such as the post component.
Both modules are imported into the main `app-module.ts.`

---

### User functionality
Once registered and logged in, users have access to several features (with more planned):

- Create Posts: Users can create posts that will be publicly shared across all users, including guests.
- Post Interactions:
  - Like Posts: Available now.
  - Comment on Posts (in development).
  - Save Posts (in development).
- Real-Time Chat: Users will be able to chat with other users in real-time using WebSockets (planned feature).
- Follow each others

---

### Planned features
- Instagram-like Stories: One of the key features to be implemented will be the ability for users to post stories, similar to Instagram’s story functionality.

## Backend
