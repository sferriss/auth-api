CREATE SCHEMA IF NOT EXISTS auth;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE auth.user (
                           id UUID DEFAULT uuid_generate_v4() PRIMARY KEY,
                           name VARCHAR(50) NOT NULL,
                           email VARCHAR(100) NOT NULL,
                           login VARCHAR(50) NOT NULL,
                           password VARCHAR(300) NOT NULL
);

CREATE UNIQUE INDEX idx_unique_email ON auth.user (email);
CREATE UNIQUE INDEX idx_unique_login ON auth.user (login);

CREATE TABLE auth.contact (
                              id UUID DEFAULT uuid_generate_v4() PRIMARY KEY,
                              phone_number VARCHAR(12) NOT NULL,
                              user_id UUID REFERENCES auth.user(id) ON DELETE CASCADE
);