-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema lims
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema lims
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `lims` DEFAULT CHARACTER SET latin1 ;
USE `lims` ;

-- -----------------------------------------------------
-- Table `lims`.`Books`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lims`.`Books` (
  `ISBN` CHAR(13) NOT NULL,
  `title` VARCHAR(256) NOT NULL,
  `genre` VARCHAR(256) NULL DEFAULT NULL,
  `author` VARCHAR(256) NULL DEFAULT NULL,
  `summary` VARCHAR(2048) NULL DEFAULT NULL,
  `datePublished` DATE NULL DEFAULT NULL,
  `imagePath` VARCHAR(1024) NULL DEFAULT NULL,
  PRIMARY KEY (`ISBN`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `lims`.`BookDetails`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lims`.`BookDetails` (
  `bookId` INT(11) NOT NULL AUTO_INCREMENT,
  `ISBN` CHAR(13) NOT NULL,
  `bookCondition` VARCHAR(256) NULL DEFAULT NULL,
  `location` VARCHAR(256) NULL DEFAULT NULL,
  `availability` VARCHAR(20) NULL DEFAULT NULL,
  PRIMARY KEY (`bookId`),
  INDEX `ISBN` (`ISBN` ASC),
  CONSTRAINT `BookDetails_ibfk_1`
    FOREIGN KEY (`ISBN`)
    REFERENCES `lims`.`Books` (`ISBN`))
ENGINE = InnoDB
AUTO_INCREMENT = 15
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `lims`.`Users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lims`.`Users` (
  `userId` INT(11) NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(256) NOT NULL,
  `password` VARCHAR(256) NOT NULL,
  `salt` VARCHAR(256) NOT NULL,
  `accountType` VARCHAR(256) NOT NULL,
  `firstName` VARCHAR(256) NOT NULL,
  `lastName` VARCHAR(256) NOT NULL,
  `address` VARCHAR(256) NOT NULL,
  `city` VARCHAR(256) NOT NULL,
  `state` CHAR(2) NOT NULL,
  `zip` CHAR(5) NOT NULL,
  `phone` CHAR(10) NOT NULL,
  PRIMARY KEY (`userId`),
  UNIQUE INDEX `username` (`username` ASC))
ENGINE = InnoDB
AUTO_INCREMENT = 10
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `lims`.`BookHistory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lims`.`BookHistory` (
  `bookHistoryId` INT(11) NOT NULL AUTO_INCREMENT,
  `userId` INT(11) NOT NULL,
  `bookId` INT(11) NULL DEFAULT NULL,
  `dateCheckout` DATE NULL DEFAULT NULL,
  `dateDue` DATE NULL DEFAULT NULL,
  `dateReturned` DATE NULL DEFAULT NULL,
  PRIMARY KEY (`bookHistoryId`),
  INDEX `userId` (`userId` ASC),
  INDEX `bookId` (`bookId` ASC),
  CONSTRAINT `BookHistory_ibfk_1`
    FOREIGN KEY (`userId`)
    REFERENCES `lims`.`Users` (`userId`),
  CONSTRAINT `BookHistory_ibfk_2`
    FOREIGN KEY (`bookId`)
    REFERENCES `lims`.`BookDetails` (`bookId`))
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `lims`.`BookRequests`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lims`.`BookRequests` (
  `requestId` INT(11) NOT NULL AUTO_INCREMENT,
  `userId` INT(11) NOT NULL,
  `ISBN` CHAR(13) NOT NULL,
  `dateRequested` DATE NOT NULL,
  PRIMARY KEY (`requestId`),
  INDEX `userId` (`userId` ASC),
  INDEX `ISBN` (`ISBN` ASC),
  CONSTRAINT `BookRequests_ibfk_1`
    FOREIGN KEY (`userId`)
    REFERENCES `lims`.`Users` (`userId`))
ENGINE = InnoDB
AUTO_INCREMENT = 6
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `lims`.`Orders`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lims`.`Orders` (
  `orderId` INT(11) NOT NULL AUTO_INCREMENT,
  `ISBN` CHAR(13) NOT NULL,
  `quantity` INT(11) NOT NULL,
  `dateOrdered` DATE NOT NULL,
  `dateExpected` DATE NULL DEFAULT NULL,
  `dateRecieved` DATE NULL DEFAULT NULL,
  PRIMARY KEY (`orderId`),
  INDEX `ISBN` (`ISBN` ASC),
  CONSTRAINT `Orders_ibfk_1`
    FOREIGN KEY (`ISBN`)
    REFERENCES `lims`.`Books` (`ISBN`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `lims`.`Reservations`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lims`.`Reservations` (
  `reservationId` INT(11) NOT NULL AUTO_INCREMENT,
  `userId` INT(11) NOT NULL,
  `bookId` INT(11) NULL DEFAULT NULL,
  `dateReserved` DATE NOT NULL,
  PRIMARY KEY (`reservationId`),
  INDEX `userId` (`userId` ASC),
  INDEX `bookId` (`bookId` ASC),
  CONSTRAINT `Reservations_ibfk_1`
    FOREIGN KEY (`userId`)
    REFERENCES `lims`.`Users` (`userId`),
  CONSTRAINT `Reservations_ibfk_2`
    FOREIGN KEY (`bookId`)
    REFERENCES `lims`.`BookDetails` (`bookId`))
ENGINE = InnoDB
AUTO_INCREMENT = 11
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `lims`.`UserReviews`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lims`.`UserReviews` (
  `reviewId` INT(11) NOT NULL AUTO_INCREMENT,
  `userId` INT(11) NOT NULL,
  `ISBN` CHAR(13) NOT NULL,
  `rating` INT(11) NOT NULL,
  `reviewText` VARCHAR(2048) NULL DEFAULT NULL,
  PRIMARY KEY (`reviewId`),
  INDEX `userId` (`userId` ASC),
  INDEX `ISBN` (`ISBN` ASC),
  CONSTRAINT `UserReviews_ibfk_1`
    FOREIGN KEY (`userId`)
    REFERENCES `lims`.`Users` (`userId`),
  CONSTRAINT `UserReviews_ibfk_2`
    FOREIGN KEY (`ISBN`)
    REFERENCES `lims`.`Books` (`ISBN`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = latin1;

USE `lims`;

DELIMITER $$
USE `lims`$$
CREATE
DEFINER=`admin`@`%`
TRIGGER `lims`.`ins_checkout`
BEFORE INSERT ON `lims`.`BookHistory`
FOR EACH ROW
SET NEW.dateDue = DATE_ADD(NEW.dateCheckout, INTERVAL 2 WEEK)$$


DELIMITER ;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
