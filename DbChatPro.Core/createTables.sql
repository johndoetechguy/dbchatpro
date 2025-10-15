-- WARNING: This schema is for context only and is not meant to be run.
-- Table order and constraints may not be valid for execution.

CREATE TABLE public.projects (
  id uuid NOT NULL DEFAULT gen_random_uuid(),
  name text NOT NULL,
  description text,
  repository_url text,
  created_by uuid,
  created_at timestamp with time zone DEFAULT now(),
  updated_at timestamp with time zone DEFAULT now(),
  CONSTRAINT projects_pkey PRIMARY KEY (id)
);

CREATE TABLE public.profiles (
  id uuid NOT NULL DEFAULT gen_random_uuid(),
  user_id uuid NOT NULL UNIQUE,
  email text,
  full_name text,
  avatar_url text,
  role text DEFAULT 'user'::text CHECK (role = ANY (ARRAY['admin'::text, 'user'::text, 'developer'::text])),
  created_at timestamp with time zone DEFAULT now(),
  updated_at timestamp with time zone DEFAULT now(),
  CONSTRAINT profiles_pkey PRIMARY KEY (id)
);

CREATE TABLE public.next_tasks (
  id text NOT NULL,
  project_id uuid,
  name text NOT NULL,
  components text[],
  effort_days integer,
  estimated_loc integer,
  acceptance_criteria text[],
  stage text CHECK (stage = ANY (ARRAY['planned'::text, 'active'::text, 'completed'::text])),
  assigned_to uuid[],
  last_updated timestamp with time zone DEFAULT now(),
  CONSTRAINT next_tasks_pkey PRIMARY KEY (id),
  CONSTRAINT next_tasks_project_id_fkey FOREIGN KEY (project_id) REFERENCES public.projects(id)
);

CREATE TABLE public.components (
  id uuid NOT NULL DEFAULT gen_random_uuid(),
  project_id uuid,
  name text NOT NULL,
  stage text CHECK (stage = ANY (ARRAY['incubator'::text, 'candidate'::text, 'resident'::text])),
  version text,
  type text CHECK (type = ANY (ARRAY['frontend'::text, 'backend'::text, 'library'::text, 'tool'::text, 'service'::text])),
  jira_code text,
  description text,
  summary text,
  context text,
  last_updated timestamp with time zone DEFAULT now(),
  CONSTRAINT components_pkey PRIMARY KEY (id),
  CONSTRAINT components_project_id_fkey FOREIGN KEY (project_id) REFERENCES public.projects(id)
);

CREATE TABLE public.commits (
  id uuid NOT NULL DEFAULT gen_random_uuid(),
  project_id uuid,
  component_name text,
  task_id text,
  commit_hash text NOT NULL UNIQUE,
  message text,
  author text,
  timestamp timestamp with time zone DEFAULT now(),
  CONSTRAINT commits_pkey PRIMARY KEY (id),
  CONSTRAINT commits_task_id_fkey FOREIGN KEY (task_id) REFERENCES public.next_tasks(id),
  CONSTRAINT commits_project_id_fkey FOREIGN KEY (project_id) REFERENCES public.projects(id)
);

CREATE TABLE public.agent_status (
  id uuid NOT NULL DEFAULT gen_random_uuid(),
  project_id uuid,
  developer_id uuid,
  status text NOT NULL DEFAULT 'disconnected'::text CHECK (status = ANY (ARRAY['connected'::text, 'disconnected'::text])),
  last_heartbeat timestamp with time zone DEFAULT now(),
  CONSTRAINT agent_status_pkey PRIMARY KEY (id),
  CONSTRAINT agent_status_developer_id_fkey FOREIGN KEY (developer_id) REFERENCES public.profiles(id),
  CONSTRAINT agent_status_project_id_fkey FOREIGN KEY (project_id) REFERENCES public.projects(id)
);