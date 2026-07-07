<div align="center">

# Mostaqel — Freelancing Platform

**A full-stack freelancing marketplace that connects clients with freelancers** — post jobs, submit proposals, chat in real time, and manage projects from a single platform.

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)](#backend)
[![Angular](https://img.shields.io/badge/Angular-21-DD0031?logo=angular&logoColor=white)](#frontend)
[![SQL Server](https://img.shields.io/badge/SQL_Server-EF_Core_9-CC2927?logo=microsoftsqlserver&logoColor=white)](#backend)
[![Redis](https://img.shields.io/badge/Redis-Presence_Tracking-DC382D?logo=redis&logoColor=white)](#backend)
[![SignalR](https://img.shields.io/badge/SignalR-Real--time-informational)](#real-time-hubs-signalr)

</div>

---

## Overview

This repository contains both halves of the platform:

| Folder | Description |
|---|---|
| [`Backend/`](./Backend) | A .NET 9 Web API built with **Clean Architecture** (Api → Application → Domain → Infrastructure), backed by SQL Server, Redis, and Cloudinary. |
| [`Client/`](./Client) | An **Angular 21** single-page app using standalone components, Angular Material, PrimeNG, and TailwindCSS. |

Users register as either a **Client** (someone hiring) or a **Worker/Freelancer** (someone offering services). Clients post jobs, freelancers submit offers, and once an offer is approved the two parties can track the project's progress and message each other in real time.

---

## Features

**For Clients**
- Post new jobs with a title, description, budget, deadline, and required skills
- Browse and filter/paginate their posted projects
- Review incoming offers (proposals) from freelancers on each job
- Approve a freelancer's offer to kick off a project
- Mark a project as completed once work is delivered
- Make  a project Status pending , in-progress and completed
 
**For Freelancers (Workers)**
- Browse open jobs and submit offers (price, description, estimated duration)
- Track submitted offers and their status
- Build a public profile with skills, rating, and completed-task count
- Track a project Status pending , in-progress and completed

**Shared / Platform-wide**
- Real-time one-to-one messaging with read receipts, powered by SignalR
- JWT-based authentication with refresh tokens, email confirmation, and password reset flows
- Profile photo upload via Cloudinary
- Searchable freelancer directory and user profile pages
- Live online/offline presence indicators (Redis-backed)
- Request lifecycle tracking (awaiting approval → in progress → completed)

---

## Tech Stack

### Backend
| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 9 (Web API), Clean Architecture (`Api` / `Application` / `Domain` / `Infrastructure`) |
| Database & ORM | SQL Server, Entity Framework Core 9 |
| Auth | ASP.NET Core Identity, JWT Bearer tokens (access + refresh), cookie fallback for SignalR |
| Validation & Mapping | FluentValidation, Mapster |
| Real-time | SignalR (`MessageHub`, `PresenceHub`) |
| Caching / Presence store | Redis (StackExchange.Redis) |
| File storage | Cloudinary (profile photos) |
| Email | SMTP (email confirmation, password reset) |
| Patterns | Generic Repository + Specification pattern, functional `Result`/`Error` handling |

### Frontend
| Layer | Technology |
|---|---|
| Framework | Angular 21 (standalone components) |
| UI | Angular Material, PrimeNG + PrimeUIX themes, TailwindCSS 4 |
| Real-time client | `@microsoft/signalr` |
| Auth helpers | `jwt-decode`, `ngx-cookie-service` |
| Extras | Swiper (carousels) |
| Testing | Vitest |

---

## Project Structure

```
Freelancing-Platform/
├── Backend/
│   ├── Api/                 # Controllers, SignalR hubs, DI setup, Program.cs
│   │   ├── Controllers/     # Auth, Clients, Workers, Users
│   │   └── Hubs/            # MessageHub, PresenceHub
│   ├── Application/         # DTOs, service interfaces, specifications, mapping config
│   ├── Domain/               # Entities (Job, Offer, Skill, Users, Messages), Result/Error, generic repo interfaces
│   ├── Infrastructure/       # EF Core DbContext & migrations, service implementations, Cloudinary/Email/Redis
│   └── Backend.sln
└── Client/
    └── src/app/
        ├── core/             # Services, HTTP interceptors, route guards
        ├── features/
        │   ├── account/      # Login, register, verify/confirm email, password reset
        │   ├── home/
        │   ├── jobs/         # Add job, job card, job profile
        │   ├── freelancer/   # Freelancer profile & card
        │   ├── requests/     # Awaiting approval / in progress / completed
        │   └── messages/     # Real-time chat UI
        ├── layout/
        └── shared/
```

---

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) 20+ and npm
- SQL Server (or LocalDB, which ships with Visual Studio)
- Redis (local instance or Docker container)
- A Cloudinary account (for photo uploads) and an SMTP-capable email account (for confirmation/reset emails) — optional if you stub these out for local dev

### 1. Clone the repository
```bash
git clone https://github.com/Ahmed-Elsherbiny-0/Freelancing-Platform.git
cd Freelancing-Platform
```

### 2. Backend setup
```bash
cd Backend
```

Configure your settings — either edit `Api/appsettings.json` directly or, better, use the .NET Secret Manager so credentials never get committed:
```bash
cd Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\MSSQLLocalDB;Database=FreeLancer;Trusted_Connection=True;MultipleActiveResultSets=true"
dotnet user-secrets set "ConnectionStrings:Redis" "localhost:6379"
dotnet user-secrets set "Jwt:Key" "<a long, random signing key>"
dotnet user-secrets set "MailSettings:Password" "<your smtp app password>"
dotnet user-secrets set "CloudinarySetting:ApiSecret" "<your cloudinary api secret>"
```

Apply EF Core migrations and run the API:
```bash
dotnet ef database update --project ../Infrastructure --startup-project .
dotnet run
```
The API listens on `https://localhost:7148` by default (see `Api/Properties/launchSettings.json`).

### 3. Frontend setup
```bash
cd Client
npm install
npm start   # runs `ng serve` with SSL enabled via ssl/localhost.pem & ssl/localhost-key.pem
```
The app expects the API at the URL configured in `src/environments/environment.development.ts` (`https://localhost:7148/`) and will run on `https://localhost:4200` by default — matching the CORS origin configured on the backend.

> First run only: your browser will warn about the self-signed dev certificate in `Client/ssl` — accept it to allow the SignalR/API connections over HTTPS.

---

## Configuration Reference

These keys live in `Backend/Api/appsettings.json` (development) and should be overridden via user-secrets, environment variables, or a secrets manager in any shared/deployed environment:

| Key | Purpose |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `ConnectionStrings:Redis` | Redis connection string (used for SignalR presence tracking) |
| `Jwt:Key` / `Issuer` / `Audience` / `ExpiryMinutes` | JWT signing key and token settings |
| `MailSettings:Mail` / `DisplayName` / `Password` / `Host` / `Port` | SMTP credentials for transactional email |
| `CloudinarySetting:CloudName` / `ApiKey` / `ApiSecret` | Cloudinary credentials for photo uploads |

---

## API Reference

All endpoints are hosted at `https://localhost:7148` in development. Unless marked **Anonymous**, an endpoint requires a JWT bearer token (`Authorization: Bearer <token>`) — the app also accepts the token via an `access_token` cookie/query string for the SignalR hubs. Every route below is copied directly from the controllers, so paths, casing, and parameter names are exact.

### `AuthController` — base route `/Auth`
All endpoints are anonymous (this controller has no `[Authorize]`).

| Method | Route | Body / Query | Response |
|---|---|---|---|
| `POST` | `/Auth/login` | Body: `LoginRequestDto { Email, Password }` | `200` `AuthResponseDto { Id, Email, FristName, LastName, PictureUrl, PicturePublicId, Token, ExpiresIn, refreshToken, RefreshTokenExpiration, IsClient }`, or a problem response |
| `POST` | `/Auth/register` | Body: `RegisterRequestDto { Email, Password, FirstName, LastName, PhoneNumber, Country, Photo?: PhotoDto, Type }` (`Type` selects **Client** or **Worker**) | `200 OK` on success, or a problem response |
| `POST` | `/Auth/refresh` | Body: `RefreshTokenRequestDto { RefreshToken }` + `access_token` cookie | `200` new `AuthResponseDto`, or a problem response |
| `POST` | `/Auth/revoke-refresh-token` | Body: `RefreshTokenRequestDto { RefreshToken }` + `access_token` cookie | `200 OK` and clears the `access_token` cookie (logout), or a problem response |
| `POST` | `/Auth/confirm-email` | Query: `ConfirmationEmailRequestDto { Code, UserId }` | `200 OK`, or a problem response |
| `POST` | `/Auth/resend-confirmation-email` | Body: `ResendConfirmationEmailRequestDto { Email }` | `200 OK`, or a problem response |
| `POST` | `/Auth/forget-password` | Body: `ForgetPasswordRequestDto { Email }` | `200 OK` (emails a reset code), or a problem response |
| `POST` | `/Auth/reset-password` | Body: `ResetPasswordRequestDto { Email, Code, NewPassword }` | `200 OK`, or a problem response |
| `GET` | `/Auth/me` | — (reads the `access_token` cookie) | `200 OK` if the cookie is present, otherwise `401 Unauthorized` |

### `ClientsController` — base route `/api/Clients`
Requires auth by default; endpoints marked **Anonymous** override this.

| Method | Route | Auth | Body / Query | Response |
|---|---|---|---|---|
| `POST` | `/api/Clients/add-project` | Required | Body: `JobRequestDto { Title, Description, Budget, RequiredSkills?: string[], Duration }` | `201 Created`, or a problem response |
| `GET` | `/api/Clients/get-project` | Anonymous | Query: `jobId` (int) | `200` `JobResponseDto { Id, Title, Description, Budget, Deadline, ReqiuiredSkills, CreatedDate, Status, PhotoUrl, FristName, LastName, Email }`, or a problem response |
| `GET` | `/api/Clients/get-all-projects` | Anonymous | Query: `JobSpecParms { PageIndex=1, PageSize=6 (max 30), Search, Budget?, Status? }` | `200` paginated list of `JobResponseDto` |
| `GET` | `/api/Clients/get-all-offers-for-job` | Required | — (uses the caller's user id) | `200` list of offers across the client's jobs, or `400 Bad Request` |
| `POST` | `/api/Clients/approve-project` | Required | Query: `workerId` (string), `jobId` (int) | `204 No Content`, or `400 Bad Request` |
| `POST` | `/api/Clients/complete-project` | Required | Query: `workerId` (string), `jobId` (int) | `204 No Content`, or `400 Bad Request` |

### `WorkersController` — base route `/api/Workers`
Requires auth by default; endpoints marked **Anonymous** override this.

| Method | Route | Auth | Body / Query | Response |
|---|---|---|---|---|
| `POST` | `/api/Workers/add-offer` | Required | Body: `OfferRequestDto { JobId, Description, Duration, Price }` | `204 No Content`, or a problem response |
| `GET` | `/api/Workers/get-offers` | Anonymous | Query: `OfferSpecParms { PageIndex=1, PageSize=6 (max 30) }` + `jobid` (int) | `200` paginated list of `OfferResponseDto { Description, Duration, Price, IsApproved, IsCompleted, JobId, Rating, CompletedTasks, PictureUrl, FristName, LastName, WorkerEamil, JobTitle, WorkerId }`, or `404 Not Found` |
| `GET` | `/api/Workers/get-worker-offers` | Required | Query: `OfferSpecParms { PageIndex, PageSize }` (uses the caller's user id) | `200` paginated list of the caller's own `OfferResponseDto`s, or `404 Not Found` |

### `UsersController` — base route `/Users`
Requires auth by default; endpoints marked **Anonymous** override this.

| Method | Route | Auth | Body / Query | Response |
|---|---|---|---|---|
| `POST` | `/Users/upload-profile-photo-cloudinary` | Anonymous | Multipart form file: `file` | `200` `PhotoDto { PublicId, Url }`, or `400 Bad Request` |
| `POST` | `/Users/add-photo-to-user` | Required | Body: `Photo { Url, PublicId }` | `200 OK` with the saved result, or a problem response |
| `DELETE` | `/Users/delete-photo-cloudinary` | Anonymous | Query: `publicId` (string) | `200 OK`, or `400 Bad Request` |
| `GET` | `/Users/get-workers` | Anonymous | Query: `WorkerSpecParms { PageIndex=1, PageSize=6 (max 30), Search, Country, Rating? }` | `200` paginated list of `WorkerDto { FirstName, LastName, Country, Email, PhotoUrl, Rating, Description, Skills, CompletedTasks, jobId }` |
| `GET` | `/Users/get-worker` | Anonymous | Query: `username` (string) | `200` worker profile, or a problem response if the user isn't found |
| `GET` | `/Users/get-user` | Anonymous | Query: `username` (string) | `200` `UserResponse { FirstName, LastName, Photo, Email }`, or a problem response |
| `POST` | `/Users/update-user-Info` | Required | Body: `UserInfoRequestDto { FirstName, LastName, Country, Skills?: string[], Description }` | `201 Created`, or `400 Bad Request` |
| `GET` | `/Users/get-user-Info` | Required | — (uses the caller's user id) | `200 OK` with the caller's info, or `400 Bad Request` |

---

## Real-time Hubs (SignalR)

### `MessageHub` — `/hubs/message`
Connect with `?user=<otherUsername>` in the query string to open a 1:1 conversation. On connect, the two users are placed into a shared group (name derived by sorting both usernames) and the existing message thread is pushed to the group.

| Direction | Name | Payload | Notes |
|---|---|---|---|
| Client → Server | `SendMessage` | `CreateMessageDto { RecipientUsername, Content }` | Throws a `HubException` if the recipient doesn't exist or equals the sender |
| Server → Client | `ReceiveMessageThread` | `MessageDto[]` | Sent once on connect with the full conversation history |
| Server → Client | `NewMessage` | `MessageDto` | Broadcast to the conversation group when a message is sent |

If the recipient isn't actively viewing the conversation, the hub also notifies them via `PresenceHub`'s connections with `GetUnReadedMessages` (updated unread counts) and `onReciveNewMessage` (an offline-style notification with sender/content).

### `PresenceHub` — `/hubs/presence`
Tracks who's online (backed by Redis) and pushes chat notifications to users who aren't inside a specific `MessageHub` conversation.

| Direction | Name | Payload | Notes |
|---|---|---|---|
| Server → Client | `onConnectUser` | `username` | Sent to everyone else when a user comes online |
| Server → Client | `onDisconnectUser` | `username` | Sent to everyone else when a user's last connection drops |
| Server → Client | `GetOnlineUsers` | `string[]` | Broadcast to all clients whenever the online set changes |
| Server → Client | `GetUnReadedMessages` | `UnReadedMessageDto[] { username, count }` | Sent to the caller on connect, and to a recipient's other connections when they receive a message while offline |
| Server → Client | `GetUserFrinds` | `PrimaryUserChatDto[] { Content, MessageSent, FullName, OtherUsername, Url }` | Sent to the caller on connect — the user's conversation/contacts list |
| Server → Client | `onReciveNewMessage` | `PrimaryUserChatDto` | Sent to sender + recipient connections when a message arrives outside an open `MessageHub` conversation |

---

## Domain Model at a Glance

- **`ApplicationUser`** — the base identity account (extends ASP.NET Core Identity), optionally linked to a `Client` profile, a `Worker` profile, or both.
- **`Job`** — posted by a `Client`; has a status, budget, deadline, required skills, and a collection of `Offer`s.
- **`Offer`** — a `Worker`'s proposal on a `Job` (price, duration, description, approval/completion flags).
- **`Worker`** — a freelancer profile with skills, rating, and completed-task count.
- **`Message` / `Group` / `Connection`** — back the real-time chat system.

