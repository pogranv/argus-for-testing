CREATE TYPE status_type AS ENUM ('phone', 'email', 'sms');

-- Таблица "Граф статусов"
CREATE TABLE graph (
    id UUID PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT
    
);

-- Таблица "Статус"
CREATE TABLE status (
    id UUID PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    sla_minutes INT,

    notification_type status_type,
    interval_minutes INT, -- NULL означает единоразовую отправку

    comment TEXT,
    mentioned_user_ids BIGINT[] NOT NULL DEFAULT '{}',

    duty_id BIGINT NOT NULL
);

-- Таблица "Привязка статусов к графу с указанием порядкового номера в графе и переходов в другие вершины"
CREATE TABLE status_flow (
    graph_id UUID NOT NULL REFERENCES graph(id) ON DELETE CASCADE,
    status_id UUID NOT NULL REFERENCES status(id) ON DELETE CASCADE,
    order_num INT NOT NULL,
    next_status_ids UUID[] NOT NULL DEFAULT '{}',
    PRIMARY KEY (graph_id, status_id)
);