# PixelNest Documentation
## Links: 
Live chat and profile photo cropper video: https://www.youtube.com/watch?v=gNDLqYFnqsU
## Short Description

PixelNest is a web application currently under development, created with the purpose of expanding my knowledge in Angular and ASP.NET. The application is backed by MSSQL for data storage, integrating a modern tech stack to provide a robust environment for learning and development.

## API endpoints listed down below:

## Authentication

  #### POST `/api/Authentication/Registration`
  - Request body: 
      ```
        {
          "firstname": "string",
          "lastname": "string",
          "username": "string",
          "password": "string",
          "email": "string"
       } 
      ```
   - Responses:
     - `200 OK` :
      ```
        {
          "message": "string",
          "isSuccess": true
        }
      ```
     - `400 Bad Request`, `404 Not Found` :
      ```
        {
          "message": "string",
          "isSuccess": false
        }
      ```

 #### POST `/api/Authentication/Logout`
  - Request body: 
      ```
        {
          "email": "string"
        }
      ```
  - Response: `200 OK`

 #### POST `/api/Authentication/Login`
  - Request body: 
      ```
        {
          "firstname": "string",
          "lastname": "string",
          "username": "string",
          "password": "string",
          "email": "string"
       } 
      ```
   - Responses:
     - `200 OK` :
      ``` 
        {
          "response": "string",
          "token": "string",
          "username": "string",
          "email": "string",
          "isSuccessful": true
        }
      ```
     - `400 Bad Request`, `404 Not Found`
    
#### POST `/api/Authentication/IsLoggedIn`
  - Request Header: `JWT TOKEN`
  - Responses: `401 Unauthorized` and `200 OK`

## Comment

 #### GET `/api/Comments/GetReplies
  - Request URL `/api/Comment/GetReplies?initialParentID=${number}`
  - Response:
    ```
        [
          {
            "commentID": 0,
            "totalLikes": 0,
            "userID": 0,
            "commentText": "string",
            "username": "string",
            "postID": 0,
            "totalReplies": 0,
            "parentCommentID": 0,
            "likedByUsers": [
              {
                "username": "string",
                "commentID": 0
              }
            ],
            "replies": [
              "string"
            ]
          }
        ]
    ```
    
  #### GET `/api/Comments/GetComments`
  - Request URL `/api/Comment/GetComments?postID=${number}`
  - Response:
    ```
        [
          {
            "commentID": 0,
            "totalLikes": 0,
            "userID": 0,
            "commentText": "string",
            "username": "string",
            "postID": 0,
            "totalReplies": 0,
            "parentCommentID": 0,
            "likedByUsers": [
              {
                "username": "string",
                "commentID": 0
              }
            ],
            "replies": [
              "string"
            ]
          }
        ]
    ```

  #### POST `/api/Comments/LikeComment`
  - Request:
  ```
    {
      "username": "string",
      "commentID": 0
    }
  ```
  - Responses: `200 OK`, `400 BadRequest`, `404 NotFound`

## Post  
#### POST `/api/Post/ShareNewPost`
- Request in form data format:
  ```
    {
      "postDescription":"string",
      "ownerUsername":"string",
      "photoDisplay":"string",
      "location":"string",
      "photos": string[]
    }
  ```
- Response:
  ```
    {
      "postID": 0,
      "isSuccessfull": true,
      "message": "string"
    }
  ```

#### POST `/api/Post/SavePost`
- Request in form data format:
  ```
    {
      "username": "string",
      "postID": 0
    }
  ```
- Responses:
  - `200 OK`, if post was saved successfully.
  - `400 BadRequest`, if request body is missing or its not right format.
  - `404 NotFound`, Database related error.

#### GET `/api/Post/GetPosts`
- Request URL format:
  - `/api/Post/GetPosts?page=1&maximumPosts=5`
  - `/api/Post/GetPosts?username=${username}&location=${string}&page=${number}&maximumPosts=${number}`
- All of these query paramters are optional. User can pass location or username only, or none. In that case all posts will be fetched.
- Response:
   ```
     [
        {
          "postID": 0,
          "ownerUsername": "string",
          "postDescription": "string",
          "totalLikes": 0,
          "totalComments": 0,
          "location": "string",
          "publishDate": "2024-10-21T08:38:27.017Z",
          "likedByUsers": [
            {
              "username": "string",
              "postID": 0
            }
          ],
          "savedByUsers": [
            {
              "username": "string",
              "postID": 0
            }
          ],
          "imagePaths": [
            {
              "pathID": 0,
              "postID": 0,
              "path": "string",
              "photoDisplay": "string"
            }
          ]
        }
    ]
   ```
   
## Story

#### GET `/api/Story/GetStories`
- Request URL: `/api/Story/GetStories?username={string}&maximum={number}`
- Username parameter is optional. If client does not send username with query, API will fetch stories of all user.
If username is passed then API will fetch stories that belong to that user.

- Response:
```
  {
  "ownerUsername": "string",
  "storiesLeft": 0,
  "stories": [
    {
      "storyID": 0,
      "ownerUsername": "string",
      "seenByUser": true,
      "imagePaths": [
        {
          "pathID": 0,
          "postID": 0,
          "storyID": 0,
          "path": "string",
          "photoDisplay": "string"
        }
      ]
    }
  ]
}
```

