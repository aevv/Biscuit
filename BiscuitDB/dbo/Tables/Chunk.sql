CREATE TABLE [dbo].[Chunk]
(
	[Chunk_Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Chunk_Map_Id] UNIQUEIDENTIFIER NOT NULL, 
    [Chunk_X] INT NOT NULL, 
    [Chunk_Y] INT NOT NULL, 
    CONSTRAINT [FK_Chunk_Map] FOREIGN KEY ([Chunk_Map_Id]) REFERENCES [Map]([Map_Id])
)
