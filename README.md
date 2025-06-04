# PixelNest Documentation
## Links: 
Live chat and profile photo cropper video: https://www.youtube.com/watch?v=gNDLqYFnqsU
## Short Description

PixelNest is a web application currently under development, created with the purpose of expanding my knowledge in Angular and ASP.NET. The application is backed by MSSQL for data storage, integrating a modern tech stack to provide a robust environment for learning and development.
## Backend
REST API for application and project strucuture is listed down below.

- Controllers
  - AuthenticationController.cs
  - CommentController.cs
  - PostController.cs
  - UserController.cs
  - NotificationController.cs
  - ChatController.cs
- Data
  - DataContext.cs
- Dto
  - Projections
  - Google
  - Websocket
- Gateway
  - BlobStorageUpload.cs
  - FileUpload.cs
- Interfaces
  - IAuthenticationRepository.cs
  - IAuthenticationService.cs
  - IChatService.cs
  - IChatRepository.cs
  - IGoogleService.cs
  - IGoogleRepository.cs
  - INotificationRepository.cs
  - INotificationService.cs
  - IPostRepository.cs
  - IPostService.cs
  - ICommentRepository.cs
  - ICommentService.cs
  - IFileUpload.cs
  - IUserService.cs
  - IUserRepository.cs
- Middleware
  - APICallLimiter.cs
  - WebSocketMiddleware.cs
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
  - SeenStory.cs
  - SeenMessage.cs
  - Message.cs
  - Notification.cs
  - Story.cs
  - User.cs
- Proxy
  - ProxyController.cs
- Repository
  - AuthenticationRepository.cs
  - CommentRepository.cs
  - PostRepository.cs
  - UserRepository.cs
  - NotificationRepository.cs
  - ChatRepository.cs
  - GoogleRepository.cs
- Responses
  - Google
    - GoogleAccountResponse.cs
    - GoogleLoginResponse.cs
  - FollowResponse.cs
  - LoginResponse.cs
  - PostResponse.cs
  - RegisterResponse.cs
- Security
  - PasswordEncoder.cs
  - TokenGenerator.cs
  - SASTokenGenerator.cs
- Services
  - Google
    - GoogleService.cs
  - Menagers
    - WebSocketConnectionMenager.cs
  - AuthenticationService.cs
  - CommentService.cs
  - PostService.cs
  - UserService.cs
  - ChatService.cs
  - NotificationService.cs
  
- Utility
  - Google
    - GoogleUtility.cs
  - BobFolderGenerator.cs
  - CommentUtility.cs
  - FolderGenerator.cs
  - PostUtility.cs
  - UserUtility.cs


## API endpoints listed down below:
## User

#### POST `/api/user/close-connection`
  - Request body: `none`
  - Response: `status code OK`, `unauthorized`

#### GET `/api/user/{clientGuid}/followings`
  - Request URL:  `/api/user/{clientGuid}/followings`
  - Response:
    ```
      [
        {
          "followingUsername": "string",
          "followingClientGuid": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        }
      ]
    ```
#### GET `/api/user/{clientGuid}/followers`
   - Request URL:  `/api/user/{clientGuid}/followers`
   - Response:
     ```
       [
        {
          "followerUsername": "string",
          "followerClientGuid": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        }
      ]
     ```
#### POST `/api/user/follow/{clientGuid}`
   - Request URL:  `/api/user/follow/{clientGuid}`
   - Response: `status code`

#### GET `/api/user/users/{clientGuid}`
   - Request URL: `/api/user/users/{clientGuid}`
   - Response:
     ```
       {
          "followings": 0,
          "followers": 0,
          "totalPosts": 0,
          "username": "string",
          "name": "string",
          "lastname": "string",
          "clientGuid": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "canFollow": true,
          "canEdit": true,
          "chatID": "string",
          "listFollowings": [
            {
              "followerUsername": "string",
              "followingGuid": "string"
            }
          ],
          "listFollowers": [
            {
              "followerUsername": "string",
              "followingGuid": "string"
            }
          ]
        }
     ```
#### GET `/api/user/followings/{clientGuid}`
 - Request URL: `/api/user/followings/{clientGuid}`
 - Response: `boolean`
#### PUT `/api/user/profile-picture`
 - Request body:
   ```
     {
        "Firstname":"string",
        "Lastname":"string",
        "Username":"string",
        "ClientGuid":"string"
        "ProfilePicture":"string($binary)"
     }
   ```
#### GET `/api/user/profile-picutre/{clientGuid}`
  - Request URL: `/api/user/profile-picutre/{clientGuid}`
  - Response: `string`
 
#### GET `/api/user/search`
  - Request URL: `/api/user/search?username=${string}`
  - Response: `string`


#### GET `/api/user/me`
  - Request URL: `/api/user/search?username=${string}`
  - Response:
```
{
  "followings": 0,
  "followers": 0,
  "totalPosts": 0,
  "username": "string",
  "email": "string",
  "profileImagePath": "string",
  "name": "string",
  "lastname": "string",
  "clientGuid": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "canFollow": true,
  "canEdit": true,
  "chatID": "string",
  "listFollowings": [
    {
      "followerUsername": "string",
      "followingGuid": "string"
    }
  ],
  "listFollowers": [
    {
      "followerUsername": "string",
      "followingGuid": "string"
    }
  ]
}
```
## Authentication

  #### POST `/api/authentication/register`
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

 #### POST `/api/authentication/logout`
  - Request body: 
      ```
        {
          "email": "string"
        }
      ```
  - Response: `200 OK`

 #### POST `/api/authentication/login`
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
    
