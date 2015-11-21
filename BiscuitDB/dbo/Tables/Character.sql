CREATE TABLE [dbo].[Character] (
    [Char_Id]         UNIQUEIDENTIFIER NOT NULL,
    [Char_Acc_Id] UNIQUEIDENTIFIER NOT NULL,
    [Char_Name]       NVARCHAR (16)    NOT NULL,
    [Char_Deleted]    BIT              DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Char_Id] ASC),
    CONSTRAINT [FK_Character_Account] FOREIGN KEY ([Char_Acc_Id]) REFERENCES [dbo].[Account] ([Acc_Id])
);

