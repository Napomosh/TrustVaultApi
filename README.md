Trust Vault
Enterprise-grade document exchange system with a focus on multi-tenancy and auditability. Developed as a high-security .NET 9+ implementation.

Technical Overview
Architecture & Design Patterns
Clean Architecture: Separation of concerns between API Controllers, Business Logic (BL), and Data Access Layer (DAL).

Multi-tenant RBAC: Database schema supports users belonging to multiple tenants with granular roles per organization.

Security Implementation
Audit Logging: Every action is recorded with actor ID, IP, User-Agent, and detailed metadata.

Token-Based Access: Specialized access_token system with TTL, one-time use flags, and hash-based verification.

Policy Enforcement: Document access is governed by specific policies (max downloads, expiration, domain restrictions).

Authentication: JWT-based identity with planned Refresh Token rotation.

Tech Stack
Backend: .NET 9, Entity Framework Core.

Frontend: Blazor WebAssembly.

Database: PostgreSQL.

Environment: Docker-compose for local development and deployment.
