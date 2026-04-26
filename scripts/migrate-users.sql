-- Migration: introduce Users table and normalize Players
--
-- Run this against the production database BEFORE deploying the new backend.
-- Existing players will have their user row synthesised from the Email column
-- that was stored on each player row. The "Name" field will be populated from
-- the email (best available data — will be overwritten with the real name on
-- the user's next login and first player creation).
--
-- Safe to run multiple times (all steps are idempotent).

BEGIN;

-- 1. Create Users table
CREATE TABLE IF NOT EXISTS "Users" (
    "Id"    text NOT NULL,
    "Email" text NOT NULL,
    "Name"  text NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Users_Email" ON "Users" ("Email");

-- 2. Populate Users from existing Players rows
--    UserId  → Users.Id
--    Email   → Users.Email  (falls back to "<userId>@unknown" if null)
--    Name    → Users.Name   (use email as stand-in; real name captured on next login)
INSERT INTO "Users" ("Id", "Email", "Name")
SELECT DISTINCT
    "UserId",
    COALESCE(NULLIF("Email", ''), "UserId" || '@unknown'),
    COALESCE(NULLIF("Email", ''), "UserId")
FROM "Players"
ON CONFLICT ("Id") DO NOTHING;

-- 3. Add FK constraint (Players.UserId → Users.Id)
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_name = 'FK_Players_Users_UserId'
    ) THEN
        ALTER TABLE "Players"
            ADD CONSTRAINT "FK_Players_Users_UserId"
            FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE;
    END IF;
END $$;

-- 4. Drop the now-redundant Email column from Players
ALTER TABLE "Players" DROP COLUMN IF EXISTS "Email";

COMMIT;
