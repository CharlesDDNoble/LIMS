CREATE DATABASE IF NOT EXISTS lims;
USE lims;

CREATE TABLE Users (
    userId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(256) UNIQUE NOT NULL,
    password VARCHAR(256) NOT NULL,
    salt VARCHAR(256) NOT NULL,
    accountType VARCHAR(256) NOT NULL,
    firstName VARCHAR(256) NOT NULL,
    lastName VARCHAR(256) NOT NULL,
    address VARCHAR(256) NOT NULL,
    city VARCHAR(256) NOT NULL,
    state CHAR(2) NOT NULL,
    zip CHAR(5) NOT NULL,
    phone CHAR(10) NOT NULL
);

CREATE TABLE Books (
    ISBN CHAR(13) NOT NULL PRIMARY KEY,
    title VARCHAR(256) NOT NULL,
    genre VARCHAR(256),
    author VARCHAR(256),
    summary VARCHAR(2048),
    datePublished DATE,
    imagePath VARCHAR(1024)
);

CREATE TABLE BookDetails (
    bookId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ISBN CHAR(13) NOT NULL,
    bookCondition VARCHAR(256),
    location VARCHAR(256),
    availability VARCHAR(20),
    FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

CREATE TABLE UserReviews (
    reviewId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    userId INT NOT NULL,
    ISBN CHAR(13) NOT NULL,
    rating INT NOT NULL,
    reviewText VARCHAR(2048),
    FOREIGN KEY (userId) REFERENCES Users(userId),
    FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

CREATE TABLE BookRequests (
    requestId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    userId INT NOT NULL,
    ISBN CHAR(13) NOT NULL,
    dateRequested DATE NOT NULL,
    FOREIGN KEY (userId) REFERENCES Users(userId),
    FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

CREATE TABLE BookHistory (
    bookHistoryId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    userId INT NOT NULL,
    ISBN CHAR(13) NOT NULL,
    dateCheckout DATE,
    dateDue DATE,
    dateReturned DATE,
    FOREIGN KEY (userId) REFERENCES Users(userId),
    FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

CREATE TABLE Reservations (
    reservationId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    userId INT NOT NULL,
    ISBN CHAR(13) NOT NULL,
    dateReserved DATE NOT NULL,
    FOREIGN KEY (userId) REFERENCES Users(userId),
    FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

CREATE TABLE Orders (
    orderId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ISBN CHAR(13) NOT NULL,
    quantity INT NOT NULL,
    dateOrdered DATE NOT NULL,
    dateExpected DATE,
    dateRecieved DATE,
    FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

INSERT INTO Books(ISBN,title,genre,author,summary,datePublished,imagePath)
VALUES ('0000000000000','A Sample Title','Fantasy','John Doe','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.','2001-01-01','placeholder-2.png');

INSERT INTO BookDetails(ISBN,bookCondition,location,availability)
VALUES ('0000000000000','Good','Q1.2.1','Available');