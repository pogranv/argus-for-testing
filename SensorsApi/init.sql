CREATE TYPE ticket_priority_type AS ENUM ('low', 'medium', 'high');

-- Таблица "Датчик"
CREATE TABLE sensors (
    id UUID PRIMARY KEY,
    ticket_name VARCHAR(255) NOT NULL,
    ticket_description TEXT NOT NULL,
    ticket_priority ticket_priority_type NOT NULL DEFAULT 'medium',
    resolve_days_count integer,
    business_process_id UUID NOT NULL
);