#### POST `/api/authentication/status`
  - Request Header: `JWT TOKEN`
  - Responses: `401 Unauthorized` and `200 OK`
  - 
#### POST `/api/authentication/token/refresh`
  - Request Header: `JWT TOKEN`
  - Responses: `401 Unauthorized` and `200 OK`
  - 
#### GET `/api/authentication/login-response`
  - Request URL: `/api/authentication/login-response?state={string}`
  - Responses: `400 BadRequest("State is Missing")`, `400 BadRequest("No login response found for the given state.")` and `200 Ok()`
  - 
#### POST `/api/authentication/save`
  - Request URL: `/api/authentication/save?state={string}`
  - Behaviour: `https://frontend/Authentication/Redirect-Page` and `https://frontend/Authentication/Save-Location`
#### GET `/api/authentication/google/signin`
  - Request URL: `/api/authentication/save?state={string}`
  - Responses: `401 Unauthorized` and `200 OK`


## Comment

 #### GET `/api/comment/replies
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
    
  #### GET `/api/comment/comments`
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

  #### POST `/api/comment/like`
  - Request:
  ```
    {
      "username": "string",
      "commentID": 0
    }
  ```
  - Responses: `200 OK`, `400 BadRequest`, `404 NotFound`

## Post  
#### GET `/api/post/{postGuid}`
- Request URL: `/api/post/{postGuid`
- Response:
```
  {
  "postID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "clientGuid": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "isDeletable": true,
  "ownerUsername": "string",
  "postDescription": "string",
  "totalLikes": 0,
  "totalComments": 0,
  "location": "string",
  "publishDate": "2025-06-04T19:55:55.678Z",
  "likedByUsers": [
    {
      "clientGuid": "string",
      "username": "string",
      "postID": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
  ],
  "savedByUsers": [
    {
      "username": "string",
      "postID": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
  ],
  "imagePaths": [
    {
      "pathID": 0,
      "postID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "storyID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "path": "string",
      "photoDisplay": "string"
    }
  ]
}
```
#### GET `/api/post/cache/state`
- Request URL: `/api/post/cache/state`
- Responses: `Ok(true)` or `Ok(false`

#### POST `/api/post/new`
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

#### POST `/api/post/{postGuid}/save`
- Request URL: `/api/post/{postGuid}/save`
- Request body: `none`
  ```
 
  ```
- Responses:
  - `200 OK`, if post was saved successfully.
  - `400 BadRequest`, if request body is missing or its not right format.
  - `404 NotFound`, Database related error.

#### GET `/api/post/posts`
- Request URL format:
  - `/api/Post/GetPosts?page=1&maximumPosts=5`
  - `/api/Post/GetPosts?clientGuid=${string}&location=${string}&page=${number}&maximumPosts=${number}`
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
   
#### DELETE `/api/post/{postGuid}`
- Request URL: `/api/post/{postGuid}`
- Response:
 ```
     {
        "isValid": true,
        "isSuccess": true,
        "message": "string"
     }
 ```

#### POST `api/post/{postGuid}/like`
- Request URL: `api/post/{postGuid}/like`
- Responses: `Ok()`, `BadRequest()` and `NotFound()`

#### POST `api/post/{postGuid}/comment`
- Request URL: `api/post/{postGuid}/comment`
- Responses: `Ok()`, `BadRequest()` and `NotFound()`

## Story

#### GET `/api/story/stories`
- Request URL: `/api/story/stories?forCurrentUser={boolean}`
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

#### POST `api/sotry/new
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

#### POST `api/story/seen
- Request body:
```
  {
     "storyID": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
```
- Response body:
```
  {
    "isSuccessful": true,
    "message": "string",
    "storyID": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
```
#### GET `api/story/viewers
- Request URL: `api/story/viewers?storyID={string}`
- Response body:
```
  [
    {
      username: "string"
    }
  ]
```

## Notification

#### POST `/api/notification/open`
- Request body:
```
  {
    "notificationID": [
        0
      ]
  }
```
- Responses: `bool`

#### GET `api/notification/unread/count`
- Request URL:  `api/notification/unread/count`
- Responses: `number`

#### GET `api/notification/notifications`
- Request URL:  `api/notification/notifications`
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

#### GET `api/chat/unread-messages`
- Request URL `api/chat/unread-messages`
- Response body: `number`

#### POST `api/chat/message/read`
- Request body:
  ```
  {
    "messageID": [
      0
    ]
  }
  ```
- Response body: `bool`

#### GET `api/chat/chats`
- Request URL: `api/chat/chats`
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

#### GET `api/chat/messages`
- Request URL: `api/chat/messages?chatID={string}`
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

#### POST `api/chat/message/send`
- Request body:
  ```
    {
      "message": "string",
      "clientGuid": "string"
    }
  ```
- Response: `bool`

#### POST `api/chat/room/leave/{targetClientGuid}`
- Request body: `none`
- Request URL: `api/chat/room/leave/{targetClientGuid}`
- Response: `bool`
- 
#### POST `api/chat/room/join/{targetClientGuid}`
- Request body: `none`
- Request URL: `api/chat/room/join/{targetClientGuid}`
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
  - Comment on Posts.
  - Save Posts.
- Real-Time Chat: Users are able to chat with other users in real-time using WebSockets.
- Follow each others
- Upload stories: Users are able to post stories, similar to Instagram's story functionality.
