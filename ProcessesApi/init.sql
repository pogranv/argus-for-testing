CREATE TYPE ticket_priority_type AS ENUM ('low', 'medium', 'high');

-- Таблица "Очередь"
CREATE TABLE processes (
    id UUID PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    graph_id UUID NOT NULL
);

-- Таблица "Тикет"
CREATE TABLE tickets (
    id UUID PRIMARY KEY,
    name VARCHAR(255),
    description TEXT,
    ticket_priority ticket_priority_type NOT NULL DEFAULT 'medium',
    deadline TIMESTAMP,
    created_at TIMESTAMP DEFAULT NOW() NOT NULL,
    updated_at TIMESTAMP DEFAULT NOW() NOT NULL,
    author_id BIGINT NOT NULL,
    executor_id BIGINT NOT NULL,
    notification_ids BIGINT[] NOT NULL DEFAULT '{}',
    business_process_id UUID NOT NULL REFERENCES processes(id) ON DELETE CASCADE,
    status_id UUID NOT NULL
);

-- Таблица "Комментарий"
CREATE TABLE comments (
    id UUID PRIMARY KEY,
    text TEXT NOT NULL,
    mentioned_user_ids BIGINT[] NOT NULL DEFAULT '{}',
    created_at TIMESTAMP DEFAULT NOW() NOT NULL,
    author_id BIGINT NOT NULL,
    ticket_id UUID NOT NULL REFERENCES tickets(id) ON DELETE CASCADE
);