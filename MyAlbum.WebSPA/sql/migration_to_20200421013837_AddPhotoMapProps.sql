ALTER TABLE [Photos] ADD [CenterLat] float NULL;

GO

ALTER TABLE [Photos] ADD [CenterLng] float NULL;

GO

ALTER TABLE [Photos] ADD [LocLat] float NULL;

GO

ALTER TABLE [Photos] ADD [LocLng] float NULL;

GO

ALTER TABLE [Photos] ADD [MapFilePath] nvarchar(max) NULL;

GO

ALTER TABLE [Photos] ADD [MapZoom] int NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200421013837_AddPhotoMapProps', N'3.1.0');

GO


