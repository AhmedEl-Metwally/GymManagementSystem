GymManagementSystem

A gym administration system built with ASP.NET Core MVC (.NET 9), extended with an AI assistant that answers administrators' questions from live gym data, using Semantic Kernel and a local LLM (Ollama).

The business system comes first. The AI is a capability added on top of it: it reads data through the same services and follows the same role rules as the rest of the application.

Contents
Features
How the AI assistant works
Security and privacy
Architecture
Tech stack
Getting started
Roles and permissions
Testing status
Known limitations
Roadmap
How it was built
Engineering notes
Author
Features
Gym management
Area	What it does
Members	Create, edit, view and delete members. Profile photo upload with custom validation (JPG/PNG, max 5 MB). One-to-one health record (height, weight, blood type, notes). Unique email and phone. A member with upcoming sessions cannot be deleted.
Trainers	Create, edit, view and delete trainers with a specialty (GeneralFitness, Yoga, Boxing, CrossFit). Egyptian mobile number validation. A trainer with upcoming sessions cannot be deleted.
Plans	List, view and edit membership plans. Activate or deactivate a plan (soft delete). A plan with active memberships cannot be edited or deactivated.
Memberships	Assign a plan to a member (only one active membership at a time). The end date is calculated from the plan duration. The status (Active or Expired) is derived from the end date. Memberships can be cancelled.
Sessions	Create training sessions with category, trainer, capacity (1 to 25) and a date range. Editing and deleting are restricted to protect existing bookings.
Bookings	Book a member into an upcoming session (requires an active membership and a free slot), cancel a booking, and mark attendance while a session is ongoing.
Dashboard	Total members, active members, trainers, and upcoming, ongoing and completed sessions.
Authentication	Login by email, logout, access denied page. ASP.NET Core Identity with the roles SuperAdmin and Admin. Navigation depends on the role.
AI assistant

A chat widget (visible to SuperAdmin and Admin only) answers questions in Arabic or English by calling tools that read real data:

Example question	Tool used
How many members do we have? / كام عضو عندنا؟	members-get_membership_overview
Whose membership ends this week? this month?	members-get_expiring_memberships (days = 7 or 30)
Is a given member's membership active?	members-find_member
What plans do we offer?	plans-get_membership_plans, plans-find_membership_plan
Who are the trainers? Who teaches CrossFit?	trainers-get_trainers, trainers-find_trainers_by_specialty
What sessions are there today, tomorrow, this week? When is the next Yoga session?	sessions-get_sessions (period, category)

The assistant refuses what no tool provides (payments, who is booked in a session, and personal details such as email, phone, address, date of birth or health data).

How the AI assistant works
POST /Assistant/Ask +anti-forgery token
system prompt + question+ allowed tools
OpenAI-compatible API
tool call
Chat widget (vanilla JS)
AssistantController
GymAssistantService
Semantic Kernel
Ollama (local LLM)
Filters: authorization +logging
Plugins: plans, trainers,sessions, members
Business services (BLL)
SQL Server

Request lifecycle

The widget sends the question with an anti-forgery token. The server accepts at most 500 characters.
GymAssistantService loads the system prompt from a text file and registers only the plugins allowed for the current user's roles.
The model decides which tool to call (automatic function calling). The arguments it supplies are validated in code.
A filter checks authorization again and logs the call (name, arguments, duration, truncated result).
The plugin calls the existing service layer and returns a small result object that contains no sensitive fields.
The model writes the reply from the tool result. The widget renders it as plain text.
The conversation is stored per user on the server for the chat window.

Design decisions

Decision	Reason
Local LLM through Ollama (OpenAI-compatible endpoint)	Data stays on the machine, no usage cost. Trade-off: slower and less accurate than large hosted models.
Plugins call the services, not the DbContext	Business rules stay in one place.
Dates are resolved in code (today, tomorrow, next_7_days, upcoming)	The model never calculates dates. Sessions are matched by overlap, so a running session counts as today.
Plugins return small result records	Data minimization: what no tool returns cannot reach the model.
Deny-by-default role policy, enforced in two layers	A tool that is not registered cannot be called, and a filter blocks any call that slips through.
Plugins are added per request	They depend on scoped services (EF Core, Unit of Work) and the current user.
System prompt in a file	Can be edited without rebuilding.
Each question is sent to the model on its own	With chat history in the prompt, the model sometimes skipped tools and answered from earlier replies. The history is still stored and shown in the widget (SendHistoryToModel = false).
Security and privacy
Authorization: deny by default, per plugin, by role. Checked at registration and again in a function invocation filter.
Data minimization: no tool returns email, phone, address, date of birth, health records or payments.
Argument validation: empty values are rejected, ranges are clamped, name searches need at least 2 characters, result lists are capped.
Logging: results of member tools are redacted (except the overview counts), the memberName argument is masked, and long results are truncated.
Web security: POST endpoints with anti-forgery tokens, a message length limit, generic error messages for the user (details only in server logs), and model output rendered with textContent (no innerHTML) to prevent XSS.
Conversation memory: kept on the server per user (IMemoryCache, sliding expiration), so the browser cannot inject assistant or system messages.
Secrets: the API key is read from .NET User Secrets, not from source control.
Architecture

