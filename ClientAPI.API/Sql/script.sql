/*script.sql crate table Clients*/
CREATE TABLE ClientDB.Clients (
id varchar(100) NOT NULL,
 name varchar(100) NOT NULL,
`size` varchar(100) NOT NULL,
CONSTRAINT Clients_pk PRIMARY KEY (id)
)
    ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;