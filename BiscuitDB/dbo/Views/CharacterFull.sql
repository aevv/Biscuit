CREATE VIEW [dbo].[CharacterFull]
	AS SELECT * FROM Character C
	INNER JOIN CharacterLocation L
	ON C.Char_Id = L.Loc_Char_Id