Three-layer architecture with the Repository and Unit of Work patterns.

GymManagementSystem
├── GymManagementDAL            Data access
│   ├── Entities                Member, Trainer, Plan, Session, Category, MemberPlan, MemberSession, HealthRecord
│   ├── Data
│   │   ├── Context             GymDbContext (Identity + gym tables)
│   │   ├── Configuration       Fluent API configurations
│   │   ├── DataSeed            Plans, categories, roles and admin users
│   │   └── Migrations
│   └── Repositories            Generic and specific repositories, Unit of Work
├── GymManagementBLL            Business logic
│   ├── Services                Interfaces, implementations, attachment (file upload) service
│   │   └── AI                  GymAssistantService, AiToolPolicy
│   ├── Plugins                 Semantic Kernel plugins and their result records
│   ├── Filters                 Function logging and tool authorization filters
│   ├── ViewModels
│   ├── Mapping                 AutoMapper profile
│   ├── Settings/AI             AiSettings
│   └── SettingsOfAttachment    File size and extension validation
└── GymManagementPL             ASP.NET Core MVC
    ├── Controllers             Including AssistantController
    ├── Views                   Including the _AssistantWidget partial
    ├── AI/Prompts              system-prompt.txt
    └── wwwroot                 css, js (assistant-widget.js), images, seed JSON files

Code organization

Extension methods separate service registration, database seeding and the middleware pipeline from Program.cs.
A ServiceManager groups the service interfaces behind a single access point.
AllGlobalUsings files remove repeated using directives.
Interface-based services, AutoMapper for ViewModel mapping, and dependency injection throughout.
Tech stack
Area	Technology
Framework	ASP.NET Core MVC, .NET 9
Data	Entity Framework Core (SQL Server), ASP.NET Core Identity
Mapping	AutoMapper
AI	Semantic Kernel (OpenAI connector), Ollama, IBM Granite 4.1 (8B)
Front end	Razor views, Bootstrap 5, Bootstrap Icons, jQuery validation, vanilla JavaScript (chat widget)
Getting started

Developed and tested on Windows with Visual Studio.

Prerequisites: .NET 9 SDK, SQL Server, Ollama.

Clone the repository
bash
   git clone <repository-url>
   cd GymManagementSystem
Set the connection string in GymManagementPL/appsettings.json (ConnectionStrings:DefaultConnectionString).
Create the database
bash
   dotnet ef database update --project GymManagementDAL --startup-project GymManagementPL

On first run the application seeds plans, categories, roles and two development accounts.

Install the model
bash
   ollama pull granite4.1:8b
Configure the AI section in GymManagementPL/appsettings.json:
json
   "AI": {
     "Endpoint": "http://localhost:11434/v1",
     "ModelId": "granite4.1:8b"
   }

Ollama does not check the key, so any value works. Keep it out of source control:

bash
   dotnet user-secrets init --project GymManagementPL
   dotnet user-secrets set "AI:ApiKey" "ollama" --project GymManagementPL
Run
bash
   dotnet run --project GymManagementPL

Or press F5 in Visual Studio.

Log in with a seeded account (login is by email; the accounts are defined in IdentityDbContextSeeding, for development only).

Tip: Ollama unloads an idle model after a few minutes, which makes the next request slow. Setting OLLAMA_KEEP_ALIVE (for example 30m) keeps it loaded longer.

