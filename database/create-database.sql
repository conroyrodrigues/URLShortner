CREATE DATABASE DataStore;
GO

USE DataStore;
GO

CREATE TABLE UrlShorteners
(
    UrlShortenerId          INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    OriginalUrl             VARCHAR(255) NOT NULL,  
    ShortUrl                VARCHAR(MAX) NOT NULL,
    CreatedOn               DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedOn               DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT UQ_OriginalUrl UNIQUE (OriginalUrl) 
);
GO

INSERT INTO UrlShorteners (OriginalUrl, ShortUrl) VALUES('https://www.google.com','	https://5343cd7c');
INSERT INTO UrlShorteners (OriginalUrl, ShortUrl) VALUES('https://www.yahoo.com/','	https://4fb6d1fb');
INSERT INTO UrlShorteners (OriginalUrl, ShortUrl) VALUES('https://www.duckduckgo.com','	https://6fb6d1fb');