#### POST `api/Story/PublishStory
- Request body:
```
  {
    username: "string",
    photoDisplay: "string",
    storyImage: string[]
  }
```
- Response body:
```
  {
    "isSuccessful": bool,
    "message": "string",
    "storyID": 0
  }
```

#### POST `api/Story/MarkStoryAsSeen
- Request body:
```
  {
    username: "string",
    storyID: 0
  }
```
- Response body:
```
  {
    "isSuccessful": bool,
    "message": "string",
    "storyID": 0
  }
```
#### GET `api/Story/GetViewers
- Request URL: `/api/Story/GetViewers?username={string}&storyID={number}`
- Response body:
```
  [
    {
      username: "string"
    }
  ]
```

## Notification

#### POST `api/Notification/MarkAsOpened`
- Request body:
```
  {
    "notificationID": [
        0
      ]
  }
```
- Responses: `bool`

#### GET `api/Notification/CountNotifications`
- Request URL:  `api/Notification/CountNotifications`
- Responses: `number`

#### GET `api/Notification/GetAllNotifications`
- Request URL:  `api/Notification/GetAllNotifications`
- Response body:
  ```
  [
    {
      "username": "string",
      "message": "string",
      "date": "2025-02-13T15:55:28.587Z",
      "postID": 0,
      "notificationID": 0,
      "imagePath": [
        {
          "pathID": 0,
          "postID": 0,
          "storyID": 0,
          "path": "string",
          "photoDisplay": "string"
        }
      ]
    }
  ]
  ```

## Chat

#### GET `api/Chat/GetNumberOfMessages`
- Request URL `api/Chat/GetNumberOfMessages`
- Response body: `number`

#### POST `api/Chat/MarkAsRead`
- Request body:
  ```
  {
    "messageID": [
      0
    ]
  }
  ```
- Response body: `bool`

#### GET `api/Chat/GetUserChats`
- Request URL: `api/Chat/GetUserChats`
- Response body:
  ```
  [
    {
      "chatID": "string",
      "messages": [
        {
          "sender": "string",
          "receiver": "string",
          "message": "string",
          "messageID": 0,
          "isSeen": bool,
          "source": "string",
          "dateSent": "2025-02-19T17:43:16.516Z"
        }
      ]
    }
  ]
  ```

#### GET `api/Chat/GetUserToUserMessages`
- Request URL: `api/Chat/GetUserToUserMessages?targetUsername={username}`
- Response body:
  ```
  [
    {
      "sender": "string",
      "receiver": "string",
      "message": "string",
      "messageID": 0,
      "isSeen": bool,
      "source": "string",
      "dateSent": "2025-02-19T17:45:04.472Z"
    }
  ]
  ```

#### POST `api/Chat/SendMessage`
- Request body:
  ```
    {
      "message": "string",
      "senderUsername": "string",
      "receiverUsername": "string"
    }
  ```
- Response: `bool`

#### POST `api/Chat/LeaveRoom`
- Request body: `none`
- Request URL: `api/Chat/LeaveRoom?receiverUsername={username}`
- Response: `bool`
- 
#### POST `api/Chat/JoinRoom`
- Request body: `none`
- Request URL: `api/Chat/JoinRoom?receiverUsername={username}`
- Response: `bool`
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
- Post Location Feature: Each post can be associated with a location. When a user clicks on a location attached to a post, the Location Component opens. This component displays posts related to that location, allowing users to interact with a map. Users can select specific locations to see photos from those places or browse posts based on locations of interest.


#### Location Component
- Map Interactivity: The Location Component integrates with a map interface, allowing users to explore posts based on geographic locations.
- Post Filtering by Location: Users can filter posts based on specific areas by interacting with the map. When a location is selected, the component displays posts relevant to that region.
- User Engagement: Within the map, users can choose the location they want to explore and see photos or posts from that place, providing a more immersive way to interact with posts.
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
REST API for application and project strucuture is listed down below.

- Controllers
  - AuthenticationController.cs
  - CommentController.cs
  - PostController.cs
  - UserController.cs
- Data
  - DataContext.cs
- Dto
- Gateway
  - FileUpload.cs
- Interfaces
  - IAuthenticationRepository.cs
  - IAuthenticationService.cs
  - IPostRepository.cs
  - IPostService.cs
  - ICommentRepository.cs
  - ICommentService.cs
  - IFileUpload.cs
  - IUserService.cs
  - IUserRepository.cs
- Mappers
  - UserMapper.cs
- Migrations
- Models
  - Comment.cs
  - Follow.cs
  - ImagePath.cs
  - LikedComments.cs
  - LikedPosts.cs
  - Post.cs
  - SavedPosts.cs
  - Story.cs
  - User.cs
- Proxy
  - ProxyController.cs
- Repository
  - AuthenticationRepository.cs
  - CommentRepository.cs
  - PostRepository.cs
  - UserRepository.cs
- Responses
  - FollowResponse.cs
  - LoginResponse.cs
  - PostResponse.cs
  - RegisterResponse.cs
- Security
  - PasswordEncoder.cs
  - TokenGenerator.cs
- Services
  - AuthenticationService.cs
  - CommentService.cs
  - PostService.cs
  - UserService.cs
- Utility
  - CommentUtility.cs
  - FolderGenerator.cs
  - PostUtility.cs
  - UserUtility.cs