Configuration
Key	Purpose
ConnectionStrings:DefaultConnectionString	SQL Server connection
AI:Endpoint	OpenAI-compatible endpoint (Ollama: http://localhost:11434/v1)
AI:ModelId	Model name
AI:ApiKey	Dummy key for Ollama, stored in User Secrets
Roles and permissions

AI tools by role (defined in AiToolPolicy):

Plugin	SuperAdmin	Admin
plans	✅	✅
sessions	✅	✅
trainers	✅	❌
members	✅	❌

Navigation: SuperAdmin sees Members and Trainers. Admin sees Sessions, Plans, Member Plans and Member Session.

Testing status
The AI layer was tested manually and checked through structured logs: which tool is chosen, which arguments are sent, permission checks (including a denied call), and log redaction.
A full test matrix with realistic data is still pending.
There are no automated tests yet.
Known limitations
Small local model. In testing it occasionally misread a tool result (for example, answering that there were no sessions although the tool returned three). Replies about critical data should be double-checked until critical answers are formatted in code.
Speed. On a laptop with 16 GB RAM and a 4 GB GPU, replies take roughly 10 to 45 seconds. Most of the time is spent generating tokens (about 0.25 s per token), not in the tools. The first request after an idle period can take about a minute while the model loads.
No follow-up questions. Each question is answered on its own, so users must ask complete questions.
Conversation memory lives in the process memory and is lost on restart.
Logging. The user's message is logged in full, which needs a privacy review before production.
Seeding builds file paths with Windows separators.
AutoMapper prints a license warning in development, and the referenced version shows a vulnerability warning. It should be updated or replaced.
Roadmap
 Full test matrix with realistic data, plus automated tests (tool policy, plugins).
 Format critical replies in code and skip the second model call (faster and deterministic).
 Response cache.
 Streaming replies.
 Privacy review of request logging.
 Restrict sessions to a single day.
 Move seeded credentials to configuration.
 Update or replace AutoMapper.
How it was built
Phase 1: Business system
Data layer: entities (BaseEntity with created and updated dates, GymUser as the base of members and trainers, an owned Address, a HealthRecord sharing the members table), Fluent API configurations (check constraints, unique indexes, composite keys), EF Core migrations and JSON-based seeding.
Repositories: a generic repository, specific repositories (sessions, memberships, bookings) and a Unit of Work.
Services: interface-based services with ViewModels and AutoMapper for members (with photo upload), trainers, plans, sessions, memberships, bookings and analytics.
Presentation: MVC controllers, Razor views and a responsive Bootstrap interface.
Identity: ASP.NET Core Identity with seeded roles and role-aware navigation.
Phase 2: AI assistant
Provider decision: a local Ollama model through its OpenAI-compatible endpoint. Settings are bound from the AI section and the key is kept in User Secrets.
Kernel and service: Semantic Kernel registration, and GymAssistantService with a system prompt loaded from a file.
Plugins: plans, trainers, sessions and members, each with a small result record and clear tool descriptions.
Function calling: automatic function calling, with arguments validated in code and dates resolved in code.
Observability: a function invocation filter that logs tool name, arguments, duration and results, plus request timing logs.
Role-based tool access: AiToolPolicy with deny by default, plugins registered per user and a second enforcement filter (verified through a logged denied call).
Privacy in logs: redaction of member tool results and of the memberName argument.
Reliability fixes: empty-argument guards, overlap-based session matching, scoped tool descriptions, and a guard against raw tool-call text in replies.
Measurement: analysis of the logs showed that generated tokens, not tool code, dominate latency.
Chat widget: a floating widget (Razor partial, CSS, vanilla JavaScript) with anti-forgery protected endpoints, length limits, timeouts, session-expiry handling and per-user server-side conversation memory.
History fix: the model no longer receives the chat history, because it caused skipped tool calls.
Prompt hardening: required tool calls for data questions, fixed one-line reply formats, and clear rules for empty results.
Phase 3: Refactoring

Done independently: extension methods for service registration, seeding and pipeline setup, a ServiceManager for the service interfaces, and AllGlobalUsings files.

Engineering notes
Scoped services and the root provider. Registering plugins at startup failed because they depend on scoped services. They are now added to the kernel per request.
Two authorization layers. The second layer was tested by bypassing the first and confirming that the denied call was logged and blocked.
A silent filter bug. An inverted date filter returned no sessions. It stayed hidden while the database had no sessions, because an empty result looked correct. Lesson: test with data.
Tool selection. The model picks the nearest tool when none fits. Tool descriptions state their scope, and tools reject empty arguments so a wrong call cannot return wrong data.
Chat history versus tools. With history in the prompt, the model sometimes answered from earlier replies without calling a tool, including an invented member count. Sending each question on its own removed this behavior in testing.
Author

Ahmed Mohamed,
Junior .NET Developer

GitHub: AhmedEl-Metwally
LinkedIn: ahmed-mohamed-dotnet
