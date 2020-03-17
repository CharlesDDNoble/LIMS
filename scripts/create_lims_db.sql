CREATE DATABASE IF NOT EXISTS lims;
USE lims;

CREATE TABLE Users (
    userId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(256),
    password VARCHAR(256),
	salt VARCHAR(256),
    accountType VARCHAR(256),
    firstName VARCHAR(256),
    lastName VARCHAR(256),
    address VARCHAR(256),
    state CHAR(2),
    phone CHAR(10)
);

CREATE TABLE Books (
    ISBN CHAR(13) NOT NULL PRIMARY KEY,
    title VARCHAR(256),
    genre VARCHAR(256),
    author VARCHAR(256),
    summary VARCHAR(2048),
    datePublished DATE,
    imagePath VARCHAR(1024)
);

CREATE TABLE BookDetails (
    bookId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ISBN CHAR(13),
    bookCondition VARCHAR(256),
    location VARCHAR(256),
    availability VARCHAR(20),
    FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

CREATE TABLE UserReviews (
    reviewId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    userId INT,
    ISBN CHAR(13),
    rating INT,
    reviewText VARCHAR(2048),
    FOREIGN KEY (userId) REFERENCES Users(userId),
    FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

CREATE TABLE BookRequests (
    requestId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    userId INT,
    ISBN CHAR(13),
    dateRequested DATE,
    FOREIGN KEY (userId) REFERENCES Users(userId),
    FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

CREATE TABLE BookHistory (
    bookHistoryId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    userId INT,
    ISBN CHAR(13),
    dateCheckout DATE,
    dateDue DATE,
    dateReturned DATE,
    FOREIGN KEY (userId) REFERENCES Users(userId),
    FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

CREATE TABLE Reservations (
    reservationId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    userId INT,
    ISBN CHAR(13),
    dateReserved DATE,
    FOREIGN KEY (userId) REFERENCES Users(userId),
    FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

CREATE TABLE Orders (
    orderId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ISBN CHAR(13),
    quantity INT,
    dateOrdered DATE,
    dateExpected DATE,
    dateRecieved DATE,
    FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

INSERT INTO Users(username,password,salt,accountType,firstName,lastName,address,state,phone)
VALUES ('test','a87a830412e85a54a6c391d55051a3b4f8aa7eb270be5e985f9497fc8b47adbb','63479ad69a090b258277ec8fba6f99419a2ffb248981510657c944ccd1148e97','guest','John','Doe','123 Example Ave.','MI','0123456789');

INSERT INTO Books(ISBN,title,genre,author,summary,datePublished,imagePath)
VALUES ('0000000000000','A Sample Title','Fantasy','John Doe','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.','2001-01-01','placeholder-2.png');

INSERT INTO BookDetails(ISBN,bookCondition,location,availability)
VALUES ('0000000000000','Good','Q1.2.1','Available');