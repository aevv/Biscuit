CREATE TABLE [dbo].[Layer]
(
	[Layer_Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Layer_Chunk_Id] UNIQUEIDENTIFIER NOT NULL, 
    [Layer_Filepath] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_Layer_Chunk] FOREIGN KEY ([Layer_Chunk_Id]) REFERENCES [Chunk]([Chunk_Id])
)
