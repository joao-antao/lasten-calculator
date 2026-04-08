Welcome human! The .github folder holds many documents and folders. Below is an explanation of the layers and how they are structured in this repository.

## Layer 1 - Always-On Context

### Instructions (.github/copilot-instructions.md + .github/instructions/*.instructions.md) 
* a. Passive memory. 
* b. Applies to every prompt automatically. 

Use for: coding standards, framework rules, repository conventions

## Layer 2 - On-Demand Capabilities

### Prompt Files (.github/prompts/*.prompt.md) 
Manually invoked via slash commands. 

Use for: /security-review, /release-notes, /changelog

### Custom Agents (.github/agents/*.agent.md) 
Specialist personas with their own tools and MCP servers. 

Use for: planning agent → implementation agent → review agent (chained via handoffs)

### Skills (.github/skills/<name>/SKILL.md) 
Self-contained folders: instructions + scripts + references. 

Progressively loaded, the LLM reads the description first, loads full instructions only when relevant. 

Use for: repeatable runbooks, incident triage, IaC risk analysis

## Layer 3 - Enforcement & Automation

### Hooks (.github/hooks/*.json) 
Deterministic shell commands at lifecycle events — preToolUse, postToolUse, errorOccurred, and more. 

The preToolUse hook can approve or deny tool executions before they happen. 

Use for: policy gates, file access controls, audit logging

### Agentic Workflows (.github/workflows/ as Markdown) 
Natural language automation compiled to GitHub Actions via gh aw. Read-only permissions by default. 

→ Use for: issue triage, CI failure analysis, scheduled maintenance

# Why this matters

✔ Instructions → coding standards enforcement 

✔ Agents + Skills → repeatable operational workflows 

✔ Hooks → deterministic compliance gates

✔ Workflows → autonomous repo maintenance