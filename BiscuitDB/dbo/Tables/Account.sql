CREATE TABLE [dbo].[Account] (
    [Acc_Id]       UNIQUEIDENTIFIER NOT NULL,
    [Acc_Username] NVARCHAR (50)    NOT NULL,
    [Acc_Password] NVARCHAR (50)    NOT NULL,
    [Acc_Deleted] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([Acc_Id] ASC)
);

