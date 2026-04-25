Setup steps

  1. Google Cloud Console (free)

  1. Go to https://console.cloud.google.com → APIs & Services → Credentials
  2. Create an OAuth 2.0 Client ID (type: Web application)
  3. Add authorized JavaScript origins:
    - http://localhost:7070
    - https://teamsplit.onrender.com
  4. Copy the Client ID (ends in .apps.googleusercontent.com)

  2. Neon (free PostgreSQL)

  1. Sign up at https://neon.tech, create a project
  2. Copy the connection string (format: postgresql://user:pass@host/db?sslmode=require)

  3. Local development

  Create frontend/.env.local:
  VITE_GOOGLE_CLIENT_ID=your-client-id.apps.googleusercontent.com

  Run the backend with env vars:
  GOOGLE_CLIENT_ID=... DATABASE_URL=... dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj

  4. Render deployment

  In your Render dashboard, set these environment variables:

  ┌───────────────────┬───────────────────────┬────────────────────────┐
  │      Service      │          Key          │         Value          │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ DATABASE_URL          │ Neon connection string │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ GOOGLE_CLIENT_ID      │ Google client ID       │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ Frontend (static) │ VITE_GOOGLE_CLIENT_ID │ Google client ID       │
  └───────────────────┴───────────────────────┴────────────────────────┘

  The EnsureCreated() call will create the Players table automatically on first boot. Each Google account gets its own isolated
  player list.

✻ Crunched for 6m 35s

※ recap: Added Google login and per-user player lists. Everything builds. Next step: create the Google OAuth client ID and Neon
  database, then set the two env vars locally to test. (disable recaps in /config)

─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
❯ 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
  ⏵⏵ accept edits on (shift+tab to cycle)

  Frontend:
  - Login screen with Google Sign-In button (@react-oauth/google)
  - Token sent as Authorization: Bearer <token> on every API call
  - On 401, user is automatically signed out
  - Added "Jugadores" nav button (every user manages their own list now)
  - Avatar + logout button in the header

  ---
  Setup steps

  1. Google Cloud Console (free)

  1. Go to https://console.cloud.google.com → APIs & Services → Credentials
  2. Create an OAuth 2.0 Client ID (type: Web application)
  3. Add authorized JavaScript origins:
    - http://localhost:7070
    - https://teamsplit.onrender.com
  4. Copy the Client ID (ends in .apps.googleusercontent.com)

  2. Neon (free PostgreSQL)

  1. Sign up at https://neon.tech, create a project
  2. Copy the connection string (format: postgresql://user:pass@host/db?sslmode=require)

  3. Local development

  Create frontend/.env.local:
  VITE_GOOGLE_CLIENT_ID=your-client-id.apps.googleusercontent.com

  Run the backend with env vars:
  GOOGLE_CLIENT_ID=... DATABASE_URL=... dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj

  4. Render deployment

  In your Render dashboard, set these environment variables:

  ┌───────────────────┬───────────────────────┬────────────────────────┐
  │      Service      │          Key          │         Value          │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ DATABASE_URL          │ Neon connection string │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ GOOGLE_CLIENT_ID      │ Google client ID       │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ Frontend (static) │ VITE_GOOGLE_CLIENT_ID │ Google client ID       │
  └───────────────────┴───────────────────────┴────────────────────────┘

  The EnsureCreated() call will create the Players table automatically on first boot. Each Google account gets its own isolated player list.

✻ Crunched for 6m 35s

※ recap: Added Google login and per-user player lists. Everything builds. Next step: create the Google OAuth client ID and Neon database, then set the two env vars locally to test. (disable recaps in
  /config)

──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
❯ 
──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
  ⏵⏵ accept edits on (shift+tab to cycle)

  Backend:
  - Replaced SQLite with PostgreSQL (Npgsql.EntityFrameworkCore.PostgreSQL)
  - Added Microsoft.AspNetCore.Authentication.JwtBearer for Google token validation
  - PlayerEntity now has a UserId field; primary key is composite (UserId, Name)
  - All endpoints require a valid Google ID token and filter data by that user's ID
  - Reads two env vars: DATABASE_URL and GOOGLE_CLIENT_ID

  Frontend:
  - Login screen with Google Sign-In button (@react-oauth/google)
  - Token sent as Authorization: Bearer <token> on every API call
  - On 401, user is automatically signed out
  - Added "Jugadores" nav button (every user manages their own list now)
  - Avatar + logout button in the header

  ---
  Setup steps

  1. Google Cloud Console (free)

  1. Go to https://console.cloud.google.com → APIs & Services → Credentials
  2. Create an OAuth 2.0 Client ID (type: Web application)
  3. Add authorized JavaScript origins:
    - http://localhost:7070
    - https://teamsplit.onrender.com
  4. Copy the Client ID (ends in .apps.googleusercontent.com)

  2. Neon (free PostgreSQL)

  1. Sign up at https://neon.tech, create a project
  2. Copy the connection string (format: postgresql://user:pass@host/db?sslmode=require)

  3. Local development

  Create frontend/.env.local:
  VITE_GOOGLE_CLIENT_ID=your-client-id.apps.googleusercontent.com

  Run the backend with env vars:
  GOOGLE_CLIENT_ID=... DATABASE_URL=... dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj

  4. Render deployment

  In your Render dashboard, set these environment variables:

  ┌───────────────────┬───────────────────────┬────────────────────────┐
  │      Service      │          Key          │         Value          │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ DATABASE_URL          │ Neon connection string │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ GOOGLE_CLIENT_ID      │ Google client ID       │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ Frontend (static) │ VITE_GOOGLE_CLIENT_ID │ Google client ID       │
  └───────────────────┴───────────────────────┴────────────────────────┘

  The EnsureCreated() call will create the Players table automatically on first boot. Each Google account gets its own isolated player list.

✻ Crunched for 6m 35s

※ recap: Added Google login and per-user player lists. Everything builds. Next step: create the Google OAuth client ID and Neon database, then set the two env vars locally to test. (disable recaps in /config)

─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
❯ 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
  ⏵⏵ accept edits on (shift+tab to cycle)

  Backend:
  - Replaced SQLite with PostgreSQL (Npgsql.EntityFrameworkCore.PostgreSQL)
  - Added Microsoft.AspNetCore.Authentication.JwtBearer for Google token validation
  - PlayerEntity now has a UserId field; primary key is composite (UserId, Name)
  - All endpoints require a valid Google ID token and filter data by that user's ID
  - Reads two env vars: DATABASE_URL and GOOGLE_CLIENT_ID

  Frontend:
  - Login screen with Google Sign-In button (@react-oauth/google)
  - Token sent as Authorization: Bearer <token> on every API call
  - On 401, user is automatically signed out
  - Added "Jugadores" nav button (every user manages their own list now)
  - Avatar + logout button in the header

  ---
  Setup steps

  1. Google Cloud Console (free)

  1. Go to https://console.cloud.google.com → APIs & Services → Credentials
  2. Create an OAuth 2.0 Client ID (type: Web application)
  3. Add authorized JavaScript origins:
    - http://localhost:7070
    - https://teamsplit.onrender.com
  4. Copy the Client ID (ends in .apps.googleusercontent.com)

  2. Neon (free PostgreSQL)

  1. Sign up at https://neon.tech, create a project
  2. Copy the connection string (format: postgresql://user:pass@host/db?sslmode=require)

  3. Local development

  Create frontend/.env.local:
  VITE_GOOGLE_CLIENT_ID=your-client-id.apps.googleusercontent.com

  Run the backend with env vars:
  GOOGLE_CLIENT_ID=... DATABASE_URL=... dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj

  4. Render deployment

  In your Render dashboard, set these environment variables:

  ┌───────────────────┬───────────────────────┬────────────────────────┐
  │      Service      │          Key          │         Value          │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ DATABASE_URL          │ Neon connection string │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ GOOGLE_CLIENT_ID      │ Google client ID       │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ Frontend (static) │ VITE_GOOGLE_CLIENT_ID │ Google client ID       │
  └───────────────────┴───────────────────────┴────────────────────────┘

  The EnsureCreated() call will create the Players table automatically on first boot. Each Google account gets its own isolated player
  list.

✻ Crunched for 6m 35s

※ recap: Added Google login and per-user player lists. Everything builds. Next step: create the Google OAuth client ID and Neon database,
  then set the two env vars locally to test. (disable recaps in /config)

──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
❯ 
──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
  ⏵⏵ accept edits on (shift+tab to cycle)
  1. Sign up at https://neon.tech, create a project
  2. Copy the connection string (format: postgresql://user:pass@host/db?sslmode=require)

  3. Local development

  Create frontend/.env.local:
  VITE_GOOGLE_CLIENT_ID=your-client-id.apps.googleusercontent.com

  Run the backend with env vars:
  GOOGLE_CLIENT_ID=... DATABASE_URL=... dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj

  4. Render deployment

  In your Render dashboard, set these environment variables:

  ┌───────────────────┬───────────────────────┬────────────────────────┐
  │      Service      │          Key          │         Value          │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ DATABASE_URL          │ Neon connection string │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ GOOGLE_CLIENT_ID      │ Google client ID       │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ Frontend (static) │ VITE_GOOGLE_CLIENT_ID │ Google client ID       │
  └───────────────────┴───────────────────────┴────────────────────────┘

  The EnsureCreated() call will create the Players table automatically on first boot. Each Google account gets its own isolated
  player list.

✻ Crunched for 6m 35s

※ recap: Added Google login and per-user player lists. Everything builds. Next step: create the Google OAuth client ID and Neon
  database, then set the two env vars locally to test. (disable recaps in /config)

─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
❯ 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
  ⏵⏵ accept edits on (shift+tab to cycle)
  1. Sign up at https://neon.tech, create a project
  2. Copy the connection string (format: postgresql://user:pass@host/db?sslmode=require)

  3. Local development

  Create frontend/.env.local:
  VITE_GOOGLE_CLIENT_ID=your-client-id.apps.googleusercontent.com

  Run the backend with env vars:
  GOOGLE_CLIENT_ID=... DATABASE_URL=... dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj

  4. Render deployment

  In your Render dashboard, set these environment variables:

  ┌───────────────────┬───────────────────────┬────────────────────────┐
  │      Service      │          Key          │         Value          │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ DATABASE_URL          │ Neon connection string │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ GOOGLE_CLIENT_ID      │ Google client ID       │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ Frontend (static) │ VITE_GOOGLE_CLIENT_ID │ Google client ID       │
  └───────────────────┴───────────────────────┴────────────────────────┘

  The EnsureCreated() call will create the Players table automatically on first boot. Each Google account gets its own isolated
  player list.

✻ Crunched for 6m 35s

※ recap: Added Google login and per-user player lists. Everything builds. Next step: create the Google OAuth client ID and Neon
  database, then set the two env vars locally to test. (disable recaps in /config)

─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
❯ 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
  ⏵⏵ accept edits on (shift+tab to cycle)
  1. Sign up at https://neon.tech, create a project
  2. Copy the connection string (format: postgresql://user:pass@host/db?sslmode=require)

  3. Local development

  Create frontend/.env.local:
  VITE_GOOGLE_CLIENT_ID=your-client-id.apps.googleusercontent.com

  Run the backend with env vars:
  GOOGLE_CLIENT_ID=... DATABASE_URL=... dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj

  4. Render deployment

  In your Render dashboard, set these environment variables:

  ┌───────────────────┬───────────────────────┬────────────────────────┐
  │      Service      │          Key          │         Value          │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ DATABASE_URL          │ Neon connection string │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ API (backend)     │ GOOGLE_CLIENT_ID      │ Google client ID       │
  ├───────────────────┼───────────────────────┼────────────────────────┤
  │ Frontend (static) │ VITE_GOOGLE_CLIENT_ID │ Google client ID       │
  └───────────────────┴───────────────────────┴────────────────────────┘

  The EnsureCreated() call will create the Players table automatically on first boot. Each Google account gets its own isolated
  player list.

✻ Crunched for 6m 35s

※ recap: Added Google login and per-user player lists. Everything builds. Next step: create the Google OAuth client ID and Neon
  database, then set the two env vars locally to test. (disable recaps in /config)