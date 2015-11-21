declare @accId uniqueidentifier = newid()
insert into account values(@accId, 'admin', 'password', 0)

declare @charId uniqueidentifier = newid()
insert into character values(@charId, @accId, 'test', 0)

declare @mapId uniqueidentifier = newid()
insert into Map values(@mapId, 'Test', 'Test map', 1)

declare @chunk1 uniqueidentifier = newid()
insert into chunk values(@chunk1, @mapId, 0, 0)

declare @layer1 uniqueidentifier = newid()
insert into layer values(@layer1, @chunk1, 'world/map/test/0x0.bmc')

insert into CharacterLocation values (newid(), 0, 0, @mapId, @charId)