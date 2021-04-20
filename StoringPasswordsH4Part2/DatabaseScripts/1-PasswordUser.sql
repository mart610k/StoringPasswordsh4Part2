CREATE DATABASE StoringPasswordsH4;

USE StoringPasswordsH4;

CREATE TABLE UserPassword(`ID` varchar(32) primary key, `Password` varchar(256), `Salt` varchar(64));