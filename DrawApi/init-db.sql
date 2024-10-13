CREATE DATABASE SketchToImage;
USE SketchToImage;

-- Create the Users table
CREATE TABLE Users (
    UserId INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) NOT NULL,
    Password VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create the Sketches table
CREATE TABLE Sketches (
    SketchId INT AUTO_INCREMENT PRIMARY KEY,
    SketchName VARCHAR(255) NOT NULL UNIQUE,
    Prompt VARCHAR(255),
    UploadAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UserId INT,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- Create the GeneratedImage table
CREATE TABLE GeneratedImage (
    ImageId INT AUTO_INCREMENT PRIMARY KEY,
    ImageName VARCHAR(255) NOT NULL UNIQUE,
    SketchId INT,
    FOREIGN KEY (SketchId) REFERENCES Sketches(SketchId)
);

-- Insert initial data into Users table
INSERT INTO Users (Username, Password, Email) VALUES
('user1', 'password1', 'user1@example.com'),
('user2', 'password2', 'user2@example.com');

-- Insert initial data into Sketches table
INSERT INTO Sketches (SketchName, Prompt, UserId) VALUES
('sketch1', 'Draw a cat', 1),
('sketch2', 'Draw a dog', 2);

-- Insert initial data into GeneratedImage table
INSERT INTO GeneratedImage (ImageName, SketchId) VALUES
('image1', 1),
('image2', 2);

-- Create the view SketchToImageView
CREATE VIEW SketchToImageView AS
SELECT 
    Sketches.UserId,
    Sketches.SketchId,
    Sketches.SketchName,
    Sketches.Prompt,
    GeneratedImage.ImageId,
    GeneratedImage.ImageName,
    Sketches.UploadAt
FROM 
    Sketches
JOIN 
    GeneratedImage ON Sketches.SketchId = GeneratedImage.SketchId;
