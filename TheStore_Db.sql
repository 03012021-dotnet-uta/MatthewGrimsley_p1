/*CREATE DATABASE TheStore_Db;*/

SET DATEFORMAT mdy;
GO

CREATE TABLE states(
	state_name varchar(50) PRIMARY KEY,
	abbreviation char(2) NOT NULL,
	tax_percent numeric(5,3) NOT NULL
);

CREATE TABLE parts(
	part_number int PRIMARY KEY IDENTITY(42000, 1),
	part_name varchar(50) NOT NULL,
	part_description varchar(300),
	price smallmoney NOT NULL,
	sale_percent numeric(5,3) DEFAULT(0.00),
	image_link varchar(300)
);

CREATE TABLE stores(
	store_number int PRIMARY KEY IDENTITY(10000, 1),
	store_name varchar(50) NOT NULL,
	city varchar(50),
	state_name varchar(50) FOREIGN KEY REFERENCES states(state_name),
	zip_code numeric(5),
	street_address varchar(100)
);

CREATE TABLE accounts(
	account_number int PRIMARY KEY IDENTITY(1000, 1),
	username varchar(50) NOT NULL,
	first_name varchar(50),
	last_name varchar(50),
	email varchar(50),
	phone_number numeric(14,0),
	permissions tinyint NOT NULL,
	salt char(64) NOT NULL,
	hash char(64) NOT NULL,
	default_store int FOREIGN KEY REFERENCES stores(store_number)
);

CREATE TABLE inventory(
	store_number int,
	part_number int,
	quantity int,
	CONSTRAINT store_part_pk PRIMARY KEY(store_number, part_number),
	FOREIGN KEY(store_number) REFERENCES stores(store_number),
	FOREIGN KEY(part_number) REFERENCES parts(part_number)
);

CREATE TABLE orders(
	order_number int PRIMARY KEY IDENTITY(1, 1),
	account_number int FOREIGN KEY REFERENCES accounts(account_number),
	store_number int FOREIGN KEY REFERENCES stores(store_number),
	subtotal smallmoney,
	tax smallmoney,
	total_price smallmoney,
	date_time datetime
);

CREATE TABLE orders_parts(
	order_number int,
	part_number int,
	unit_price smallmoney,
	quantity int,
	CONSTRAINT order_part_pk PRIMARY KEY(order_number, part_number),
	FOREIGN KEY(order_number) REFERENCES orders(order_number),
	FOREIGN KEY(part_number) REFERENCES parts(part_number)
);