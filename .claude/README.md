# Claude Code Configuration

This directory contains configuration for Claude Code (claude.ai/code) to assist with development of this project.

## 🔒 Security First

**CRITICAL**: See [SECRETS_MANAGEMENT.md](SECRETS_MANAGEMENT.md) for how to handle tokens and API keys safely.

- ✅ `settings.json` - Safe to commit (no secrets)
- 🔒 `settings.local.json` - **Gitignored** (contains secrets, NEVER commit)

## Directory Structure

```
.claude/
├── README.md                    # This file
├── SECRETS_MANAGEMENT.md        # 🔒 How to handle tokens safely
├── settings.json                # ✅ Project config (committed)
├── settings.local.json          # 🔒 User secrets (gitignored)
├── rules/                       # Development standards
│   ├── git-conventions.md
│   ├── spec-first.md
│   ├── testing.md
│   └── godot-conventions.md
├── skills/                      # User-invocable commands
│   ├── verify-coverage/
│   ├── audit-build/
│   └── commit-and-push/
├── agents/                      # Specialized subagents
│   ├── code-reviewer.md
│   ├── test-writer.md
│   └── ui-designer.md
├── agent-memory/                # Agent learning storage
│   ├── code-reviewer/
│   ├── test-writer/
│   └── ui-designer/
├── workflows/                   # Multi-agent orchestration
├── output-styles/               # Response formatting
├── research/                    # Research and analysis docs
└── audits/                      # Configuration audit reports
```

## Quick Start

### Using Skills (Slash Commands)

```bash
/verify-coverage                      # Check test coverage
/audit-build                          # Analyze build errors
/commit-and-push                      # Smart commit workflow
```

### Delegating to Agents

```bash
"Have the code-reviewer agent review this file"
"Ask the test-writer agent to create tests for X class"
"Ask the ui-designer agent to design the terminal screen"
```

### Running Workflows

```bash
"Use a workflow to implement the CRT shader effect"
```

## Configuration Files

### settings.json (Committed)
- Permissions (full repo access)
- Build verification hooks
- Default environment variables
- **No secrets**

### settings.local.json (Gitignored) 🔒
- User-specific secrets (GITHUB_TOKEN, etc.)
- Local environment overrides
- **Automatically created on first use**
- **Never committed to git**

## Security

All sensitive files are protected by `.gitignore`:
- `.claude/settings.local.json`
- `.claude/**/*.secret`
- `.claude/**/*.key`
- `.env` and `.env.*`

See [SECRETS_MANAGEMENT.md](SECRETS_MANAGEMENT.md) for complete security guide.

## Documentation

- [SECRETS_MANAGEMENT.md](SECRETS_MANAGEMENT.md) - Token and secrets handling
- [rules/](rules/) - Development standards
- [skills/](skills/) - Available commands
- [agents/](agents/) - Specialized agents
- [workflows/](workflows/) - Multi-agent orchestration

## Maintenance

Run periodic configuration audits:
```bash
/audit-claude-config
```

This checks:
- Rule relevance
- Skill effectiveness
- Agent utilization
- Memory growth
- Configuration health

## Getting Help

- Check CLAUDE.md in project root for development guide
- Consult specific rule files in `.claude/rules/`
- Ask Claude directly: "How do I...?"
