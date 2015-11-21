﻿CREATE TABLE [dbo].[Map]
(
	[Map_Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Map_Name] NVARCHAR(255) NOT NULL, 
    [Map_Description] NVARCHAR(MAX) NOT NULL, 
    [Map_Active] BIT NOT NULL DEFAULT 1
)
