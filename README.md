<h1>Trust Vault</h1>

<p>
  <strong>Trust Vault</strong> is a secure multi-tenant document exchange system built with .NET 9 and Blazor WebAssembly. 
  It is designed as an enterprise-grade solution for controlled document distribution with a heavy emphasis on auditability and cryptographic integrity.
</p>

<hr />

<h3>🏗 Architectural Design</h3>
<ul>
  <li>
    <b>Clean Architecture:</b> Strict separation of concerns between API Controllers, Business Logic (BL), and Data Access Layer (DAL).
  </li>
  <li>
    <b>Multi-tenant RBAC:</b> The database schema supports a "One user — multiple organizations" model with granular roles assigned per tenant.
  </li>
</ul>

<h3>🛡 Security & Audit Features</h3>
<ul>
  <li>
    <b>Immutable Audit Log:</b> Comprehensive tracking of every action, including Actor ID, IP Address, User-Agent, and metadata.
  </li>
  <li>
    <b>Token-Based Access:</b> A specialized <code>access_token</code> system featuring TTL (Time-To-Live), one-time use flags, and hash-based verification.
  </li>
  <li>
    <b>Policy Enforcement:</b> Document access is governed by specific <code>policy</code> entities, controlling max downloads, expiration, and domain restrictions.
  </li>
  <li>
    <b>Secure Authentication:</b> JWT-based identity implementation with a foundation for Refresh Token rotation.
  </li>
</ul>

<h3>🛠 Tech Stack</h3>
<ul>
  <li><b>Backend:</b> .NET 9, Entity Framework Core.</li>
  <li><b>Frontend:</b> Blazor WebAssembly (Razor components/classes).</li>
  <li><b>Database:</b> PostgreSQL.</li>
  <li><b>Environment:</b> Docker-compose for containerized development and deployment.</li>
</ul>

<hr />

<p>
  <i>This project is currently in Stage 1, focusing on secure manual document uploads and infrastructure for advanced encryption.</i>
</p>
