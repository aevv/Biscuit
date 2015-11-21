CREATE TABLE [dbo].[CharacterLocation]
(
	[Loc_Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Loc_X] FLOAT NOT NULL, 
    [Loc_Y] FLOAT NOT NULL, 
    [Loc_Map_Id] UNIQUEIDENTIFIER NOT NULL, 
    [Loc_Char_Id] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_CharacterLocation_Map] FOREIGN KEY ([Loc_Map_Id]) REFERENCES [Map]([Map_Id]), 
    CONSTRAINT [FK_CharacterLocation_Character] FOREIGN KEY ([Loc_Char_Id]) REFERENCES [Character]([Char_Id])
)
