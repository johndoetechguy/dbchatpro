INSERT INTO public.projects (id, name, description, repository_url, created_by, created_at, updated_at)
VALUES ('11111111-1111-1111-1111-111111111111', 'Project Alpha', 'First test project', 'https://github.com/example/alpha', '33333333-3333-3333-3333-333333333333', NOW(), NOW());
INSERT INTO public.projects (id, name, description, repository_url, created_by, created_at, updated_at)
VALUES ('22222222-2222-2222-2222-222222222222', 'Project Beta', 'Second test project', 'https://github.com/example/beta', '44444444-4444-4444-4444-444444444444', NOW(), NOW());

INSERT INTO public.profiles (id, user_id, email, full_name, avatar_url, role, created_at, updated_at)
VALUES ('33333333-3333-3333-3333-333333333333', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'alice@example.com', 'Alice Anderson', 'https://example.com/avatar/alice.png', 'admin', NOW(), NOW());
INSERT INTO public.profiles (id, user_id, email, full_name, avatar_url, role, created_at, updated_at)
VALUES ('44444444-4444-4444-4444-444444444444', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'bob@example.com', 'Bob Brown', 'https://example.com/avatar/bob.png', 'developer', NOW(), NOW());

INSERT INTO public.next_tasks (id, project_id, name, components, effort_days, estimated_loc, acceptance_criteria, stage, assigned_to, last_updated)
VALUES ('task-001', '11111111-1111-1111-1111-111111111111', 'Setup CI/CD', ARRAY['ci','cd'], 3, 200, ARRAY['Pipeline runs on push','Notifies on failure'], 'planned', ARRAY['aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa']::uuid[], NOW());
INSERT INTO public.next_tasks (id, project_id, name, components, effort_days, estimated_loc, acceptance_criteria, stage, assigned_to, last_updated)
VALUES ('task-002', '22222222-2222-2222-2222-222222222222', 'Implement Auth', ARRAY['backend'], 5, 350, ARRAY['JWT issued','Login required'], 'active', ARRAY['bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb']::uuid[], NOW());

-- INSERTS FOR public.components
INSERT INTO public.components (id, project_id, name, stage, version, type, jira_code, description, summary, context, last_updated)
VALUES ('55555555-5555-5555-5555-555555555555', '11111111-1111-1111-1111-111111111111', 'API Gateway', 'incubator', 'v0.1', 'service', 'API-1', 'Handles API routing', 'API entry point', 'Initial context', NOW());
INSERT INTO public.components (id, project_id, name, stage, version, type, jira_code, description, summary, context, last_updated)
VALUES ('66666666-6666-6666-6666-666666666666', '22222222-2222-2222-2222-222222222222', 'Frontend UI', 'resident', 'v1.0', 'frontend', 'UI-1', 'User interface', 'Main UI', 'React SPA', NOW());

INSERT INTO public.commits (id, project_id, component_name, task_id, commit_hash, message, author, timestamp)
VALUES ('77777777-7777-7777-7777-777777777777', '11111111-1111-1111-1111-111111111111', 'API Gateway', 'task-001', 'abc123', 'Initial commit', 'Alice', NOW());
INSERT INTO public.commits (id, project_id, component_name, task_id, commit_hash, message, author, timestamp)
VALUES ('88888888-8888-8888-8888-888888888888', '22222222-2222-2222-2222-222222222222', 'Frontend UI', 'task-002', 'def456', 'Add login page', 'Bob', NOW());

INSERT INTO public.agent_status (id, project_id, developer_id, status, last_heartbeat)
VALUES ('99999999-9999-9999-9999-999999999999', '11111111-1111-1111-1111-111111111111', '33333333-3333-3333-3333-333333333333', 'connected', NOW());
INSERT INTO public.agent_status (id, project_id, developer_id, status, last_heartbeat)
VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', '22222222-2222-2222-2222-222222222222', '44444444-4444-4444-4444-444444444444', 'disconnected', NOW());


-- NEXT SET

